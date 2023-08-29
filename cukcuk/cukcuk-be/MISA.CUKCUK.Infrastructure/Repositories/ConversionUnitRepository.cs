using MISA.CUKCUK.Domain;

namespace MISA.CUKCUK.Infrastructure
{
    /// <summary>
    /// Repository đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ConversionUnitRepository : BaseRepository<ConversionUnit>, IConversionUnitRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository đơn vị chuyển đổi
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ConversionUnitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion
    }
}

