//using Microsoft.Extensions.Localization;
//using MISA.CUKCUK.Domain;
//using NSubstitute.ReturnsExtensions;

//namespace MISA.CUKCUK.Application.Tests
//{
//    /// <summary>
//    /// Unit test cho domain service nhân viên
//    /// </summary>
//    /// Created by: nlnhat (24/07/2023)
//    [TestFixture]
//    public class EmployeeDoaminServiceTests
//    {
//        #region Properties
//        /// <summary>
//        /// Repository nhân viên
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IEmployeeRepository _repository { get; set; }
//        /// <summary>
//        /// Domain service phòng ban
//        /// </summary>
//        /// Created by: nlnhat (18/07/2023)
//        private IUnitDomainService _departmentDomainService;
//        /// <summary>
//        /// Resource
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IStringLocalizer<Resource> _resource { get; set; }
//        /// <summary>
//        /// Domain service nhân viên
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private EmployeeDomainService _domainService { get; set; }
//        #endregion

//        #region SetUp
//        /// <summary>
//        /// Setup instance
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [SetUp]
//        public void SetUp()
//        {
//            _repository = Substitute.For<IEmployeeRepository>();
//            _departmentDomainService = Substitute.For<IUnitDomainService>(); 
//            _resource = Substitute.For<IStringLocalizer<Resource>>();
//            _domainService = new EmployeeDomainService(_repository, _departmentDomainService, _resource);
//        }
//        #endregion

//        #region Methods

//        #region CheckDuplicatedCodeAsync
//        /// <summary>
//        /// Unit test check trùng code (trường hợp code bị trùng)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task CheckDuplicatedCodeAsync_ExistEmployee_ThrowException()
//        {
//            // Arrange
//            var employeeCheck = new Employee() { EmployeeId = Guid.NewGuid(), EmployeeCode = "NV-1234" };
//            var codeCheck = employeeCheck.EmployeeCode;

//            var employeeExist = new Employee() { EmployeeId = Guid.NewGuid() };

//            _repository.GetAsync(codeCheck).Returns(employeeExist);

//            var expectedUserMsg = _resource["EmployeeCodeDuplicated"] ?? string.Empty;

//            // Act
//            var actualException = Assert.ThrowsAsync<ConflictException>(async () 
//                => await _domainService.CheckDuplicatedCodeAsync(employeeCheck));

//            var actualUserMsg = actualException.UserMsg ?? string.Empty;

//            // Assert
//            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

//            await _repository.Received(1).GetAsync(codeCheck);
//        }
//        /// <summary>
//        /// Unit test check trùng code (trường hợp thành công do code không trùng)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task CheckDuplicatedCodeAsync_NotExistEmployee_Success()
//        {
//            // Arrange
//            var employeeCheck = new Employee() { EmployeeId = Guid.NewGuid(), EmployeeCode = "NV-1234" };
//            var codeCheck = employeeCheck.EmployeeCode;

//            _repository.GetAsync(codeCheck).ReturnsNull();

//            // Act
//            await _domainService.CheckDuplicatedCodeAsync(employeeCheck);

//            // Assert
//            await _repository.Received(1).GetAsync(codeCheck);
//        }
//        /// <summary>
//        /// Unit test check trùng code (trường hợp thành công do code trùng với chính nhân viên đó)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task CheckDuplicatedCodeAsync_ExistThisEmployee_Success()
//        {
//            // Arrange
//            var employeeCheck = new Employee() { EmployeeId = Guid.NewGuid(), EmployeeCode = "NV-1234" };
//            var codeCheck = employeeCheck.EmployeeCode;

//            _repository.GetAsync(codeCheck).Returns(employeeCheck);

//            // Act
//            await _domainService.CheckDuplicatedCodeAsync(employeeCheck);

//            // Assert
//            await _repository.Received(1).GetAsync(codeCheck);
//        }
//        #endregion

//        #region CheckRangeCodeAsync
//        /// <summary>
//        /// Unit test check code trong khoảng cho phép (trường hợp sai format code)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public void CheckRangeCodeAsync_WrongFormatCode_ThrowException()
//        {
//            // Arrange
//            var codeCheck = "NV-ABC?";
//            var expectedUserMsg = $"{_resource["EmployeeCodeFormat"]} {MaterialConstant.PrefixCode}{_resource["Number"]}";

//            // Act
//            var actualException = Assert.ThrowsAsync<ValidateException>(async ()
//                => await _domainService.CheckRangeCodeAsync(codeCheck));

//            var actualUserMsg = actualException.UserMsg ?? string.Empty;

//            // Assert
//            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));
//        }
//        /// <summary>
//        /// Unit test check code trong khoảng cho phép (trường hợp code ngoài khoảng)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task CheckRangeCodeAsync_OutOfRangeCode_ThrowException()
//        {
//            // Arrange
//            var codeCheck = "NV-15000";
//            var maxCode = 1000;
//            var limitCode = maxCode + MaterialConstant.OffsetCode;

//            _repository.GetMaxCodeAsync().Returns(maxCode);

//            var expectedUserMsg = $"{_resource["EmployeeCodeLessThan"]} {++limitCode}";

//            // Act
//            var actualException = Assert.ThrowsAsync<ValidateException>(async ()
//                => await _domainService.CheckRangeCodeAsync(codeCheck));

//            var actualUserMsg = actualException.UserMsg ?? string.Empty;

//            // Assert
//            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

//            await _repository.Received(1).GetMaxCodeAsync();
//        }
//        /// <summary>
//        /// Unit test check code trong khoảng cho phép (trường hợp thành công)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task CheckRangeCodeAsync_ValidCode_Success()
//        {
//            // Arrange
//            var codeCheck = "NV-1100";
//            var maxCode = 1000;

//            _repository.GetMaxCodeAsync().Returns(maxCode);

//            // Act
//            await _domainService.CheckRangeCodeAsync(codeCheck);

//            // Assert
//            await _repository.Received(1).GetMaxCodeAsync();
//        }
//        #endregion

//        #endregion
//    }
//}
