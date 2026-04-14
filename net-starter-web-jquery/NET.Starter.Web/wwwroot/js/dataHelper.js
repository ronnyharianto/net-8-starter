(function () {
    async function loadCompanies() {
        const response = await AjaxHelper.doAjax({
            url: `${apiBaseUrl}/v1/company/all`
        });

        if (response?.succeeded) {
            return response.obj;
        }

        return [];
    }

    async function loadRoles() {
        const response = await AjaxHelper.doAjax({
            url: `${apiBaseUrl}/v1/role/all`
        });

        if (response?.succeeded) {
            return response.obj;
        }

        return [];
    }

    async function loadPermissions() {
        const response = await AjaxHelper.doAjax({
            url: `${apiBaseUrl}/v1/permission/all`
        });

        if (response?.succeeded) {
            return response.obj;
        }

        return [];
    }

    async function loadMyCompanies() {
        const response = await AjaxHelper.doAjax({
            url: `${apiBaseUrl}/v1/account/my-companies`
        });

        if (response?.succeeded) {
            return response.obj;
        }

        return [];
    }

    window.DataHelper = {
        loadCompanies,
        loadRoles,
        loadPermissions,
        loadMyCompanies
    };
})();