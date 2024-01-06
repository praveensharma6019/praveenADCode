/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./assets/js/helper/element/attributes.js":
/*!************************************************!*\
  !*** ./assets/js/helper/element/attributes.js ***!
  \************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\t\n\tget(el, atr){\n\t\tif(el && (el.getAttribute(atr) || el.getAttribute(atr) === '')){\n\t\t\treturn el.getAttribute(atr);\n\t\t}\n\t\treturn false;\n\t},\n\t\t\t\n\tset(elm, attr, val){\n\t\tif(elm && elm.setAttribute){\n\t\t\telm.setAttribute(attr, val);\n\t\t};\n\t},\n\t\n\thas(elm, attr){\n\t\t\n\t},\n\t\n\tremove(elm, attr){\n\t\tif(elm && elm.removeAttribute){\n\t\t\telm.removeAttribute(attr);\n\t\t};\n\t}\n\t\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/attributes.js?");

/***/ }),

/***/ "./assets/js/helper/element/class.js":
/*!*******************************************!*\
  !*** ./assets/js/helper/element/class.js ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\tadd(el, cls) {\n\t\tif(el){\n\t\t\tvar hasCls = this.has(el, cls);\n\t\t\tif(!hasCls){\n\t\t\t\tif(el.classList.value){\n\t\t\t\t\tvar clses = el.classList.value;\n\t\t\t\t\t\tel.classList.value = clses+' '+cls;\n\t\t\t\t}else{\n\t\t\t\t\tlet\tclsList =\tel.getAttribute('class');\n\t\t\t\t\tif(clsList){\n\t\t\t\t\t\tclsList = clsList.split(' ');\n\t\t\t\t\t}else{\n\t\t\t\t\t\tclsList = []\n\t\t\t\t\t}\n\t\t\t\t\tclsList.push(cls);\n\t\t\t\t\tel.setAttribute('class', clsList.join(' '));\n\t\t\t\t}\n\t\t\t}\n\t\t}\n\t},\n\t\n\tremove(elm, cls) {\n\t\tif(elm){\n\t\t\tlet clss = elm.classList.value;\n\t\t\tif(!clss){\n\t\t\t\tclss = elm.getAttribute('class');\n\t\t\t};\n\n\t\t\tif(clss){\n\t\t\t\tclss = clss.split(' ');\n\t\t\t\tlet clsI = clss.indexOf(cls);\n\t\t\t\tif(clsI >= 0){\n\t\t\t\t\tclss.splice(clsI, 1);\n\t\t\t\t\tclss = clss.join(' ');\t\n\t\t\t\t\telm.setAttribute('class', clss);\n\t\t\t\t\tthis.remove(elm, cls);\n\t\t\t\t};\n\t\t\t}\t  \n\t\t}\n\t},\n\t\t\t\n\thas(elm, cls){\n\t\tvar clsList = []\n\t\t\n\t\tif(elm && elm.classList){\n\t\t\tclsList =\telm.getAttribute('class');\n\t\t\t//clsList = elm.classList.value;\n\t\t    if(clsList){\n\t\t    \tclsList = clsList.split(' ');\n\t\t    }\n\t\t};\n\t\tif(clsList && clsList.indexOf(cls) > -1){\n\t\t\treturn true;\n\t\t};\n\t\treturn false;\n\t},\n\t\n\ttoggle(elm, cls){\n\t\tvar hasClass = this.has(elm, cls);\n\t    if(hasClass){\n\t      this.remove(elm, cls);\n\t    }else{\n\t      this.add(elm, cls)\n\t    }\n\t}\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/class.js?");

/***/ }),

/***/ "./assets/js/helper/element/create/form.js":
/*!*************************************************!*\
  !*** ./assets/js/helper/element/create/form.js ***!
  \*************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\tinit(arg){\n\t\t/*--var temp = {\n\t\t\t\tconf:{\n\t\t\t\t\tsubmit:false,\n\t\t\t\t\tskipAppend:true,\n\t\t\t\t\tattrs:{\n\t\t\t\t\t\tmethod:'post',\n\t\t\t\t\t\taction:''\n\t\t\t\t\t}\n\t\t\t\t},\n\t\t\t\tinputs:[\n\t\t\t\t   {\n\t\t\t\t\t   name:'test',\n\t\t\t\t\t   id:'testid'\n\t\t\t\t   },\n\t\t\t\t   {\n\t\t\t\t\t   name:'test1',\n\t\t\t\t\t   id:'testid1',\n\t\t\t\t\t   type:'text'\n\t\t\t\t   }\n\t\t\t\t]\n\t\t\t}\n\t\targ = temp;--*/\n\t\tvar conf = arg.conf;\n\t\tif(conf && arg.inputs){\n\t\t\tvar inputs = arg.inputs,\n\t\t\t\tf = this.createForm(conf.attrs);\n\t\t\tfor(var i in inputs){\n\t\t\t\tf.appendChild(this.createInput(inputs[i]));\n\t\t\t};\n\t\t\treturn this.submitForm(f, conf);\n\t\t}\n\t\treturn false;\n\t},\n\t\t\n\tsubmitForm(f, arg){\n\t\tif(arg.submit){\n\t\t\tdocument.body.appendChild(f);\n\t\t\tf.submit();\n\t\t}else{\n\t\t\treturn f;\n\t\t}\n\t},\n\t\t\n\tcreateForm(arg){\n\t\treturn this.setMultipleAttr(document.createElement('form'), arg);\n\t},\n\t\t\n\tcreateInput(arg){\n\t\tif(!arg.type){\n\t\t\targ.type = 'hidden';\n\t\t};\n\t\treturn this.setMultipleAttr(document.createElement('input'), arg);\n\t},\n\n\tsetMultipleAttr(el, arg){\n\t\tfor(var a in arg){\n\t\t\tif(arg[a]){\n\t\t\t\tel.setAttribute(a, arg[a]);\n\t\t\t}\n\t\t};\n\t\treturn el;\n\t}\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/create/form.js?");

