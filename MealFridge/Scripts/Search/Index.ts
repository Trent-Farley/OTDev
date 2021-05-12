window.onload = () => {
    const prevSearch = window.sessionStorage.getItem("prevSearch");
    if (prevSearch !== null) {
        let newSearch = <HTMLInputElement>document.getElementById("sbn");
        newSearch.value = prevSearch;
        window.sessionStorage.clear();
        searchByName()
    }
};

let pageNumber: number = 0;
let searchparam: string = "";
function inventorySearch(): void {
    let search = $("#inventorySearch");
    let refine: HTMLInputElement = <HTMLInputElement>document.getElementById("panCheck");
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
            SearchType: "Ingredient",
            Refine: refine.checked,
            PageNumber: pageNumber
        },
        error: (err) => { console.log(err); },
        success: (recipeCards) => {
            $("#morebutton").removeClass("d-none");
            if (search.val().toString() != searchparam) {
                pageNumber = 0;
                searchparam = search.val().toString();
            }
            if (pageNumber < 1) {
                $("#main").empty();
                $("#main").html(recipeCards);
                ++pageNumber;
            }
            else {
                $("#main").append(recipeCards);
                ++pageNumber;
            }
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
            SearchType: type.value,
            PageNumber: pageNumber
        },
        error: (err) => { console.log(err); },
        success: (recipeCards) => {
            $("#morebutton").removeClass("d-none");

            if (search.value.toString() != searchparam) {
                pageNumber = 0;
                searchparam = search.value.toString();
            }
            if (pageNumber < 1) {
                $("#main").empty();
                $("#main").html(recipeCards);
                ++pageNumber;
            }
            else {
                $("#main").append(recipeCards);
                ++pageNumber;
            }
        }
    })
}
function addFavorite(id: string): void {
    $.ajax({
        url: "/Search/SavedRecipe",
        method: "POST",
        data: {
            id: parseInt(id, 10),
            other: "Favorite"
        },
        success: _ => {
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-primary alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Added</strong> Recipe has been added to your favorites
                </div>
            `);
        },
        error: _ => {
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Error</strong> Something went wrong, maybe this recipe has been favorited already?
                </div>
            `);
        }
    });
}

function addShelf(id: string): void {
    $.ajax({
        url: "/Search/SavedRecipe",
        method: "POST",
        data: {
            id: parseInt(id, 10),
            other: "Shelved"
        },
        success: _ => {
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Removed</strong> Recipe will not be shown again
                </div>
            `);
        },
        error: _ => {
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Error</strong> Something went wrong, maybe this recipe has been removed already?
                </div>
            `);
        }
    });
}

function commonInventory(list: string, amount: string): void {
    console.log(list)
    $.ajax({
        url: "/Search/EmptyInventory",
        method: "POST",
        data: {
            list: list,
            amount: parseInt(amount)
        },
        //success: (data) => {
        //    $("#fridge-table-main").empty();
        //    $("#fridge-table-main").html(data);
        //},
        error: (err) => { console.log(err); }
    })
    alert("Your ingredient has been cooked!");
}
const inputSearch: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
inputSearch.addEventListener("keydown", (e) => {
    //checks whether the pressed key is "Enter"
    if (e.keyCode === 13) {
        searchByName();
    }
});
