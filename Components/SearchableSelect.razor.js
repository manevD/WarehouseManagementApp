export function register(dotNetReference, element) {

    if (!element) {
        return;
    }

    // Remove previous handler if it exists
    if (element._searchableSelectOutsideClick) {

        document.removeEventListener(
            "click",
            element._searchableSelectOutsideClick
        );
    }

    // Create new outside-click handler
    element._searchableSelectOutsideClick = function (event) {

        if (!element.contains(event.target)) {

            dotNetReference.invokeMethodAsync(
                "CloseDropdown"
            );
        }
    };

    // Register after current click event finishes
    setTimeout(function () {

        document.addEventListener(
            "click",
            element._searchableSelectOutsideClick
        );

    }, 0);
}


export function unregister(dotNetReference, element) {

    if (!element) {
        return;
    }

    if (element._searchableSelectOutsideClick) {

        document.removeEventListener(
            "click",
            element._searchableSelectOutsideClick
        );

        element._searchableSelectOutsideClick = null;
    }
}