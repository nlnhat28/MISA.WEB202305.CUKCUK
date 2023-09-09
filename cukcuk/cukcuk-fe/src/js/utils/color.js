/**
 * Tạo list màu theo thứ tự
 * 
 * Author: nlnhat (07/09/2023)
 * @param {*} count Số lượng
 * @param {*} start Màu bắt đầu
 * @param {*} end Màu kết thúc
 * @returns Danh sách màu
 */
export const generateColors = (count, start, end) => {
  const step = end / count;
  const colors = [];
  let hue = start;

  for (let i = 0; i < count; i++) {
    colors.push(`hsl(${hue}, 100%, 60%)`);
    hue = (hue + step) % 360;
  }

  return colors;
}
