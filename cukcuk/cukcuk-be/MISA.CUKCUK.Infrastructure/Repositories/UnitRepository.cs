using Dapper;
using MISA.CUKCUK.Domain;
using System.Data;
using static Dapper.SqlMapper;

namespace MISA.CUKCUK.Infrastructure
{
    /// <summary>
    /// Repository đơn vị tính
    /// </summary>
    /// <typeparam name="Unit">Entity đơn vị tính</typeparam>
    /// Created by: nlnhat (17/08/2023)
    public class UnitRepository : BaseRepository<Unit>, IUnitRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository đơn vị tính
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public UnitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy đơn vị tính theo tên
        /// </summary>
        /// <param name="name">Tên đơn vị tính</param>
        /// <returns>Đơn vị có tên tương ứng</returns>
        public async Task<Unit> GetByNameAsync(string name)
        {
            var proc = $"{Procedure}GetByName";

            var param = new DynamicParameters();
            param.Add($"p_{Table}Name", name);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Unit>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }
        #endregion
    }
}

