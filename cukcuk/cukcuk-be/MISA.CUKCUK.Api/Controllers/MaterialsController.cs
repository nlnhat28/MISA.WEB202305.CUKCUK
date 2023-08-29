using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Application;
using MISA.CUKCUK.Domain;

namespace MISA.CUKCUK.Api
{
    /// <summary>
    /// Lớp controller nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class MaterialsController : BaseController<MaterialDto, Material>
    {
        #region Fields
        /// <summary>
        /// Service nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IMaterialService _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo controller nguyên vật liệu
        /// </summary>
        /// <param name="service">Service nguyên vật liệu</param>
        /// Created by: nlnhat (17/08/2023)
        public MaterialsController(IMaterialService service) : base(service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy mã mới nguyên vật liệu
        /// </summary>
        /// <param name="materialName">Tên nguyên vật liệu</param>
        /// <returns>Mã mới nguyên vật liệu</returns>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("NewCode")]
        public async Task<IActionResult> GetNewCodeAsync([FromQuery] string materialName)
        {
            var newCode = await _service.GetNewCodeAsync(materialName);
            return StatusCode(StatusCodes.Status200OK, newCode);
        }
        /// <summary>
        /// Lọc nâng cao nguyên vật liệu (Tìm kiếm, phân trang, sắp xếp, lọc theo cột)
        /// </summary>
        /// <param name="filterRequestDto">Dto chứa các thuộc tính lọc</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        [HttpPost("Filter")]
        public async Task<IActionResult> FilterAsync([FromBody] FilterRequestDto filterRequestDto)
        {
            var result = await _service.FilterAsync(
                filterRequestDto.KeySearch, filterRequestDto.PagingModel, filterRequestDto.SortModels, filterRequestDto.FilterModels);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Xuất nguyên vật liệu
        /// </summary>
        /// <param name="filterRequestDto">Dto chứa các thuộc tính lọc</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        [HttpPost("Excel")]
        public async Task<IActionResult> ExportAsync([FromBody] FilterRequestDto filterRequestDto)
        {
            var data = await _service.ExportToExcelAsync(filterRequestDto.KeySearch, filterRequestDto.SortModels, filterRequestDto.FilterModels);
            string fileName = "materials-export.xlsx";
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        #endregion
    }
}