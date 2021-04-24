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
    $.ajax({
        url: "/MealPlan/GetFavorites"
    });
}