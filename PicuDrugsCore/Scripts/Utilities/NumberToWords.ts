export function getSuffix(inpt: number) {
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

export function seperate(d: number, seperator: string) {
    var parts = d.toString().split(".");
    if (typeof (seperator) == 'undefined') { seperator = " "; }
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, seperator);
    return parts.join(".");
}

export enum exceed { equal, gt, lt }

export interface largenumber {digits: string, suffix:string, exceeds: exceed}

export function largeNumberWords(largeNumber: number) : largenumber {
    const suffix = ['Thousand', 'Million', 'Billion', 'Trillion', 'Quadrillion', 'Quintillion', 'Sextillion', 'Septillion', 'Octillion', 'Nonillion', 'Decillion', 'Undecillion', 'Duodecillion', 'Tredecillion', 'Quattuordecillion', 'Quindecillion', 'Sexdecillion', 'Septendecillion', 'Octodecillion', 'Novemdecillion', 'Vigintillion', 'Centillion'],
        log10 = Math.log(10),
        infiniteVal = !isFinite(largeNumber);
    let absVal = Math.abs(largeNumber);
    if (absVal < 10000) {
        return {
            digits: largeNumber.toString(), 
            suffix: '',
            exceeds: exceed.equal
        }
    }
    if (infiniteVal) {
        absVal = Number.MAX_VALUE;
    }
    const logVal = Math.log(absVal) / log10,
        logMultiple3 = Math.floor((logVal) / 3) * 3;
    let lookupVal = (logMultiple3 / 3) - 1;
    if (lookupVal > 21) { lookupVal = 21; }
    return {
        digits: (largeNumber<0?'-':'') + (absVal / Math.pow(10, logMultiple3)).toPrecision((logVal - logMultiple3) >= 2 ? 3 : 2),
        suffix: suffix[lookupVal],
        exceeds: infiniteVal
            ? largeNumber === Number.POSITIVE_INFINITY
                ? exceed.gt
                : exceed.lt
            :exceed.equal
    }

}