(function () {
    function convertToCSV(data, excludeFields = []) {
        if (!data || !data.length) return '';

        const header = Object.keys(data[0]).filter(key => !excludeFields.includes(key));
        const csvRows = [];
        csvRows.push(header.join('|'));

        for (const row of data) {
            const values = header.map(field => {
                let value = row[field] ?? '';

                if (typeof value === 'string' && (/^0\d+$/).test(value)) {
                    value = `="${value}"`;
                } else if (typeof value === 'number' && value.toString().length > 11) {
                    value = `="${value}"`;
                } else {
                    value = value.toString().replace(/"/g, '""');
                    value = `"${value}"`;
                }

                return value;
            });
            csvRows.push(values.join('|'));
        }

        return csvRows.join('\n');
    }


    function exportCSV(data, excludeFields = [], filename = 'data.csv') {
        if (!Array.isArray(data)) {
            console.error('Data must be an array.');
        }

        const csv = convertToCSV(data, excludeFields);

        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);

        const link = document.createElement("a");
        link.setAttribute("href", url);
        link.setAttribute("download", filename);
        link.click();
    }

    window.ExportHelper = {
        exportCSV
    };
})();
