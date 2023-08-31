using Microsoft.Extensions.Localization;

namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Domain service check validate nghiệp vụ nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public class MaterialDomainService : IMaterialDomainService
    {
        #region Fields
        /// <summary>
        /// Repository nguyên vật liệu
        /// </summary>
        private readonly IMaterialRepository _repository;
        /// <summary>
        /// Domain service đơn vị tính
        /// </summary>
        private readonly IUnitDomainService _unitDomainService;
        /// <summary>
        /// Domain service nhà kho
        /// </summary>
        private readonly IWarehouseDomainService _warehouseDomainService;
        /// <summary>
        /// Resource lưu trữ thông báo
        /// </summary>
        private readonly IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        public MaterialDomainService(
            IMaterialRepository repository,
            IUnitDomainService unitDomainService,
            IWarehouseDomainService warehouseDomainService,
            IStringLocalizer<Resource> resource)
        {
            _repository = repository;
            _unitDomainService = unitDomainService;
            _warehouseDomainService = warehouseDomainService;
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check trùng mã nguyên vật liệu
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu để check</param>
        /// <param name="materialCode">Mã nguyên vật liệu để check</param>
        /// <exception cref="ConflictException">Exception mã đã tồn tại</exception>
        /// Created by: nlnhat (17/08/2023)
        public async Task CheckDuplicatedCodeAsync(Guid materialId, string materialCode)
        {
            var materialExist = await _repository.GetByCodeAsync(materialCode);

            // Nếu trùng mã và trùng với nguyên vật liệu khác (tránh trường hợp trùng vs chính nguyên vật liệu đấy)
            if (materialExist != null && materialId != materialExist?.MaterialId)
                throw new ConflictException(
                    MISAErrorCode.MaterialCodeDuplicated,
                    $"{_resource["MaterialCode"]} <{materialCode}> {_resource["Duplicated"]}",
                    new ExceptionData("MaterialCode", materialCode, ExceptionKey.FormItem, "FormItem"));
        }
        /// <summary>
        /// Check tồn tại đơn vị tính hay không
        /// </summary>
        /// <param name="unitId">Id đơn vị tính để check</param>
        /// Created by: nlnhat (30/08/2023)
        public async Task CheckExistUnitAsync(Guid unitId)
        {
            await _unitDomainService.CheckExistUnitAsync(unitId);
        }
        /// <summary>
        /// Check tồn tại kho hay không
        /// </summary>
        /// <param name="warehouseId">Id nhà kho để check</param>
        /// Created by: nlnhat (30/08/2023)
        public async Task CheckExistWarehouseAsync(Guid warehouseId)
        {
            await _warehouseDomainService.CheckExistWarehouseAsync(warehouseId);
        }
        #endregion
    }
}
