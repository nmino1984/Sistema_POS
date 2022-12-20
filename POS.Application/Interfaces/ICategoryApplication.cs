using POS.Application.Commons.Bases;
using POS.Application.ViewModels.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface ICategoryApplication
    {
        Task<BaseResponse<BaseEntityResponse<CategoryResponseViewModel>>> ListCategories(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<CategorySelectResponseViewModel>>> ListSelectCategories();
        Task<BaseResponse<CategorySelectResponseViewModel>> GetCategoriesById(int id);
        Task<BaseResponse<bool>> RegisterCategory(CategoryResponseViewModel requestViewModel);
        Task<BaseResponse<bool>> EditCategory(int categoryId, CategoryResponseViewModel requestViewModel);
        Task<BaseResponse<bool>> DeleteCategory(int id);

    }
}
