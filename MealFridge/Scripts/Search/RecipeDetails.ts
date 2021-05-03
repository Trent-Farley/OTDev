function addToShoppingList(id: string) {
    $.ajax({
        traditional: true,
        url: "/Shopping/AddRecipeIngredients",
        method: "POST",
        data: {
            'id': id
        },
        success: (data) => {
            alert("Added Ingredients to shopping list!")
        },
        error: (err) => { console.error(err) }
    })
}

function getDetails(id: string): void {
    $.ajax({
        url: "/Search/RecipeDetails",
        method: "POST",
        data: {
            QueryValue: id
        },
        success: (data) => {
            $("#modal-container").empty();
            $("#modal-container").html(data);
            $('#recipe-modal').modal("show");

            document.getElementById('button-cart').addEventListener('click', () => {
                addToShoppingList(id);
            })
        },
        error: (err) => { console.log(err); }
    });
}