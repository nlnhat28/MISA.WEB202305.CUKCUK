//using AutoMapper;
//using Microsoft.Extensions.Localization;
//using MISA.CUKCUK.Domain;

//namespace MISA.CUKCUK.Application.Tests
//{
//    /// <summary>
//    /// Unit test cho material service
//    /// </summary>
//    /// Created by: nlnhat (24/07/2023)
//    [TestFixture]
//    public class MaterialServiceTests
//    {
//        #region Properties
//        /// <summary>
//        /// Repository nguyên vật liệu
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IMaterialRepository _repository { get; set; }
//        /// <summary>
//        /// Repository nguyên vật liệu
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private IMaterialDomainService _domainService { get; set; }
//        /// <summary>
//        /// Repository nguyên vật liệu
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
//        /// Service nguyên vật liệu
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        private MaterialService _service { get; set; }
//        #endregion

//        #region SetUp
//        /// <summary>
//        /// Setup instance
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [SetUp]
//        public void SetUp()
//        {
//            _repository = Substitute.For<IMaterialRepository>();
//            var conversionUnitRepository = Substitute.For<IConversionUnitRepository>();
//            _domainService = Substitute.For<IMaterialDomainService>();
//            _mapper = Substitute.For<IMapper>();
//            _resource = Substitute.For<IStringLocalizer<Resource>>();
//            _unitOfWork = Substitute.For<IUnitOfWork>();
//            _service = new MaterialService(
//                _repository, conversionUnitRepository, _domainService, _resource, _mapper, _unitOfWork);
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// Unit test validate khoảng code
//        /// </summary>
//        /// Created by: nlnhat (24/07/2023)
//        [Test]
//        public async Task ValidateRangeCodeAsync_NotMatchingCode_Success()
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
//        #endregion
//    }
//}
