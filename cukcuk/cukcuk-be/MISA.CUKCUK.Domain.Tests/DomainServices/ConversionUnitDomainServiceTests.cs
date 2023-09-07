using Microsoft.Extensions.Localization;
using MISA.CUKCUK.Domain;
using NSubstitute.ReturnsExtensions;

namespace MISA.CUKCUK.Application.Tests
{
    /// <summary>
    /// Unit test cho domain service đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (07/09/2023)
    [TestFixture]
    public class ConversionUnitDomainServiceTests
    {
        #region Properties
        /// <summary>
        /// Repository đơn vị chuyển đổi
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private IConversionUnitRepository _repository { get; set; }
        /// <summary>
        /// Resource
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private IStringLocalizer<Resource> _resource { get; set; }
        /// <summary>
        /// Domain service đơn vị chuyển đổi
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private ConversionUnitDomainService _domainService { get; set; }
        #endregion

        #region SetUp
        /// <summary>
        /// Setup instance
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IConversionUnitRepository>();
            var unitDomainService = Substitute.For<IUnitDomainService>();
            _resource = Substitute.For<IStringLocalizer<Resource>>();
            _domainService = new ConversionUnitDomainService(_repository, unitDomainService, _resource);
        }
        #endregion

        #region Methods

        #region CheckExistConversionUnitsAsync/// <summary>
        /// Unit test check tồn tại danh sách đơn vị chuyển đổi (trường hợp 10 ids không tồn tại hết)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistConversionUnitsAsync_10Ids_ThrowException()
        {
            // Arrange
            // Danh sách 10 ids
            var ids = new List<Guid>();
            for (int i = 0; i < 10; i++)
            {
                ids.Add(Guid.NewGuid());
            }
            // Danh sách 10 đơn vị chuyển đổi có id không nằm trong danh sách 10 ids trên
            var conversionUnits = new List<ConversionUnit>();
            var materialId = Guid.NewGuid();

            for (int i = 0; i < 10; i++)
            {
                conversionUnits.Add(new ConversionUnit()
                {
                    ConversionUnitId = Guid.NewGuid(),
                    MaterialId = materialId
                });
            }

            _repository.GetManyAsync(ids).Returns(conversionUnits);

            var expectedUserMsg = _resource["ConversionUnitNotFound"] ?? string.Empty;

            // Act
            var actualException = Assert.ThrowsAsync<NotFoundException>(async ()
                => await _domainService.CheckExistConversionUnitsAsync(ids, materialId));

            var actualUserMsg = actualException.UserMsg ?? string.Empty;

            // Assert
            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

            await _repository.Received(1).GetManyAsync(ids);
        }
        /// <summary>
        /// Unit test check tồn tại danh sách đơn vị chuyển đổi (trường hợp 10 ids, 10 id tồn tại, nhưng sai MaterialId)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistConversionUnitsAsync_Exist10Ids_ThrowException()
        {
            // Arrange
            // Danh sách 10 ids
            var ids = new List<Guid>();
            for (int i = 0; i < 10; i++)
            {
                ids.Add(Guid.NewGuid());
            }
            // Danh sách 10 đơn vị chuyển đổi có id nằm trong danh sách 10 ids trên nhưng sai MaterialId
            var conversionConversionUnits = new List<ConversionUnit>();
            var materialId = Guid.NewGuid();

            for (int i = 0; i < 10; i++)
            {
                conversionConversionUnits.Add(new ConversionUnit()
                {
                    ConversionUnitId = ids[i],
                    MaterialId = Guid.NewGuid()
                });
            }

            _repository.GetManyAsync(ids).Returns(conversionConversionUnits);

            var expectedUserMsg = _resource["ConversionUnitNotFound"] ?? string.Empty;

            // Act
            var actualException = Assert.ThrowsAsync<NotFoundException>(async ()
                => await _domainService.CheckExistConversionUnitsAsync(ids, materialId));

            var actualUserMsg = actualException.UserMsg ?? string.Empty;

            // Assert
            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

            await _repository.Received(1).GetManyAsync(ids);
        }
        /// <summary>
        /// Unit test check tồn tại danh sách đơn vị chuyển đổi (trường hợp 10 ids, tồn tại 10/10)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistConversionUnitsAsync_10Ids_Success()
        {
            // Arrange
            // Danh sách 10 ids
            var ids = new List<Guid>();
            for (int i = 0; i < 10; i++)
            {
                ids.Add(Guid.NewGuid());
            }
            // Danh sách 10 đơn vị chuyển đổi có id nằm hết trong danh sách 10 ids trên
            var conversionUnits = new List<ConversionUnit>();
            var materialId = Guid.NewGuid();

            for (int i = 0; i < 10; i++)
            {
                conversionUnits.Add(new ConversionUnit()
                {
                    ConversionUnitId = ids[i],
                    MaterialId = materialId
                });
            }

            _repository.GetManyAsync(ids).Returns(conversionUnits);

            // Act
            await _domainService.CheckExistConversionUnitsAsync(ids, materialId);

            // Assert
            await _repository.Received(1).GetManyAsync(ids);
        }
        #endregion

        #endregion
    }
}
