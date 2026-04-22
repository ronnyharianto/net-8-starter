(function () {
    function initDatePicker(options) {
        const {
            $el,
            ...opts
        } = options;

        $el.datepicker({
            dateFormat: "yy-mm-dd",
            changeYear: true,
            changeMonth: true,
            ...opts
        });
    }

    function init($el, options = {}) {
        const {
            ...opts
        } = options;

        $el.datepicker({
            dateFormat: "yy-mm-dd",
            changeYear: true,
            changeMonth: true,
            ...opts
        });
    }

    window.DatePickerHelper = {
        initDatePicker,
        init
    };
})();
