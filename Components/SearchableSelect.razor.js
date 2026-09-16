window.searchableSelect = {

    current: null,
    outsideClickHandler: null,

    register: function (dotNetReference, element) {

        if (
            this.current &&
            this.current.reference !== dotNetReference
        ) {
            try {
                this.current.reference.invokeMethodAsync(
                    "CloseDropdown"
                );
            }
            catch (e) {
                console.warn(e);
            }
        }

        this.removeOutsideClickHandler();

        this.current = {
            reference: dotNetReference,
            element: element
        };

        this.outsideClickHandler = (event) => {

            if (!this.current)
                return;

            const currentElement =
                this.current.element;

            if (
                currentElement &&
                currentElement.contains(event.target)
            ) {
                return;
            }

            const reference =
                this.current.reference;

            this.current = null;

            this.removeOutsideClickHandler();

            try {
                reference.invokeMethodAsync(
                    "CloseDropdown"
                );
            }
            catch (e) {
                console.warn(e);
            }
        };

        document.addEventListener(
            "click",
            this.outsideClickHandler,
            false
        );
    },


    removeOutsideClickHandler: function () {

        if (this.outsideClickHandler) {

            document.removeEventListener(
                "click",
                this.outsideClickHandler,
                false
            );

            this.outsideClickHandler = null;
        }
    },


    unregister: function (dotNetReference) {

        if (
            !this.current ||
            this.current.reference === dotNetReference
        ) {
            this.current = null;

            this.removeOutsideClickHandler();
        }
    }
};