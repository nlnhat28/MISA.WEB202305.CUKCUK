namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Giao diện validate nghiệp vụ nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public interface IMaterialDomainService
    {
        /// <summary>
        /// Check trùng mã nguyên vật liệu
        /// </summary>
        /// <param name="warehouse">Entity kho để check</param>
        /// Created by: nlnhat (17/08/2023)
        Task CheckDuplicatedCodeAsync(Material material);
    }
}
