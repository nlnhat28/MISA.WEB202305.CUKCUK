/**
 * Make a debounce function
 * 
 * Author: nlnhat (12/07/2023)
 * @param {*} func Function applied debounce
 * @param {*} delay Deley time to execute
 * @returns
 */
export const debounce = (func, delay) => {
  let timeoutId;
  return (...args) => {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => {
      func.apply(this, args);
    }, delay);
  };
};
