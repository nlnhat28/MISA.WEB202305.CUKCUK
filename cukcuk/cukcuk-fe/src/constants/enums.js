import resources from "@/constants/resources.js";
const enums = {
  /**
   * Type of button
   */
  buttonType: {
    primary: 0,
    secondary: 1,
    danger: 2,
    danger: 3,
    linkDanger: 4,
    link: 5,
    linkIcon: 6,
  },
  /**
   * Type of toast message
   */
  toastType: {
    info: 0,
    error: 1,
    success: 2,
    warning: 3,
  },
  /**
   * Type of dialog
   */
  dialogType: {
    info: 0,
    danger: 1,
    question: 2,
    warning: 3,
    error: 4,
  },
  /**
   * Gender
   */
  gender: {
    male: {
      id: 0,
      name: resources["vn"].male,
    },
    female: {
      id: 1,
      name: resources["vn"].female,
    },
    other: {
      id: 2,
      name: resources["vn"].other,
    },
    undefined: {
      id: 3,
      name: null,
    },
  },
  /**
   * Mode for form
   */
  formMode: {
    create: 0,
    update: 1,
    duplicate: 2,
  },
  /**
   * Direction of combobox
   */
  direction: {
    up: 0,
    down: 1,
  },
  /**
   * Status code of response
   */
  status: {
    ok: 200,
    created: 201,
    noContent: 204,
    serverError: 500,
    clientError: 400,
  },
  /**
   * Server error code
   */
  serverErrorCode: {
    duplicatedEmployeeCode: 1062,
  },
  /**
   * Key code
   */
  keyCode: {
    tab: 9,
    enter: 13,
    shift: 16,
    ctrl: 17,
    alt: 18,
    esc: 27,
    space: 32,
    left: 37,
    up: 38,
    right: 39,
    down: 40,
    insert: 45,
    delete: 46,
    digit1: 49,
    a: 65,
    b: 66,
    c: 67,
    d: 68,
    e: 69,
    f: 70,
    m: 77,
    n: 78,
    r: 82,
    s: 83,
    t: 84,
    u: 85,
    v: 86,
    y: 89,
    f1: 112,
    f2: 113,
    f3: 114,
    f8: 119,
  },
  /**
   * Type of sort
   */
  sortType: {
    disable: 0,
    asc: 1,
    desc: 2,
    blur: 3,
  },
  /**
   * Type of compare
   */
  compareType: {
    contain: 1,
    notContain: 2,
    equal: 3,
    less: 4,
    more: 5,
    range: 6,
    empty: 7,
    startWith: 8,
    endWith: 9,
    lessEqual: 10,
    moreEqual: 11,
    notEqual: 12,
  },
  /**
   * Type of logic
   */
  logicType: {
    and: 1,
    or: 2,
  },
  /**
   * Type of data filter
   */
  filterType: {
    disable: 0,
    text: 1,
    date: 2,
    selectId: 3,
    selectName: 4,
    selectMany: 5,
    number: 6,
  },
  /**
   * Apply state
   */
  applyState: {
    activated: 1,
    deactivated: 2,
  },
  /**
   * Type of exception data
   */
  exceptionKey: {
    formItem: 0,
  },
  /**
   * Type of exception data
   */
  operator: {
    multiple: 0,
    divide: 1,
  },
  /**
   * Type of exception data
   */
  timeUnit: {
    day: 0,
    month: 1,
    year: 2,
  },
  /**
   * Edit mode
   */
  editMode: {
    noEdit: 0,
    create: 1,
    update: 2,
    delete: 3,
  },
  /**
   *
   */
  color: {
    primary: "#45a5ff",
    danger: "#ff0000",
    secondary: "#ccc",
  },
};

export default enums;