/***/ }),

/***/ "./assets/js/helper/element/create/iframe.js":
/*!***************************************************!*\
  !*** ./assets/js/helper/element/create/iframe.js ***!
  \***************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\tinit(arg){\n\t\t/*arg = {\n\t\t\tid:'',\n\t\t\turl:'',\n\t\t\tparams:{\n\n\t\t\t},\n\t\t\tcallback:{\n\t\t\t\tonerror:function(arg){\n\t\t\t\t\t\n\t\t\t\t},\n\t\t\t\tonload:function(arg){\n\t\t\t\t\t\n\t\t\t\t}\n\t\t\t}\n\t\t}--*/\n\n\t\tif(arg.url){\n\t\t\tvar app = this.getApp(),\n\t\t\t\tiframe = document.createElement('iframe');\n\n\t\t\t\tapp.helper.element.remove(arg.id);\n\n\t\t\t\tiframe.frameBorder=0;\n\t\t\t\tiframe.width = 0;\n\t\t\t\tiframe.height = 0;\n\t\t\t\tiframe.id = arg.id?arg.id:'';\n\t\t\t\tiframe.setAttribute(\"src\", app.helper.url.get(arg.url, arg.params));\n\n\t\t\tif(arg.callback){\n\t\t\t\tif(arg.callback.onerror){\n\t\t\t\t\tiframe.onerror = function(){\n\t\t\t\t\t\targ.callback.onerror(arg)\n\t\t\t\t\t}\n\t\t\t\t}\n\t\t\t\tif(arg.callback.onload){\n\t\t\t\t\tiframe.onload = function(){\n\t\t\t\t\t\targ.callback.onload(arg)\n\t\t\t\t\t}\n\t\t\t\t}\n\t\t\t}\n\n\t\t\tdocument.body.appendChild(iframe);\n\t\t}\n\t}\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/create/iframe.js?");

/***/ }),

/***/ "./assets/js/helper/element/create/index.js":
/*!**************************************************!*\
  !*** ./assets/js/helper/element/create/index.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _form__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./form */ \"./assets/js/helper/element/create/form.js\");\n/* harmony import */ var _iframe__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./iframe */ \"./assets/js/helper/element/create/iframe.js\");\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    form:_form__WEBPACK_IMPORTED_MODULE_0__[\"default\"],\n\tiframe:_iframe__WEBPACK_IMPORTED_MODULE_1__[\"default\"]\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/create/index.js?");

/***/ }),

/***/ "./assets/js/helper/element/events/bind.js":
/*!*************************************************!*\
  !*** ./assets/js/helper/element/events/bind.js ***!
  \*************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\n\tkeyup(elm, arg, clb){\n\t\tif(elm){\n\t\t\telm.onkeyup = function(e) {\n\t\t\t    if(clb){\n\t\t\t    \tclb(e, arg);\n\t\t\t    }\n\t\t\t};\n\t\t};\n\t},\n\t\t\t\n\tkeydown(elm, arg, clb){\n\t\tif(elm){\n\t\t\telm.onkeydown = function(e) {\n\t\t\t    if(clb){\n\t\t\t    \tclb(e, arg);\n\t\t\t    }\n\t\t\t};\n\t\t};\n\t},\n\n\tonfocus(elm, arg, clb){\n\t\tif(elm){\n\t\t\telm.onkeydown = function(e) {\n\t\t\t    if(clb){\n\t\t\t    \tclb(e, arg);\n\t\t\t    }\n\t\t\t};\n\t\t};\n\t},\n\n\tbindCallback(e, type){\n        let arg = e.currentTarget.arg;\n        if(arg.callback && arg.callback[type]){\n            arg.callback[type](e, e.currentTarget);\n        };\n\t},\n\n\tbindEvents(elm, events){\n        for(var a in events){\n            elm.addEventListener(a, this.bindCallback);\n        }\n    },\n\n\tinit(id, arg){\n        let that = this;\n        setTimeout(function(){\n            var elm = getCore().helper.element.get.byId(id);\n            if(elm && arg){\n                elm.arg = arg;\n                that.bindEvents(elm, arg.events);\n            }\n        }, 100);\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/events/bind.js?");

/***/ }),

/***/ "./assets/js/helper/element/events/index.js":
/*!**************************************************!*\
  !*** ./assets/js/helper/element/events/index.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _bind__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./bind */ \"./assets/js/helper/element/events/bind.js\");\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    bind:_bind__WEBPACK_IMPORTED_MODULE_0__[\"default\"]\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/events/index.js?");

