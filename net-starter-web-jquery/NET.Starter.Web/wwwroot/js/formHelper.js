(function () {
    function clearForm(formElement, validator) {
        formElement.find('input, select, textarea').each(function () {
            const type = $(this).attr('type');

            if (type === 'checkbox' || type === 'radio') {
                $(this).prop('checked', false);
            } else if (this.tagName.toLowerCase() === 'select') { 
                $(this).val(null).trigger('change');
            }
            else {
                $(this).val('');
            }

            $(this).removeClass('text-danger');
        });

        if (validator != null)
            validator.resetForm();
    }

    function getFormData(formElement) {
        const disabledElement = formElement.find(':disabled');
        disabledElement.removeAttr('disabled');

        const formDataArray = formElement.find('input[name], select[name], textarea[name]').serializeArray();
        disabledElement.attr('disabled', true);

        const formDataObject = {};
        formDataArray.forEach(({ name, value }) => {
            const keys = name.replace(/\[\]$/, '').split('.');
            let current = formDataObject;

            keys.forEach((key, index) => {
                if (index === keys.length - 1) {
                    if (name.endsWith('[]')) {
                        current[key] = current[key] || [];
                        current[key].push(value);
                    } else if (current[key] !== undefined) {
                        current[key] = Array.isArray(current[key])
                            ? [...current[key], value]
                            : [current[key], value];
                    } else {
                        current[key] = value;
                    }
                } else {
                    current[key] = current[key] || {};
                    current = current[key];
                }
            });
        });

        return formDataObject;
    }

    function changeStringEmptyToNull(obj, keys) {
        keys.forEach(path => {
            const parts = path.split('.');
            let current = obj;

            for (let i = 0; i < parts.length - 1; i++) {
                if (!current || typeof current !== 'object') return;
                current = current[parts[i]];
            }

            const lastKey = parts[parts.length - 1];
            if (current && current[lastKey] === '') {
                current[lastKey] = null;
            }
        });
    }

    function populateFormData(formElement, formData, validator) {
        clearForm(formElement, validator);

        const elements = formElement.find('input[name], select[name], textarea[name]');

        $.each(elements, function (key, el) {
            const element = $(el);
            const name = element.attr('name');
            const delay = element.data('populate-delay');
            const value = getValueByKeyPath(formData, name);

            if (value !== undefined) {
                if (delay !== undefined)
                    setTimeout(function () {
                        setDataToElement(element, value);
                    }, delay);
                else
                    setDataToElement(element, value);
            }
        });
    }

    function getValueByKeyPath(obj, keyPath) {
        return keyPath.replace(/\[\]$/, '').split('.').reduce((acc, key) => acc?.[key], obj);
    }

    function setDataToElement(element, value) {
        const tagName = element.prop('tagName').toLowerCase();
        const type = element.attr('type');

        if (type === 'checkbox') {
            if (element.attr('name')?.endsWith('[]')) {
                if (Array.isArray(value)) {
                    element.prop('checked', value.includes(element.val()));
                }
            } else {
                element.prop('checked', !!value);
            }
        } else if (type === 'radio') {
            element.prop('checked', element.val() === value);
        } else {
            element.val(value);

            if (tagName === 'select') {
                element.trigger('change');
            }
        }
    }

    function populateDdl(ddlElement, data, valueField, textField, isFirstOptionEmpty = false, firstOptionValue = null, firstOptionText = null) {
        ddlElement.empty();

        if (isFirstOptionEmpty) {
            ddlElement.append($(`<option value="${firstOptionValue ?? ""}">${firstOptionText ?? "&nbsp"}</option>`));
        }

        for (let i = 0; i < data.length; i++) {
            ddlElement.append($('<option value="' + data[i][valueField] + '">' + data[i][textField] + '</option>'));
        }
    }

    function errorPlacement(error, element) {
        if (element.hasClass('select2-hidden-accessible')) {
            error.insertAfter(element.next('.select2'));
        }
        else if (element.closest('.input-group').length) {
            error.insertAfter(element.closest('.input-group'));
        }
        else {
            error.insertAfter(element);
        }
    }

    function numericMoney(selector) {
        $(document).on('focusin', selector, function () {
            const value = $(this).val();

            $(this).val(value.replace(/\./g, ''));
        });

        $(document).on('focusout', selector, function () {
            const value = $(this).val().replace(/\./g, '');

            if (value) {
                $(this).val(FormatterHelper.fmtMoney(value));
            }
        });

        numericOnly(selector);
    };

    function numericOnly(selector, allowDash = false) {
        $(document).on('keydown', selector, function (e) {
            const allowedKeys = [
                'Backspace', 'Delete', 'Tab', 'Escape', 'Enter',
                'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown',
                'Home', 'End'
            ];

            if (allowDash) {
                allowedKeys.push('-');
            }

            if (allowedKeys.includes(e.key)) {
                return;
            }

            if (!((e.key >= '0' && e.key <= '9') || (e.key >= 'Numpad0' && e.key <= 'Numpad9'))) {
                e.preventDefault();
            }
        });
    }

    window.FormHelper = {
        clearForm,
        getFormData,
        changeStringEmptyToNull,
        populateFormData,
        populateDdl,
        errorPlacement,
        numericMoney,
        numericOnly
    };
})();