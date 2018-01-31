"use strict";
function CloneProtoProperties(proto) {
    var returnVar = {};
    for (var p in proto) {
        if (p && !(p === 'constructor' || p[0] === '_')) {
            returnVar[p] = proto[p];
        }
    }
    return returnVar;
}
//# sourceMappingURL=CloneProtoProperties.js.map