using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Helpers;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Statics;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity  
    {
        private readonly POSContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(POSContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var getAll = await _entity
                .Where(w => w.State.Equals((int)StateTypes.Active) && w.AuditDeleteUser == null && w.AuditDeleteDate == null).AsNoTracking().ToListAsync();

            return getAll;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var getById = await _entity.AsNoTracking().Where(w => w.Id == id).FirstOrDefaultAsync();

            return getById!;
        }

        public async Task<bool> RegisteAsync(T entity)
        {
            entity.AuditCreateUser = 1;
            entity.AuditCreateDate = DateTime.Now;

            await _context.AddAsync(entity);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> EditAsync(T entity)
        {
            entity.AuditUpdateUser = 1;
            entity.AuditUpdateDate = DateTime.Now;

            _context.Update(entity);
            _context.Entry(entity).Property(p => p.AuditCreateUser).IsModified = false;
            _context.Entry(entity).Property(p => p.AuditCreateDate).IsModified = false;

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            T entity = await GetByIdAsync(id);

            entity!.AuditDeleteUser = 1;
            entity!.AuditDeleteDate = DateTime.Now;

            _context.Update(entity);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _entity;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IQueryable<TDTO> Ordering<TDTO>(BasePaginationRequest request, IQueryable<TDTO> queryable, bool pagination = false) where TDTO : class
        {
            IQueryable<TDTO> queryViewModel = request.Order == "desc" ? queryable.OrderBy($"{request.Sort} descending") : queryable.OrderBy($"{request.Sort} ascending");

            if (pagination)
            {
                queryViewModel = queryViewModel.Paginate(request);
            }
            
            return queryViewModel;
        }
    }
}
