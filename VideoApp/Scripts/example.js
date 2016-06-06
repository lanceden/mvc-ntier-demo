/**
 * This tiny script just helps us demonstrate
 * what the various example callbacks are doing
 */
var Example = (function () {
    "use strict";

    var elem,
        hideHandler,
        that = {};

    that.init = function (ele) {
        elem = $(ele);
    };

    that.show = function (text) {
        clearTimeout(hideHandler);
        document.getElementById("spanAlert").innerHTML = text;
        elem.delay(200).fadeIn().delay(5000).fadeOut();
    };
    return that;
}());