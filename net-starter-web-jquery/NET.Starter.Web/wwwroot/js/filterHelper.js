(function () {
    function setupFilter(btnFilter, btnReset, filterContainer, gridPaging) {
        if (!btnFilter || !btnReset || !filterContainer || !gridPaging) return;

        const filter = history.state?.filter;
        if (filter) {
            FormHelper.populateFormData(filterContainer, filter, null);
        }

        btnFilter.on('click', function () {
            const formDataObject = FormHelper.getFormData(filterContainer);
            history.pushState({ filter: formDataObject }, '', '?mode=' + CommonHelper.generateUUID());

            gridPaging.api().draw();
        });

        btnReset.on('click', function () {
            filterContainer.find('input,select').val(null).trigger('change');
            history.pushState(null, '', '?reset');

            gridPaging.api().draw();
        });
    }

    window.FilterHelper = {
        setupFilter
    };
})();