using AutoMapper;
using Microsoft.Extensions.Localization;
using MISA.CUKCUK.Domain;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data;

namespace MISA.CUKCUK.Application
{
    /// <summary>
    /// Triển khai service nguyên vật liệu từ giao diện
    /// </summary> 
    /// Created by: nlnhat (15/08/2023)
    public class MaterialService : BaseService<MaterialDto, Material>, IMaterialService
    {
        #region Fields
        /// <summary>
        /// Repository nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (18/08/2023)
        private new readonly IMaterialRepository _repository;
        /// <summary>
        /// Repository nhật ký nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (18/08/2023)
        private readonly IMaterialAuditRepository _materialAuditRepository;
        /// <summary>
        /// Repository đơn vị chuyển đổi
        /// </summary>
        /// Created by: nlnhat (18/08/2023)
        private readonly IConversionUnitRepository _conversionUnitRepository;
        /// <summary>
        /// Domain service nguyên vật liệu, validate nghiệp vụ
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IMaterialDomainService _domainService;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service nguyên vật liệu
        /// </summary>
        /// <param name="repository">Repository nguyên vật liệu</param>
        /// <param name="domainService">Domain service nguyên vật liệu</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map nguyên vật liệu</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (17/08/2023)
        public MaterialService(IMaterialRepository repository,
                               IMaterialAuditRepository materialAuditRepository,
                               IConversionUnitRepository conversionUnitrepository,
                               IMaterialDomainService domainService,
                               IStringLocalizer<Resource> resource, IMapper mapper, IUnitOfWork unitOfWork)
                             : base(repository, resource, mapper, unitOfWork)
        {
            _repository = repository;
            _materialAuditRepository = materialAuditRepository;
            _conversionUnitRepository = conversionUnitrepository;
            _domainService = domainService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Map dto tạo mới sang thực thể nguyên vật liệu
        /// </summary>
        /// <param name="materialDto">Dto tạo mới nguyên vật liệu</param>
        /// <returns>Thực thể nguyên vật liệu</returns>
        /// Created by: nlnhat (17/08/2023)
        public override Material MapCreateDtoToEntity(MaterialDto materialDto)
        {
            materialDto.MaterialId = Guid.NewGuid();
            materialDto.IsUnfollow ??= false;
            materialDto.CreatedDate ??= DateTime.UtcNow;

            var material = _mapper.Map<Material>(materialDto);

            return material;
        }
        /// <summary>
        /// Map dto cập nhật sang thực thể nguyên vật liệu
        /// </summary>
        /// <param name="materialDto">Dto cập nhật nguyên vật liệu</param>
        /// <returns>Thực thể nguyên vật liệu</returns>
        /// Created by: nlnhat (17/08/2023)
        public override Material MapUpdateDtoToEntity(MaterialDto materialDto)
        {
            materialDto.ModifiedDate = DateTime.UtcNow;

            var material = _mapper.Map<Material>(materialDto);

            return material;
        }
        /// <summary>
        /// Map đơn vị chuyển đổi
        /// </summary>
        /// <param name="conversionUnitDtos">Danh sách dto đơn vị chuyển đổi</param>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Danh sách đơn vị chuyển đổi tạo mới</returns>
        /// Created by: nlnhat (17/08/2023)
        public List<ConversionUnit> MapCreateConversionUnits(List<ConversionUnitDto>? conversionUnitDtos, Guid materialId)
        {
            var conversionUnits = _mapper.Map<List<ConversionUnit>>(conversionUnitDtos);
            conversionUnits.ForEach(unit =>
            {
                unit.MaterialId = materialId;
            });
            return conversionUnits;
        }
        /// <summary>
        /// Validate tổng thể (input + nghiệp vụ) nguyên vật liệu
        /// </summary>
        /// <param name="material">Entity nguyên vật liệu</param>
        /// Created by: nlnhat (17/08/2023)
        public override async Task ValidateAsync(Material material)
        {
            // Check trùng mã nguyên vật liệu
            await _domainService.CheckDuplicatedCodeAsync(material.MaterialId, material.MaterialCode);

            // Check tồn tại đơn vị tính
            await _domainService.CheckExistUnitAsync(material.UnitId);

            // Check tồn tại nhà kho
            if (material.WarehouseId != null)
            {
                await _domainService.CheckExistWarehouseAsync((Guid)material.WarehouseId);
            }

            // Check khoảng code cho phép nếu code có dạng TNVL + Số
            var materialCode = material.MaterialCode;
            var materialName = material.MaterialName;
            await ValidateRangeCodeAsync(materialCode, materialName);
        }
        /// <summary>
        /// Check khoảng code nguyên vật liệu
        /// </summary>
        /// <param name="materialCode">Mã nguyên vật liệu</param>
        /// <param name="materialName">Tên nguyên vật liệu</param>
        /// Created by: nlnhat (17/08/2023)
        private async Task ValidateRangeCodeAsync(string materialCode, string materialName)
        {
            var prefix = ApplicationHelper.GetPrefixCode(materialName);

            var pattern = string.Format(@"^{0}(\d+)$", prefix);
            var matchingCode = Regex.Match(materialCode.ToUpper(), pattern);

            if (matchingCode.Success)
            {
                var number = Convert.ToInt32(matchingCode.Groups[1].Value);
                await _domainService.CheckRangeCodeAsync(prefix, number);
            }
        }
        /// <summary>
        /// Validate danh sách đơn vị chuyển đổi
        /// </summary>
        /// <param name="conversionUnits">Danh sách đơn vị chuyển đổi thêm và cập nhật</param>
        /// <param name="unitId">Id của đơn vị tính chính</param>
        /// <exception cref="ValidateException">Đơn vị chuyển đổi bị trùng nhau hoặc trùng đơn vị tính chính</exception>
        public async Task ValidateConversionUnitsAsync(List<ConversionUnit> conversionUnits, Guid unitId)
        {
            // Check các tên đơn vị chuyển đổi bị trùng nhau hoặc trùng đơn vị tính chính hay không
            foreach (var unit in conversionUnits)
            {
                if (unit.DestinationUnitId == unitId)
                {
                    throw new ValidateException(
                        MISAErrorCode.ConversionUnitDuplicatedUnit,
                        $"{_resource["ConversionUnit"]} <{unit.DestinationUnitName}> {_resource["Duplicated"]} {_resource["Unit"]}",
                        new ExceptionData("DestinationUnit", unit.ConversionUnitId.ToString()));
                }
                else if (conversionUnits.Any(otherUnit =>
                    unit.DestinationUnitId == otherUnit.DestinationUnitId
                    && unit.ConversionUnitId != otherUnit.ConversionUnitId))
                {
                    throw new ValidateException(
                        MISAErrorCode.ConversionUnitDuplicated,
                        $"{_resource["ConversionUnit"]} <{unit.DestinationUnitName}> {_resource["Duplicated"]}",
                        new ExceptionData("DestinationUnit", unit.ConversionUnitId.ToString()));
                }
            }

            // Check tồn tại đơn vị muốn chuyển đổi hay không
            var conversionUnitIds = conversionUnits.Select(conversionUnit => conversionUnit.DestinationUnitId).ToList();
            await _domainService.CheckExistDestinationUnitsAsync(conversionUnitIds);
        }
        /// <summary>
        /// Validate danh sách đơn vị chuyển đổi cập nhật
        /// </summary>
        /// <param name="conversionUnits">Danh sách đơn vị chuyển đổi cập nhật</param>
        /// <param name="materialId">Id của nguyên vật liệu</param>
        public async Task ValidateUpdateConversionUnitsAsync(List<ConversionUnit> conversionUnits, Guid materialId)
        {
            // Check tồn tại đơn vị chuyển đổi update hay không
            var conversionUnitIds = conversionUnits.Select(conversionUnit => conversionUnit.ConversionUnitId).ToList();
            await _domainService.CheckExistConversionUnitsAsync(conversionUnitIds, materialId);
        }
        /// <summary>
        /// Lấy nguyên vật liệu theo id
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Dto nguyên vật liệu được tìm thấy</returns>
        /// <exception cref="NotFoundException">Không tìm thấy nguyên vật liệu</exception>
        /// Created by: nlnhat (18/08/2023)
        public override async Task<MaterialDto> GetAsync(Guid materialId)
        {
            var material = await _repository.GetAsync(materialId) ??
                throw new NotFoundException(
                    MISAErrorCode.MaterialNotFound,
                    _resource["MaterialNotFound"],
                    new ExceptionData("MaterialId", materialId.ToString()));

            var materialDto = _mapper.Map<MaterialDto>(material);
            materialDto.ConversionUnits = _mapper.Map<List<ConversionUnitDto>>(material.ConversionUnits);

            return materialDto;
        }
        /// <summary>
        /// Tạo mới 1 nguyên vật liệu
        /// </summary>
        /// <param name="materialDto">Dto nguyên vật liệu</param>
        /// <returns>Id nguyên vật liệu mới</returns>
        /// Created by: nlnhat (18/08/2023)
        public override async Task<Guid> CreateAsync(MaterialDto materialDto)
        {
            var material = MapCreateDtoToEntity(materialDto);
            await ValidateAsync(material);

            var conversionUnitDtos = materialDto.ConversionUnits;

            // Map và validate đơn vị chuyển đổi
            var conversionUnits = MapCreateConversionUnits(conversionUnitDtos, material.MaterialId);
            if (conversionUnits != null)
                await ValidateConversionUnitsAsync(conversionUnits, material.UnitId);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await _repository.InsertAsync(material);

                var materialAudit = new MaterialAudit(Guid.NewGuid(), result, EditMode.Create, DateTime.UtcNow);
                await _materialAuditRepository.InsertAsync(materialAudit);

                if (conversionUnits?.Count > 0)
                {
                    await _conversionUnitRepository.InsertManyAsync(conversionUnits);
                }

                await _unitOfWork.CommitAsync();
                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Cập nhật 1 nguyên vật liệu
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <param name="materialDto">Dto nguyên vật liệu</param>
        /// <returns>Số bản ghi ảnh hưởng</returns>
        /// Created by: nlnhat (18/08/2023)
        public override async Task<int> UpdateAsync(Guid materialId, MaterialDto materialDto)
        {
            // Kiểm tra nguyên vật liệu tồn tại không
            _ = await _repository.GetAsync(materialId) ??
                throw new NotFoundException(
                    MISAErrorCode.MaterialNotFound,
                    _resource["MaterialNotFound"],
                    new ExceptionData(materialId.ToString()));

            // Map và validate nguyên vật liệu
            var material = MapUpdateDtoToEntity(materialDto);
            await ValidateAsync(material);

            // Phân loại edit mode 
            var conversionUnitDtos = materialDto.ConversionUnits;

            var createConversionUnits = new List<ConversionUnit>();
            var updateConversionUnits = new List<ConversionUnit>();
            var deleteConversionUnits = new List<Guid>();

            if (conversionUnitDtos != null)
            {
                foreach (var unitDto in conversionUnitDtos)
                {
                    var unit = _mapper.Map<ConversionUnit>(unitDto);

                    switch (unitDto.EditMode)
                    {
                        case EditMode.Create:
                            {
                                unit.MaterialId = material.MaterialId;
                                createConversionUnits.Add(unit);
                                break;
                            }
                        case EditMode.Update:
                            {
                                updateConversionUnits.Add(unit);
                                break;
                            }
                        case EditMode.Delete:
                            {
                                deleteConversionUnits.Add(unit.ConversionUnitId);
                                break;
                            }
                        default: break;
                    }
                }
            }
            // Validate đơn vị chuyển đổi
            var conversionUnits = createConversionUnits.Concat(updateConversionUnits).ToList();
            if (conversionUnits != null)
                await ValidateConversionUnitsAsync(conversionUnits, material.UnitId);

            if (updateConversionUnits != null)
                await ValidateUpdateConversionUnitsAsync(updateConversionUnits, material.MaterialId);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await _repository.UpdateAsync(material);

                if (createConversionUnits?.Count > 0)
                {
                    await _conversionUnitRepository.InsertManyAsync(createConversionUnits);
                }
                if (updateConversionUnits?.Count > 0)
                {
                    await _conversionUnitRepository.UpdateManyAsync(updateConversionUnits);
                }
                if (deleteConversionUnits?.Count > 0)
                {
                    await _conversionUnitRepository.DeleteManyAsync(deleteConversionUnits);
                }

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Xoá nguyên vật liệu
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Số lượng bản ghi thay đổi</returns>
        public override async Task<int> DeleteAsync(Guid materialId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await _repository.DeleteAsync(materialId);

                var materialAudit = new MaterialAudit(Guid.NewGuid(), materialId, EditMode.Delete, DateTime.UtcNow);
                await _materialAuditRepository.InsertAsync(materialAudit);

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Lấy mã mới nguyên vật liệu
        /// </summary>
        /// <param name="materialName">Tiền tố của mã</param>
        /// <returns>Mã mới nguyên vật liệu</returns>
        /// Created by: nlnhat(17/08/2023)
        public async Task<string> GetNewCodeAsync(string materialName)
        {
            var prefix = ApplicationHelper.GetPrefixCode(materialName);

            var maxCode = await _repository.GetMaxCodeAsync(prefix);
            var result = $"{prefix}{++maxCode}";
            return result;
        }
        /// <summary>
        /// Lọc nâng cao nguyên vật liệu (Tìm kiếm, phân trang, sắp xếp, lọc theo cột)
        /// </summary>
        /// <param name="keySearch">Từ khoá tìm kiếm</param>
        /// <param name="pagingModel">Các thuộc tính phân trang</param>
        /// <param name="sortModels">Các điều kiện sắp xếp</param>
        /// <param name="filterModels">Các điều kiện lọc</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<FilterResultModel<Material>> FilterAsync(
            string? keySearch, PagingModel? pagingModel, List<SortModel>? sortModels, List<FilterModel>? filterModels)
        {
            if (pagingModel != null && pagingModel.PageNumber < 1)
            {
                pagingModel.PageNumber = 1;
            }

            var result = await _repository.FilterAsync(keySearch, pagingModel, sortModels, filterModels);
            return result;
        }
        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="keySearch">Từ khoá tìm kiếm</param>
        /// <param name="sortModels">Các điều kiện sắp xếp</param>
        /// <param name="filterModels">Các điều kiện lọc</param>
        /// <returns>Excel data</returns>
        /// <exception cref="NotFoundException">Không tìm thấy nguyên vật liệu</exception>
        /// Created by: nlnhat (18/08/2023)
        public async Task<MemoryStream> ExportToExcelAsync(string? keySearch, List<SortModel>? sortModels, List<FilterModel>? filterModels)
        {
            //Lấy data
            var materials = await _repository.ExportAsync(keySearch, sortModels, filterModels) ??
                     throw new NotFoundException(
                    MISAErrorCode.MaterialNotFound,
                    _resource["MaterialNotFound"]);

            var materialExcelDtos = new List<MaterialExcelDto>();
            var index = 0;
            foreach (var material in materials)
            {
                var materialDto = _mapper.Map<MaterialExcelDto>(material);
                materialDto.Index = ++index;
                materialDto.ConversionUnits = _mapper.Map<List<ConversionUnitExcelDto>>(material.ConversionUnits);
                materialExcelDtos.Add(materialDto);
            }

            var startRow = 3;
            var materialFirstColumn = MaterialExcelDto.FirstColumn;
            var materialLastColumn = MaterialExcelDto.LastColumn;

            var conversionFirstColumn = ConversionUnitExcelDto.FirstColumn;
            var conversionLastColumn = ConversionUnitExcelDto.LastColumn;

            try
            {
                // Khởi tạo excel package
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using var package = new ExcelPackage();
                var sheetName = _resource["MaterialsList"];
                var sheet = package.Workbook.Worksheets.Add(sheetName);

                // Get type
                var materialType = typeof(MaterialExcelDto);
                var conversionType = typeof(ConversionUnitExcelDto);

                // Get tên bảng
                var materialTableName = materialType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                var conversionTableName = conversionType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;

                // Get properties
                var materialProperties = materialType.GetProperties();
                var conversionProperties = conversionType.GetProperties();
                var properties = materialProperties.Concat(conversionProperties);

                // Đổ dữ liệu
                var row = startRow + 2;
                foreach (var material in materialExcelDtos)
                {
                    var conversionUnits = material.ConversionUnits;
                    var conversionLength = conversionUnits?.Count ?? 0;

                    var range = sheet.Cells[$"{materialFirstColumn}{row}:{conversionLastColumn}{row + Math.Max(0, conversionLength - 1)}"];
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);

                    if (conversionLength == 0)
                    {
                        var tempMaterial = new List<MaterialExcelDto>
                        {
                            material
                        };
                        var cells = sheet.Cells[$"{materialFirstColumn}{row}"];
                        cells.LoadFromCollection(tempMaterial);
                        row++;
                    }
                    else
                    {
                        foreach (var property in materialProperties)
                        {
                            var excelDisplayAttribute = property.GetCustomAttribute<ExcelDisplayAttribute>();
                            if (excelDisplayAttribute == null)
                                continue;

                            var columnLetter = excelDisplayAttribute.ColumnLetter;
                            var cells = sheet.Cells[$"{columnLetter}{row}:{columnLetter}{row + conversionLength - 1}"];

                            cells.Merge = true;
                            cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cells.Value = property.GetValue(material);
                        }
                        sheet.Cells[$"{conversionFirstColumn}{row}"].LoadFromCollection(conversionUnits);
                        row += conversionLength;
                    }
                }

                // Chỉnh lại style các cột
                foreach (var property in properties)
                {
                    // Lấy thông tin cột
                    var excelDisplayAttribute = property.GetCustomAttribute<ExcelDisplayAttribute>();
                    if (excelDisplayAttribute == null)
                        continue;

                    var columnLetter = excelDisplayAttribute.ColumnLetter;
                    var columnName = excelDisplayAttribute.ColumnName;
                    var horizontalAlign = excelDisplayAttribute?.HorizontalAlignment ?? ExcelHorizontalAlignment.Left;
                    var numberFormat = excelDisplayAttribute?.NumberFormat;

                    // Chỉnh phần header
                    var headerCell = sheet.Cells[$"{columnLetter}{startRow + 1}"];
                    headerCell.Value = columnName;
                    headerCell.Style.Font.Bold = true;

                    // Format dữ liệu
                    if (numberFormat != null)
                    {
                        sheet.Cells[$"{columnLetter}:{columnLetter}"].Style.Numberformat.Format = numberFormat;
                    }

                    // Chỉnh cho 1 cột
                    var column = sheet.Cells[$"{columnLetter}:{columnLetter}"];
                    column.AutoFitColumns();
                    column.Style.HorizontalAlignment = horizontalAlign;
                    column.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Thêm border
                    sheet.Cells[$"{columnLetter}{startRow}:{columnLetter}{row - 1}"].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
                }
                // Chỉnh tiêu đề
                var titleRange = sheet.Cells[$"{materialFirstColumn}1:{conversionLastColumn}1"];
                titleRange.Merge = true;
                titleRange.Value = sheetName;
                titleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Font.Size = 22;

                // Chỉnh tên bảng material
                var materialNameRange = sheet.Cells[$"{materialFirstColumn}{startRow}:{materialLastColumn}{startRow}"];
                materialNameRange.Merge = true;
                materialNameRange.Value = materialTableName;
                materialNameRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                materialNameRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                materialNameRange.Style.Font.Bold = true;
                materialNameRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                materialNameRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                // Chỉnh tên bảng conversion
                var converisonNameRange = sheet.Cells[$"{conversionFirstColumn}{startRow}:{conversionLastColumn}{startRow}"];
                converisonNameRange.Merge = true;
                converisonNameRange.Value = conversionTableName;
                converisonNameRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                converisonNameRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                converisonNameRange.Style.Font.Bold = true;
                converisonNameRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                converisonNameRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                // Chỉnh header
                sheet.Rows[startRow].Height = 22;
                sheet.Rows[startRow + 1].Height = 22;

                var materialHeader = sheet.Cells[$"{materialFirstColumn}{startRow + 1}:{materialLastColumn}{startRow + 1}"];
                materialHeader.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
                materialHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                materialHeader.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                var conversionHeader = sheet.Cells[$"{conversionFirstColumn}{startRow + 1}:{conversionLastColumn}{startRow + 1}"];
                conversionHeader.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
                conversionHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                conversionHeader.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                var memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);
                memoryStream.Position = 0;

                return memoryStream;
            }
            catch (Exception exception)
            {
                throw new IncompleteException(MISAErrorCode.MaterialExportError, _resource["MaterialExportError"], exception.Message);
            }
        }
        /// <summary>
        /// Đếm số lượng theo các năm
        /// </summary>
        /// <returns>Danh sách số lượng theo năm</returns>
        /// Created by: nlnhat (08/09/2023)
        public async Task<IEnumerable<CountByYearModel>> CountByYear()
        {
            var result = await _materialAuditRepository.CountByYear();
            return result;
        }
        /// <summary>
        /// Đếm số lượng theo các kho
        /// </summary>
        /// <returns>Danh sách số lượng theo kho</returns>
        /// Created by: nlnhat (08/09/2023)
        public async Task<IEnumerable<CountByWarehouseModel>> CountByWarehouse()
        {
            var result = await _repository.CountByWarehouse();
            return result;
        }
        /// <summary>
        /// Đếm số lượng theo trạng thái theo dõi
        /// </summary>
        /// <returns>Số lượng theo trạng thái</returns>
        /// Created by: nlnhat (08/09/2023)
        public async Task<CountByFollowModel> CountByFollow()
        {
            var result = await _repository.CountByFollow();
            return result;
        }
        #endregion
    }
}