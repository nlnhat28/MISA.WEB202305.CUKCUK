using Microsoft.Extensions.Localization;
using System.Linq;

namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Domain service check validate nghiệp vụ đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public class ConversionUnitDomainService : IConversionUnitDomainService
    {
        #region Fields
        /// <summary>
        /// Repository đơn vị chuyển đổi
        /// </summary>
        private readonly IConversionUnitRepository _repository;
        /// <summary>
        /// Repository đơn vị tính
        /// </summary>
        private readonly IUnitRepository _unitRepository;
        /// <summary>
        /// Resource lưu trữ thông báo
        /// </summary>
        private readonly IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        public ConversionUnitDomainService(
            IConversionUnitRepository repository,
            IUnitRepository unitRepository,
            IStringLocalizer<Resource> resource)
        {
            _repository = repository;
            _unitRepository = unitRepository;
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check tồn tại danh sách đơn vị muốn chuyển đổi
        /// </summary>
        /// <param name="destinationUnitIds">Danh sách id đơn vị muốn chuyển đổi</param>
        /// <exception cref="NotFoundException">Không tìm thấy đơn vị tính</exception>
        /// Created by: nlnhat (30/08/2023)
        public async Task CheckExistDestinationUnitsAsync(List<Guid> destinationUnitIds)
        {
            var units = await _unitRepository.GetAllAsync() ?? 
                throw new NotFoundException(
                    MISAErrorCode.UnitNotFound,
                    _resource["UnitNotFound"]);

            var unitIds = units.Select(unit => unit.UnitId);

            foreach (var destinationUnitId in destinationUnitIds)
            {
                if (!unitIds.Any(unitId => unitId == destinationUnitId))
                {
                    throw new NotFoundException(
                        MISAErrorCode.UnitNotFound,
                        _resource["UnitNotFound"],
                        new ExceptionData("DestinationUnit", destinationUnitId.ToString()));
                }
            }
        }
        #endregion
    }
}