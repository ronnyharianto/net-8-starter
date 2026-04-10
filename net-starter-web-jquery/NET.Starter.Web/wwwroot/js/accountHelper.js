(function () {
    function userAccessAvailable() {
        if (
            localStorage.getItem("token") == null ||
            localStorage.getItem("tokenExpiresAt") == null ||
            localStorage.getItem("refresh") == null ||
            localStorage.getItem("refreshExpiresAt") == null ||
            localStorage.getItem("fullName") == null ||
            localStorage.getItem("pictureUrl") == null
        )
            return false;

        const tokenExpiresAt = new Date(localStorage.getItem("tokenExpiresAt"));

        return new Date() < tokenExpiresAt;
    }

    function userSessionAvailable() {
        if (
            localStorage.getItem("refresh") == null ||
            localStorage.getItem("refreshExpiresAt") == null
        )
            return false;

        const refreshExpiresAt = new Date(localStorage.getItem("refreshExpiresAt"));

        return new Date() < refreshExpiresAt;
    }

    function setSessionData({ token, refresh, tokenExpiresAt, refreshExpiresAt, fullName, pictureUrl }) {
        localStorage.setItem("token", token);
        localStorage.setItem("refresh", refresh);
        localStorage.setItem("tokenExpiresAt", tokenExpiresAt);
        localStorage.setItem("refreshExpiresAt", refreshExpiresAt);
        localStorage.setItem("fullName", fullName);
        localStorage.setItem("pictureUrl", pictureUrl);
    }

    async function refreshMyToken(urlApi) {
        const result = await AjaxHelper.doAjax({
            url: `${urlApi}/v1/account/refresh-token`,
            tokenType: "refresh"
        });

        if (result?.succeeded) {
            localStorage.removeItem("myCompanies");
            setSessionData(result.obj);

            window.location.reload();
        } else {
            logoutUser();
        }
    }

    async function changeCurrentCompany(urlApi, companyId) {
        const result = await AjaxHelper.doAjax({
            url: `${urlApi}/v1/account/change-company/${companyId}`,
            tokenType: "token"
        });

        if (result?.succeeded) {
            localStorage.removeItem("myCompanies");
            setSessionData(result.obj);

            window.location.reload();
        } else {
            logoutUser();
        }
    }

    function logoutUser() {
        localStorage.clear();
        window.location.href = "/login";
    }

    function userPicture() {
        return localStorage.getItem("pictureUrl");
    }

    function userName() {
        return localStorage.getItem("fullName");
    }

    function decodeJwtToken() {
        const token = localStorage.getItem("token");
        if (!token || typeof token !== "string") return [];

        const parts = token.split('.');
        if (parts.length !== 3) return [];

        try {
            const payload = parts[1].replace(/-/g, '+').replace(/_/g, '/');
            const padded = payload + '='.repeat((4 - payload.length % 4) % 4);
            const decoded = atob(padded);
            return JSON.parse(decoded);
        } catch (err) {
            console.error('Failed to decode JWT payload:', err);
            return null;
        }
    }

    function userPermission() {
        return decodeJwtToken()?.permissions ?? [];
    }

    function userCurrentCompany() {
        const myCompanies = JSON.parse(localStorage.getItem("myCompanies"));
        const currentCompany = myCompanies.find(d => d.companyId == decodeJwtToken()?.current_company)

        return currentCompany ?? null;
    }

    function showMenus() {
        const userPermissions = this.userPermission();

        const $menuContainer = $("#menu");
        const $menusWithAccessRequirement = $menuContainer.find("[data-access]");

        for (let i = 0; i < $menusWithAccessRequirement.length; i++) {
            const $menuItem = $($menusWithAccessRequirement[i]);
            const requiredPermission = $menuItem.data('access');

            if (userPermissions.includes(requiredPermission)) {
                $menuItem.removeClass('d-none');
                $menuItem.parents('.d-none').removeClass('d-none');
            }
        }
    }

    window.AccountHelper = {
        userAccessAvailable,
        userSessionAvailable,
        setSessionData,
        refreshMyToken,
        changeCurrentCompany,
        logoutUser,
        userPicture,
        userName,
        userPermission,
        userCurrentCompany,
        showMenus
    };
})();
