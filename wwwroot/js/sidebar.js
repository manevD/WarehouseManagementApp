window.warehouseToggleSidebar = function () {

    const sidebar = document.querySelector(".sidebar");
    const overlay = document.querySelector(".mobile-overlay");

    if (!sidebar) {
        return;
    }

    const isOpen = sidebar.classList.contains("is-open");

    if (isOpen) {
        sidebar.classList.remove("is-open");

        if (overlay) {
            overlay.classList.remove("is-open");
        }
    }
    else {
        sidebar.classList.add("is-open");

        if (overlay) {
            overlay.classList.add("is-open");
        }
    }
};


window.warehouseCloseSidebar = function () {

    const sidebar = document.querySelector(".sidebar");
    const overlay = document.querySelector(".mobile-overlay");

    if (sidebar) {
        sidebar.classList.remove("is-open");
    }

    if (overlay) {
        overlay.classList.remove("is-open");
    }
};