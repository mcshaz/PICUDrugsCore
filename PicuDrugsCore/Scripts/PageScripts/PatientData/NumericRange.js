"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var NumericRange = (function () {
    function NumericRange(min, max) {
        if (max === void 0) {
            this.Min = this.Max = min;
            this.NonRange = true;
        }
        else {
            if (max < min) {
                throw new Error("max must be > min");
            }
            this.Min = min;
            this.Max = max;
            this.NonRange = min === max;
        }
    }
    return NumericRange;
}());
exports.NumericRange = NumericRange;
var IntegerRange = (function (_super) {
    __extends(IntegerRange, _super);
    function IntegerRange(min, max) {
        var _this = this;
        if (Math.floor(min) != min) {
            throw new Error("min is not an integer");
        }
        if (max !== void 0 && Math.floor(max) != max) {
            throw new Error("min is not an integer");
        }
        _this = _super.call(this, min, max) || this;
        return _this;
    }
    return IntegerRange;
}(NumericRange));
exports.IntegerRange = IntegerRange;
//# sourceMappingURL=NumericRange.js.map