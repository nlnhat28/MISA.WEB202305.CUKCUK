import {
  regexDigitRange,
  regexOnlyDigit,
  regexDigitLength,
  regexEmail,
} from "@/js/utils/regex.js";
import resources from "@/constants/resources.js";
import { cleanFormatIntNumber, removeSpace } from "@/js/utils/clean-format.js";
import { isNullOrWhiteSpace, isNullOrEmpty } from "@/js/utils/string.js";

/**
 * Validate material code
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Material code to check
 * @returns Error message if invalid, null if valid
 */
export const validateMaterialCode = (label, value) => {
  try {
    if (isNullOrWhiteSpace(value))
      return `${label} ${resources["vn"].cannotEmpty}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate full name
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Material name to check
 * @returns Error message if invalid, null if valid
 */
export const validateMaterialName = (label, value) => {
  try {
    if (isNullOrWhiteSpace(value))
      return `${label} ${resources["vn"].cannotEmpty}`;
    else {
      const words = value.trim().split(/\s+/);
      if (!words.every((word) => /^[A-Za-z]/.test(word)))
        return `${resources["vn"].wordsIn} ${label} ${resources["vn"].startByAlphabet}`;
    }
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate warehouse
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Warehouse to check
 * @param {*} selects Warehouse selects
 * @returns Error message if invalid, null if valid
 */
export const validateWarehouse = (label, value, selects) => {
  try {
    if (isNullOrEmpty(value)) return null;
    if (!isValueInSelects(value, selects))
      return `${label} ${resources["vn"].notInSelects}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate unit
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Unit to check
 * @param {*} selects Unit selects
 * @returns Error message if invalid, null if valid
 */
export const validateUnit = (label, value, selects) => {
  try {
    if (isNullOrEmpty(value)) return `${label} ${resources["vn"].cannotEmpty}`;
    if (!isValueInSelects(value, selects))
      return `${label} ${resources["vn"].notInSelects}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate unit name
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Unit name to check
 * @returns Error message if invalid, null if valid
 */
export const validateUnitName = (label, value) => {
  try {
    if (isNullOrWhiteSpace(value))
      return `${label} ${resources["vn"].cannotEmpty}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate unit name
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Unit name to check
 * @returns Error message if invalid, null if valid
 */
export const validateWarehouseCode = (label, value) => {
  try {
    if (isNullOrWhiteSpace(value))
      return `${label} ${resources["vn"].cannotEmpty}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate unit name
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Unit name to check
 * @returns Error message if invalid, null if valid
 */
export const validateWarehouseName = (label, value) => {
  try {
    if (isNullOrWhiteSpace(value))
      return `${label} ${resources["vn"].cannotEmpty}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Check value is in selects or not
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} value Value to check
 * @param {*} selects Selects to check
 * @returns True if in selects, otherwise false
 */
export const isValueInSelects = (value, selects) => {
  try {
    for (const select of selects) {
      if (select.name == value) {
        return true;
      }
    }
  } catch (error) {
    console.error(error);
  }
  return false;
};
/**
 * Validate date
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} value Date to check
 * @returns False if invalid date, true if valid
 */
export const isValidDate = (date) => {
  try {
    if (date == null || date == "") return true;
    const timestamp = Date.parse(date);
    if (isNaN(timestamp)) return false;
  } catch (error) {
    console.error(error);
  }
  return true;
};
/**
 * Validate a value is month or not
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Value to check
 * @returns False if invalid date, true if valid
 */
export const validateMonth = (label, value) => {
  try {
    const month = parseInt(value, 10);
    if (isNaN(month) || month < 1 || month > 12)
      return `${label} ${resources["vn"].invalid}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate year
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Date to check
 * @returns False if invalid date, true if valid
 */
export const validateYear = (label, value) => {
  try {
    const year = parseInt(value, 10);
    if (isNaN(year) || year < 0 || year > 9999)
      return `${label} ${resources["vn"].invalid}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate date over today
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Date to check
 * @returns Error message if date over today, null if valid
 */
export const validateDateOverToday = (label, date) => {
  try {
    if (date == null || date == "") return null;
    const today = new Date();
    const selected = new Date(date);
    if (selected > today) {
      return `${label} ${resources["vn"].cannotBiggerToday}`;
    } else {
      return null;
    }
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate identity code
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Identity code to check
 * @returns Error message if invalid, null if valid
 */
export const validateIdentityNumber = (label, value) => {
  try {
    value = removeSpace(value);
    if (
      isNullOrEmpty(value) ||
      regexDigitLength(value, 9) ||
      regexDigitLength(value, 12)
    )
      return null;
    return resources["vn"].invalidIdentityNumber;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate bank account
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Bank account to check
 * @returns Error message if invalid, null if valid
 */
export const validateBankAccount = (label, value) => {
  try {
    value = removeSpace(value);
    if (isNullOrEmpty(value) || regexOnlyDigit(value)) return null;
    return `${label} ${resources["vn"].onlyDigit}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate phone number
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Phone number to check
 * @returns Error message if invalid, null if valid
 */
export const validatePhoneNumber = (label, value) => {
  try {
    value = removeSpace(value);
    if (isNullOrEmpty(value) || regexDigitRange(value, 9, 11)) return null;
    return resources["vn"].invalidPhoneNumber;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate landline number
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Landline number to check
 * @returns Error message if invalid, null if valid
 */
export const validateLandlineNumber = (label, value) => {
  try {
    value = removeSpace(value);
    if (isNullOrEmpty(value) || regexDigitRange(value, 9, 11)) return null;
    return resources["vn"].invalidPhoneNumber;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate email
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Email to check
 * @returns Error message if invalid, null if valid
 */
export const validateEmail = (label, value) => {
  try {
    if (isNullOrEmpty(value) || regexEmail(value)) return null;
    return resources["vn"].invalidEmail;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate email
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Email to check
 * @returns Error message if invalid, null if valid
 */
export const validatePersonalTaxCode = (label, value) => {
  try {
    value = removeSpace(value);
    if (isNullOrEmpty(value) || regexOnlyDigit(value)) return null;
    return `${label} ${resources["vn"].onlyDigit}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
/**
 * Validate email
 *
 * Author: nlnhat (28/06/2023)
 *
 * @param {*} label Name of field to validate
 * @param {*} value Email to check
 * @returns Error message if invalid, null if valid
 */
export const validateSalary = (label, value) => {
  try {
    value = cleanFormatIntNumber(value);
    if (isNullOrEmpty(value) || regexOnlyDigit(value)) return null;
    return `${label} ${resources["vn"].onlyDigit}`;
  } catch (error) {
    console.error(error);
  }
  return null;
};
