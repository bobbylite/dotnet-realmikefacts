// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.addEventListener('load', () => {
    const button = document.getElementById("next");
    button.setAttribute("data-loading-text", '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Sign in');
    const form = button.closest("form");

    form.addEventListener("submit", function () {
        if (button.getAttribute("data-loading") === "true") {
            event.preventDefault();
        } else {
            button.disabled = true;
            button.setAttribute("data-loading", "true");
            button.innerHTML = button.getAttribute("data-loading-text");
            const spinner = button.querySelector('.spinner-border');
            spinner.classList.remove('d-none');
        }
    });

    const forgotPasswordButton = document.getElementById("forgotPassword");
    forgotPasswordButton.href = "https://realmikefacts.b2clogin.com/realmikefacts.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_realmikefacts_forgot_password&client_id=3d0989fb-80d7-42d9-b1e9-6e9528c105bc&nonce=defaultNonce&redirect_uri=https%3A%2F%2Frealmikefacts.azurewebsites.net%2Fsignin-oidc&scope=openid&response_type=id_token&prompt=login";

});