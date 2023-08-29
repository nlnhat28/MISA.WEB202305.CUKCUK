using Microsoft.Extensions.Localization;

namespace MISA.CUKCUK.Domain
{
    /// <summary>
    /// Domain service check validate nghiệp vụ nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public class MaterialDomainService : IMaterialDomainService
    {
        #region Fields
        /// <summary>
        /// Repository nguyên vật liệu
        /// </summary>
        private readonly IMaterialRepository _repository;
        /// <summary>
        /// Resource lưu trữ thông báo
        /// </summary>
        private readonly IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        public MaterialDomainService(
            IMaterialRepository repository,
            IStringLocalizer<Resource> resource)
        {
            _repository = repository;
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check trùng mã nguyên vật liệu
        /// </summary>
        /// <param name="material">Entity nguyên vật liệu để check</param>
        /// <exception cref="ConflictException">Exception mã đã tồn tại</exception>
        /// Created by: nlnhat (17/08/2023)
        public async Task CheckDuplicatedCodeAsync(Material material)
        {
            var materialCode = material.MaterialCode;
            var materialExist = await _repository.GetByCodeAsync(materialCode);

            // Nếu trùng mã và trùng với nguyên vật liệu khác (tránh trường hợp trùng vs chính nguyên vật liệu đấy)
            if (materialExist != null && material?.MaterialId != materialExist?.MaterialId)
                throw new ConflictException(
                    MISAErrorCode.MaterialCodeDuplicated,
                    $"{_resource["MaterialCode"]} <{materialCode}> {_resource["Duplicated"]}",
                    new ExceptionData("MaterialCode", materialCode, ExceptionKey.FormItem, "FormItem"));
        }
        #endregion
    }
}
