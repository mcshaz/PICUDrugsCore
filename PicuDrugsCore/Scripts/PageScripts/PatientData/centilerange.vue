<!-- src/components/CentileRange.vue -->

<template>
    <div class="centile" v-show="lowerVal" :class="{alert: true, 'alert-info':!warnCrossed, 'alert-warning':warnCrossed && !limitCrossed, 'alert-danger':limitCrossed }">
        <span class="lower">{{lowerVal}}<sup>{{lowerSuffix}}</sup></span>
        <span v-if="upperVal!==lowerVal">
            - 
            <span class="upper">{{upperVal}}<sup>{{upperSuffix}}</sup></span>
        </span>
        <span class="centileDescr">
            centile
        </span>
        <div v-if="warnCrossed">
            only 1 in {{denominator}}
            <span v-if="largeNumWord">
                {{largeNumWord}} 
                <small>
                    (10<sup>{{largeNumExp10}}</sup>) 
                </small>
            </span>
            weigh {{moreLess}}. 
            <div v-if="!limitCrossed" class="form-check form-check-inline">
                <input type="checkbox" id="acceptCentile" :checked="acceptWarning" class="form-check-input" required/> 
                <label for="acceptCentile" class="form-check-label">I confirm this is the correct weight</label>
            </div>
        </div>
    </div>
</template>


<script lang="ts">
import Vue from 'vue'
import { largeNumberWords, getSuffix } from '../../Utilities/NumberToWords';
import { NumericRange } from './NumericRange';

const warnCentileUbound = 99;
const warnCentileLbound = 1;
const limitCentileUbound = 100 - 1e-7;
const limitCentileLbound = 1e-12;
enum moreOrLess {more, less, ''}
export default Vue.component("CentileRange",{
    props:['centiles'],
    data(){
        return {
            p_warnCrossed: false,
            p_limitCrossed: false,
            p_isValid:false,
            p_acceptWarning:false,
            lowerVal: '',
            lowerSuffix: '',
            upperVal:'',
            upperSuffix:'',
            moreLess: moreOrLess[''],
            denominator:'',
            largeNumWord:'',
            largeNumExp10:null as null | number
        }
    },
    computed:{
        limitCrossed:{
            get(this:any){
                return this.p_limitCrossed;
            },
            set(newVal:boolean){
                this.p_limitCrossed = newVal;
                this.setValidity();
            }
        },
        warnCrossed:{
            get(this:any){
                return this.p_warnCrossed;
            },
            set(newVal:boolean){
                this.p_warnCrossed = newVal;
                this.setValidity();
            }
        },
        acceptWarning:{
            get(this:any){
                return this.p_acceptWarning;
            },
            set(newVal:boolean){
                this.p_acceptWarning = newVal;
                this.setValidity();
            }
        }
    },
    watch:{
        centiles(newVal:NumericRange | null){
            this.setWarnings();
            if (newVal){
                let c = centileText(newVal.min);
                this.lowerVal = c.centile;
                this.lowerSuffix = c.suffix;
                if (!newVal.nonRange){
                    c = centileText(newVal.max);
                }
                this.upperVal = c.centile;
                this.upperSuffix = c.suffix;
            } else {
                this.lowerVal = this.lowerSuffix = this.upperVal = this.upperSuffix = '';
            }
            //this.sameVal = this.lowerVal === this.upperVal;
        }
    },
    methods:{
        setValidity(){
            const isValid = !(this.p_limitCrossed || (this.p_warnCrossed && !this.p_acceptWarning));
            const emit = this.p_isValid !== isValid;
            this.p_isValid = isValid;
            if (emit){
                this.$emit("validCentile", this.p_isValid);
            }
        },
        setWarnings(){
            const self = this;
            if (!this.centiles){
                this.warnCrossed = this.limitCrossed = false;
                clearNum();
            } else {
                this.limitCrossed = this.centiles.max < limitCentileLbound || this.centiles.min >limitCentileUbound;
                this.warnCrossed = this.centiles.max < warnCentileLbound || this.centiles.min > warnCentileUbound;
                if (this.limitCrossed || this.warnCrossed){
                    let denom;
                    if (this.centiles.max < warnCentileLbound){
                        denom = 100/this.centiles.max;
                        this.moreLess = moreOrLess.less;
                    } else {
                        denom = 100/(100-this.centiles.min);
                        this.moreLess = moreOrLess.more;
                    }
                    let words = largeNumberWords(denom);
                    this.denominator = words.digits;
                    this.largeNumWord = words.suffixName;
                    this.largeNumExp10 = words.exp10;
                } else {
                    clearNum();
                }
            }

            function clearNum(){
                self.denominator=self.largeNumWord='';
                self.moreLess = moreOrLess[''];
                self.largeNumExp10 = null;
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

