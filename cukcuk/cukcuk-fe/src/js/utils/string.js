import { removeSpace } from "./clean-format";

/**
 * Check string is null or white space
 *
 * Author: nlnhat (02/07/2023)
 * @param {string} value Value to check
 * @returns True if  null or white space, otherwise false
 */
export const isNullOrWhiteSpace = (value) => {
  value = removeSpace(value);
  return value == null || value == "";
};
/**
 * Check string is null or white space
 *
 * Author: nlnhat (02/07/2023)
 * @param {string} value Value to check
 * @returns True if  null or white space, otherwise false
 */
export const isNullOrEmpty = (value) => {
  return value == null || value === "";
};
/**
 * Capitalize first char
 *
 * Author: nlnhat (28/07/2023)
 *
 * @param {string} text String to format
 * @returns {string} String format: test value => Test value
 */
export const capitalizeFirstChar = (text) => {
  if (typeof text !== "string" || text.length === 0) {
    return text;
  }
  return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
};
