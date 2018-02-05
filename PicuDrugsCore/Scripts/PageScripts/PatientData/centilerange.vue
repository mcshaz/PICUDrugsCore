<!-- src/components/centilerange.vue -->

<template>
    <div class="centile" :v-if="lowerVal" :class="{alert: true, 'alert-info':!warnCrossed, 'alert-warning':warnCrossed, 'alert-danger':limitCrossed }">
        <span class="lower">{{lowerVal}}<sup>{{lowerSuffix}}</sup></span>
        <span :v-if="sameVal">
            - 
            <span class="upper">{{upperVal}}<sup>{{upperSuffix}}</sup></span>
        </span>
        <span class="centileDescr">
            centile
        </span>
    </div>
</template>


<script lang="ts">
import Vue from 'vue'
import { getSuffix } from '../../Utilities/NumberToWords';
const warnCentileUbound = 99;
const warnCentileLbound = 1;
const limitCentileUbound = 100 - 1e-7;
const limitCentileLbound = 1e-12;
export default Vue.extend({
    data:function(){
        return {
            p_lowerCentile: null as null | number,
            p_upperCentile: null as null | number,
            warnOnly: false,
            limitCrossed: false,
            lowerVal: '',
            lowerSuffix: '',
            upperVal:'',
            upperSuffix:'',
            sameVal:true
        }
    },
    computed:{
        lowerCentile:{
            get: function(this:any){return this.p_lowerCentile; },
            set: function(newVal:number | null){
                this.p_lowerCentile = newVal;
                this.setWarnings();
                if (newVal === null){
                    this.lowerVal = this.lowerSuffix = '';
                } else {
                    let c = centileText(newVal);
                    this.lowerVal = c.centile;
                    this.lowerSuffix = c.suffix;
                }
                this.sameVal = this.lowerVal === this.upperVal;
            }
        },
        upperCentile:{
            get: function(this:any){return this.p_upperCentile; },
            set: function(newVal: number | null){
                this.p_upperCentile = newVal;
                this.setWarnings();
                if (newVal === null){
                    this.upperVal = this.upperSuffix = '';
                } else {
                    let c = centileText(newVal);
                    this.upperVal = c.centile;
                    this.upperSuffix = c.suffix;
                }
                this.sameVal = this.lowerVal === this.upperVal;
            }
        }
    },
    methods:{
        setWarnings(){
            if (this.p_upperCentile === null && this.p_upperCentile === null){
                this.warnOnly = this.limitCrossed = false;
            } else {
                let minVal = this.p_lowerCentile === null
                    ? this.p_upperCentile
                    : this.p_lowerCentile;
                 let maxVal = this.p_upperCentile === null
                    ? this.p_lowerCentile as number
                    : this.p_upperCentile;
                this.limitCrossed = minVal < limitCentileLbound || maxVal >limitCentileUbound;
                this.warnOnly = !this.limitCrossed && minVal < warnCentileLbound || maxVal > warnCentileUbound;
            }
            
        }
    }
});

function centileText(centile:number){
    let l = Math.round(centile);
    if (l < 1){
        return {centile:"<1", suffix: "st" }
    }
    if (l >= 100){
        return {centile:">99", suffix: "th" }
    }
    return {centile:l.toString(), suffix: getSuffix(l)};
}

</script>

