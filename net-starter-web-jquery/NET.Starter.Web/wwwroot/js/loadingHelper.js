(function () {
    let countLoad = 1;

    function show() {
        if (countLoad == 0) {
            $("body").removeClass("loaded");
        }

        countLoad++;
    }

    function hide() {
        countLoad--;

        if (countLoad <= 0) {
            $("body").addClass("loaded");
        }
    }

    window.LoadingHelper = {
        show,
        hide
    };
})();
