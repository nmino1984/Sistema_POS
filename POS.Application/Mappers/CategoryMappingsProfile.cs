using AutoMapper;
using POS.Application.ViewModels.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Utilities.Statics;

namespace POS.Application.Mappers
{
    public class CategoryMappingsProfile : Profile
    {
        public CategoryMappingsProfile()
        {
            CreateMap<Category, CategoryResponseViewModel>()
                .ForMember(x => x.StateCategory, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
                .ReverseMap();

            CreateMap<BaseEntityResponse<Category>, BaseEntityResponse<CategoryResponseViewModel>>()
                .ReverseMap();

            CreateMap<CategoryResponseViewModel, Category>();

            CreateMap<Category, CategorySelectResponseViewModel>()
                .ReverseMap();
        }
    }
}
