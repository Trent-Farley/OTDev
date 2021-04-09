document.getElementById('print-list-button').addEventListener('click', e => {
    window.print();
})
const ingredients = [];

function addToShoppingList(ingreds: Array<string>) {
    document.getElementById("shoppingListTable").innerHTML += `
    ${ingreds.map(i => {
        return `<tr> <td> ${i} </td> </tr>`
    }).join('')}
    `;
    $("#shoppingList").collapse("show");
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
            $("#hidden-ingredients p").each(function () {
                //Add only one. Logic required to add multiple later. Maybe 
                //turn ingredients into a list of objects with the amount needed
                if (!ingredients.includes($(this).text())) {
                    //$(this) refers to the text inside of the p tag. 
                    ingredients.push($(this).text())
                }
            });
            document.getElementById('button-cart').addEventListener('click', () => {
                addToShoppingList(ingredients);
            })
        },
        error: (err) => { console.log(err); }
    });
}




