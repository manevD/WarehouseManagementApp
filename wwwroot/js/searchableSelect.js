window.searchableSelect = {

    register: function (dotNetReference, element) {

        if (!element) {
            return;
        }

        // Remove old handler
        if (element._searchableSelectOutsideClick) {

            document.removeEventListener(
                "click",
                element._searchableSelectOutsideClick
            );
        }

        element._searchableSelectOutsideClick = function (event) {

            if (!element.contains(event.target)) {

                dotNetReference.invokeMethodAsync(
                    "CloseDropdown"
                );
            }
        };

        // Wait one tick so the click that opens
        // the dropdown does not immediately close it.
        setTimeout(function () {

            document.addEventListener(
                "click",
                element._searchableSelectOutsideClick
            );

        }, 0);
    },


    unregister: function (dotNetReference) {

        document
            .querySelectorAll(".searchable-select")
            .forEach(function (element) {

                if (element._searchableSelectOutsideClick) {

                    document.removeEventListener(
                        "click",
                        element._searchableSelectOutsideClick
                    );

                    element._searchableSelectOutsideClick = null;
                }

            });
    }

};