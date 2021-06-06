function unitQuantity(id: string, type: string): void {
    $("#modal-container").empty();
    $("#modal-container").html(`
            <div class="modal fade" id="selector" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Enter the amount and measurement</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <label for="amount">Amount of item:</label>
                            <input type="number" id="amount" min="0" max="1000">
                            <label for="units">Unit type:</label>
                            <select id="units">
                                <option value="Teaspoon">Teaspoon</option>
                                <option value="Tablespoon">Tablespoon</option>
                                <option value="Cup">Cup</option>
                                <option value="Pint">Pint</option>
                                <option value="Quart">Quart</option>
                                <option value="Gallon">Gallon</option>
                                <option value="Milliliter">Milliliter</option>
                                <option value="Liter">Liter</option>
                                <option value="Gram">Gram</option>
                                <option value="Kilogram">Kilogram</option>
                                <option value="Ounce">Ounce</option>
                                <option value="Pound">Pound</option>
                            </select>
                        </div>
                        <div class="modal-footer" id="buttons">
                            
                        </div>
                    </div>
                </div>
            </div>
        `);
    if (type == "inventory") {
        $('#buttons').html(`
            <button type="button" class="btn btn-primary" id="addFridge">Submit</button>
        `);
        const addFridge = document.getElementById("addFridge");
        addFridge.addEventListener("click", () => {
            let amount = $("#amount").val().toString();
            let unit = $("#units").val().toString();
            $('#selector').modal("hide")
            addIngredient(id, amount, unit)
        })
    }
    else {
        $('#buttons').html(`
            <button type="button" class="btn btn-primary" id="addShopping">Submit</button>
        `)
        const addShop = document.getElementById("addShopping");
        addShop.addEventListener("click", () => {
            let amount = $("#amount").val().toString();
            let unit = $("#units").val().toString();
            $('#selector').modal("hide");
            addShopping(id, amount, unit);
        })
    }
    $('#selector').modal();
}



function addIngredient(id: string, amount: string, unit: string): void {
    let current = parseInt($("#current-card-" + id).text(), 10);
    $("#current-card-" + id).empty()
    $("#current-card-" + id).append((current + 1).toString());//Add one for updated value
    //fridge-table-main
    $.ajax({
        url: "/Fridge/AddItem",
        method: "POST",
        data: {
            id: id,
            amount: parseInt(amount, 10),
            unit: unit
        },
        success: (data) => {
            $("#fridge-table-main").empty();
            $("#fridge-table-main").html(data);
        },
        error: (err) => { console.log(err); }
    })
}

function addShopping(id: string, amount: string, unit: string): void {
    $.ajax({
        url: "/Shopping/AddItem",
        method: "POST",
        data: {
            id: id,
            amount: parseInt(amount, 10),
            unit: unit
        },
        success: (data) => {
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-primary alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  Added item to shopping list!
                </div>
            `)
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