/***/ }),

/***/ "./assets/js/helper/element/get.js":
/*!*****************************************!*\
  !*** ./assets/js/helper/element/get.js ***!
  \*****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\t\n\tbyClass(a){\n\t\t\n\t},\n\n\tbyName(name){\n\t\tif(name){\n\t\t\tlet elm = document.getElementsByTagName(name)[0];\n\t\t\tif(elm){\n\t\t\t\treturn elm;\n\t\t\t}\n\t\t};\n\t\treturn false;\n\t},\n\t\n\tbyId(a){\n\t\tif(a){\n\t\t\tvar elm = document.getElementById(a);\n\t\t\tif(elm){\n\t\t\t\treturn elm;\n\t\t\t}\n\t\t}\n\t\treturn false;\n\t},\n\t\n\tbyAttr(a){\n\t\t\n\t},\n\n\theight(elm){\n\t\tif(elm){\n\t\t\treturn elm.offsetHeight;\n\t\t}\n\t\treturn 0;\n\t\tlet topOffset = elm.offsetTop;\n        let height = elm.offsetHeight;\n\t},\n\n\toffset(elm, from){\n\t\tif(typeof elm === 'string'){\n\t\t\telm = this.byId(elm);\n\t\t};\n\n\t\tif(elm){\n\t\t\tif(from === 'left'){\n\t\t\t\t\n\t\t\t}else{\n\t\t\t\treturn elm.offsetTop\n\t\t\t}\n\t\t}\n\t\treturn 0;\n\t},\n\n\tscrollTo(elm, from, px){\n\t\tlet val = (px?px:0)\n\t\tif(elm){\n\t\t\tswitch(from) {\n\t\t\t\tcase 'left':\n\t\t\t\t\telm.scrollTop = val;\n\t\t\t\tbreak;\n\t\t\t\tdefault:\n\t\t\t\t\telm.scrollTop = val;\n\t\t\t}\n\t\t}\n\t},\n\n\tscroll(elm, from){\n\t\tif(elm){\n\t\t\tif(from === 'left'){\n\t\t\t\t\n\t\t\t}else{\n\t\t\t\treturn elm.offsetTop\n\t\t\t}\n\t\t}\n\t\twindow.pageYOffset\n\t}\n\t\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/get.js?");

/***/ }),

/***/ "./assets/js/helper/element/index.js":
/*!*******************************************!*\
  !*** ./assets/js/helper/element/index.js ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _scroll__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./scroll */ \"./assets/js/helper/element/scroll.js\");\n/* harmony import */ var _attributes__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./attributes */ \"./assets/js/helper/element/attributes.js\");\n/* harmony import */ var _create___WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./create/ */ \"./assets/js/helper/element/create/index.js\");\n/* harmony import */ var _events___WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./events/ */ \"./assets/js/helper/element/events/index.js\");\n/* harmony import */ var _class__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./class */ \"./assets/js/helper/element/class.js\");\n/* harmony import */ var _get__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./get */ \"./assets/js/helper/element/get.js\");\n\n\n\n\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\tget:_get__WEBPACK_IMPORTED_MODULE_5__[\"default\"],\n\tattr:_attributes__WEBPACK_IMPORTED_MODULE_1__[\"default\"],\n\tevent:_events___WEBPACK_IMPORTED_MODULE_3__[\"default\"],\n\tclass:_class__WEBPACK_IMPORTED_MODULE_4__[\"default\"],\n\tcreate:_create___WEBPACK_IMPORTED_MODULE_2__[\"default\"],\n\tscroll:_scroll__WEBPACK_IMPORTED_MODULE_0__[\"default\"],\n\n\tinsetAfter(prnt, elm){\n\t\tprnt.parentNode.insertBefore(elm, prnt.nextSibling)\n\t},\n\n\tremove(id){\n\t\tvar elm = this.get.byId(id);\n\t\tif(elm){\n\t\t\telm.parentNode.removeChild(elm);\n\t\t}\n\t}\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/index.js?");

/***/ }),

/***/ "./assets/js/helper/element/scroll.js":
/*!********************************************!*\
  !*** ./assets/js/helper/element/scroll.js ***!
  \********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\n    scrollTop(elm, to){\n        if(elm.scrollTop){\n            elm.scrollTop = (to?to:0)\n        }\n    },\n\n    byId(id, type, to){\n        if(id){\n\t\t\tlet elm = document.getElementById(id);\n\t\t\tif(elm){\n\t\t\t\tswitch(type) {\n                    case 'left':\n                      // code block\n                    break;\n                    case 'top':\n                        this.scrollTop(elm, to);\n                    break;\n                    default:\n                      // code block\n                  }\n\t\t\t}\n\t\t}\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/element/scroll.js?");

/***/ }),

/***/ "./assets/js/helper/event/index.js":
/*!*****************************************!*\
  !*** ./assets/js/helper/event/index.js ***!
  \*****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\n    dispatch(name, elm, cb, arg){\n        if(name && elm){\n            const event = new Event(name, {\n                bubbles:true,\n                composed:false,\n                cancelable:true,\n                arg:(arg?arg:false),\n                cb:(cb?cb:()=>{}),\n            });\n            elm.dispatchEvent(event);\n        }\n    },\n\n    bind(name, elm, cb){\n        if(name && elm){\n            elm.addEventListener(name, (e) => {\n                if(cb){\n                    cb(e);\n                }\n            });\n        }\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/event/index.js?");

/***/ }),

