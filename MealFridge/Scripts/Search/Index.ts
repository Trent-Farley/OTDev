window.onload = () => {
    const prevSearch = window.sessionStorage.getItem("prevSearch");
    if (prevSearch !== null) {
        let newSearch = <HTMLInputElement>document.getElementById("sbn");
        newSearch.value = prevSearch;
        searchByName(true)
    }
};

let pageNumber: number = 0;
let searchparam: string = "";

function stateChange(control) {
    switch (control.value.charAt(0)) {
        case '\u2753': //Question mark
            control.value = '\u2705'
            control.setAttribute("incuisine", "true")
            break;

        case '\u2705': //Check Mark
            control.value = '\u274C'
            control.removeAttribute("incuisine")
            control.setAttribute("excuisine", "true")
            break;

        case '\u274C': //X Mark
            control.value = '\u2753'
            control.removeAttribute("excuisine")
            break;
    }
}
function inventorySearch(): void {
    let search = $("#inventorySearch");
    let refine: HTMLInputElement = <HTMLInputElement>document.getElementById("panCheck");
    if (search.val() == "") {
        $("#main").empty();
        $("#main").append("You have no saved ingredients. Visit the Inventory page to add ingredients to your fridge.")
        return;
    }
    let inCuisine = "";
    $("[inCuisine]").each(function (i, el) {
        inCuisine += el.id + ',';
    });
    let exCuisine = "";
    $("[exCuisine]").each(function (i, el) {
        exCuisine += el.id + ',';
    });
    $.ajax({
        url: "/Search/SearchByIngredient",
        type: "POST",
        data: {
            QueryValue: search.val(),
            SearchType: "Ingredient",
            Refine: refine.checked,
            PageNumber: pageNumber,
            CuisineInclude: inCuisine,
            CuisineExclude: exCuisine
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
let CURRENT_NO_SEARCHES = 0;
function rateLimit(): boolean {
    if (CURRENT_NO_SEARCHES < 5) {
        ++CURRENT_NO_SEARCHES;
    }
    let loggedIn = window.sessionStorage.getItem("in");

    if (CURRENT_NO_SEARCHES > 4 && loggedIn === "false") {
        $("#modal-container").empty();
        $("#modal-container").html(`
    <div class="modal show" id="recipe-modal" tabindex="-1" role="dialog" aria-labelledby="modal_title" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered  modal-lg modal-dialog-scrollable w-100" role="document">
        <div class="modal-content">
            <div class="modal-header bg-warning">
                <h5 class="modal-title" id="modal_title">Warning!</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
              <div class="modal-body">
                Looks like you are past the limit of 5 searches. Please sign in to search more
             </div>
        </div>
    <div>
    <div>

`);
        $('#recipe-modal').modal("show");
        return false;
    }
    else {
        return true;
    }
}
function searchByName(isPage: boolean): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
    let type: HTMLInputElement = <HTMLInputElement>document.getElementById("searchType");
    let cheap: HTMLInputElement = <HTMLInputElement>document.getElementById("cheapCheck");
    if (rateLimit()) {
        if (isPage) {
            pageNumber = 0;
        }
        if (!search.value) {
            $("#warning-toast-body").empty();
            $("#warning-toast-body").append("Search can not be empty!");
            $(".toast").toast('show');
            return;
        }
        let inCuisine = "";
        $("[inCuisine]").each(function (i, el) {
            inCuisine += el.id + ',';
        });
        let exCuisine = "";
        $("[exCuisine]").each(function (i, el) {
            exCuisine += el.id + ',';
        });
        $.ajax({
            url: "/Search/SearchByName",
            type: "POST",
            data: {
                QueryValue: search.value,
                SearchType: type.value,
                Cheap: cheap.checked,
                PageNumber: pageNumber,
                CuisineInclude: inCuisine,
                CuisineExclude: exCuisine
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
}
function undoFavorite(id: string): void {
    $.ajax({
        url: "/AccountManagement/DeleteRecipe",
        method: "POST",
        data: {
            id: parseInt(id)
        },
        success: _ => {
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-primary alert-dismissible fade show" role="alert" id="favUndoSuccess">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Favorite Undone</strong> Recipe has been removed to your favorites
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
                  <strong>Error</strong> Something went wrong.
                </div>
            `);
        }
    });
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
                <div class="alert alert-primary alert-dismissible fade show" role="alert" id="FavoriteSuccess">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Added to Favorites</strong> Click here to undo
                  <button id="undoFavButton" class="btn btn-danger" onclick=undoFavorite(${id}) title="Undo Favorite"><i class="fas fa-undo"></i></button>
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
        error: (err) => { console.log(err); }
    })
    alert("Your ingredient has been cooked!");
}
const inputSearch: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
inputSearch.addEventListener("keydown", (e) => {
    //checks whether the pressed key is "Enter"
    if (e.keyCode === 13) {
        searchByName(true);
    }
});