using Microsoft.Extensions.Localization;

namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Domain service check validate nghiệp vụ đơn vị tính
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public class UnitDomainService : IUnitDomainService
    {
        #region Fields
        /// <summary>
        /// Repository đơn vị tính
        /// </summary>
        private readonly IUnitRepository _repository;
        /// <summary>
        /// Resource lưu trữ thông báo
        /// </summary>
        private readonly IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        public UnitDomainService(
            IUnitRepository repository,
            IStringLocalizer<Resource> resource)
        {
            _repository = repository;
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check trùng tên đơn vị tính
        /// </summary>
        /// <param name="unit">Entity đơn vị tính để check</param>
        /// <exception cref="ConflictException">Exception tên đã tồn tại</exception>
        /// Created by: nlnhat (17/08/2023)
        public async Task CheckDuplicatedNameAsync(Unit unit)
        {
            var unitName = unit.UnitName;
            var unitExist = await _repository.GetByNameAsync(unitName);

            // Nếu trùng tên và trùng với đơn vị khác (tránh trường hợp trùng vs chính đơn vị đấy)
            if (unitExist != null && unit?.UnitId != unitExist?.UnitId)
                throw new ConflictException(
                    MISAErrorCode.UnitNameDuplicated,
                    $"{_resource["UnitName"]} <{unitName}> {_resource["Duplicated"]}",
                    new ExceptionData("UnitName", unitName, ExceptionKey.FormItem, "FormItem"));
        }
        #endregion
    }
}
