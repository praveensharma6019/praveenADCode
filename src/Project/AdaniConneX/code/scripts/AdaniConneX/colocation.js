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

/***/ "scripts/AdaniConneX/components/awards.js":
/*!****************************************!*\
  !*** ./assets/js/components/awards.js ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

                eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n        helpers.plugins.owl.init('#awardsList', {\n            items:2,\n            responsiveClass: true,\n            responsive: {\n                0:{\n                    dots:false,\n                    items:1.1\n                },\n                1340:{\n                    items:2,\n                    dots:true\n                }\n            }\n        });\n    }\n});\n\n//# sourceURL=webpack://electricity/./scripts/AdaniConneX/components/awards.js?");

                /***/
            }),

/***/ "scripts/AdaniConneX/components/contact-us.js":
/*!********************************************!*\
  !*** ./assets/js/components/contact-us.js ***!
  \********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

                eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n        let cls = 'contact-active';\n        let target = 'contactUsHolder'\n        helpers.plugins.toggle.init({\n            class:cls,\n            target:target,\n            action:'contactUsBtn'\n        });\n\n        helpers.plugins.toggle.init({\n            class:cls,\n            target:target,\n            action:'contactUsBackdrop'\n        });\n    }\n});\n\n//# sourceURL=webpack://electricity/./scripts/AdaniConneX/components/contact-us.js?");

                /***/
            }),

/***/ "scripts/AdaniConneX/components/global.js":
/*!****************************************!*\
  !*** ./assets/js/components/global.js ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

                eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _search__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./search */ \"scripts/AdaniConneX/components/search.js\");\n/* harmony import */ var _contact_us__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./contact-us */ \"scripts/AdaniConneX/components/contact-us.js\");\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n        _search__WEBPACK_IMPORTED_MODULE_0__[\"default\"].init();\n        _contact_us__WEBPACK_IMPORTED_MODULE_1__[\"default\"].init();\n        helpers.plugins.menu.init();\n        helpers.plugins.accordion.init();\n    }\n});\n\n//# sourceURL=webpack://electricity/./scripts/AdaniConneX/components/global.js?");

                /***/
            }),

/***/ "scripts/AdaniConneX/components/search.js":
/*!****************************************!*\
  !*** ./assets/js/components/search.js ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

                eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n\n        let bdy = document.body\n        let cls = 'mobile-search-opened';\n        \n        helpers.plugins.toggle.init({\n            class:cls,\n            target:bdy,\n            action:'mobileSearch'\n        });\n\n        helpers.plugins.toggle.init({\n            class:cls,\n            target:bdy,\n            action:'mobileSearchClose'\n        })\n    }\n});\n\n//# sourceURL=webpack://electricity/./scripts/AdaniConneX/components/search.js?");

                /***/
            }),

/***/ "scripts/AdaniConneX/pages/colocation/index.js":
/*!*********************************************!*\
  !*** ./assets/js/pages/colocation/index.js ***!
  \*********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

                eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _components_awards__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./../../components/awards */ \"scripts/AdaniConneX/components/awards.js\");\n/* harmony import */ var _components_global__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./../../components/global */ \"scripts/AdaniConneX/components/global.js\");\n\n\n\n(function () {\n    setTimeout(() => {\n        _components_global__WEBPACK_IMPORTED_MODULE_1__[\"default\"].init();\n        _components_awards__WEBPACK_IMPORTED_MODULE_0__[\"default\"].init();\n\n        // Tab\n        $(\".tab-title-wrp ul li a\").click((e) => {\n            const $this = $(e.currentTarget);\n\n            const tabAttr = $this.attr(\"title\");\n            $(\".tab-acc-content\").hide();\n            $(`.tab-acc-content[tab-content=\"${tabAttr}\"]`).show();\n\n            $this.parent().siblings().find(\"a\").removeClass(\"active\");\n            $this.toggleClass(\"active\");\n        });\n\n        // Accordion\n        $(\".acc-title\").click((e) => {\n            const $this = $(e.currentTarget);\n\n            const tabAttr = $this.attr(\"title\");\n            $this\n                .closest(\".acc-wrp\")\n                .siblings()\n                .find(\".tab-acc-content\")\n                .slideUp();\n            $(`.tab-acc-content[tab-content=\"${tabAttr}\"]`).slideToggle();\n\n            $this\n                .closest(\".acc-wrp\")\n                .siblings()\n                .find(\".acc-title\")\n                .removeClass(\"active\");\n            $this\n                .closest(\".acc-wrp\")\n                .siblings()\n                .removeClass(\"active\");\n            $this.toggleClass(\"active\");\n            $this.parent().toggleClass(\"active\");\n        });\n\n        // Hero banner arrow click\n        console.log(\"Hero banner arrow click\");\n        $(\".hero-section .banner-arrow\").click(() => {\n            const bannerHeight = $(\".hero-section\").outerHeight();\n            const bannerOffset = $(\".hero-section\").offset().top;\n            const scrollPos = bannerHeight + bannerOffset;\n\n            $(\"body, html\").animate({ scrollTop: scrollPos });\n        });\n    }, 1000);\n})();\n\n\n//# sourceURL=webpack://electricity/./scripts/AdaniConneX/pages/colocation/index.js?");

                /***/
            })

        /******/
    });
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
            /******/
        }
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
            /******/
        };
/******/
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
        /******/
    }
/******/
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for (var key in definition) {
/******/ 				if (__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
                    /******/
                }
                /******/
            }
            /******/
        };
        /******/
    })();
/******/
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
        /******/
    })();
/******/
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if (typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
                /******/
            }
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
            /******/
        };
        /******/
    })();
/******/
/************************************************************************/
/******/
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = __webpack_require__("scripts/AdaniConneX/pages/colocation/index.js");
    /******/
    /******/
})()
    ;