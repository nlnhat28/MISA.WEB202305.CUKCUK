﻿using MISA.CUKCUK.Domain;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MISA.CUKCUK.Application
{
    /// <summary>
    /// Các phương thức trợ giúp tầng Appication
    /// </summary>
    /// Created by: nlnhat (22/07/2023)
    public class ApplicationHelper
    {
        #region Methods
        /// <summary>
        /// Lấy tên đơn vị thời gian
        /// </summary>
        /// <param name="timeUnit">Enum đơn vị thời gian</param>
        /// <returns>Tên đơn vị thời gian</returns>
        /// Created by: nlnhat (22/08/2023)
        public static string GetTimeUnitName(TimeUnit? timeUnit)
        {
            return timeUnit switch
            {
                TimeUnit.Day => "Ngày",
                TimeUnit.Month => "Tháng",
                TimeUnit.Year => "Năm",
                _ => string.Empty,
            };
        }
        /// <summary>
        /// Lấy tên phép tính
        /// </summary>
        /// <param name="_operator">Enum phép tính</param>
        /// <returns>Tên phép tính</returns>
        /// Created by: nlnhat (22/08/2023)
        public static string GetOperatorName(Operator? _operator)
        {
            return _operator switch
            {
                Operator.Multiple => "*",
                Operator.Divide => "/",
                _ => string.Empty,
            };
        }
        /// <summary>
        /// Lấy danh sách các vai trò khác
        /// </summary>
        /// <param name="isCustomer">Là khách hàng hay không</param>
        /// <param name="isProvider">Là nhà cung cấp hay không</param>
        /// <returns>Danh sách các vai trò khác</returns>
        /// Created by: nlnhat (22/07/2023)
        public static List<string> GetOtherRoles(bool? isCustomer, bool? isProvider)
        {
            var otherRoles = new List<string>();
            if (isCustomer == true)
            {
                otherRoles.Add("Khách hàng");
            }
            if (isProvider == true)
            {
                otherRoles.Add("Nhà cung cấp");
            };
            return otherRoles;
        }
        /// <summary>
        /// Lấy tên các vai trò khác
        /// </summary>
        /// <param name="isCustomer">Là khách hàng hay không</param>
        /// <param name="isProvider">Là nhà cung cấp hay không</param>
        /// <returns>Tên các vai trò khác</returns>
        /// Created by: nlnhat (22/07/2023)
        public static string GetOtherRolesName(bool? isCustomer, bool? isProvider)
        {
            var otherRoles = GetOtherRoles(isCustomer, isProvider);
            return string.Join(", ", otherRoles);
        }
        /// <summary>
        /// Tạo mô tả đơn vị chuyển đổi
        /// </summary>
        /// <param name="unitName">Tên đơn vị tính</param>
        /// <param name="destinationUnitName">Tên đơn vị chuyển đổi</param>
        /// <param name="conversionOperator">Phép tính</param>
        /// <param name="rate">Tỉ lệ chuyển đổi</param>
        /// <returns>Mô tả đơn vị chuyển đổi</returns>
        public static string GetConversionUnitDescription(string? unitName, string? destinationUnitName, Operator? conversionOperator, decimal? rate)
        {
            if (unitName != null && destinationUnitName != null && conversionOperator != null && rate != null)
            {
                return conversionOperator switch
                {
                    Operator.Multiple => $"1 {unitName} = {rate} * {destinationUnitName}",
                    Operator.Divide => $"1 {unitName} = 1 / {rate} {destinationUnitName}",
                    _ => string.Empty
                };
            }
            return string.Empty;
        }
        /// <summary>
        /// Chuyển thời gian từ utc sang local
        /// </summary>
        /// <param name="dateTime">Thời gian utc</param>
        /// <returns>Thời gian local</returns>
        /// Created by: nlnhat (22/07/2023)
        public static DateTime? ConvertDateUtcToLocal(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var localTimeZone = TimeZoneInfo.Local;
                var localDateTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)dateTime, localTimeZone);
                localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);

                return localDateTime;
            }
            return null;
        }
        /// <summary>
        /// Chuyển thời gian từ local sang utc
        /// </summary>
        /// <param name="dateTime">Thời gian local</param>
        /// <returns>Thời gian utc</returns>
        /// Created by: nlnhat (22/07/2023)
        public static DateTime? ConvertDateLocalToUtc(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var localDateTimeOffset = new DateTimeOffset((DateTime)dateTime);

                var utcDateTimeOffset = localDateTimeOffset.ToUniversalTime();
                var utcDateTime = utcDateTimeOffset.UtcDateTime;

                return utcDateTime;
            }
            return null;
        }
        /// <summary>
        /// Lấy chữ cái đầu mỗi từ trong chuỗi. Vd: Nguyên vật liệu -> Nvl
        /// </summary>
        /// <param name="text">Chuỗi đầu vào</param>
        /// <returns>Chuỗi mới tạo thành từ chữ cái đầu mỗi chuỗi</returns>
        /// Created by: nlnhat (17/08/2023)
        public static string GetFirstCharEachWord(string text)
        {
            var trimmedAndNormalized = Regex.Replace(text.Trim(), @"\s+", " ");

            var result = new string(trimmedAndNormalized.Split(' ').Select(word => word[0]).ToArray());
            return result;
        }
        /// <summary>
        /// Chuyển Tiếng Việt có dấu sang không dấu
        /// </summary>
        /// <param name="input">Chuỗi đầu vào</param>
        /// <returns>Chuỗi mới không dấu. Ví dụ: Tiếng Việt -> Tieng Viet</returns>
        /// Created by: nlnhat (17/08/2023)
        public static string ConvertDiacritics(string input)
        {
            var normalized = input.Normalize(NormalizationForm.FormD);
            normalized = normalized.Replace("Đ", "D").Replace("đ", "d");

            var chars = normalized.Where(
                character => CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark);

            var result = new string (chars.ToArray());
            return result;
        }
        /// <summary>
        /// Loại bỏ kí tự không phải là chữ cái
        /// </summary>
        /// <param name="input">Chuỗi đầu vào</param>
        /// <returns>Chuỗi mới chỉ bao gồm chữ cái. Ví dụ: new1@ string -> newstring</returns>
        /// Created by: nlnhat (17/08/2023)
        public static string RemoveNonLetters(string input)
        {
            var pattern = "[^a-zA-Z]";
            var result = Regex.Replace(input, pattern, "");
            return result;
        }
        /// <summary>
        /// Tạo prefix code từ chữ cái đầu của tên
        /// </summary>
        /// <param name="name">Tên</param>
        /// <returns>Chuỗi prefix code được lấy từ các chữ cái đầu của tên, loại bỏ dấu, ký tự đặc biệt và in hoa</returns>
        /// Created by: nlnhat (17/08/2023)
        public static string GetPrefixCode(string name)
        {
            var prefix = GetFirstCharEachWord(name);
            prefix = ConvertDiacritics(prefix);
            prefix = RemoveNonLetters(prefix);
            prefix = prefix.ToUpper();
            return prefix;
        }
        #endregion
    }
}
