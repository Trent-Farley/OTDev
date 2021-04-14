
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
    window.location.href = "/Search";
}

function getHomeDetails(id: string): void {
    $.ajax({
        url: "/Home/RecipeDetails",
        method: "POST",
        data: {
            QueryValue: id
        },

        success: (data) => {
            $("#modal-container").empty();
            $("#modal-container").html(data);
            $("#button-cart").remove();
            $('#recipe-modal').modal("show");
        },
        error: (err) => { console.log(err); }
    });

}

