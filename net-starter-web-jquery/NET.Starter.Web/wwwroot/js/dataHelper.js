(function () {
    async function loadCompanies() {
        const loadCompaniesResponse = await AjaxHelper.doAjax({
            url: `${apiBaseUrl}/v1/company/all`
        });

        if (loadCompaniesResponse?.succeeded) {
            return loadCompaniesResponse.obj;
        }
        else {
            return [];
        }
    }

    async function loadRoles() {
        const loadRolesResponse = await AjaxHelper.doAjax({
            url: `${apiBaseUrl}/v1/role/all`
        });

        if (loadRolesResponse?.succeeded) {
            return loadRolesResponse.obj;
        }
        else {
            return [];
        }
    }

    async function loadPermissions() {
        const loadPermissionsResponse = await AjaxHelper.doAjax({
            url: `${apiBaseUrl}/v1/permission/all`
        });

        if (loadPermissionsResponse?.succeeded) {
            return loadPermissionsResponse.obj;
        }
        else {
            return [];
        }
    }

    async function loadMyCompanies() {
        const loadMyCompaniesResponse = await AjaxHelper.doAjax({
            url: `${apiBaseUrl}/v1/account/my-companies`
        });

        if (loadMyCompaniesResponse?.succeeded) {
            return loadMyCompaniesResponse.obj;
        }
        else {
            return [];
        }
    }

    window.DataHelper = {
        loadCompanies,
        loadRoles,
        loadPermissions,
        loadMyCompanies
    };
})();