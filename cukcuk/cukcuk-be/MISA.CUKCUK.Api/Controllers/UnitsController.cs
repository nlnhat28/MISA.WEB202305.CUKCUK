using MISA.CUKCUK.Application;
using MISA.CUKCUK.Domain;

namespace MISA.CUKCUK.Api
{
    /// <summary>
    /// Lớp controller đơn vị tính
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class UnitsController : BaseController<UnitDto, Unit>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo controller đơn vị tính
        /// </summary>
        /// <param name="service">Service đơn vị tính</param>
        /// Created by: nlnhat (13/07/2023)
        public UnitsController(IUnitService service) : base(service)
        {
        }
        #endregion
    }
}