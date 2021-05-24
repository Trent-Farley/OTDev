function removeRestrictedIngredient(id: string): void {
    $.ajax({
        url: "/AccountManagement/RemoveRestrictedIngredient",
        method: "POST",
        data: {
            id: parseInt(id)
        },
        success: (data) => {
            location.reload();
        },
        error: (err) => { console.log(err); }
    })
}

function updateDiet(): void {
    let Whole30: HTMLInputElement = <HTMLInputElement>document.getElementById("Whole30");
    let DairyFree: HTMLInputElement = <HTMLInputElement>document.getElementById("DairyFree");
    let GlutenFree: HTMLInputElement = <HTMLInputElement>document.getElementById("GlutenFree");
    let Keto: HTMLInputElement = <HTMLInputElement>document.getElementById("Keto");
    let Vegan: HTMLInputElement = <HTMLInputElement>document.getElementById("Vegan");
    let Vegetarian: HTMLInputElement = <HTMLInputElement>document.getElementById("Vegetarian");
    let LactoVeg: HTMLInputElement = <HTMLInputElement>document.getElementById("LactoVeg");
    let OvoVeg: HTMLInputElement = <HTMLInputElement>document.getElementById("OvoVeg");
    let Paleo: HTMLInputElement = <HTMLInputElement>document.getElementById("Paleo");
    let Pescetarian: HTMLInputElement = <HTMLInputElement>document.getElementById("Pescetarian");
    let Primal: HTMLInputElement = <HTMLInputElement>document.getElementById("Primal");
    let Metric: HTMLInputElement = <HTMLInputElement>document.getElementById("Metric");
    $.ajax({
        url: "/AccountManagement/UpdateDiet",
        method: "POST",
        data: {
            whole30: Whole30.checked,
            dairyFree: DairyFree.checked,
            glutenFree: GlutenFree.checked,
            keto: Keto.checked,
            Vegan: Vegan.checked,
            vegetarian: Vegetarian.checked,
            lactoVeg: LactoVeg.checked,
            ovoVeg: OvoVeg.checked,
            paleo: Paleo.checked,
            pescetarian: Pescetarian.checked,
            primal: Primal.checked,
            metric: Metric.checked
        },
        success: (data) => {
            $("#alert").empty();
            $("#alert").html(`
                <div class="alert alert-primary alert-dismissible fade show" role="alert">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <strong>Updated</strong> Your diet settings have been saved
                </div>
            `);
        },
        error: (err) => { console.log(err); }
    })
}