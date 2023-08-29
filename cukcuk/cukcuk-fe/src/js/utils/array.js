/**
 * Reserve an array
 *
 * Author: nlnhat (02/07/2023)
 * @param {Array} array Array to reserve
 */
export const reserveArray = (array) => {
  try {
    const reserveArray = Array.from(array).reverse();
    return reserveArray;
  } catch (error) {
    console.error(error);
  }
  return array;
};
/**
 * Check an array has duplicated item
 *
 * Author: nlnhat (02/07/2023)
 * @param {Array} array Array to check
 * @return True if has, otherwise false
 */
export const duplicatedItem = (array) => {
  try {
    const setArray = new Set(array);
    return setArray.size !== array.length;
  } catch (error) {
    console.error(error);
  }
  return false;
};
/**
 * Sort select array by name of item
 *
 * Author: nlnhat (02/07/2023)
 * @param {Array} array Array to sort
 */
export const sortByName = (array) => {
  try {
    return array.sort((a, b) => {
      if (a.name < b.name) return -1;
      if (a.name > b.name) return 1;
      return 0;
    });
  } catch (error) {
    console.error(error);
  }
  return array;
};
