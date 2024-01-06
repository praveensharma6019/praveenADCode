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

/***/ "./assets/js/components/awards.js":
/*!****************************************!*\
  !*** ./assets/js/components/awards.js ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init() {\n        helpers.plugins.owl.init('#awardsList', {\n            items: 2,\n            responsiveClass: true,\n            responsive: {\n                0: {\n                    dots: false,\n                    items: 1.1\n                },\n                1340: {\n                    items: 2,\n                    dots: true\n                }\n            }\n        });\n\n        helpers.plugins.owl.init('#awardsList3', {\n            items: 3,\n            responsiveClass: true,\n            responsive: {\n                0: {\n                    dots: false,\n                    items: 1.1\n                },\n                1340: {\n                    items: 3,\n                    dots: true\n                }\n            }\n        });\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/components/awards.js?");

/***/ }),

/***/ "./assets/js/components/contact-us.js":
/*!********************************************!*\
  !*** ./assets/js/components/contact-us.js ***!
  \********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n        let cls = 'contact-active';\n        let target = 'contactUsHolder'\n        helpers.plugins.toggle.init({\n            class:cls,\n            target:target,\n            action:'contactUsBtn'\n        });\n\n        helpers.plugins.toggle.init({\n            class:cls,\n            target:target,\n            action:'contactUsBackdrop'\n        });\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/components/contact-us.js?");

/***/ }),

/***/ "./assets/js/components/explore-location.js":
/*!**************************************************!*\
  !*** ./assets/js/components/explore-location.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n        helpers.plugins.owl.init('#data-center-list', {\n            items:3,\n            responsiveClass: true,\n            responsive: {\n                0:{\n                    dots:false,\n                    items:1.1\n                },\n                1340:{\n                    items:3,\n                    dots:true\n                }\n            }\n        });\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/components/explore-location.js?");

/***/ }),

/***/ "./assets/js/components/global.js":
/*!****************************************!*\
  !*** ./assets/js/components/global.js ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var _search__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./search */ \"./assets/js/components/search.js\");\n/* harmony import */ var _contact_us__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./contact-us */ \"./assets/js/components/contact-us.js\");\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n        _search__WEBPACK_IMPORTED_MODULE_0__[\"default\"].init();\n        _contact_us__WEBPACK_IMPORTED_MODULE_1__[\"default\"].init();\n        helpers.plugins.menu.init();\n        helpers.plugins.accordion.init();\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/components/global.js?");

/***/ }),

/***/ "./assets/js/components/search.js":
/*!****************************************!*\
  !*** ./assets/js/components/search.js ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n\n        let bdy = document.body\n        let cls = 'mobile-search-opened';\n        \n        helpers.plugins.toggle.init({\n            class:cls,\n            target:bdy,\n            action:'mobileSearch'\n        });\n\n        helpers.plugins.toggle.init({\n            class:cls,\n            target:bdy,\n            action:'mobileSearchClose'\n        })\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/components/search.js?");

/***/ }),

/***/ "./assets/js/components/sustainability.js":
/*!************************************************!*\
  !*** ./assets/js/components/sustainability.js ***!
  \************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n        helpers.plugins.video.init({\n            btn:'sustainabilityVideoToggle',\n            player:'sustainabilityVideo',\n            holder:'sustainabilityVideoHolder'\n        })\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/components/sustainability.js?");

/***/ }),

/***/ "./assets/js/components/testimonial.js":
/*!*********************************************!*\
  !*** ./assets/js/components/testimonial.js ***!
  \*********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    init(){\n        helpers.plugins.owl.init('#testimonial', {items:1, dots:true});\n    }\n});\n\n//# sourceURL=webpack://electricity/./assets/js/components/testimonial.js?");

/***/ }),

/***/ "./assets/js/pages/dc-details/index.js":
/*!*********************************************!*\
  !*** ./assets/js/pages/dc-details/index.js ***!
  \*********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _components_awards__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./../../components/awards */ \"./assets/js/components/awards.js\");\n/* harmony import */ var _components_global__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./../../components/global */ \"./assets/js/components/global.js\");\n/* harmony import */ var _components_testimonial__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./../../components/testimonial */ \"./assets/js/components/testimonial.js\");\n/* harmony import */ var _components_sustainability__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./../../components/sustainability */ \"./assets/js/components/sustainability.js\");\n/* harmony import */ var _components_explore_location__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./../../components/explore-location */ \"./assets/js/components/explore-location.js\");\n\n\n\n\n\n\n\n(function(){\n\n    helpers.page.extend('dc-details', {\n        init(){\n            _components_global__WEBPACK_IMPORTED_MODULE_1__[\"default\"].init();\n            _components_awards__WEBPACK_IMPORTED_MODULE_0__[\"default\"].init();\n            _components_testimonial__WEBPACK_IMPORTED_MODULE_2__[\"default\"].init();\n            _components_sustainability__WEBPACK_IMPORTED_MODULE_3__[\"default\"].init();\n            _components_explore_location__WEBPACK_IMPORTED_MODULE_4__[\"default\"].init();\n\n            // Hero banner arrow click\n            console.log('Hero banner arrow click');\n            $('.hero-section .banner-arrow').click(() => {\n                const bannerHeight = $('.hero-section').outerHeight();\n                const bannerOffset = $('.hero-section').offset().top;\n                const scrollPos = bannerHeight + bannerOffset;\n                $('body, html').animate({scrollTop: scrollPos})\n            });\n        }\n     });\n\n     // Initialize and add the map\n        function initMap() {\n            // The location of Uluru\n            const uluru = { lat: -25.344, lng: 131.036 };\n            // The map, centered at Uluru\n            const map = new google.maps.Map(document.getElementById(\"map\"), {\n                zoom: 4,\n                center: uluru,\n            });\n            // The marker, positioned at Uluru\n            const marker = new google.maps.Marker({\n                position: uluru,\n                map: map,\n            });\n        }\n\n        setTimeout(() => {\n            initMap();\n        }, 200);\n})();\n\n//# sourceURL=webpack://electricity/./assets/js/pages/dc-details/index.js?");

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
/******/ 	var __webpack_exports__ = __webpack_require__("./assets/js/pages/dc-details/index.js");
/******/ 	
/******/ })()
;