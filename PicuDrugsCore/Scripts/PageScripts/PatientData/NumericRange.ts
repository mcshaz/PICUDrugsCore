export class NumericRange {
    public readonly min: number;
    public readonly max: number;
    public readonly nonRange: boolean;

    constructor(min: number, max?: number) {
        if (max === void 0) {
            this.min = this.max = min;
            this.nonRange = true;
        } else {
            if (max < min) {
                throw new Error("max must be >= min");
            }
            this.min = min;
            this.max = max;
            this.nonRange = min === max;
        }
    }
}

export class IntegerRange extends NumericRange {
    constructor(min: number, max?: number) {
        if (Math.floor(min) != min) {
            throw new Error("min is not an integer");
        }
        if (max !== void 0 && Math.floor(max) != max) {
            throw new Error("min is not an integer");
        }
        super(min, max);
    }
}
