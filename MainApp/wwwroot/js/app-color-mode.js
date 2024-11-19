
/*!
 * Color mode toggler for Bootstrap's docs (https://getbootstrap.com/)
 * Copyright 2011-2022 The Bootstrap Authors
 * Licensed under the Creative Commons Attribution 3.0 Unported License.
 */

(() => {
    'use strict'

    const key = "AppTheme";

    const storedTheme = localStorage.getItem(key);

    const getPreferredTheme = () => {
        if (storedTheme) {
            let theme = JSON.parse(storedTheme);
            return theme;
        }

        var mode = window.matchMedia('(prefers-color-scheme: light)').matches ? 'dark' : 'light';

        return capitalizeWord(mode);
    };

    const setTheme = function (theme) {
        var color = theme;
        if (color.toLowerCase() === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            document.documentElement.setAttribute('data-bs-theme', 'dark')
            localStorage.setItem(key, JSON.stringify(theme));
        } else {
            document.documentElement.setAttribute('data-bs-theme', color.toLowerCase())
            localStorage.setItem(key, JSON.stringify(theme));
        }
    };

    const capitalizeWord = function (str) {
        return str
            .toLowerCase() // Convert the string to lowercase
            .split(' ') // Split the string into words
            .map(word => word.charAt(0).toUpperCase() + word.slice(1)) // Capitalize each word
            .join(' '); // Join the words back together
    }

    setTheme(getPreferredTheme());

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        if (storedTheme !== "light" || storedTheme !== "dark") {
            setTheme(getPreferredTheme())
        }
    });

    window.updateColorMode = (theme) => {
        setTheme(theme);
    };

})()
