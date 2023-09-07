using Microsoft.Extensions.Localization;
using MISA.CUKCUK.Domain;
using NSubstitute.ReturnsExtensions;

namespace MISA.CUKCUK.Application.Tests
{
    /// <summary>
    /// Unit test cho domain service nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (07/09/2023)
    [TestFixture]
    public class MaterialDoaminServiceTests
    {
        #region Properties
        /// <summary>
        /// Repository nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private IMaterialRepository _repository { get; set; }
        /// <summary>
        /// Resource
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private IStringLocalizer<Resource> _resource { get; set; }
        /// <summary>
        /// Domain service nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private MaterialDomainService _domainService { get; set; }
        #endregion

        #region SetUp
        /// <summary>
        /// Setup instance
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IMaterialRepository>();

            var unitDomainService = Substitute.For<IUnitDomainService>();
            var warehousementDomainService = Substitute.For<IWarehouseDomainService>();
            var conversionUnitDomainService = Substitute.For<IConversionUnitDomainService>();

            _resource = Substitute.For<IStringLocalizer<Resource>>();
            _domainService = new MaterialDomainService(
                _repository, unitDomainService, warehousementDomainService, conversionUnitDomainService, _resource);
        }
        #endregion

        #region Methods

        #region CheckDuplicatedCodeAsync
        /// <summary>
        /// Unit test check trùng code (trường hợp code bị trùng)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckDuplicatedCodeAsync_ExistMaterial_ThrowException()
        {
            // Arrange
            var materialCheck = new Material() { MaterialId = Guid.NewGuid(), MaterialCode = "NL102" };
            var idCheck = materialCheck.MaterialId;
            var codeCheck = materialCheck.MaterialCode;

            var materialExist = new Material() { MaterialId = Guid.NewGuid() };

            _repository.GetByCodeAsync(codeCheck).Returns(materialExist);

            var expectedUserMsg = $"{_resource["MaterialCode"]} <{codeCheck}> {_resource["Duplicated"]}" ?? string.Empty;

            // Act
            var actualException = Assert.ThrowsAsync<ConflictException>(async ()
                => await _domainService.CheckDuplicatedCodeAsync(idCheck, codeCheck));

            var actualUserMsg = actualException.UserMsg ?? string.Empty;

            // Assert
            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

            await _repository.Received(1).GetByCodeAsync(codeCheck);
        }
        /// <summary>
        /// Unit test check trùng code (trường hợp thành công do code không trùng)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckDuplicatedCodeAsync_NotExistMaterial_Success()
        {
            // Arrange
            var materialCheck = new Material() { MaterialId = Guid.NewGuid(), MaterialCode = "NL102" };
            var idCheck = materialCheck.MaterialId;
            var codeCheck = materialCheck.MaterialCode;

            _repository.GetByCodeAsync(codeCheck).ReturnsNull();

            // Act
            await _domainService.CheckDuplicatedCodeAsync(idCheck, codeCheck);

            // Assert
            await _repository.Received(1).GetByCodeAsync(codeCheck);
        }
        /// <summary>
        /// Unit test check trùng code (trường hợp thành công do code trùng với chính nguyên vật liệu đó)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckDuplicatedCodeAsync_ExistThisMaterial_Success()
        {
            // Arrange
            var materialCheck = new Material() { MaterialId = Guid.NewGuid(), MaterialCode = "NL102" };
            var idCheck = materialCheck.MaterialId;
            var codeCheck = materialCheck.MaterialCode;

            _repository.GetByCodeAsync(codeCheck).Returns(materialCheck);

            // Act
            await _domainService.CheckDuplicatedCodeAsync(idCheck, codeCheck);

            // Assert
            await _repository.Received(1).GetByCodeAsync(codeCheck);
        }
        #endregion

        #region CheckRangeCodeAsync
        /// <summary>
        /// Unit test check code trong khoảng cho phép (trường hợp code ngoài khoảng)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckRangeCodeAsync_OutOfRangeCode_ThrowException()
        {
            // Arrange
            var prefixCode = "NL";
            var codeNumber = 15000;
            var maxCode = 1000;
            var limitCode = maxCode + MaterialConstant.OffsetCode;

            _repository.GetMaxCodeAsync(prefixCode).Returns(maxCode);

            var expectedUserMsg = $"{_resource["MaterialCodeLessThan"]} {++limitCode}";

            // Act
            var actualException = Assert.ThrowsAsync<ValidateException>(async ()
                => await _domainService.CheckRangeCodeAsync(prefixCode, codeNumber));

            var actualUserMsg = actualException.UserMsg ?? string.Empty;

            // Assert
            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

            await _repository.Received(1).GetMaxCodeAsync(prefixCode);
        }
        /// <summary>
        /// Unit test check code trong khoảng cho phép (trường hợp thành công)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckRangeCodeAsync_ValidCode_Success()
        {
            // Arrange
            var prefixCode = "NL";
            var codeNumber = 1002;
            var maxCode = 1000;

            _repository.GetMaxCodeAsync(prefixCode).Returns(maxCode);

            // Act
            await _domainService.CheckRangeCodeAsync(prefixCode, codeNumber);

            // Assert
            await _repository.Received(1).GetMaxCodeAsync(prefixCode);
        }
        #endregion

        #endregion
    }
}
