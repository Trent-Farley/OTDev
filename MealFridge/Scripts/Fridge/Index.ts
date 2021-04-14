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

function banIngred(id: string): void {
    $.ajax({
        url: "/Fridge/Restriction",
        method: "POST",
        data: {
            id: parseInt(id, 10),
            other: "Banned"
        }
    });
}

function hideIngred(id: string): void {
    $.ajax({
        url: "/Fridge/Restriction",
        method: "POST",
        data: {
            id: parseInt(id, 10),
            other: "Dislike"
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

