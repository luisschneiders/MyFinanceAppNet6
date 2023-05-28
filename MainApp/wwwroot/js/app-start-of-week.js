(() => {
    'use strict'

    const key = "AppStartOfWeek";
    const locale = window.navigator.language;
    const date = new Date();
    const weekdays = [];

    while (!weekdays[date.getDay()]) {
        weekdays[date.getDay()] = date.toLocaleString(locale, { weekday: 'long' });
        date.setDate(date.getDate() + 1);
    };

    const storedStartOfWeek = localStorage.getItem(key);

    const getPreferredStartOfWeek = () => {
        if (storedStartOfWeek) {
            return storedStartOfWeek;
        }

        return JSON.stringify(weekdays[1]); // Set default to "Monday";
    };

    const setStartOfWeek = function (weekday) {
        localStorage.setItem(key, weekday);
    };

    setStartOfWeek(getPreferredStartOfWeek());

})()
