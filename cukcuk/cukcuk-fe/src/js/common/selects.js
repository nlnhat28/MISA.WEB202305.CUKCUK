import enums from "@/constants/enums";
import resources from "@/constants/resources";

const selects = {
  /**
   * Giới tính
   */
  gender: [
    {
      id: enums.gender.male.id,
      name: enums.gender.male.name,
    },
    {
      id: enums.gender.female.id,
      name: enums.gender.female.name,
    },
    {
      id: enums.gender.other.id,
      name: enums.gender.other.name,
    },
  ],
  /**
   * True/False
   */
  boolean: [
    {
      id: 0,
      name: resources["vn"].no,
    },
    {
      id: 1,
      name: resources["vn"].yes,
    },
  ],
  /**
   * Đơn vị thời gian
   */
  timeUnit: [
    {
      id: enums.timeUnit.day,
      name: resources["vn"].day,
    },
    {
      id: enums.timeUnit.month,
      name: resources["vn"].month,
    },
    {
      id: enums.timeUnit.year,
      name: resources["vn"].year,
    },
  ],
  /**
   * Phép tính
   */
  operator: [
    {
      id: enums.operator.multiple,
      name: "*",
      note: `* - ${resources["vn"].multiple}`,
    },
    {
      id: enums.operator.divide,
      name: "/",
      note: `/ - ${resources["vn"].divide}`,
    },
  ],
};

export default selects;