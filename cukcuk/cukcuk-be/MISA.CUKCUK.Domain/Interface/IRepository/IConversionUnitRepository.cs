﻿namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Giao diện repository đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public interface IConversionUnitRepository : IBaseRepository<ConversionUnit>
    {
        /// <summary>
        /// Lấy danh sách id đơn vị chuyển đổi theo nguyên vật liệu
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Danh sách id đơn vi chuyển đổi</returns>
        /// Created by: nlnhat (30/08/2023)
        Task<IEnumerable<Guid>> GetByMaterialId(Guid materialId);
    }
}
