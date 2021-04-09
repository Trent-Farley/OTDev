window.onload = () => {
    const prevSearch = window.sessionStorage.getItem("prevSearch");
    if (prevSearch !== null) {
        let newSearch = <HTMLInputElement>document.getElementById("sbn");
        newSearch.value = prevSearch;
        window.sessionStorage.clear();
        searchByName()
    }
};
function inventorySearch(): void {
    let search = $("#inventorySearch");
    if (search.val() == "") {
        $("#main").empty();
        $("#main").append("You have no saved ingredients. Visit the Inventory page to add ingredients to your fridge.")
        return;
    }
    $.ajax({
        url: "/Search/SearchByIngredient",
        type: "POST",
        data: {
            QueryValue: search.val(),
            SearchType: "Ingredient"
        },
        error: (err) => { console.log(err); },
        success: (recipeCards) => {
            $("#main").empty();
            $("#main").html(recipeCards);
        }
    })
}
function searchByName(): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
    let type: HTMLInputElement = <HTMLInputElement>document.getElementById("searchType");
    if (!search.value) {
        $("#warning-toast-body").empty();
        $("#warning-toast-body").append("Search can not be empty!");
        $(".toast").toast('show');
        return;
    }
    $.ajax({
        url: "/Search/SearchByName",
        type: "POST",
        data: {
            QueryValue: search.value,
            SearchType: type.value
        },
        error: (err) => { console.log(err); },
        success: (recipeCards) => {
            $("#main").empty();
            $("#main").html(recipeCards);
        }
    })
}

const inputSearch: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
inputSearch.addEventListener("keydown", (e) => {
    //checks whether the pressed key is "Enter"
    if (e.keyCode === 13) {
        searchByName();
    }
});
