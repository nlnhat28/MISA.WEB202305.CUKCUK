namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Giao diện repository đơn vị tính
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public interface IUnitRepository : IBaseRepository<Unit>
    {
        /// <summary>
        /// Lấy đơn vị tính theo tên
        /// </summary>
        /// <param name="unitName">Tên của đơn vị tính</param>
        /// <returns>Đơn vị tính có tên trùng với param</returns>
        /// Created by: nlnhat (16/08/2023)
        Task<Unit> GetByNameAsync(string unitName);
    }
}