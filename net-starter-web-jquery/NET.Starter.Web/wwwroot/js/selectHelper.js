(function () {
    async function init($el, dataSource, options = {}) {
        const rawLabel = $el.siblings('label').text();
        const labelText = rawLabel.replace(/\*+$/, '').trim();

        const {
            ajaxMethod = 'GET',
            valueField = $el.attr('name') ?? 'id',
            textField = 'name',
            isFirstOptionEmpty = false,
            placeholder = labelText ? `- Select ${labelText} -` : null,
            dropdownParent = $el.parent(), //$el.closest('form').length ? $el.closest('form') : $('body'),
            ...opts
        } = options;

        let data = [];
        
        try {
            if (typeof dataSource === 'function') {
                data = await dataSource();
            }
            else if (typeof dataSource === 'string' && dataSource) {
                const response = await AjaxHelper.doAjax({
                    method: ajaxMethod,
                    url: dataSource
                });

                if (response?.succeeded) {
                    data = response.obj;
                }
            }
            else if (Array.isArray(dataSource)) {
                data = dataSource;
            }
            
            let mappedData = data.map(d => {
                let value, text;

                if (typeof d === "object" && d !== null) {
                    value = d[valueField];
                    text = typeof textField === "function"
                        ? textField(d)
                        : d[textField];
                } else {
                    value = d;
                    text = d;
                }

                let item = {
                    id: value,
                    text: text,
                    metadata: d
                };

                return item;
            });

            if ($el.hasClass("select2-hidden-accessible")) {
                $el.select2('destroy');
                $el.empty();
            }

            if (isFirstOptionEmpty) {
                mappedData.unshift({ id: "", text: placeholder, metadata: null });
            }
            
            $el.select2({
                dropdownParent: dropdownParent,
                data: mappedData,
                ...($el.attr('multiple') ? { closeOnSelect: false } : {}),
                ...(isFirstOptionEmpty ? {} : { placeholder: placeholder }),
                ...opts
            });
        }
        catch (error) {
            throw error;
        }
    }

    async function multiInit($els, dataSource, options = {}) {
        $els.each((i, el) => {
            init($(el), dataSource, options);
        });
    }

    function retrieveMetadata($el) {
        return $el.select2('data')[0].metadata;
    }

    function selectOption($el, value, options = {}) {
        $el.val(value).trigger('change');

        let data = $el.select2('data');
        if (!data.length) {
            const option = $el.find('option').filter(function () {
                return $(this).text().trim() === value;
            });

            if (option.length) {
                $el.val(option.val()).trigger('change');
            }

            data = $el.select2('data');
        }

        if (data.length > 0) {
            $el.trigger({
                type: 'select2:select',
                params: {
                    data: data[0]
                }
            });
        }
    }

    window.SelectHelper = {
        init,
        multiInit,
        retrieveMetadata,
        selectOption
    };
})();
