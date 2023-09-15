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
/**
 * Choose file
 *
 * Author: nlnhat (09/08/2023)
 */
export const chooseFile = () => {
  try {
    const input = document.createElement("input");
    input.type = "file";
    input.style.display = "none";
    input.addEventListener("change", (event) => {
      const selectedFile = event.target.files[0];
      if (selectedFile) {
        return selectedFile;
      } else {
        return null;
      }
    });
    input.click();
  } catch (error) {
    console.error(error);
    return null
  }
};