/***/ "./assets/js/helper/index.js":
/*!***********************************!*\
  !*** ./assets/js/helper/index.js ***!
  \***********************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _page__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./page */ \"./assets/js/helper/page/index.js\");\n/* harmony import */ var _event__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./event */ \"./assets/js/helper/event/index.js\");\n/* harmony import */ var _element___WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./element/ */ \"./assets/js/helper/element/index.js\");\n/* harmony import */ var _plugins___WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./plugins/ */ \"./assets/js/helper/plugins/index.js\");\n/* harmony import */ var _user_agent___WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./user-agent/ */ \"./assets/js/helper/user-agent/index.js\");\n\n\n\n\n\n\n(function(){\n    window.helpers = {\n        page:_page__WEBPACK_IMPORTED_MODULE_0__[\"default\"],\n        event:_event__WEBPACK_IMPORTED_MODULE_1__[\"default\"],\n        device:_user_agent___WEBPACK_IMPORTED_MODULE_4__[\"default\"],\n        element:_element___WEBPACK_IMPORTED_MODULE_2__[\"default\"],\n        plugins:_plugins___WEBPACK_IMPORTED_MODULE_3__[\"default\"] \n    };\n\n    window.helpers.plugins.include.init();\n})();\n\n//# sourceURL=webpack://electricity/./assets/js/helper/index.js?");

/***/ }),

/***/ "./assets/js/helper/page/index.js":
/*!****************************************!*\
  !*** ./assets/js/helper/page/index.js ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\n    onInclude(){\n        let page = helpers.element.attr.get(document.body, 'data-page');\n        if(window.pages && window.pages[page] && window.pages[page].init){\n            window.pages[page].init();\n            helpers.plugins.input.init();\n            window.contactUs = function(){\n                helpers.plugins.validation.init({form:{id:'contact-us'}});\n            }\n        };\n    },\n\n    extend(name, arg){\n        window.pages = window.pages || {};\n        if(name && arg){\n            window.pages[name] = arg;\n        }\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/page/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/accordion/index.js":
/*!*****************************************************!*\
  !*** ./assets/js/helper/plugins/accordion/index.js ***!
  \*****************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    config:{\n        activeClass:'active',\n        link:'adl-accordion-action'\n    },\n\n    toggle(event, arg, h, c, t){\n        let cElm = event.currentTarget;\n        let holder =document.getElementById(h);\n        let target =document.getElementById(t);\n        let screen = cElm.getAttribute('data-accordion-skip-lh');\n\n        if(screen){\n            screen = parseInt(screen);\n        }\n\n        if((!screen) || (screen && (screen > 0) && (screen >= window.innerWidth))){\n            if(holder && c){\n                let aCls = this.getElement(arg, 'activeClass', true);\n                let containers = holder.getElementsByClassName(c);\n\n                for(let a in containers){\n                    let elm = containers[a];\n                    if(elm.getAttribute){\n                        helpers.element.class.remove(elm, aCls);\n                    }\n                }\n\n                if(target){\n                    helpers.element.class.toggle(target, aCls);\n                }\n            }\n        }\n    },\n\n    getElement(arg, type, idOnly){\n        let conf = this.config;\n        let id = conf[type];\n\n        if(arg && arg[type]){\n            id = arg[type];\n        };\n\n        if(idOnly){\n            return id\n        }else{\n            return document.getElementsByClassName(id)\n        }\n    },\n\n    bind(arg){\n        let that = this;\n        let elms = this.getElement(arg, 'link');\n\n        if(elms){\n            for(let a in elms){\n                let elm = elms[a];\n                if(elm.getAttribute){\n                    let target = elm.getAttribute('data-accordion-target');\n                    let holder = elm.getAttribute('data-accordion-holder');\n                    let contents = elm.getAttribute('data-accordion-content');\n\n                    let open = function(e){\n                        that.toggle(e, arg, holder, contents, target);\n                    };\n\n                    if(holder){\n                        elm.addEventListener(\"click\", open);\n                    }\n                }\n            }\n        }\n    },\n\n    init(arg){\n        this.bind(arg);\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/accordion/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/include/index.js":
/*!***************************************************!*\
  !*** ./assets/js/helper/plugins/include/index.js ***!
  \***************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    \"baseTag\":\"include\",\n    \"attrTag\":\"component\",\n    \"baseDir\":\"components\",\n\n    componentFilePath(componentName){\n        return `./${this.baseDir}/${componentName}/index.html`;\n    },\n\n    start(){\n        let tags = document.getElementsByTagName(this.baseTag);\n            tags = Object.values(tags);\n        let len = tags.length;\n    \n        if(tags && tags.length > 0){\n            tags.forEach((element, index) => {\n                let component = element.getAttribute(this.attrTag);\n\n                if (component){\n                    let file = this.componentFilePath(component);\n    \n                    fetch(file).then((response) =>\n                        response.status === 200 ? response.text() : \"\"\n                    ).then((html) => {\n                        element.innerHTML = html;\n                        if(len === (index+1)){\n                            setTimeout(()=>{\n                                helpers.page.onInclude();\n                            }, 10);\n                        }\n                    });\n                }\n            });\n        }else{\n            helpers.page.onInclude(true);\n        }\n    },\n\n    init(){\n        this.start();\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/include/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/index.js":
/*!*******************************************!*\
  !*** ./assets/js/helper/plugins/index.js ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _owl___WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./owl/ */ \"./assets/js/helper/plugins/owl/index.js\");\n/* harmony import */ var _menu___WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./menu/ */ \"./assets/js/helper/plugins/menu/index.js\");\n/* harmony import */ var _input___WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./input/ */ \"./assets/js/helper/plugins/input/index.js\");\n/* harmony import */ var _video___WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./video/ */ \"./assets/js/helper/plugins/video/index.js\");\n/* harmony import */ var _toggle___WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./toggle/ */ \"./assets/js/helper/plugins/toggle/index.js\");\n/* harmony import */ var _include___WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./include/ */ \"./assets/js/helper/plugins/include/index.js\");\n/* harmony import */ var _accordion___WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./accordion/ */ \"./assets/js/helper/plugins/accordion/index.js\");\n/* harmony import */ var _validation__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./validation */ \"./assets/js/helper/plugins/validation/index.js\");\n\n\n\n\n\n\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    owl:_owl___WEBPACK_IMPORTED_MODULE_0__[\"default\"],\n    menu:_menu___WEBPACK_IMPORTED_MODULE_1__[\"default\"],\n    input:_input___WEBPACK_IMPORTED_MODULE_2__[\"default\"],\n    video:_video___WEBPACK_IMPORTED_MODULE_3__[\"default\"],\n    toggle:_toggle___WEBPACK_IMPORTED_MODULE_4__[\"default\"],\n    include:_include___WEBPACK_IMPORTED_MODULE_5__[\"default\"],\n    accordion:_accordion___WEBPACK_IMPORTED_MODULE_6__[\"default\"],\n    validation:_validation__WEBPACK_IMPORTED_MODULE_7__[\"default\"]\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/input/index.js":
