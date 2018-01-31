function CloneProtoProperties(proto: object) {
    let returnVar = {};
    for (let p in proto) {
        if (p && !(p === 'constructor' || p[0] === '_')) {
            returnVar[p] = proto[p];
        }
    }
    return returnVar;
}