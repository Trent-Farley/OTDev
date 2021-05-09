let RECIPEID: string = "";
function getMealPlan(): void {
    $.ajax({
        url: "/MealPlan/MealPlan",
        type: "POST",
        data: {
            days: 7
        },
        error: (err) => { console.log(err); },
        success: (generatedMeals) => {
            $("#meals").html(generatedMeals);
        }
    })
}
function getFavorites(id: string): void {
    RECIPEID = id;
    $.ajax({
        url: "/MealPlan/GetFavoritses",
        type: "POST",
        error: (err) => { console.log(err); },
        success: (savedRecipes) => {
            $("#modal-container").empty();
            $("#modal-container").html(savedRecipes);
            $('#recipe-modal').modal("show");
        }
    });
}
function swapOut(id: string): void {
    let modalTitle = $("#mtitle-" + id).text();
    let title = $("#title-" + RECIPEID);
    title.text(modalTitle);
    $("#img-" + RECIPEID).attr("src", $("#mimg-" + id).attr("src"));
    $("#recipe-modal").modal('toggle');
}
function getMealDetails(recipeId) {
    $.ajax({
        url: "/MealPlan/MealDetails",
        method: "POST",
        data: {
            QueryValue: recipeId
        },
        success: (data) => {
            $("#modal-container").empty();
            $("#modal-container").html(data);
            $('#meal-modal').modal("show");
        },
        error: (err) => { console.log(err); }
    });
}
function regenerate(mealDay, currentId) {
    $.ajax({
        url: "/MealPlan/RegenerateMeal",
        method: "POST",
        data: {
            mealDay: mealDay
        },
        success: (data) => {
            console.log(currentId);
            $("#" + currentId).html(data);
        },
        error: (err) => { console.log(err); }
    });
}