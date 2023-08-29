namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Giao diện validate nghiệp vụ đơn vị
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public interface IUnitDomainService
    {
        /// <summary>
        /// Check trùng tên đơn vị
        /// </summary>
        /// <param name="unit">Entity đơn vị để check</param>
        /// Created by: nlnhat (17/08/2023)
        Task CheckDuplicatedNameAsync(Unit unit);
    }
}
