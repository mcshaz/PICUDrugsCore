"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
function getSuffix(inpt) {
    switch (inpt % 10) {
        case 1:
            return "st";
        case 2:
            return "nd";
        case 3:
            return "rd";
        default:
            return "th";
    }
}
exports.getSuffix = getSuffix;
function seperate(d, seperator) {
    var parts = d.toString().split(".");
    if (typeof (seperator) == 'undefined') {
        seperator = " ";
    }
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, seperator);
    return parts.join(".");
}
exports.seperate = seperate;
var exceed;
(function (exceed) {
    exceed[exceed["equal"] = 0] = "equal";
    exceed[exceed["gt"] = 1] = "gt";
    exceed[exceed["lt"] = 2] = "lt";
})(exceed = exports.exceed || (exports.exceed = {}));
function largeNumberWords(largeNumber) {
    var suffix = ['Thousand', 'Million', 'Billion', 'Trillion', 'Quadrillion', 'Quintillion', 'Sextillion', 'Septillion', 'Octillion', 'Nonillion', 'Decillion', 'Undecillion', 'Duodecillion', 'Tredecillion', 'Quattuordecillion', 'Quindecillion', 'Sexdecillion', 'Septendecillion', 'Octodecillion', 'Novemdecillion', 'Vigintillion', 'Centillion'], log10 = Math.log(10), infiniteVal = !isFinite(largeNumber);
    var absVal = Math.abs(largeNumber);
    if (absVal < 10000) {
        return {
            digits: largeNumber.toString(),
            suffix: '',
            exceeds: exceed.equal
        };
    }
    if (infiniteVal) {
        absVal = Number.MAX_VALUE;
    }
    var logVal = Math.log(absVal) / log10, logMultiple3 = Math.floor((logVal) / 3) * 3;
    var lookupVal = (logMultiple3 / 3) - 1;
    if (lookupVal > 21) {
        lookupVal = 21;
    }
    return {
        digits: (largeNumber < 0 ? '-' : '') + (absVal / Math.pow(10, logMultiple3)).toPrecision((logVal - logMultiple3) >= 2 ? 3 : 2),
        suffix: suffix[lookupVal],
        exceeds: infiniteVal
            ? largeNumber === Number.POSITIVE_INFINITY
                ? exceed.gt
                : exceed.lt
            : exceed.equal
    };
}
exports.largeNumberWords = largeNumberWords;
//# sourceMappingURL=NumberToWords.js.map