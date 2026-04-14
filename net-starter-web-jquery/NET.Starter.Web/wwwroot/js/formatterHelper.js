(function () {
    function fmtMoney(angka) {
        return new Number(angka).toLocaleString('id-ID', {
            style: 'decimal',
            minimumFractionDigits: 0,
            maximumFractionDigits: 0
        });
    }

    function fmtDate(dt, format) {
        const pad = (num) => String(num).padStart(2, '0');

        if (!(dt instanceof Date)) {
            dt = new Date(dt);
        }

        if (isNaN(dt.getTime())) {
            throw new Error("Invalid date");
        }

        const formatters = {
            "MMM yyyy": new Intl.DateTimeFormat('id-ID', { month: 'short', year: 'numeric' }),
            "dd MMM yyyy": new Intl.DateTimeFormat('id-ID', { day: '2-digit', month: 'short', year: 'numeric' }),
            "dd MMM yyyy HH:mm": new Intl.DateTimeFormat('id-ID', { day: '2-digit', month: 'short', year: 'numeric' }),
        };

        if (format === "MMM yyyy" || format === "dd MMM yyyy") {
            return formatters[format].format(dt);
        }
        else if (format === "yyyy-MM-dd") {
            return `${dt.getFullYear()}-${pad(dt.getMonth() + 1)}-${pad(dt.getDate())}`;
        }
        else if (format === "dd/MM/yyyy") {
            return `${pad(dt.getDate())}/${pad(dt.getMonth() + 1)}/${dt.getFullYear()}`;
        }
        else if (format === "dd-MM-yyyy") {
            return `${pad(dt.getDate())}-${pad(dt.getMonth() + 1)}-${dt.getFullYear()}`;
        }
        else if (format === "HH:mm") {
            return `${pad(dt.getHours())}:${pad(dt.getMinutes())}`;
        }
        else if (format === "dd MMM yyyy HH:mm") {
            return `${formatters[format].format(dt)} ${pad(dt.getHours())}:${pad(dt.getMinutes())}`;
        }
        else {
            throw new Error("Format tidak dikenali");
        }
    }

    window.FormatterHelper = {
        fmtMoney,
        fmtDate
    };
})();