/**
 * Validate only digits
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} value Value to check
 * @returns True if match
 */
export const regexOnlyDigit = (value) => {
  const regex = /^\d+$/;
  return regex.test(value);
};
/**
 * Validate only digits with fixed length
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} value Value to check
 * @param {*} length Fixed length
 * @returns True if match
 */
export const regexDigitLength = (value, length) => {
  const regex = new RegExp(`^\\d{${length}}$`);
  return regex.test(value);
};
/**
 * Validate only digits in range
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} value Value to check
 * @param {*} maxLength Maximum length
 * @returns True if match
 */
export const regexDigitMaxLength = (value, maxLength) => {
  const regex = new RegExp(`^\\d{0,${maxLength}}$`);
  return regex.test(value);
};
/**
 * Validate only digits in range
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} value Value to check
 * @param {*} minLength Minimum length
 * @returns True if match
 */
export const regexDigitMinLength = (value, minLength) => {
  const regex = new RegExp(`^\\d{${minLength},}$`);
  return regex.test(value);
};
/**
 * Validate only digits in range
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} value Value to check
 * @param {*} minLength Minimum length
 * @param {*} maxLength Maximum length
 * @returns True if match
 */
export const regexDigitRange = (value, min, max) => {
  const regex = new RegExp(`^\\d{${min},${max}}$`);
  return regex.test(value);
};
/**
 * Validate only digits in range
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} value Value to check
 * @returns True if match
 */
export const regexEmail = (value) => {
  const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return regex.test(value);
};
