(function () {
    var vm = new Vue({
        el: '#drug-list',
        computed: {
            dob: {
                get: function () {
                    return this._dob;
                },
                set: function (newVal) {
                    this._dob = newVal;
                    if (newVal !== null && newVal !== '') {
                        let now = new Date();
                        let beforeBirthday = new Date(newVal).setYear(now.getYear()) < now;
                        this.ageYears = now.getYear() - newVal.getYear() - beforeBirthday ? 1 : 0;
                        this.ageMonths = beforeBirthday ? now.getMonth + 12 - newVal.getMonth() : now.getMonth() - newVal.getMonth();
                        this.ageDays = now.getDate() >= newVal.getDate() ? now.getDate() - newVal.getDate() : new Date(newVal.getYear(),
                            newVal.getMonth() + 1,
                            0).getDate() + now.getDate();
                    }
                }
            },
            ageYears: {
                get: function () {
                    return this.ageYears;
                },
                set: function (newVal) {
                    if (newVal !== null && newVal !== '') {
                        dob = null;
                    }
                    this.ageYears = newVal;
                }
            },
            ageMonths: {
                get: function () {
                    return this.ageMonths;
                },
                set: function (newVal) {
                    if (newVal !== null && newVal !== '') {
                        dob = null;
                    }
                    if (newVal > 12) {
                        age.years += Math.floor(newVal);
                        newVal = newVal % 12;
                    }
                    this.ageMonths = newVal;
                }
            },
            ageDays: {
                get: function () {
                    return this.ageDays;
                },
                set: function (newVal) {
                    if (newVal !== null && newVal !== '') {
                        dob = null;
                    }
                    this.ageDays = newVal;
                }
            }
        }
    });
})();
