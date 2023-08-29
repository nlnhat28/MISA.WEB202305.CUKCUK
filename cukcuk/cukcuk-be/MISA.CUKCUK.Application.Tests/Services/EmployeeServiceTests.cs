//using AutoMapper;
//using Microsoft.Extensions.Localization;
//using MISA.CUKCUK.Domain;

//namespace MISA.CUKCUK.Application.Tests
//{
//    /// <summary>
//    /// Unit test cho employee service
//    /// </summary>
//    /// Created by: nlnhat (24/07/2023)
//    [TestFixture]
//    public class EmployeeServiceTests
//    {
//        #region Properties
//        /// <summary>
//        /// Repository nhân viên
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IEmployeeRepository _repository { get; set; }
//        /// <summary>
//        /// Repository nhân viên
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IEmployeeDomainService _domainService { get; set; }
//        /// <summary>
//        /// Repository nhân viên
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IMapper _mapper { get; set; }
//        /// <summary>
//        /// Resource
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IStringLocalizer<Resource> _resource { get; set; }
//        /// <summary>
//        /// Unit of work
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IUnitOfWork _unitOfWork { get; set; }
//        /// <summary>
//        /// Service nhân viên
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private EmployeeService _service { get; set; }
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
//            _domainService = Substitute.For<IEmployeeDomainService>();
//            _mapper = Substitute.For<IMapper>();
//            _resource = Substitute.For<IStringLocalizer<Resource>>();
//            _unitOfWork = Substitute.For<IUnitOfWork>();
//            _service = new EmployeeService(_repository, _domainService, _resource, _mapper, _unitOfWork);
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// Unit test xoá nhiều (danh sách id rỗng)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public void DeleteManyAsync_EmptyIds_ThrowValidateException()
//        {
//            // Arrange
//            var ids = new List<Guid>();
//            var expectedUserMsg = _resource["Parameterless"] ?? string.Empty;

//            // Act
//            var actualException = Assert.ThrowsAsync<ValidateException>(async () => await _service.DeleteManyAsync(ids));
//            var actualUserMsg = actualException.UserMsg ?? string.Empty;

//            // Assert
//            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));
//        }
//        /// <summary>
//        /// Unit test xoá nhiều (danh sách 10 ids, không tìm được hết)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task DeleteManyAsync_10Ids_ThrowNotFoundException()
//        {
//            // Arrange

//            // List 10 ids
//            var ids = new List<Guid>();
//            for (int i = 0; i < 10;  i++)
//            {
//                ids.Add(Guid.NewGuid());
//            }

//            // List 8 employees
//            var employees = new List<Employee>();
//            for (int i = 0; i < 8; i++)
//            {
//                employees.Add(new Employee());
//            }

//            _repository.GetManyAsync(ids).Returns(employees);

//            var expectedUserMsg = _resource["NotFoundAll"] ?? string.Empty;

//            // Act
//            var actualException = Assert.ThrowsAsync<NotFoundException>(async () => await _service.DeleteManyAsync(ids));
//            var actualUserMsg = actualException.UserMsg ?? string.Empty;

//            // Assert
//            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

//            await _repository.Received(1).GetManyAsync(ids);
//        }
//        /// <summary>
//        /// Unit test xoá nhiều (10 ids không xoá được hết)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task DeleteManyAsync_10Ids_ThrowIncompleteException()
//        {
//            // Arrange

//            // List 10 ids
//            var ids = new List<Guid>();
//            for (int i = 0; i < 10; i++)
//            {
//                ids.Add(Guid.NewGuid());
//            }

//            // List 10 employees
//            var employees = new List<Employee>();
//            for (int i = 0; i < 10; i++)
//            {
//                employees.Add(new Employee());
//            }

//            var deletedCount = 8;

//            _repository.GetManyAsync(ids).Returns(employees);
//            _repository.DeleteAsync(ids).Returns(deletedCount);

//            var expectedUserMsg = _resource["IncompleteDelete"] ?? string.Empty;

//            // Act
//            var actualException = Assert.ThrowsAsync<IncompleteException>(async () => await _service.DeleteManyAsync(ids));
//            var actualUserMsg = actualException.UserMsg ?? string.Empty;

//            // Assert
//            Assert.That(actualUserMsg, Is.EqualTo(expectedUserMsg));

//            await _repository.Received(1).GetManyAsync(ids);
//            await _repository.Received(1).DeleteAsync(ids);
//            await _unitOfWork.Received(1).BeginTransactionAsync();
//            await _unitOfWork.Received(1).RollbackAsync();
//        }
//        /// <summary>
//        /// Unit test xoá nhiều (10 ids xoá được hết)
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task DeleteManyAsync_10Ids_ValidResult()
//        {
//            // Arrange

//            // List 10 ids
//            var ids = new List<Guid>();
//            for (int i = 0; i < 10; i++)
//            {
//                ids.Add(Guid.NewGuid());
//            }

//            // List 10 employees
//            var employees = new List<Employee>();
//            for (int i = 0; i < 10; i++)
//            {
//                employees.Add(new Employee());
//            }

//            var expectedResult = 10;

//            _repository.GetManyAsync(ids).Returns(employees);
//            _repository.DeleteAsync(ids).Returns(expectedResult);
            
//            // Act
//            var actualResult = await _service.DeleteManyAsync(ids);

//            // Assert
//            Assert.That(actualResult, Is.EqualTo(expectedResult));

//            await _repository.Received(1).GetManyAsync(ids);
//            await _repository.Received(1).DeleteAsync(ids);
//            await _unitOfWork.Received(1).BeginTransactionAsync();
//            await _unitOfWork.Received(1).CommitAsync();
//        }
//        #endregion
//    }
//}
