using MISA.CUKCUK.Domain;

namespace MISA.CUKCUK.Application
{
    /// <summary>
    /// Giao diện service nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public interface IMaterialService : IBaseService<MaterialDto, Material>
    {
        /// <summary>
        /// Lấy mã mới cho nguyên vật liệu
        /// </summary>
        /// <param name="materialName">Tên nguyên vật liệu</param>
        /// <returns>Mã mới có dạng Prefix-Số</returns>
        /// Created by: nlnhat (17/08/2023)
        Task<string> GetNewCodeAsync(string materialName);
        /// <summary>
        /// Lọc nâng cao nguyên vật liệu (Tìm kiếm, phân trang, sắp xếp, lọc theo cột)
        /// </summary>
        /// <param name="keySearch">Từ khoá tìm kiếm</param>
        /// <param name="pagingModel">Các thuộc tính phân trang</param>
        /// <param name="sortModels">Các điều kiện sắp xếp</param>
        /// <param name="filterModels">Các điều kiện lọc</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        Task<FilterResultModel<Material>> FilterAsync(string? keySearch, PagingModel? pagingModel,
            List<SortModel>? sortModels, List<FilterModel>? filterModels);
        /// <summary>
        /// Export to excel
        /// </summary>
        /// <param name="keySearch">Từ khoá tìm kiếm</param>
        /// <param name="sortModels">Các điều kiện sắp xếp</param>
        /// <param name="filterModels">Các điều kiện lọc</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        Task<MemoryStream> ExportToExcelAsync(string? keySearch, List<SortModel>? sortModels, List<FilterModel>? filterModels);
        /// <summary>
        /// Map đơn vị chuyển đổi
        /// </summary>
        /// <param name="conversionUnitDtos">Danh sách dto đơn vị chuyển đổi</param>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Danh sách đơn vị chuyển đổi tạo mới</returns>
        /// Created by: nlnhat (17/08/2023)
        List<ConversionUnit> MapCreateConversionUnits(List<ConversionUnitDto>? conversionUnitDtos, Guid materialId);
        /// <summary>
        /// Validate danh sách đơn vị chuyển đổi
        /// </summary>
        /// <param name="conversionUnitDtos">Danh sách dto đơn vị chuyển đổi</param>
        /// <param name="unitId">Id của đơn vị tính chính</param>
        /// <exception cref="ValidateException">Đơn vị chuyển đổi bị trùng nhau hoặc trùng đơn vị tính chính</exception>
        void ValidateConversionUnit(List<ConversionUnitDto> conversionUnitDtos, Guid unitId);
    }
}
