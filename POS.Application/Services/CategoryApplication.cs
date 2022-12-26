using AutoMapper;
using FluentValidation;
using POS.Application.Commons.Bases;
using POS.Application.Interfaces;
using POS.Application.Validators.Category;
using POS.Application.ViewModels.Request;
using POS.Application.ViewModels.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Statics;

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


        public async Task<BaseResponse<BaseEntityResponse<CategoryResponseViewModel>>> ListCategories(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<CategoryResponseViewModel>>();
            var categories = await _unitOfWork.Category.ListCategories(filters);

            if (categories is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<BaseEntityResponse<CategoryResponseViewModel>>(categories);
                response.Message = ReplyMessages.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessages.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }

        public async Task<BaseResponse<IEnumerable<CategorySelectResponseViewModel>>> ListSelectCategories()
        {
            var response = new BaseResponse<IEnumerable<CategorySelectResponseViewModel>>();
            var categories = await _unitOfWork.Category.GetAllAsync();

            if (categories is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<CategorySelectResponseViewModel>>(categories);
                response.Message = ReplyMessages.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessages.MESSAGE_QUERY_EMPTY;
            }

            return response;

        }

        public async Task<BaseResponse<CategoryResponseViewModel>> GetCategoryById(int id)
        {
            var response = new BaseResponse<CategoryResponseViewModel>();
            var category = await _unitOfWork.Category.GetByIdAsync(id);

            if (category is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<CategoryResponseViewModel>(category);
                response.Message = ReplyMessages.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessages.MESSAGE_QUERY_EMPTY;
            }


            return response;

        }

        public async Task<BaseResponse<bool>> RegisterCategory(CategoryRequestViewModel requestViewModel)
        {
            var response = new BaseResponse<bool>();
            var validationResult = await _validationRules.ValidateAsync(requestViewModel);

            if (!validationResult.IsValid) 
            {
                response.IsSuccess = false; 
                response.Message = ReplyMessages.MESSAGE_VALIDATE;
                response.Errors = validationResult.Errors;

                return response;
            }
             
            var category = _mapper.Map<Category>(requestViewModel);
            response.Data = await _unitOfWork.Category.RegisteAsync(category);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessages.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessages.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> EditCategory(int categoryId, CategoryRequestViewModel requestViewModel)
        {
            var response = new BaseResponse<bool>();
            var categoryEdit = await GetCategoryById(categoryId);

            if (categoryEdit is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessages.MESSAGE_QUERY_EMPTY;
            }
            else 
            {
                var validationResult = await _validationRules.ValidateAsync(requestViewModel);

                if (!validationResult.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessages.MESSAGE_VALIDATE;
                    response.Errors = validationResult.Errors;

                    return response;
                }

                var category = _mapper.Map<Category>(requestViewModel);
                category.Id = categoryId;
                response.Data = await _unitOfWork.Category.EditAsync(category);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessages.MESSAGE_UPDATE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessages.MESSAGE_FAILED;
                }
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteCategory(int categoryId)
        {
            var response = new BaseResponse<bool>();
            var categoryDelete = await GetCategoryById(categoryId);

            if (categoryDelete is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessages.MESSAGE_QUERY_EMPTY;
            }
            else 
            {
                response.Data = await _unitOfWork.Category.DeleteAsync(categoryId);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessages.MESSAGE_DELETE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessages.MESSAGE_FAILED;
                }
            }

            return response;
        }
    }
}
