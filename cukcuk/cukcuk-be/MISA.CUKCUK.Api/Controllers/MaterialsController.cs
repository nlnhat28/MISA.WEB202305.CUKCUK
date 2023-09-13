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
        /// <summary>
        /// Web host environment
        /// </summary>
        /// Created by: nlnhat (11/09/2023)
        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo controller nguyên vật liệu
        /// </summary>
        /// <param name="service">Service nguyên vật liệu</param>
        /// Created by: nlnhat (17/08/2023)
        public MaterialsController(IMaterialService service, IWebHostEnvironment webHostEnvironment) : base(service)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
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
        [HttpPost("Excel/Export")]
        public async Task<IActionResult> ExportAsync([FromBody] FilterRequestDto filterRequestDto)
        {
            var data = await _service.ExportToExcelAsync(filterRequestDto.KeySearch, filterRequestDto.SortModels, filterRequestDto.FilterModels);
            string fileName = "materials-export.xlsx";
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        /// <summary>
        /// Lấy file nhập khẩu mẫu
        /// </summary>
        /// <returns>File nhập khẩu mẫu</returns>
        /// Created by: nlnhat (16/08/2023)
        [HttpGet("Excel/ImportTemplate")]
        public async Task<IActionResult> GetImportTemplateAsync()
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Assets/Materials/MaterialsImportTemplate.xlsx");

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                string fileName = "materials-import-template.xlsx";
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                return NotFound();
            }
        }
        /// <summary>
        ///  Map dữ liệu từ file nhập khẩu
        /// </summary>
        /// <param name="file">File excel nhập khẩu</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        [HttpPost("Excel/MappingImport")]
        public async Task<IActionResult> MapImportAsync([FromForm]IFormFile file)
        {
            var result = await _service.MapImportFileAsync(file);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        ///  Nhập dữ liệu từ excel
        /// </summary>
        /// <param name="materialDtos">Dto nguyên vật liệu</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        [HttpPost("Excel/Import")]
        public async Task<IActionResult> ImportFromExcelAsync(List<MaterialDto> materialDtos)
        {
            var result = await _service.ImportFromExcelAsync(materialDtos);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Đếm số lượng theo các năm
        /// </summary>
        /// <returns>Danh sách số lượng theo năm</returns>
        /// Created by: nlnhat (08/09/2023)
        [HttpGet("CountByYear")]
        public async Task<IActionResult> CountByYear()
        {
            var result = await _service.CountByYear();
            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Đếm số lượng theo các năm
        /// </summary>
        /// <returns>Danh sách số lượng theo năm</returns>
        /// Created by: nlnhat (08/09/2023)
        [HttpGet("CountByWarehouse")]
        public async Task<IActionResult> CountByWarehouse()
        {
            var result = await _service.CountByWarehouse();
            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Đếm số lượng theo trạng thái theo dõi
        /// </summary>
        /// <returns>Số lượng theo trạng thái</returns>
        /// Created by: nlnhat (08/09/2023)
        [HttpGet("CountByFollow")]
        public async Task<IActionResult> CountByFollow()
        {
            var result = await _service.CountByFollow();
            return StatusCode(StatusCodes.Status200OK, result);
        }
        #endregion
    }
}