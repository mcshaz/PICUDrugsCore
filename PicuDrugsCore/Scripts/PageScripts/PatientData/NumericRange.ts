export class NumericRange {
    public readonly Min: number;
    public readonly Max: number;
    public readonly NonRange: boolean;

    constructor(min: number, max?: number) {
        if (max === void 0) {
            this.Min = this.Max = min;
            this.NonRange = true;
        } else {
            if (max < min) {
                throw new Error("max must be > min");
            }
            this.Min = min;
            this.Max = max;
            this.NonRange = min === max;
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
