/*! For license information please see flight-components-atoms.js.LICENSE.txt */
(self.webpackChunkadl_flight_booking=self.webpackChunkadl_flight_booking||[]).push([["flight-components-atoms"],{"./src/categories/flight/components/atoms/depart-date/index.js":(__unused_webpack_module,__webpack_exports__,__webpack_require__)=>{"use strict";eval('__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var atoms__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! atoms */ "./src/components/atoms/index.js");\n/* harmony import */ var react_datepicker__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! react-datepicker */ "./node_modules/react-datepicker/dist/react-datepicker.min.js");\n/* harmony import */ var react_datepicker__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_datepicker__WEBPACK_IMPORTED_MODULE_2__);\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");\nfunction ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }\n\nfunction _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }\n\nfunction _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }\n\nfunction _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _unsupportedIterableToArray(arr, i) || _nonIterableRest(); }\n\nfunction _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance.\\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }\n\nfunction _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }\n\nfunction _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }\n\nfunction _iterableToArrayLimit(arr, i) { if (typeof Symbol === "undefined" || !(Symbol.iterator in Object(arr))) return; var _arr = []; var _n = true; var _d = false; var _e = undefined; try { for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }\n\nfunction _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }\n\n\n\n\n\nvar OrgInput = function OrgInput(props) {\n  var _useState = (0,react__WEBPACK_IMPORTED_MODULE_1__.useState)(new Date()),\n      _useState2 = _slicedToArray(_useState, 2),\n      startDate = _useState2[0],\n      setStartDate = _useState2[1];\n\n  var _useState3 = (0,react__WEBPACK_IMPORTED_MODULE_1__.useState)(false),\n      _useState4 = _slicedToArray(_useState3, 2),\n      focused = _useState4[0],\n      setFocused = _useState4[1];\n\n  var _useState5 = (0,react__WEBPACK_IMPORTED_MODULE_1__.useState)(props.details),\n      _useState6 = _slicedToArray(_useState5, 2),\n      details = _useState6[0],\n      setDetails = _useState6[1];\n\n  (0,react__WEBPACK_IMPORTED_MODULE_1__.useEffect)(function () {\n    setDetails(props.details);\n  }, [props.details]);\n\n  var onOpen = function onOpen() {\n    setFocused(true);\n  };\n\n  var closePopup = function closePopup() {\n    debugger;\n    setFocused(false);\n  };\n\n  var openPopup = function openPopup() {\n    onOpen();\n  };\n\n  var onSearch = function onSearch() {};\n\n  var inputBox = function inputBox() {\n    if (focused) {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("div", {\n        className: "org-static full"\n      });\n    } else {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("div", {\n        className: "input-wrapper error theme-no filled org-static " + (props.noBorder ? \'no-border\' : \'\')\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n        className: "input mr-n"\n      }, _get.data(details, \'ct\')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("label", {\n        className: "placholder anim"\n      }, _get.i18n(props.type, props.i18nK))));\n    }\n  };\n\n  var optionsHead = function optionsHead(type) {\n    var lbl = _get.i18n(type, props.i18nK);\n\n    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n      className: "full ao-hding mr-n"\n    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n      className: "ao-hding-ih"\n    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("i", {\n      className: "i-language"\n    })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n      className: "full uc"\n    }, lbl)));\n  };\n\n  var select = function select(arg, type) {\n    setDetails(function (prev) {\n      return _objectSpread({}, arg);\n    });\n    setFocused(false);\n  };\n\n  var airportList = function airportList(type) {\n    var list = props[type];\n\n    if (list && list.length > 0) {\n      return list.map(function (arg, i) {\n        return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("li", {\n          className: "full",\n          key: props.uuid + i,\n          onClick: function onClick(e) {\n            select(arg, type);\n          }\n        }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n          className: "full ao-city mr-n"\n        }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n          className: "full"\n        }, arg.ct, ", ", arg.cn), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n          className: "ao-code"\n        }, arg.ac)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n          className: "full ao-name mr-n"\n        }, arg.an)));\n      });\n    }\n\n    return \'\';\n  };\n\n  var renderOptions = function renderOptions(type) {\n    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, optionsHead(type), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("ul", {\n      className: "full ao-list"\n    }, airportList(type)));\n  };\n\n  var popupBody = function popupBody() {\n    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement((react_datepicker__WEBPACK_IMPORTED_MODULE_2___default()), {\n      selected: startDate,\n      onChange: function onChange(date) {\n        return setStartDate(date);\n      }\n    }));\n  };\n\n  var getUi = function getUi() {\n    if (!props.blank) {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("div", {\n        className: "input-wrapper error theme-no filled org-static "\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n        className: "input mr-n"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement((react_datepicker__WEBPACK_IMPORTED_MODULE_2___default()), {\n        selected: startDate,\n        onChange: function onChange(date) {\n          return setStartDate(date);\n        }\n      })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n        className: "placholder anim"\n      }, _get.i18n(props.type, props.i18nK))));\n    } else {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("div", {\n        className: "full sb-wrpr blank-holder has-selected"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n        className: "full sb-lable no-bdr"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n        className: "blank"\n      }, "Round Trip")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n        className: "full placeholder lh-12"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n        className: "blank"\n      }, "Trip type")));\n    }\n  };\n\n  return getUi();\n};\n\natoms__WEBPACK_IMPORTED_MODULE_0__.PopupBox.OrgInput = {\n  recent: [],\n  popular: [],\n  details: {},\n  uuid: helpers.random.id()\n};\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (OrgInput);\n\n//# sourceURL=webpack://adl-flight-booking/./src/categories/flight/components/atoms/depart-date/index.js?')},"./src/categories/flight/components/atoms/index.js":(__unused_webpack_module,__webpack_exports__,__webpack_require__)=>{"use strict";eval('__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   "TripType": () => (/* reexport safe */ _trip_type__WEBPACK_IMPORTED_MODULE_0__.default),\n/* harmony export */   "OrgInput": () => (/* reexport safe */ _org_input__WEBPACK_IMPORTED_MODULE_1__.default),\n/* harmony export */   "DepartDate": () => (/* reexport safe */ _depart_date__WEBPACK_IMPORTED_MODULE_2__.default)\n/* harmony export */ });\n/* harmony import */ var _trip_type__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./trip-type */ "./src/categories/flight/components/atoms/trip-type/index.js");\n/* harmony import */ var _org_input__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./org-input */ "./src/categories/flight/components/atoms/org-input/index.js");\n/* harmony import */ var _depart_date__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./depart-date */ "./src/categories/flight/components/atoms/depart-date/index.js");\n\n\n\n\n//# sourceURL=webpack://adl-flight-booking/./src/categories/flight/components/atoms/index.js?')},"./src/categories/flight/components/atoms/org-input/index.js":(__unused_webpack_module,__webpack_exports__,__webpack_require__)=>{"use strict";eval('__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var atoms__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! atoms */ "./src/components/atoms/index.js");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");\nfunction ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }\n\nfunction _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }\n\nfunction _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }\n\nfunction _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _unsupportedIterableToArray(arr, i) || _nonIterableRest(); }\n\nfunction _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance.\\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }\n\nfunction _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }\n\nfunction _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }\n\nfunction _iterableToArrayLimit(arr, i) { if (typeof Symbol === "undefined" || !(Symbol.iterator in Object(arr))) return; var _arr = []; var _n = true; var _d = false; var _e = undefined; try { for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }\n\nfunction _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }\n\n\n\n\nvar OrgInput = function OrgInput(props) {\n  var _useState = (0,react__WEBPACK_IMPORTED_MODULE_1__.useState)(false),\n      _useState2 = _slicedToArray(_useState, 2),\n      focused = _useState2[0],\n      setFocused = _useState2[1];\n\n  var _useState3 = (0,react__WEBPACK_IMPORTED_MODULE_1__.useState)(props.details),\n      _useState4 = _slicedToArray(_useState3, 2),\n      details = _useState4[0],\n      setDetails = _useState4[1];\n\n  (0,react__WEBPACK_IMPORTED_MODULE_1__.useEffect)(function () {\n    setDetails(props.details);\n  }, [props.details]);\n\n  var onOpen = function onOpen() {\n    setFocused(true);\n  };\n\n  var closePopup = function closePopup() {\n    debugger;\n    setFocused(false);\n  };\n\n  var openPopup = function openPopup() {\n    onOpen();\n  };\n\n  var onSearch = function onSearch() {};\n\n  var inputBox = function inputBox() {\n    if (focused) {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("div", {\n        className: "org-static full"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(atoms__WEBPACK_IMPORTED_MODULE_0__.InputText, {\n        focused: true,\n        onChange: onSearch,\n        noBorder: props.noBorder,\n        value: _get.data(details, \'ct\'),\n        placeholder: _get.i18n(props.type, props.i18nK)\n      }));\n    } else {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("div", {\n        className: "input-wrapper error theme-no filled org-static " + (props.noBorder ? \'no-border\' : \'\')\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n        className: "input mr-n"\n      }, _get.data(details, \'ct\')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("label", {\n        className: "placholder anim"\n      }, _get.i18n(props.type, props.i18nK))));\n    }\n  };\n\n  var optionsHead = function optionsHead(type) {\n    var lbl = _get.i18n(type, props.i18nK);\n\n    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n      className: "full ao-hding mr-n"\n    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n      className: "ao-hding-ih"\n    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("i", {\n      className: "i-language"\n    })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n      className: "full uc"\n    }, lbl)));\n  };\n\n  var select = function select(arg, type) {\n    setDetails(function (prev) {\n      return _objectSpread({}, arg);\n    });\n    setFocused(false);\n  };\n\n  var airportList = function airportList(type) {\n    var list = props[type];\n\n    if (list && list.length > 0) {\n      return list.map(function (arg, i) {\n        return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("li", {\n          className: "full",\n          key: props.uuid + i,\n          onClick: function onClick(e) {\n            select(arg, type);\n          }\n        }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n          className: "full ao-city mr-n"\n        }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n          className: "full"\n        }, arg.ct, ", ", arg.cn), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n          className: "ao-code"\n        }, arg.ac)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n          className: "full ao-name mr-n"\n        }, arg.an)));\n      });\n    }\n\n    return \'\';\n  };\n\n  var renderOptions = function renderOptions(type) {\n    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, optionsHead(type), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("ul", {\n      className: "full ao-list"\n    }, airportList(type)));\n  };\n\n  var popupBody = function popupBody() {\n    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(react__WEBPACK_IMPORTED_MODULE_1__.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("div", {\n      className: "full airport-options"\n    }, renderOptions(\'recent\'), renderOptions(\'popular\')));\n  };\n\n  var getUi = function getUi() {\n    if (!props.blank) {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement(atoms__WEBPACK_IMPORTED_MODULE_0__.PopupBox, {\n        open: openPopup,\n        active: focused,\n        onOpen: onOpen,\n        label: inputBox,\n        body: popupBody,\n        onClose: closePopup,\n        noBorder: props.noBorder,\n        placeholder: _get.i18n(\'tripType\', props.i18nK)\n      });\n    } else {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("div", {\n        className: "full sb-wrpr blank-holder has-selected"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n        className: "full sb-lable no-bdr"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n        className: "blank"\n      }, "Round Trip")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("p", {\n        className: "full placeholder lh-12"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_1__.createElement("span", {\n        className: "blank"\n      }, "Trip type")));\n    }\n  };\n\n  return getUi();\n};\n\natoms__WEBPACK_IMPORTED_MODULE_0__.PopupBox.OrgInput = {\n  recent: [],\n  popular: [],\n  details: {},\n  uuid: helpers.random.id()\n};\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (OrgInput);\n\n//# sourceURL=webpack://adl-flight-booking/./src/categories/flight/components/atoms/org-input/index.js?')},"./src/categories/flight/components/atoms/trip-type/index.js":(__unused_webpack_module,__webpack_exports__,__webpack_require__)=>{"use strict";eval('__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");\n/* harmony import */ var atoms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! atoms */ "./src/components/atoms/index.js");\n\n\n\nvar TripType = function TripType(props) {\n  var getUi = function getUi() {\n    if (!props.blank) {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement(atoms__WEBPACK_IMPORTED_MODULE_1__.SelectBox, {\n        options: props.options,\n        selected: props.selected,\n        noBorder: props.noBorder,\n        placeholder: _get.i18n(\'tripType\', props.i18nK)\n      });\n    } else {\n      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("div", {\n        className: "full sb-wrpr blank-holder has-selected"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("p", {\n        className: "full sb-lable no-bdr"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("span", {\n        className: "blank"\n      }, "Round Trip")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("p", {\n        className: "full placeholder lh-12"\n      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("span", {\n        className: "blank"\n      }, "Trip type")));\n    }\n  };\n\n  return getUi();\n};\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (TripType);\n\n//# sourceURL=webpack://adl-flight-booking/./src/categories/flight/components/atoms/trip-type/index.js?')}}]);