/**
 * Copy deep a object
 * 
 * Author: nlnhat (24/08/2023)
 * @param {*} object Original object
 * @returns New object
 */
export const copyObject = (object) => {
    try {
        const newObject = JSON.parse(JSON.stringify(object));
        return newObject;
    } catch (error) {
        console.log(error);
        return null;        
    }
}
/**
 * Stringify a object
 * 
 * Author: nlnhat (24/08/2023)
 * @param {*} object Original object
 * @returns Object dạng string
 */
export const stringify = (object) => {
  try {
    return JSON.stringify(object);
  } catch (error) {
    console.log(error);
    return null;
  }
};
/**
 * Compare 2 objects
 * 
 * Author: nlnhat (24/08/2023)
 * @param {*} object Object 1
 * @param {*} object Object 2
 * @returns True if same
 */
export const sameObject = (object1, object2) => {
    const object1Json = JSON.stringify(object1);
    const object2Json = JSON.stringify(object2);
    console.log(object1Json);
    console.log(object2Json);
    return (object1Json == object2Json)
}