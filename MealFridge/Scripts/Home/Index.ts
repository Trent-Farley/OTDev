const search: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
search.addEventListener("keydown", (e) => {
    //checks whether the pressed key is "Enter"
    if (e.keyCode === 13) {
        searchFromMainPage();
    }
});

function searchFromMainPage(): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
    window.sessionStorage.setItem("prevSearch", search.value);
    window.location.href = "/SearchByName";
}
