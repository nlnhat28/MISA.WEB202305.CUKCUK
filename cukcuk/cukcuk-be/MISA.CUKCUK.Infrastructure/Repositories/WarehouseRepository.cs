using Dapper;
using MISA.CUKCUK.Domain;
using System.Data;
using static Dapper.SqlMapper;

namespace MISA.CUKCUK.Infrastructure
{
    /// <summary>
    /// Repository kho
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository kho
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public WarehouseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy kho theo mã
        /// </summary>
        /// <param name="code">Mã kho</param>
        /// <returns>Kho có mã tương ứng</returns>
        public async Task<Warehouse> GetByCodeAsync(string code)
        {
            var proc = $"{Procedure}GetByCode";

            var param = new DynamicParameters();
            param.Add($"p_{Table}Code", code);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Warehouse>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }
        #endregion
    }
}

