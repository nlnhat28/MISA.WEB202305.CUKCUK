﻿namespace MISA.CUKCUK.Domain
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
        /// <param name="materialId">Id nguyên vật liệu để check</param>
        /// <param name="materialCode">Mã nguyên vật kiệu để check</param>
        /// Created by: nlnhat (17/08/2023)
        Task CheckDuplicatedCodeAsync(Guid materialId, string materialCode);
        /// <summary>
        /// Check tồn tại đơn vị tính hay không
        /// </summary>
        /// <param name="unitId">Id đơn vị tính để check</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistUnitAsync(Guid unitId);
        /// <summary>
        /// Check tồn tại kho hay không
        /// </summary>
        /// <param name="warehouseId">Id nhà kho để check</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistWarehouseAsync(Guid warehouseId);
        /// <summary>
        /// Check tồn tại danh sách đơn vị chuyển đổi
        /// </summary>
        /// <param name="conversionUnitIds">Danh sách id đơn vị chuyển đổi</param>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistConversionUnitsAsync(List<Guid> conversionUnitIds, Guid materialId);
        /// <summary>
        /// Check tồn tại danh sách đơn vị muốn chuyển đổi
        /// </summary>
        /// <param name="destinationUnitIds">Danh sách id đơn vị muốn chuyển đổi</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistDestinationUnitsAsync(List<Guid> destinationUnitIds);
    }
}
