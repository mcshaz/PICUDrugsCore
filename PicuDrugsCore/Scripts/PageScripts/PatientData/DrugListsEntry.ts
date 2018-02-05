import Vue from 'vue'
import weightage from './weightage.vue'

let vm = new Vue({
    el: '#drug-list',
    render: h=>h(weightage)
});

export default vm;