/*!*************************************************!*\
  !*** ./assets/js/helper/plugins/input/index.js ***!
  \*************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    bind(elm){\n        const onKeyup = (e) => {\n            let cls = 'filled';\n            let elm = e.currentTarget;\n\n            if((elm.value+'').length > 0){\n                helpers.element.class.add(elm, cls);\n            }else{\n                helpers.element.class.remove(elm, cls)\n            }\n        };\n    \n        elm.addEventListener('keyup', onKeyup);\n    },\n\n    init(){\n        let inputs = document.getElementsByClassName('input');\n\n        for(let a in inputs){\n            let elm = inputs[a];\n            let type = elm.tagName;\n\n            console.log(type);\n            switch(type) {\n                case 'INPUT':\n                  this.bind(elm);\n                break;\n                case 'TEXTAREA':\n                    this.bind(elm);\n                break;\n                default:\n                  \n              }\n        };\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/input/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/menu/index.js":
/*!************************************************!*\
  !*** ./assets/js/helper/plugins/menu/index.js ***!
  \************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    config:{\n        closer:'menu-closer',\n        holder:'menuHolder',\n        opener:'menuOpener',\n        openCls:'menu-opened'\n    },\n\n    closeMenu(arg){\n        let conf = this.config;\n        let oc = conf.openCls;\n\n        if(arg && arg.openCls){\n            oc = arg.openCls;\n        };\n        helpers.element.class.remove(document.body, oc);\n    },\n\n    menuToggle(arg, id){\n        let that = this;\n        let conf = this.config;\n        let oc = conf.openCls;\n\n        if(arg && arg.openCls){\n            oc = arg.openCls;\n        };\n\n        let elm = document.getElementById(id);\n\n        let open = function(e){\n            helpers.element.class.toggle(document.body, oc);\n        };\n\n        if(elm){\n            elm.addEventListener(\"click\", open);\n        }\n    },\n\n    getElement(arg, type, idOnly){\n        let conf = this.config;\n        let id = conf[type];\n\n        if(arg && arg[type]){\n            id = arg[type];\n        };\n\n        if(idOnly){\n            return id\n        }else{\n            return document.getElementById(id)\n        }\n    },\n\n    intToggle(arg, type){\n        let id = this.getElement(arg, type, true);\n\n        if(id){\n            this.menuToggle(arg, id)\n        }\n    },\n\n    initCloser(arg){\n        let that = this;\n        let holder = this.getElement(arg, 'holder')\n        let closer = function(e){\n            let elm = e.target;\n\n            if(elm === holder){\n                that.closeMenu(arg);\n            }else{\n                let conf = that.config;\n                let cls = conf.closer;\n\n                if(arg && arg.closer){\n                    cls = arg.closer;\n                };\n\n                if(cls){\n                    let isCloser = helpers.element.class.has(elm, cls);\n                    if(isCloser){\n                        that.closeMenu(arg);\n                    }\n                }\n            }\n        }\n\n        window.addEventListener('click', closer);\n    },\n\n    init(arg){\n        this.intToggle(arg, 'opener');\n        this.initCloser(arg);\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/menu/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/owl/index.js":
/*!***********************************************!*\
  !*** ./assets/js/helper/plugins/owl/index.js ***!
  \***********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(id, arg){\n        if(id && $(id).owlCarousel){\n            let conf = arg?arg:{};\n                conf.loop = true;\n                conf.autoplay = true;\n                conf.autoplayTimeout = 5000;\n                conf.autoplayHoverPause = true;\n\n            $(id).owlCarousel(conf);\n        }\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/owl/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/toggle/index.js":
/*!**************************************************!*\
  !*** ./assets/js/helper/plugins/toggle/index.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(arg){\n        if(arg && arg.action && arg.target && arg.class){\n            let target = arg.target;\n            let elmH = helpers.element;\n            let action = elmH.get.byId(arg.action);\n\n            if(typeof target === 'string'){\n                target = elmH.get.byId(arg.target);\n            }\n\n            let toggle = () => {\n                elmH.class.toggle(target, arg.class);\n            }\n\n            if(action && target){\n                action.addEventListener(\"click\", toggle);\n            }\n        }\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/toggle/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/validation/index.js":
/*!******************************************************!*\
  !*** ./assets/js/helper/plugins/validation/index.js ***!
  \******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _ui___WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./ui/ */ \"./assets/js/helper/plugins/validation/ui/index.js\");\n/* harmony import */ var _validator___WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./validator/ */ \"./assets/js/helper/plugins/validation/validator/index.js\");\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    ui:_ui___WEBPACK_IMPORTED_MODULE_0__[\"default\"],\n    start(arg, data){\n        return _validator___WEBPACK_IMPORTED_MODULE_1__[\"default\"].init(arg, data);\n    },\n\n    init(arg){\n        let resp = this.ui.init(arg);\n        let form = this.start(resp.validation, resp.data);\n        let error = this.ui.errors(form, arg);\n        return form\n    }\n    \n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/validation/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/validation/ui/index.js":
