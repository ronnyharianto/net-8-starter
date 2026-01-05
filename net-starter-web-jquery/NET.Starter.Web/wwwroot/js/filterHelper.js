(function () {
    function setupFilter($btnFilter, $btnReset, $filterContainer, $gridPaging) {
        if (!$btnFilter || !$btnReset || !$filterContainer || !$gridPaging) return;

        const filter = history.state?.filter;
        if (filter) {
            FormHelper.populateFormData($filterContainer, filter, null);
        }

        const $elFilter = $filterContainer.find(
            'input[type="text"], input[type="number"], input[type="search"], input[type="date"]'
        );

        $btnFilter.on('click', function () {
            const formDataObject = FormHelper.getFormData($filterContainer);
            history.pushState({ filter: formDataObject }, '', '?mode=' + CommonHelper.generateUUID());

            $gridPaging.api().draw();
        });

        $elFilter.on('keyup', function (e) {
            if (e.isComposing) return;

            if (e.key === 'Enter') {
                e.preventDefault();
                $btnFilter.trigger('click');
            }
        });

        $btnReset.on('click', function () {
            $filterContainer.find('input,select').val(null).trigger('change');
            history.pushState(null, '', '?reset');

            $gridPaging.api().draw();
        });
    }

    window.FilterHelper = {
        setupFilter
    };
})();