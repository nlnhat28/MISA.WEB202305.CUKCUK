/**
 * Download file from blob
 *
 * Author: nlnhat (09/08/2023)
 * @param {*} blob Blob data
 * @param {*} fileName File name
 */
export const download = (blob, fileName) => {
  try {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
  } catch (error) {
    console.error(error);
  }
};
