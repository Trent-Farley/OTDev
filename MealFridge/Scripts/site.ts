$(document).on('ajaxStart', () => {
    $("#loading").removeClass("d-none");
    $("#loading").show();
});
$(document).on('ajaxStop', () => {
    $("#loading").addClass("d-none");
    $("#loading").hide();
});
$(document).on('load', () => {
    $("#loading").removeClass("d-none");
    $("#loading").show();
});
$(document).on('ready', () => {
    $("#loading").addClass("d-none");
    $("#loading").hide();
})