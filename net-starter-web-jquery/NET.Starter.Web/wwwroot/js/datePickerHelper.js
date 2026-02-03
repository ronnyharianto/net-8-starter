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

    window.DatePickerHelper = {
        initDatePicker
    };
})();