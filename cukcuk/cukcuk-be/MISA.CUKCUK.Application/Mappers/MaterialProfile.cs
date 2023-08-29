using AutoMapper;
using MISA.CUKCUK.Domain;

namespace MISA.CUKCUK.Application
{
    /// <summary>
    /// Tạo mapper nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class MaterialProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public MaterialProfile()
        {
            CreateMap<MaterialModel, MaterialDto>()
            .ForMember(dest => dest.TimeUnitName, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.GetTimeUnitName(src.TimeUnit));
            })
            .ForMember(dest => dest.ConversionUnits, opt =>
            {
                opt.Ignore();
            })
            .ForMember(dest => dest.CreatedDate, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.ConvertDateUtcToLocal(src.CreatedDate));
            })
            .ForMember(dest => dest.ModifiedDate, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.ConvertDateUtcToLocal(src.ModifiedDate));
            });
            CreateMap<MaterialDto, Material>();
            CreateMap<MaterialModel, MaterialExcelDto>()
           .ForMember(dest => dest.TimeUnitName, opt =>
           {
               opt.MapFrom(src =>
                   ApplicationHelper.GetTimeUnitName(src.TimeUnit));
           })
           .ForMember(dest => dest.ConversionUnits, opt =>
           {
               opt.Ignore();
           });
        }
        #endregion
    }
}