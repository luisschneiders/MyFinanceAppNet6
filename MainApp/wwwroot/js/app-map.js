(() => {
    'use strict'

    window.addScriptInteractiveMapGoogle = (url) => {
        // Load the script only once
        if (!document.querySelector(`script[src="${url}"]`)) {
            const script = document.createElement('script');
            script.src = url;
            script.defer = true;
            script.async = true;
            document.head.appendChild(script);
        }
    };

})()
