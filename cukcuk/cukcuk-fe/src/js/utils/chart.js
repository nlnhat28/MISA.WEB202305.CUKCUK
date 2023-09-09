import { formatDecimal } from "@/js/utils/format.js";
/**
 * Format % cho đồ thị tròn
 *
 * Author: nlnhat (09/09/2023)
 * @param {*} value Giá trị
 * @param {*} ctx Data context
 * @returns Label với số liệu %
 */
export const percentageFormatter = (value, ctx) => {
  try {
    let datasets = ctx.chart.data.datasets;
    if (datasets.indexOf(ctx.dataset) === datasets.length - 1) {
      let sum = datasets[0].data.reduce((a, b) => a + b, 0);
      let percentage = formatDecimal((value / sum) * 100) + "%";
      return percentage;
    } else {
      return percentage;
    }
  } catch (error) {
    console.error(error);
    return null;
  }
};
