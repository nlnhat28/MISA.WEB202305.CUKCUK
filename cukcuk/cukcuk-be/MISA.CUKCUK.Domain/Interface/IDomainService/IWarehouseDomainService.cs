namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Giao diện validate nghiệp vụ nhà kho
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public interface IWarehouseDomainService
    {
        /// <summary>
        /// Check trùng mã kho
        /// </summary>
        /// <param name="warehouse">Entity kho để check</param>
        /// Created by: nlnhat (17/08/2023)
        Task CheckDuplicatedCodeAsync(Warehouse warehouse);
    }
}
