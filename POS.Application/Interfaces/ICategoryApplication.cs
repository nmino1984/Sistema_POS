using POS.Application.Commons.Bases;
using POS.Application.ViewModels.Request;
using POS.Application.ViewModels.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface ICategoryApplication
    {
        Task<BaseResponse<BaseEntityResponse<ViewModels.Response.CategoryResponseViewModel>>> ListCategories(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<CategorySelectResponseViewModel>>> ListSelectCategories();
        Task<BaseResponse<CategoryResponseViewModel>> GetCategoryById(int id);
        Task<BaseResponse<bool>> RegisterCategory(CategoryRequestViewModel requestViewModel);
        Task<BaseResponse<bool>> EditCategory(int categoryId, CategoryRequestViewModel requestViewModel);
        Task<BaseResponse<bool>> DeleteCategory(int id);

    }
}
