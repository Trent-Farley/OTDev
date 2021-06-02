function addToShoppingList(id: string) {
    $.ajax({
        traditional: true,
        url: "/Shopping/AddRecipeIngredients",
        method: "POST",
        data: {
            'id': id
        },
        success: _ => {
            $("#alert").empty();
            $("#alert").html(`
            <div class="alert alert-primary alert-dismissible fade show" role="alert">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
                </button>
                <strong>Added</strong> ingredients to your <a href="/Shopping/">Shopping List!</a>
            </div>
        `);
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
        },
        error: (err) => { console.log(err); }
    });
}