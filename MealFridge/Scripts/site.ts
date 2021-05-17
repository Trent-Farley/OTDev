$(document).on('ajaxStart', () => {
    $("#loading").removeClass("d-none");
    $("#loading").show();
});
$(document).on('ajaxStop', () => {
    $("#loading").removeClass("d-none");
    $("#loading").hide();
});