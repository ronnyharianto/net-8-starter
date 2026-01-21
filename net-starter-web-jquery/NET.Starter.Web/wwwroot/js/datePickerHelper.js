(function () {
    function initDatePicker(options) {
        const {
            $el,
            minDate,
            maxDate,
            onClose
        } = options;

        $el.datepicker({
            dateFormat: "yy-mm-dd",
            onClose: onClose,
            minDate: minDate,
            maxDate: maxDate
        });
    }

    window.DatePickerHelper = {
        initDatePicker
    };
})();