//using Microsoft.Extensions.Localization;

//namespace MISA.CUKCUK.Domain
//{
//    /// <summary>
//    /// Domain service check validate nghiệp vụ nhân viên
//    /// </summary>
//    /// Created by: nlnhat (17/07/2023)
//    public class EmployeeDomainService : IEmployeeDomainService
//    {
//        #region Fields
//        /// <summary>
//        /// Repository nhân viên
//        /// </summary>
//        private readonly IEmployeeRepository _repository;
//        /// <summary>
//        /// Domain service phòng ban
//        /// </summary>
//        /// Created by: nlnhat (18/07/2023)
//        private readonly IUnitDomainService _departmentDomainService;
//        /// <summary>
//        /// Resource lưu trữ thông báo
//        /// </summary>
//        private readonly IStringLocalizer<Resource> _resource;
//        #endregion

//        #region Constructors
//        public EmployeeDomainService(
//            IEmployeeRepository employeeRepository,
//            IUnitDomainService departmentDomainService,
//            IStringLocalizer<Resource> resource)
//        {
//            _repository = employeeRepository;
//            _departmentDomainService = departmentDomainService;
//            _resource = resource;
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// Check trùng mã nhân viên
//        /// </summary>
//        /// <param name="employee">Entity nhân viên để check</param>
//        /// <exception cref="ConflictException">Exception mã đã tồn tại</exception>
//        /// Created by: nlnhat (17/07/2023)
//        public async Task CheckDuplicatedCodeAsync(Employee employee)
//        {
//            var employeeCode = employee.EmployeeCode;
//            var employeeExist = await _repository.GetAsync(employeeCode);

//            // Nếu trùng mã và trùng với nhân viên khác (tránh trường hợp trùng vs chính nhân viên đấy)
//            if (employeeExist != null && employee?.EmployeeId != employeeExist?.EmployeeId)
//                throw new ConflictException(
//                    MISAErrorCode.EmployeeCodeDuplicated,
//                     _resource["EmployeeCodeDuplicated"],
//                    new ExceptionData(ExceptionKey.FormItem, "EmployeeCode", employeeCode));
//        }
//        /// <summary>
//        /// Check mã nằm trong khoảng cho phép
//        /// </summary>
//        /// <param name="employeeCode">Mã để check</param>
//        /// <exception cref="ValidateException">Exception mã không hợp lệ</exception>
//        /// Created by: nlnhat (17/07/2023)
//        public async Task CheckRangeCodeAsync(string employeeCode)
//        {
//            int hyphenIndex = employeeCode.IndexOf('-');
//            string numberString = employeeCode[(hyphenIndex + 1)..];

//            if (int.TryParse(numberString, out int newCode))
//            {
//                // Code lớn nhất hiện tại
//                var maxCode = await _repository.GetMaxCodeAsync();

//                // Code lớn nhất cho phép
//                var limitCode = maxCode + MaterialConstant.OffsetCode;

//                if (newCode > limitCode)
//                    throw new ValidateException(
//                        MISAErrorCode.EmployeeCodeOutOfRange,
//                        $"{_resource["EmployeeCodeLessThan"]} {++limitCode}",
//                        new ExceptionData(ExceptionKey.FormItem, "EmployeeCode", employeeCode));
//            }
//            else
//            {
//                throw new ValidateException(
//                    MISAErrorCode.EmployeeCodeWrongFormat,
//                    $"{_resource["EmployeeCodeFormat"]} {MaterialConstant.PrefixCode}{_resource["Number"]}",
//                    new ExceptionData(ExceptionKey.FormItem, "EmployeeCode", employeeCode));
//            }
//        }
//        /// <summary>
//        /// Check tồn tại phòng ban hay không
//        /// </summary>
//        /// <param name="departmentId">Id phòng ban để check</param>
//        /// Created by: nlnhat (17/07/2023)
//        public async Task CheckExistDepartmentAsync(Guid departmentId)
//        {
//            await _departmentDomainService.CheckExistDepartmentAsync(departmentId);
//        }
//        #endregion
//    }
//}
