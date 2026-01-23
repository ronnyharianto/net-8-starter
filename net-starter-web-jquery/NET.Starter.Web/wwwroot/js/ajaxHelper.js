(function () {
    function getDefaultHeaders(tokenType = "token") {
        const token = localStorage.getItem(tokenType);
        return {
            "Authorization": `Bearer ${token}`,
        };
    }

    const glbBaseRequest = {
        timeout: 10000
    };

    async function doAjaxAnonymous({ method = "GET", url, data = null, customHeaders = {} }) {
        try {
            LoadingHelper.show();

            const response = await $.ajax({
                ...glbBaseRequest,
                method,
                url,
                data: data ? JSON.stringify(data) : null,
                headers: {
                    "Content-Type": "application/json",
                    ...customHeaders
                }
            });

            LoadingHelper.hide();
            return response;
        } catch (error) {
            Swal.fire({
                title: "Error",
                text: error.responseJSON?.message ?? "Something went wrong",
                icon: "error",
            });

            LoadingHelper.hide();
            return error.responseJSON;
        }
    }

    async function doAjax({ method = "GET", url, data = null, tokenType = "token", customHeaders = {} }) {
        try {
            LoadingHelper.show();

            const response = await $.ajax({
                ...glbBaseRequest,
                method,
                url,
                data: data ? JSON.stringify(data) : null,
                headers: {
                    ...getDefaultHeaders(tokenType),  
                    "Content-Type": "application/json",
                    ...customHeaders
                }
            });

            LoadingHelper.hide();
            return response;
        } catch (error) {
            if (error.status === 401) {
                const sessionAvailable = AccountHelper.userSessionAvailable();
                LoadingHelper.hide();

                if (!sessionAvailable) {
                    Swal.fire({
                        title: "Error",
                        text: "Your session has expired. Please login again.",
                        icon: "error",
                    }).then(() => {
                        AccountHelper.logoutUser();
                    });
                }
                else {
                    Swal.fire({
                        title: "Error",
                        text: "Your access needs to be updated. The page will be reloaded.",
                        icon: "error",
                        timer: 5000,
                        timerProgressBar: true,
                        showConfirmButton: true,
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                    }).then(() => {
                        window.location.reload();
                    });
                }
            }
            else {
                Swal.fire({
                    title: "Error",
                    text: error.responseJSON?.message ?? "Something went wrong",
                    icon: "error",
                });

                LoadingHelper.hide();
                return error.responseJSON;
            }
        }
    }

    async function doAjaxUploadFile({ url, file, fieldName = "file", tokenType = "token", customHeaders = {} }) {
        try {
            LoadingHelper.show();

            const formData = new FormData();
            formData.append(fieldName, file);

            const response = await $.ajax({
                ...glbBaseRequest,
                method: "POST",
                url,
                data: formData,
                processData: false,
                contentType: false,
                headers: {
                    ...getDefaultHeaders(tokenType),
                    ...customHeaders
                }
            });

            LoadingHelper.hide();
            return response;
        } catch (error) {
            console.error(`[doAjaxUploadFile] Error POST ${url}:`, error);

            LoadingHelper.hide();
            return error.responseJSON;
        }
    }

    async function refreshToken(apiBaseUrl) {
        const url = apiBaseUrl + "/v1/account/refresh-token";
        const response = await doAjax({
            method: "POST",
            url,
            tokenType: "refresh"
        });

        if (response.succeeded) {
            setSessionData(response.obj);
        } else {
            logoutUser();
        }
    }

    window.AjaxHelper = {
        getDefaultHeaders,
        glbBaseRequest,
        doAjaxAnonymous,
        doAjax,
        doAjaxUploadFile,
        refreshToken
    };
})();
