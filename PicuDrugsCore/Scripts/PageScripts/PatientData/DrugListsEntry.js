"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var vue_1 = require("vue");
var weightage_vue_1 = require("./weightage.vue");
var vm = new vue_1.default({
    el: '#drug-list',
    render: function (h) { return h(weightage_vue_1.default); }
});
exports.default = vm;
//# sourceMappingURL=DrugListsEntry.js.map