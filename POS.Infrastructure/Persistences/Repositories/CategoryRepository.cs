using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Statics;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly POSContext _context;

        public CategoryRepository(POSContext context)
        {
            _context = context;
        }

        public async Task<BaseEntityResponse<Category>> ListCategories(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<Category>();

            var categories = (from c in _context.Categories
                              where c.AuditDeleteUser == null && c.AuditDeleteDate == null
                              select c).AsNoTracking().AsQueryable();

            if (filters.NumFilter is not null && !String.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        categories = categories.Where(w => w.Name!.Contains(filters.TextFilter));
                        break;
                    case 2:
                        categories = categories.Where(w => w.Description!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                categories = categories.Where(w => w.State.Equals(filters.StateFilter));
            }

            if (!String.IsNullOrEmpty(filters.StartDate) && !String.IsNullOrEmpty(filters.EndDate))
            {
                categories = categories.Where(w => w.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && w.AuditCreateDate <= Convert.ToDateTime(filters.EndDate));
            }

            if (filters.Sort is null)
                filters.Sort = "CategoryId";

            response.TotalRecords = await categories.CountAsync();
            response.Items = await Ordering(filters, categories, !(bool)filters.Download!).ToListAsync();

            return response;

        }

        public async Task<IEnumerable<Category>> ListSelectCategories()
        {
            var categories = await _context.Categories
                .Where(w => w.State.Equals((int)StateTypes.Active) && w.AuditDeleteUser == null && w.AuditDeleteDate == null).AsNoTracking().ToListAsync();

            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _context.Categories
                .Where(w => w.CategoryId == id).AsNoTracking().FirstOrDefaultAsync();

            return category!;
        }

        public async Task<bool> RegisterCategory(Category category)
        {
            //Momentaneamente se va a utilizar el User con id = 1...
            //posteriormente se pondrá el que está autenticado
            category.AuditCreateUser = 1;
            //Igual con la fecha del registro
            category.AuditCreateDate = DateTime.Now;

            await _context.AddAsync(category);

            //Devuelve la Cantidad de Filas afectadas
            var rowsAffected = await _context.SaveChangesAsync();

            //En caso que se añada una fila va a devolver 1, que es > 0
            return rowsAffected > 0;
        }

        public async Task<bool> EditCategory(Category category)
        {
            //Momentaneamente se va a utilizar el User con id = 1...
            //posteriormente se pondrá el que está autenticado
            category.AuditUpdateUser = 1;
            //Igual con la fecha del registro
            category.AuditUpdateDate = DateTime.Now;

            _context.Update(category);
            //Las Propiedades de AuditCreateUser y AuditCreateDate van a ser excluidas para no Modificarlas
            //en caso de que por error se modifiquen y mantener la legalidad de los datos
            _context.Entry(category).Property(p => p.AuditCreateUser).IsModified = false;
            _context.Entry(category).Property(p => p.AuditCreateDate).IsModified = false;

            //Devuelve la Cantidad de Filas afectadas
            var rowsAffected = await _context.SaveChangesAsync();

            //En caso que se añada una fila va a devolver 1, que es > 0
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .Where(w => w.CategoryId == id).AsNoTracking().FirstOrDefaultAsync();

            category!.AuditDeleteUser = 1;
            category!.AuditDeleteDate = DateTime.Now;

            _context.Update(category);

            //Devuelve la Cantidad de Filas afectadas
            var rowsAffected = await _context.SaveChangesAsync();

            //En caso que se añada una fila va a devolver 1, que es > 0
            return rowsAffected > 0;

        }
    }
}
