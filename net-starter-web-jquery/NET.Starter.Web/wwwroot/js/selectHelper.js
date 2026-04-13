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
                dropdownParent: $el.closest('form').length ? $el.closest('form') : $('body'),
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

    window.SelectHelper = {
        init,
        multiInit,
        retrieveMetadata
    };
})();
