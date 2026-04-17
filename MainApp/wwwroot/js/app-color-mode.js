
/*!
 * Color mode toggler for Bootstrap's docs (https://getbootstrap.com/)
 * Copyright 2011-2022 The Bootstrap Authors
 * Licensed under the Creative Commons Attribution 3.0 Unported License.
 */

(() => {
    'use strict';

    const key = "AppTheme";

    // --- Helpers ---
    const getStoredTheme = () => {
        try {
            const value = localStorage.getItem(key);
            return value ? JSON.parse(value) : null;
        } catch {
            return null;
        }
    };

    const getSystemTheme = () => {
        return window.matchMedia('(prefers-color-scheme: dark)').matches
            ? 'dark'
            : 'light';
    };

    const getPreferredTheme = () => {
        const stored = getStoredTheme();

        if (stored && ['light', 'dark', 'auto'].includes(stored.toLowerCase())) {
            return stored.toLowerCase();
        }

        return 'auto';
    };

    const applyTheme = (theme) => {
        const resolvedTheme =
            theme === 'auto'
                ? (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light')
                : theme;

        document.documentElement.setAttribute('data-bs-theme', resolvedTheme);
    };

    const setTheme = (theme) => {
        const normalized = theme.toLowerCase();

        localStorage.setItem(key, JSON.stringify(theme));
        applyTheme(normalized);
    };

    // --- Init ---
    applyTheme(getPreferredTheme());

    // --- React to system changes ---
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');

    mediaQuery.addEventListener('change', () => {
        const stored = getStoredTheme();

        if (!stored || stored === 'auto') {
            requestAnimationFrame(() => {
                applyTheme('auto');
            });
        }
    });

    // --- Public API ---
    window.updateColorMode = (theme) => {
        setTheme(theme);
    };

})();
