const search = document.getElementById("sbn");
search.addEventListener("keydown", (e) => {
    if (e.keyCode === 13) {
        searchFromMainPage();
    }
});
function searchFromMainPage() {
    let search = document.getElementById("sbn");
    window.sessionStorage.setItem("prevSearch", search.value);
    window.location.href = "/Search";
}
//# sourceMappingURL=Index.js.map