/*!*********************************************************!*\
  !*** ./assets/js/helper/plugins/validation/ui/index.js ***!
  \*********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    config:{\n        attr:{\n            min:'data-input-min',\n            max:'data-input-max',\n            regex:'data-input-regex',\n            error:'data-input-error',\n            success:'data-input-success',\n            required:\"data-input-required\",\n            validation:'[data-has-validation=\"enable\"]'\n        }\n    },\n\n    toggle(elm, error){\n        if(elm && elm.classList){\n            let cls = 'error';\n            let clsl = elm.classList;\n\n            if(error) {\n                if(clsl.contains(cls)){\n\n                }else{\n                    clsl.add(cls);\n                }\n            }else{\n                if(clsl.contains(cls)){\n                    clsl.remove(cls);\n                }\n            }\n        }\n    },\n\n    errors(validation, arg){\n        if(arg && arg.form && arg.form.id){\n            let attrs = this.config.attr;\n            let fileds = validation.fields;\n            let form = document.getElementById(arg.form.id);\n\n            if(form){\n                let inputs = form.querySelectorAll(attrs.validation);\n                for(let a in inputs){\n                    let input = inputs[a];\n\n                    if(input.getAttribute){\n                        let name = input.getAttribute('name');\n\n                        if(fileds[name]){\n                            this.toggle(input, fileds[name].error);\n                        }\n                    }\n                }\n            }\n        }\n    },\n\n    getAttr(elm, attr, fb){\n        let val = elm.getAttribute(attr);\n\n        if(val){\n            return val;\n        }else{\n            if(typeof val != 'undefined'){\n                return fb;\n            }\n        }\n\n        return false;\n    },\n\n    validation(elm){\n        let attrs = this.config.attr;\n        let min = this.getAttr(elm, attrs.min, false);\n        let max = this.getAttr(elm, attrs.max, false);\n        let regex = this.getAttr(elm, attrs.regex, false);\n        let success = this.getAttr(elm, attrs.success, 'Valid');\n        let error = this.getAttr(elm, attrs.error, 'Invalid input');\n        let required = this.getAttr(elm, attrs.required, 'optional');\n        let message = {\n            error:error,\n            success:success,\n        };\n        \n        return {\n            message:message,\n            required:required,\n            regex:{\n                value:regex,\n                message:message\n            },\n            length:{\n                min:min,\n                max:max,\n                message:message\n            },\n        }\n    },\n\n    parse(elm){\n        let rval = {\n            value:''\n        };\n\n        let id = elm.getAttribute('id');\n        let name = elm.getAttribute('name');\n        let random = Math.floor(100000 + Math.random() * 900000);\n\n        if(!id){\n            id = random;\n            elm.setAttribute('id', id);\n        }\n\n        if(!name){\n            name = random;\n            elm.setAttribute('name', name);\n        }\n\n        rval.name = name;\n        rval.value = elm.value;\n        rval.validation = this.validation(elm);\n\n        return rval;\n    },\n\n    init(arg){\n        let rval = {\n            data:{},\n            validation:{}\n        };\n        if(arg && arg.form && arg.form.id){\n            rval.form = arg.form;\n            let attrs = this.config.attr;\n            let form = document.getElementById(arg.form.id);\n\n            if(form){\n                let inputs = form.querySelectorAll(attrs.validation);\n\n                for(let a in inputs){\n                    let input = inputs[a];\n\n                    if(input.getAttribute){\n                        let resp = this.parse(input);\n                        let name = resp.name; \n                        rval.data[name] = resp.value;\n                        rval.validation[name] = resp.validation;\n                    }\n                };\n            }\n        }\n\n        return rval;\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/validation/ui/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/validation/validator/get.js":
/*!**************************************************************!*\
  !*** ./assets/js/helper/plugins/validation/validator/get.js ***!
  \**************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    value(arg, map, fallback){\n\t\tlet rv = 'NA';\n\n\t\tif(arg && (map || map === '')){\n                rv = arg;\n            let sm = map.split('.');\n\t\t\t\n            for(var a in sm){\n\t\t\t\tif(typeof rv[sm[a]] != 'undefined'){\n\t\t\t\t\trv = rv[sm[a]];\n\t\t\t\t}else{\n\t\t\t\t\trv = 'NA';\n\t\t\t\t\tbreak;\n\t\t\t\t}\n\t\t\t}\n\t\t};\n\n\t\tif((typeof fallback != 'undefined') && (rv === 'NA')){\n\t\t\treturn fallback;\n\t\t};\n\n        if(rv != 'NA'){\n            return rv;\n        }else{\n            return ''\n        }\n\t},\n    \n    message(arg, type, fallback){\n        return this.value(arg, (`message.${type}`), fallback);\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/validation/validator/get.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/validation/validator/index.js":
/*!****************************************************************!*\
  !*** ./assets/js/helper/plugins/validation/validator/index.js ***!
  \****************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _get__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./get */ \"./assets/js/helper/plugins/validation/validator/get.js\");\n/* harmony import */ var _set__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./set */ \"./assets/js/helper/plugins/validation/validator/set.js\");\n/* harmony import */ var _regex__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./regex */ \"./assets/js/helper/plugins/validation/validator/regex.js\");\n/* harmony import */ var _length__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./length */ \"./assets/js/helper/plugins/validation/validator/length.js\");\nlet conf = {\n    'name':{\n        regex:'name',\n        required:'required',\n        length:{\n            min:6,\n            message:{\n\n            }\n        },\n        message:{\n            success:'Passed',\n            error:'Name only can cantain alpha bets.'\n        }\n    },\n\n    'email':{\n        regex:{\n            map:'',\n            value:'^(([^<>()[\\]\\\\.,;:\\s@\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$'\n        },\n        required:'optional',\n        message:{\n            error:'Please a valid email id.'\n        }\n    }\n}\n\nconst form = {\n    name:'dddd',\n    email:''\n};\n\n\n\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\n    schema(val){\n        return {\n            message:'',\n            valid:true,\n            error:false,\n            value:(val?val:'')\n        }\n    },\n\n    required(arg){\n        let value = '';\n        let option = arg.required;\n        let error = _get__WEBPACK_IMPORTED_MODULE_0__[\"default\"].message(arg, 'error');\n        let success = _get__WEBPACK_IMPORTED_MODULE_0__[\"default\"].message(arg, 'success');\n\n        if(arg.value){\n            value = (arg.value+'')\n        }\n\n        switch(option) {\n            case 'optional':\n                arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, true, success);\n            break;\n            case 'required':\n                if(value.length > 0){\n                    arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, true, success);\n                }else{\n                    arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, false, error);\n                }\n            break;\n            default:\n        };\n\n        return arg.validation;\n    },\n\n    start(arg){\n        let res = this.required(arg);\n\n        if(res.valid){\n            res = _length__WEBPACK_IMPORTED_MODULE_3__[\"default\"].init(arg);\n        }\n\n        if(res.valid){\n            res = _regex__WEBPACK_IMPORTED_MODULE_2__[\"default\"].init(arg);\n        }\n\n        return res;\n    },\n\n    init(a, d){\n        let arg = (a?a:{});\n        let data = (d?d:{});\n        let rval = {\n            fields:{},\n            valid:true,\n        };\n\n        \n\n        for(let a in arg){\n            let item = (arg[a]?arg[a]:{});\n                item.value = _get__WEBPACK_IMPORTED_MODULE_0__[\"default\"].value(data, a);\n                item.validation = this.schema(item.value);\n\n            let res = this.start(item);\n\n            if(rval.valid){\n                rval.valid = res.valid;\n            }\n\n            rval.fields[a] = res;\n        }\n\n        return rval;\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/validation/validator/index.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/validation/validator/length.js":
