using MISA.CUKCUK.Domain;
using System.ComponentModel.DataAnnotations;

namespace MISA.CUKCUK.Application
{
    /// <summary>
    /// Lớp dto nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public class MaterialDto : BaseDto
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Required]
        public Guid MaterialId { get; set; }
        /// <summary>
        /// Nã nguyên vật liệu
        /// </summary>
        /// </summary>
        [Required]
        [StringLength(20)]
        public string MaterialCode { get; set; }
        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        [Required]
        [StringLength(255)]
        public string MaterialName { get; set; }
        /// <summary>
        /// Khoá ngoại tham chiếu bảng đơn vị tính
        /// </summary>
        [Required]
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        public string? UnitName { get; set; }
        /// <summary>
        /// Khoá ngoại tham chiếu bảng nhà kho
        /// </summary>
        public Guid? WarehouseId { get; set; }
        /// <summary>
        /// Mã kho
        /// </summary>
        public string? WarehouseCode { get; set; }
        /// <summary>
        /// Tên kho
        /// </summary>
        public string? WarehouseName { get; set; }
        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public int? ExpiryTime { get; set; }
        /// <summary>
        /// Đơn vị thời gian hết hạn
        /// </summary>
        public TimeUnit? TimeUnit { get; set; }
        /// <summary>
        /// Tên đơn vị thời gian
        /// </summary>
        public string? TimeUnitName { get; set; }
        /// <summary>
        /// Số lượng tồn tối thiểu
        /// </summary>
        public decimal? MinimumInventory { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        [StringLength(255)]
        public string? Note { get; set; }
        /// <summary>
        /// Ngừng theo dõi hay không
        /// </summary>
        public bool? IsUnfollow { get; set; }
        /// <summary>
        /// Danh sách đơn vị chuyển đổi
        /// </summary>
        public List<ConversionUnitDto>? ConversionUnits { get; set; }
    }
}
