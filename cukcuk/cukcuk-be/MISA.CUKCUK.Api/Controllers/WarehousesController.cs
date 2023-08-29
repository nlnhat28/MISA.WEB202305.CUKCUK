using MISA.CUKCUK.Application;
using MISA.CUKCUK.Domain;

namespace MISA.CUKCUK.Api
{
    /// <summary>
    /// Lớp controller kho
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class WarehousesController : BaseController<WarehouseDto, Warehouse>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo controller kho
        /// </summary>
        /// <param name="service">Service kho</param>
        /// Created by: nlnhat (13/07/2023)
        public WarehousesController(IWarehouseService service) : base(service)
        {
        }
        #endregion
    }
}