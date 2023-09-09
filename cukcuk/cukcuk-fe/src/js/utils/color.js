/**
 * Tạo list màu theo thứ tự hue
 *
 * Author: nlnhat (07/09/2023)
 * @param {*} start Màu bắt đầu
 * @param {*} end Màu kết thúc
 * @param {*} count Số lượng
 * @returns Danh sách màu
 */
export const generateHueColors = (start, end, count) => {
  const step = end / count;
  const colors = [];
  let hue = start;

  for (let i = 0; i < count; i++) {
    colors.push(`hsl(${hue}, 100%, 60%)`);
    hue = (hue + step) % 360;
  }

  return colors;
};
/**
 * Tạo list màu theo thứ tự light và số lượng
 *
 * Author: nlnhat (07/09/2023)
 * @param {*} hue Hue color
 * @param {*} start Màu bắt đầu
 * @param {*} end Màu kết thúc
 * @param {*} numbers Array số lượng
 * @returns Danh sách màu
 */
export const generateColorsByNumbers = (hue, start, end, numbers) => {
  try {
    const set = new Set(numbers);
    const step = (end - start) / set.size;

    const count = numbers.length;
    const colors = [];
    let light = start;

    for (let i = 0; i < count; i++) {
      if (i > 0) {
          light = numbers[i] == numbers[i - 1] ? light : light + step;
      }
      colors.push(`hsl(${hue}, 100%, ${light}%)`);
    }
    return colors;
  } catch (error) {
    console.error(error);
    return [];
  }
};
/**
 * Tạo màu gradient cho biểu đồ
 * @param {*} ctx Context
 * @param {*} chartArea Chart area
 * @param {*} colors Array colors(offset, color)
 * @returns Màu gradient
 */
export const getGradient = (ctx, chartArea, colors) => {
  let gradient = ctx.createLinearGradient(
    0,
    chartArea.bottom,
    0,
    chartArea.top
  );
  colors.forEach((color) => {
    gradient.addColorStop(color.offset, color.color);
  });
  return gradient;
};
/**
 * Màu blue gradient
 *
 * Author: nlnhat (09/09/2023)
 */
export const blueGradient = [
  {
    offset: 1,
    color: "rgba(69, 165, 255, 1)",
  },
  {
    offset: 0.5,
    color: "rgba(69, 165, 255, 0.5)",
  },
  {
    offset: 0,
    color: "rgba(69, 165, 255, 0.25)",
  },
];
