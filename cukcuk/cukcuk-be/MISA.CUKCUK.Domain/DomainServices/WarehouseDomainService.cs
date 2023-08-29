using Microsoft.Extensions.Localization;

namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Domain service check validate nghiệp vụ nhà kho
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public class WarehouseDomainService : IWarehouseDomainService
    {
        #region Fields
        /// <summary>
        /// Repository nhà kho
        /// </summary>
        private readonly IWarehouseRepository _repository;
        /// <summary>
        /// Resource lưu trữ thông báo
        /// </summary>
        private readonly IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        public WarehouseDomainService(IWarehouseRepository repository, IStringLocalizer<Resource> resource)
        {
            _repository = repository;
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check trùng mã nhà kho
        /// </summary>
        /// <param name="warehouse">Entity nhà kho để check</param>
        /// <exception cref="ConflictException">Exception mã đã tồn tại</exception>
        /// Created by: nlnhat (17/08/2023)
        public async Task CheckDuplicatedCodeAsync(Warehouse warehouse)
        {
            var warehouseCode = warehouse.WarehouseCode;
            var warehouseExist = await _repository.GetByCodeAsync(warehouseCode);

            // Nếu trùng mã và trùng với kho khác (tránh trường hợp trùng vs chính kho đấy)
            if (warehouseExist != null && warehouse?.WarehouseId != warehouseExist?.WarehouseId)
                throw new ConflictException(
                    MISAErrorCode.WarehouseCodeDuplicated,
                    $"{_resource["WarehouseCode"]} <{warehouseCode}> {_resource["Duplicated"]}",
                    new ExceptionData("WarehouseCode", warehouseCode, ExceptionKey.FormItem, "FormItem"));
        }
        #endregion
    }
}
