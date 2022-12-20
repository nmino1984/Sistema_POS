using AutoMapper;
using POS.Application.Commons.Bases;
using POS.Application.Interfaces;
using POS.Application.Validators.Category;
using POS.Application.ViewModels.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Services
{
    public class CategoryApplication : ICategoryApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CategoryValidator _validationRules;

        public CategoryApplication(IUnitOfWork unitOfWork, IMapper mapper, CategoryValidator validationRules)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationRules = validationRules;
        }


        public Task<BaseResponse<BaseEntityResponse<CategoryResponseViewModel>>> ListCategories(BaseFiltersRequest filters)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IEnumerable<CategorySelectResponseViewModel>>> ListSelectCategories()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<CategorySelectResponseViewModel>> GetCategoriesById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> RegisterCategory(CategoryResponseViewModel requestViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> EditCategory(int categoryId, CategoryResponseViewModel requestViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }
    }
}
