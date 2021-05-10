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
            $("#genSLButton").show();
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

function genSLfromMP(): void {
    let temp = $('h4');
    let recipes: string[] = [];
    for (let i = 0; i < temp.length; i++) {
        recipes.push(temp[i].id.substring(6));
    }
    var postData = { values: recipes };
    $.ajax({
        url: "/Shopping/AddFromMealPlan",
        type: "POST",
        data: postData,
        traditional: true,
        error: (err) => { console.log(err); },
        success: () => {
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-primary alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Saved</strong> Your Shopping List is updated
                </div>
            `);
            $("#genSLButton").hide();

        }
    });
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