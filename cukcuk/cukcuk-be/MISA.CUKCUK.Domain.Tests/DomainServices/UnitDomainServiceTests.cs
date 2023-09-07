using Microsoft.Extensions.Localization;
using MISA.CUKCUK.Domain;
using NSubstitute.ReturnsExtensions;

namespace MISA.CUKCUK.Application.Tests
{
    /// <summary>
    /// Unit test cho domain service đơn vị tính
    /// </summary>
    /// Created by: nlnhat (07/09/2023)
    [TestFixture]
    public class UnitDomainServiceTests
    {
        #region Properties
        /// <summary>
        /// Repository đơn vị tính
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private IUnitRepository _repository { get; set; }
        /// <summary>
        /// Resource
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private IStringLocalizer<Resource> _resource { get; set; }
        /// <summary>
        /// Domain service đơn vị tính
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        private UnitDomainService _domainService { get; set; }
        #endregion

        #region SetUp
        /// <summary>
        /// Setup instance
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IUnitRepository>();
            _resource = Substitute.For<IStringLocalizer<Resource>>();
            _domainService = new UnitDomainService(_repository, _resource);
        }
        #endregion

        #region Methods

        #region CheckExistUnitAsync
        /// <summary>
        /// Unit test check tồn tại đơn vị tính (trường hợp không tồn tại)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistUnitAsync_NotExistUnit_ThrowException()
        {
            // Arrange
            var unitId = Guid.NewGuid();

            _repository.GetAsync(unitId).ReturnsNull();

            var expectedUserMsg = _resource["UnitNotFound"] ?? string.Empty;

            // Act
            var actualException = Assert.ThrowsAsync<NotFoundException>(async ()
                => await _domainService.CheckExistUnitAsync(unitId));

            var actualUserMsg = actualException.UserMsg ?? string.Empty;

            // Assert
            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

            await _repository.Received(1).GetAsync(unitId);
        }
        /// <summary>
        /// Unit test check tồn tại đơn vị tính (trường hợp tồn tại)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistUnitAsync_ExistUnit_Success()
        {
            // Arrange
            var unitId = Guid.NewGuid();

            _repository.GetAsync(unitId).Returns(new Unit());

            // Act
            await _domainService.CheckExistUnitAsync(unitId);

            // Assert
            await _repository.Received(1).GetAsync(unitId);
        }
        #endregion

        #region CheckExistUnitsAsync
        /// <summary>
        /// Unit test check tồn tại danh sách đơn vị tính (trường hợp 10 ids, có id không tồn tại)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistUnitsAsync_10Ids_ThrowException()
        {
            // Arrange
            // Danh sách 10 ids
            var ids = new List<Guid>();
            for (int i = 0; i < 10; i++)
            {
                ids.Add(Guid.NewGuid());
            }
            // Danh sách 10 đơn vị tính có id không nằm trong danh sách 10 ids trên
            var units = new List<Unit>();
            for (int i = 0; i < 10; i++)
            {
                units.Add(new Unit()
                {
                    UnitId = Guid.NewGuid()
                });
            }
            _repository.GetManyAsync(ids).Returns(units);
            var expectedUserMsg = _resource["UnitNotFound"] ?? string.Empty;

            // Act
            var actualException = Assert.ThrowsAsync<NotFoundException>(async ()
                => await _domainService.CheckExistUnitsAsync(ids));

            var actualUserMsg = actualException.UserMsg ?? string.Empty;

            // Assert
            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

            await _repository.Received(1).GetManyAsync(ids);
        }
        /// <summary>
        /// Unit test check tồn tại danh sách đơn vị tính (trường hợp 10 ids, tồn tại 10/10)
        /// </summary>
        /// Created by: nlnhat (07/09/2023) 
        [Test]
        public async Task CheckExistUnitsAsync_10Ids_Success()
        {
            // Arrange
            // Danh sách 10 ids
            var ids = new List<Guid>();
            for (int i = 0; i < 10; i++)
            {
                ids.Add(Guid.NewGuid());
            }
            // Danh sách 10 đơn vị tính có id nằm hết trong danh sách 10 ids trên
            var units = new List<Unit>();
            for (int i = 0; i < 10; i++)
            {
                units.Add(new Unit()
                {
                    UnitId = ids[i]
                });
            }
            _repository.GetManyAsync(ids).Returns(units);

            // Act
            await _domainService.CheckExistUnitsAsync(ids);

            // Assert
            await _repository.Received(1).GetManyAsync(ids);
        }
        #endregion

        #region CheckDuplicatedNameAsync
        /// <summary>
        /// Unit test check trùng tên (trường hợp tên bị trùng)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckDuplicatedNameAsync_ExistUnit_ThrowException()
        {
            // Arrange
            var unitCheck = new Unit() { UnitId = Guid.NewGuid(), UnitName = "Kg" };
            var idCheck = unitCheck.UnitId;
            var nameCheck = unitCheck.UnitName;

            var unitExist = new Unit() { UnitId = Guid.NewGuid() };

            _repository.GetByNameAsync(nameCheck).Returns(unitExist);

            var expectedUserMsg = $"{_resource["UnitName"]} <{nameCheck}> {_resource["Duplicated"]}" ?? string.Empty;

            // Act
            var actualException = Assert.ThrowsAsync<ConflictException>(async ()
                => await _domainService.CheckDuplicatedNameAsync(idCheck, nameCheck));

            var actualUserMsg = actualException.UserMsg ?? string.Empty;

            // Assert
            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

            await _repository.Received(1).GetByNameAsync(nameCheck);
        }
        /// <summary>
        /// Unit test check trùng tên (trường hợp thành công do tên không trùng)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckDuplicatedNameAsync_NotExistUnit_Success()
        {
            // Arrange
            var unitCheck = new Unit() { UnitId = Guid.NewGuid(), UnitName = "Kg" };
            var idCheck = unitCheck.UnitId;
            var nameCheck = unitCheck.UnitName;

            _repository.GetByNameAsync(nameCheck).ReturnsNull();

            // Act
            await _domainService.CheckDuplicatedNameAsync(idCheck, nameCheck);

            // Assert
            await _repository.Received(1).GetByNameAsync(nameCheck);
        }
        /// <summary>
        /// Unit test check trùng tên (trường hợp thành công do tên trùng với chính đơn vị tính đó)
        /// </summary>
        /// Created by: nlnhat (07/09/2023)
        [Test]
        public async Task CheckDuplicatedNameAsync_ExistThisUnit_Success()
        {
            // Arrange
            var unitCheck = new Unit() { UnitId = Guid.NewGuid(), UnitName = "Kg" };
            var idCheck = unitCheck.UnitId;
            var nameCheck = unitCheck.UnitName;

            _repository.GetByNameAsync(nameCheck).Returns(unitCheck);

            // Act
            await _domainService.CheckDuplicatedNameAsync(idCheck, nameCheck);

            // Assert
            await _repository.Received(1).GetByNameAsync(nameCheck);
        }
        #endregion

        #endregion
    }
}