/*!*****************************************************************!*\
  !*** ./assets/js/helper/plugins/validation/validator/length.js ***!
  \*****************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _get__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./get */ \"./assets/js/helper/plugins/validation/validator/get.js\");\n/* harmony import */ var _set__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./set */ \"./assets/js/helper/plugins/validation/validator/set.js\");\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n\n    message(arg, message, success){\n        if(message){\n            return message;\n        }else{\n            if(success){\n                return message;\n            }else{\n                let min = arg.min;\n                let max = arg.max;\n    \n                if(min > 0 && max > 0){\n                    return (`Length range is ${min} to ${max}`)\n                }else{\n                    if(min > 0){\n                        return (`Minimum required length is ${min}`)\n                    }else{\n                        if(max > 0){\n                            return (`Length can't be more than ${max}`)\n                        }\n                    }\n                }\n            }\n        }\n    },\n\n    init(arg){\n        let value = '';\n\n        if(arg.value){\n            value = (arg.value+'')\n        }\n\n        if(arg.length){\n            let min = arg.length.min;\n            let max = arg.length.max;\n            let error = _get__WEBPACK_IMPORTED_MODULE_0__[\"default\"].message(arg.length, 'error');\n            let success = _get__WEBPACK_IMPORTED_MODULE_0__[\"default\"].message(arg.length, 'success');\n                error = this.message(arg.length, error);\n                success = this.message(arg.length, success, true);\n\n            if((min && min > 0) || (max && max > 0)){\n                if(min && min > 0){\n                    if(value.length >= min){\n                        arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, true, success);\n                        if(max && max > 0){\n                            if(value.length <= max){\n                                arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, true, success);\n                            }else{\n                                arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, false, error);\n                            }\n                        }\n                    }else{\n                        arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, false, error);\n                    }\n                }else{\n                    if(max && max > 0){\n                        if(value.length <= max){\n                            arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, true, success);\n                        }else{\n                            arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, false, error);\n                        }\n                    }\n                }\n            }\n        }\n\n        return arg.validation;\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/validation/validator/length.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/validation/validator/regex.js":
