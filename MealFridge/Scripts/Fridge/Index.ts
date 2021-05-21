function addIngredient(id: string, amount: string): void {
    let current = parseInt($("#current-card-" + id).text(), 10);
    $("#current-card-" + id).empty()
    $("#current-card-" + id).append((current + 1).toString());//Add one for updated value
    //fridge-table-main
    $.ajax({
        url: "/Fridge/AddItem",
        method: "POST",
        data: {
            id: id,
            amount: parseInt(amount, 10)
        },
        success: (data) => {
            $("#fridge-table-main").empty();
            $("#fridge-table-main").html(data);
        },
        error: (err) => { console.log(err); }
    })
}

function addShopping(id: string, amount: string): void {
    $.ajax({
        url: "/Shopping/AddItem",
        method: "POST",
        data: {
            id: id,
            amount: parseInt(amount, 10)
        },
        success: (data) => {
            alert("Added item to shopping list.")
        },
        error: (err) => { console.log(err); }
    })
}
function undoIngredRestriction(id: string): void {
    $.ajax({
        url: "/AccountManagement/RemoveRestrictedIngredient",
        method: "POST",
        data: {
            id: parseInt(id)
        },
        success: (data) => {
            $("#alert").empty();
            SearchByIngredientName();
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
    })
}
function banIngred(id: string): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("ingredSearch");
    if (!search.value) {
        search.value = "";
    }
    $.ajax({
        url: "/Fridge/Restriction",
        method: "POST",
        data: {
            id: parseInt(id, 10),
            other: "Banned",
            QueryValue: search.value
        },
        success: (data) => {
            $("#fridge_main").empty();
            $("#fridge_main").html(data);
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-primary alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Removed</strong> Click Here to Undo  
                  <button id="undoFavButton" class="btn btn-danger" onclick="undoIngredRestriction(${id})"><i class="fas fa-undo"></i></button>
                </div>
            `);
        }
    });
}

function hideIngred(id: string): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("ingredSearch");
    if (!search.value) {
        search.value = "";
    }
    $.ajax({
        url: "/Fridge/Restriction",
        method: "POST",
        data: {
            id: parseInt(id, 10),
            other: "Dislike",
            QueryValue: search.value
        },
        success: (data) => {
            $("#fridge_main").empty();
            $("#fridge_main").html(data);
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-primary alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Banned</strong> Click Here to Undo  
                  <button id="undoFavButton" class="btn btn-danger" onclick="undoIngredRestriction(${id})"><i class="fas fa-undo"></i></button>
                </div>
            `);
        }
    });
}
function updateInventory(id: string, amount: string): void {
    $.ajax({
        url: "/Fridge/AddItem",
        method: "POST",
        data: {
            id: id,
            amount: parseInt(amount)
        },
        success: (data) => {
            $("#fridge-table-main").empty();
            $("#fridge-table-main").html(data);
        },
        error: (err) => { console.log(err); }
    })
}

function SearchByIngredientName(): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("ingredSearch");
    if (!search.value) {
        return;
    }
    $.ajax({
        url: "/Fridge/SearchIngredients",
        method: "POST",
        data: {
            QueryValue: search.value
        },
        success: (data) => {
            $("#fridge_main").empty();
            $("#fridge_main").html(data);
        },
        error: (err) => { console.log(err); }
    });
}

const inputSearchFridge: HTMLInputElement = <HTMLInputElement>document.getElementById("ingredSearch");
inputSearchFridge.addEventListener("keydown", (e) => {
    //checks whether the pressed key is "Enter"
    if (e.keyCode === 13) {
        SearchByIngredientName();
    }
});

