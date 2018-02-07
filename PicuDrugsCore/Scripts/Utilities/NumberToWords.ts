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

export function seperate(d: number, seperator: string = ' ') {
    var parts = d.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, seperator);
    return parts.join(".");
}

export enum exceed { equal, gt, lt }

export interface largenumber {digits: string, exp10:number, suffixName:string, exceeds: exceed}

export function largeNumberWords(largeNumber: number) : largenumber {
    const suffixName = ['thousand','million','billion','trillion','quadrillion','quintillion','sextillion','septillion','octillion','nonillion','decillion','undecillion','duodecillion','tredecillion','quattuordecillion','quindecillion','sexdecillion','septendecillion','octodecillion','novemdecillion','vigintillion','unvigintillion','duovigintillion','trevigintillion','quattuorvigintillion','quinvigintillion','sexvigintillion','septenvigintillion','octovigintillion','novemvigintillion','trigintillion','untrigintillion','duotrigintillion','tretrigintillion','quattuortrigintillion','quintrigintillion','sextrigintillion','septentrigintillion','octotrigintillion','novemtrigintillion','quadragintillion','unquadragintillion','duoquadragintillion','trequadragintillion','quattuorquadragintillion','quinquadragintillion','sexquadragintillion','septenquadragintillion','octoquadragintillion','novemquadragintillion','quinquagintillion','unquinquagintillion','duoquinquagintillion','trequinquagintillion','quattuorquinquagintillion','quinquinquagintillion','sexquinquagintillion','septenquinquagintillion','octoquinquagintillion','novemquinquagintillion','sexagintillion','unsexagintillion','duosexagintillion','tresexagintillion','quattuorsexagintillion','quinsexagintillion','sexsexagintillion','septsexagintillion','octosexagintillion','novemsexagintillion','septuagintillion','unseptuagintillion','duoseptuagintillion','treseptuagintillion','quattuorseptuagintillion','quinseptuagintillion','sexseptuagintillion','septseptuagintillion','octoseptuagintillion','novemseptuagintillion','octogintillion','unoctogintillion','duooctogintillion','treoctogintillion','quattuoroctogintillion','quinoctogintillion','sexoctogintillion','septoctogintillion','octooctogintillion','novemoctogintillion','nonagintillion','unnonagintillion','duononagintillion','trenonagintillion','quattuornonagintillion','quinnonagintillion','sexnonagintillion','septnonagintillion','octononagintillion','novenonagintillion','centillion','uncentillion ','duocentillion'],
        log10 = Math.log(10),
        infiniteVal = !isFinite(largeNumber);
    let absVal = Math.abs(largeNumber);
    if (absVal < 10000) {
        return {
            digits: seperate(Math.floor(largeNumber)), 
            suffixName: '',
            exceeds: exceed.equal,
            exp10:0
        }
    }
    if (infiniteVal) {
        absVal = Number.MAX_VALUE;
    }
    const logVal = Math.log(absVal) / log10,
        logMultiple3 = Math.floor((logVal) / 3) * 3;
    let lookupVal = (logMultiple3 / 3) - 1;
    return {
        digits: (largeNumber<0?'-':'') + (absVal / Math.pow(10, logMultiple3)).toPrecision((logVal - logMultiple3) >= 2 ? 3 : 2),
        suffixName: suffixName[lookupVal],
        exp10:logMultiple3,
        exceeds: infiniteVal
            ? largeNumber === Number.POSITIVE_INFINITY
                ? exceed.gt
                : exceed.lt
            :exceed.equal
    }

}