/*!****************************************************************!*\
  !*** ./assets/js/helper/plugins/validation/validator/regex.js ***!
  \****************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _get__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./get */ \"./assets/js/helper/plugins/validation/validator/get.js\");\n/* harmony import */ var _set__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./set */ \"./assets/js/helper/plugins/validation/validator/set.js\");\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({ \n\n    regex(arg){\n        let regx = arg.value;\n\n        if(regx){\n            if(regx instanceof RegExp){\n                return regx;\n            }else{\n                return new RegExp(regx)   \n            }\n        }\n\n        return false;\n    },\n\n    init(arg){\n        let value = '';\n\n        if(arg.value){\n            value = (arg.value+'')\n        }\n\n        if(arg.regex){\n            let regex = this.regex(arg.regex);\n\n            if(regex){\n                let error = _get__WEBPACK_IMPORTED_MODULE_0__[\"default\"].message(arg.regex, 'error');\n                let success = _get__WEBPACK_IMPORTED_MODULE_0__[\"default\"].message(arg.regex, 'success');\n\n                if(value.match(regex)){\n                    arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, true, success);\n                }else{\n                    arg.validation = _set__WEBPACK_IMPORTED_MODULE_1__[\"default\"].validation(arg.validation, false, error);\n                }\n            }\n\n        }\n\n        return arg.validation;\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/validation/validator/regex.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/validation/validator/set.js":
/*!**************************************************************!*\
  !*** ./assets/js/helper/plugins/validation/validator/set.js ***!
  \**************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    validation(a, v, m){\n\n\t\tlet rval = (a?a:{\n            valid:v,\n\t\t\tvalue:'',\n            error:(!v),\n\t\t\tmessage:''\n\t\t});\n\n\t\trval.valid = v;\n\t\trval.error = (!v);\n\t\trval.message = (m?m:'');\n\n\t\treturn rval;\n\t}\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/validation/validator/set.js?");

/***/ }),

/***/ "./assets/js/helper/plugins/video/index.js":
/*!*************************************************!*\
  !*** ./assets/js/helper/plugins/video/index.js ***!
  \*************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    class(arg, played){\n        let elm = document.getElementById(arg.holder);\n\n        if(elm){\n            let pCls = (arg.playClass?arg.playClass:'played');\n            if(played){\n                helpers.element.class.add(elm, pCls);\n            }else{\n                helpers.element.class.remove(elm, pCls);\n            }\n        }\n    },\n\n    init(arg){\n\n        if(arg && arg.btn && arg.player){\n            let that = this;\n            let elm = document.getElementById(arg.btn);\n\n            let playPause = function(e){\n                let video = document.getElementById(arg.player);\n\n                if(video){\n                    if(video.paused){\n                        video.play();\n                        that.class(arg, true); \n                    }else{\n                        video.pause();\n                        that.class(arg, false); \n                    }\n                }\n            };\n\n        if(elm){\n            elm.addEventListener(\"click\", playPause);\n        }\n        }\n        \n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/plugins/video/index.js?");

/***/ }),

/***/ "./assets/js/helper/user-agent/index.js":
/*!**********************************************!*\
  !*** ./assets/js/helper/user-agent/index.js ***!
  \**********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    get(){\n        const ua = navigator.userAgent;\n        if (/(tablet|ipad|playbook|silk)|(android(?!.*mobi))/i.test(ua)) {\n            return \"tablet\";\n        }\n        else if (/Mobile|Android|iP(hone|od)|IEMobile|BlackBerry|Kindle|Silk-Accelerated|(hpw|web)OS|Opera M(obi|ini)/.test(ua)) {\n            return \"mobile\";\n        }\n        return \"desktop\";\n    },\n\n    is(type){\n        let device = this.get();\n        return (device === type);\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/helper/user-agent/index.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = __webpack_require__("./assets/js/helper/index.js");
/******/ 	
/******/ })()
;