﻿using System.Data;
using Dapper;
using MISA.CUKCUK.Domain;
using Newtonsoft.Json;

namespace MISA.CUKCUK.Infrastructure
{
    /// <summary>
    /// Triển khai giao diện cơ sở repository chỉ đọc dữ liệu từ database
    /// </summary>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public abstract class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
    {
        #region Fields
        /// <summary>
        /// Unit of work
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        protected readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Tên bảng
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public virtual string Table { get; set; } = typeof(TEntity).Name;
        /// <summary>
        /// Tên cột khoá chính 
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public virtual string TableId { get; set; } = $"{typeof(TEntity).Name}Id";
        /// <summary>
        /// Tên stored procedure
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public virtual string Procedure { get; set; } = $"Proc_{typeof(TEntity).Name}_";
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo repository phòng ban
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (18/07/2023)
        public ReadOnlyRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var proc = $"{Procedure}GetAll";

            var result = await _unitOfWork.Connection.QueryAsync<TEntity>(
                proc, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
        /// <summary>
        /// Lấy đối tượng theo danh sách id
        /// </summary>
        /// <param name="ids">Danh sách id</param>
        /// <returns>Đối tượng có id trong danh sách</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<IEnumerable<TEntity>> GetManyAsync(IEnumerable<Guid> ids)
        {
            var proc = $"{Procedure}GetMany";

            var idsJson = JsonConvert.SerializeObject(ids);

            var param = new DynamicParameters();
            param.Add($"p_{TableId}s", idsJson);

            var result = await _unitOfWork.Connection.QueryAsync<TEntity>(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
        /// <summary>
        /// Lấy đối tượng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bản ghi có id được truy vấn</returns>
        /// Created by: nlnhat (16/08/2023)
        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            var proc = $"{Procedure}Get";

            var param = new DynamicParameters();
            param.Add($"p_{TableId}", id);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<TEntity>(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
        #endregion
    }
}
