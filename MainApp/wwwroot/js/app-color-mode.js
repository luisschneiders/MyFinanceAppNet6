
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

        return mode;
    };

    const setTheme = function (theme) {
        if (theme === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            document.documentElement.setAttribute('data-bs-theme', 'dark')
            localStorage.setItem(key, JSON.stringify(theme));
        } else {
            document.documentElement.setAttribute('data-bs-theme', theme)
            localStorage.setItem(key, JSON.stringify(theme));
        }
    };

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
