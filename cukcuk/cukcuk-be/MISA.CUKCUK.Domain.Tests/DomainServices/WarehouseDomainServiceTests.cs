using Microsoft.Extensions.Localization;
using MISA.CUKCUK.Domain;
using NSubstitute.ReturnsExtensions;

namespace MISA.CUKCUK.Application.Tests
{
    /// <summary>
    /// Unit test cho domain service nhà kho
    /// </summary>
    /// Created by: nlnhat (07/09/2023)
    [TestFixture]
    public class WarehouseDomainServiceTests
    {
        #region Properties
        /// <summary>
        /// Repository nhà kho
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private IWarehouseRepository _repository { get; set; }
        /// <summary>
        /// Resource
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private IStringLocalizer<Resource> _resource { get; set; }
        /// <summary>
        /// Domain service nhà kho
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private WarehouseDomainService _domainService { get; set; }
        #endregion

        #region SetUp
        /// <summary>
        /// Setup instance
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IWarehouseRepository>();
            _resource = Substitute.For<IStringLocalizer<Resource>>();
            _domainService = new WarehouseDomainService(_repository, _resource);
        }
        #endregion

        #region Methods

        #region CheckExistWarehouseAsync
        /// <summary>
        /// Unit test check tồn tại nhà kho (trường hợp không tồn tại)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistWarehouseAsync_NotExistWarehouse_ThrowException()
        {
            // Arrange
            var warehouseId = Guid.NewGuid();

            _repository.GetAsync(warehouseId).ReturnsNull();

            var expectedUserMsg = _resource["WarehouseNotFound"] ?? string.Empty;

            // Act
            var actualException = Assert.ThrowsAsync<NotFoundException>(async ()
                => await _domainService.CheckExistWarehouseAsync(warehouseId));

            var actualUserMsg = actualException.UserMsg ?? string.Empty;

            // Assert
            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

            await _repository.Received(1).GetAsync(warehouseId);
        }
        /// <summary>
        /// Unit test check tồn tại nhà kho (trường hợp tồn tại)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistWarehouseAsync_ExistWarehouse_Success()
        {
            // Arrange
            var warehouseId = Guid.NewGuid();

            _repository.GetAsync(warehouseId).Returns(new Warehouse());

            // Act
            await _domainService.CheckExistWarehouseAsync(warehouseId);

            // Assert
            await _repository.Received(1).GetAsync(warehouseId);
        }
        #endregion

        #region CheckDuplicatedCodeAsync
        /// <summary>
        /// Unit test check trùng code (trường hợp code bị trùng)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckDuplicatedCodeAsync_ExistWarehouse_ThrowException()
        {
            // Arrange
            var warehouseCheck = new Warehouse() { WarehouseId = Guid.NewGuid(), WarehouseCode = "KH102" };
            var idCheck = warehouseCheck.WarehouseId;
            var codeCheck = warehouseCheck.WarehouseCode;

            var warehouseExist = new Warehouse() { WarehouseId = Guid.NewGuid() };

            _repository.GetByCodeAsync(codeCheck).Returns(warehouseExist);

            var expectedUserMsg = $"{_resource["WarehouseCode"]} <{codeCheck}> {_resource["Duplicated"]}" ?? string.Empty;

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
        public async Task CheckDuplicatedCodeAsync_NotExistWarehouse_Success()
        {
            // Arrange
            var warehouseCheck = new Warehouse() { WarehouseId = Guid.NewGuid(), WarehouseCode = "KH102" };
            var idCheck = warehouseCheck.WarehouseId;
            var codeCheck = warehouseCheck.WarehouseCode;

            _repository.GetByCodeAsync(codeCheck).ReturnsNull();

            // Act
            await _domainService.CheckDuplicatedCodeAsync(idCheck, codeCheck);

            // Assert
            await _repository.Received(1).GetByCodeAsync(codeCheck);
        }
        /// <summary>
        /// Unit test check trùng code (trường hợp thành công do code trùng với chính nhà kho đó)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckDuplicatedCodeAsync_ExistThisWarehouse_Success()
        {
            // Arrange
            var warehouseCheck = new Warehouse() { WarehouseId = Guid.NewGuid(), WarehouseCode = "KH102" };
            var idCheck = warehouseCheck.WarehouseId;
            var codeCheck = warehouseCheck.WarehouseCode;

            _repository.GetByCodeAsync(codeCheck).Returns(warehouseCheck);

            // Act
            await _domainService.CheckDuplicatedCodeAsync(idCheck, codeCheck);

            // Assert
            await _repository.Received(1).GetByCodeAsync(codeCheck);
        }
        #endregion

        #endregion
    }
}
