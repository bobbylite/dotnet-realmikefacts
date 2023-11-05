// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.body.onload = loadCss = () => {
    document.getElementById("email").className = "form-control form-control-sm"
    document.getElementById("password").className = "form-control form-control-sm"
    document.getElementById("next").className = "btn btn-primary";
    document.getElementById("next").style.marginTop= "10px";
}
