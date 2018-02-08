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
    if (seperator === void 0) { seperator = ' '; }
    var parts = d.toString().split(".");
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
    var suffixName = ['thousand', 'million', 'billion', 'trillion', 'quadrillion', 'quintillion', 'sextillion', 'septillion', 'octillion', 'nonillion', 'decillion', 'undecillion', 'duodecillion', 'tredecillion', 'quattuordecillion', 'quindecillion', 'sexdecillion', 'septendecillion', 'octodecillion', 'novemdecillion', 'vigintillion', 'unvigintillion', 'duovigintillion', 'trevigintillion', 'quattuorvigintillion', 'quinvigintillion', 'sexvigintillion', 'septenvigintillion', 'octovigintillion', 'novemvigintillion', 'trigintillion', 'untrigintillion', 'duotrigintillion', 'tretrigintillion', 'quattuortrigintillion', 'quintrigintillion', 'sextrigintillion', 'septentrigintillion', 'octotrigintillion', 'novemtrigintillion', 'quadragintillion', 'unquadragintillion', 'duoquadragintillion', 'trequadragintillion', 'quattuorquadragintillion', 'quinquadragintillion', 'sexquadragintillion', 'septenquadragintillion', 'octoquadragintillion', 'novemquadragintillion', 'quinquagintillion', 'unquinquagintillion', 'duoquinquagintillion', 'trequinquagintillion', 'quattuorquinquagintillion', 'quinquinquagintillion', 'sexquinquagintillion', 'septenquinquagintillion', 'octoquinquagintillion', 'novemquinquagintillion', 'sexagintillion', 'unsexagintillion', 'duosexagintillion', 'tresexagintillion', 'quattuorsexagintillion', 'quinsexagintillion', 'sexsexagintillion', 'septsexagintillion', 'octosexagintillion', 'novemsexagintillion', 'septuagintillion', 'unseptuagintillion', 'duoseptuagintillion', 'treseptuagintillion', 'quattuorseptuagintillion', 'quinseptuagintillion', 'sexseptuagintillion', 'septseptuagintillion', 'octoseptuagintillion', 'novemseptuagintillion', 'octogintillion', 'unoctogintillion', 'duooctogintillion', 'treoctogintillion', 'quattuoroctogintillion', 'quinoctogintillion', 'sexoctogintillion', 'septoctogintillion', 'octooctogintillion', 'novemoctogintillion', 'nonagintillion', 'unnonagintillion', 'duononagintillion', 'trenonagintillion', 'quattuornonagintillion', 'quinnonagintillion', 'sexnonagintillion', 'septnonagintillion', 'octononagintillion', 'novenonagintillion', 'centillion', 'uncentillion ', 'duocentillion'], log10 = Math.log(10), infiniteVal = !isFinite(largeNumber);
    var absVal = Math.abs(largeNumber);
    if (absVal < 10000) {
        return {
            digits: seperate(Math.floor(largeNumber)),
            suffixName: '',
            exceeds: exceed.equal,
            exp10: 0
        };
    }
    if (infiniteVal) {
        absVal = Number.MAX_VALUE;
    }
    var logVal = Math.log(absVal) / log10, logMultiple3 = Math.floor((logVal) / 3) * 3;
    var lookupVal = (logMultiple3 / 3) - 1;
    return {
        digits: (largeNumber < 0 ? '-' : '') + (absVal / Math.pow(10, logMultiple3)).toPrecision((logVal - logMultiple3) >= 2 ? 3 : 2),
        suffixName: suffixName[lookupVal],
        exp10: logMultiple3,
        exceeds: infiniteVal
            ? largeNumber === Number.POSITIVE_INFINITY
                ? exceed.gt
                : exceed.lt
            : exceed.equal
    };
}
exports.largeNumberWords = largeNumberWords;
//# sourceMappingURL=NumberToWords.js.map