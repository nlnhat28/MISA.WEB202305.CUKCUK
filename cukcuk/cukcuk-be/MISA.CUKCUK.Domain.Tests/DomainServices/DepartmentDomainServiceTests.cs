//using Microsoft.Extensions.Localization;
//using MISA.CUKCUK.Domain;
//using NSubstitute.ReturnsExtensions;

//namespace MISA.CUKCUK.Application.Tests
//{
//    /// <summary>
//    /// Unit test cho domain service phòng ban
//    /// </summary>
//    /// Created by: nlnhat (24/07/2023)
//    [TestFixture]
//    public class DepartmentDomainServiceTests
//    {
//        #region Properties
//        /// <summary>
//        /// Repository phòng ban
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IDepartmentRepository _repository { get; set; }
//        /// <summary>
//        /// Resource
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IStringLocalizer<Resource> _resource { get; set; }
//        /// <summary>
//        /// Domain service phòng ban
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private DepartmentDomainService _domainService { get; set; }
//        #endregion

//        #region SetUp
//        /// <summary>
//        /// Setup instance
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [SetUp]
//        public void SetUp()
//        {
//            _repository = Substitute.For<IDepartmentRepository>();
//            _resource = Substitute.For<IStringLocalizer<Resource>>();
//            _domainService = new DepartmentDomainService(_repository, _resource);
//        }
//        #endregion

//        #region Methods

//        #region CheckExistDepartmentAsync
//        /// <summary>
//        /// Unit test check tồn tại phòng ban (trường hợp không tồn tại)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023) 
//        [Test]
//        public async Task CheckExistDepartmentAsync_NotExistDepartment_ThrowException()
//        {
//            // Arrange
//            var departmentId = Guid.NewGuid();

//            _repository.GetAsync(departmentId).ReturnsNull();

//            var expectedUserMsg = _resource["DepartmentNotFound"] ?? string.Empty;

//            // Act
//            var actualException = Assert.ThrowsAsync<NotFoundException>(async ()
//                => await _domainService.CheckExistDepartmentAsync(departmentId));

//            var actualUserMsg = actualException.UserMsg ?? string.Empty;

//            // Assert
//            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

//            await _repository.Received(1).GetAsync(departmentId);
//        }
//        /// <summary>
//        /// Unit test check tồn tại phòng ban (trường hợp tồn tại)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023) 
//        [Test]
//        public async Task CheckExistDepartmentAsync_ExistDepartment_Success()
//        {
//            // Arrange
//            var departmentId = Guid.NewGuid();

//            _repository.GetAsync(departmentId).Returns(new Department());

//            // Act
//             await _domainService.CheckExistDepartmentAsync(departmentId);

//            // Assert
//            await _repository.Received(1).GetAsync(departmentId);
//        }
//        #endregion

//        #endregion
//    }
//}
