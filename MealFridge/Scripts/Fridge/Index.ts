window.onload = () => {
    const prevSearch = window.sessionStorage.getItem("prevSearch");
    if (prevSearch !== null) {
        let newSearch = <HTMLInputElement>document.getElementById("ingredSearch");
        newSearch.value = prevSearch;
        window.sessionStorage.clear();
        searchByName()
    }
};



class IngredientSearch {
    private readonly URL: string = "/api/SearchByIngredientName/"
    private readonly query: string;
    public ingredients: [string];
    constructor(query: string) {
        this.query = query;
    }

    public async getPossibleIngredients(): Promise<void | [string]> {

        return Promise.resolve(this.fetchAPI(this.query).then(data => {
            console.log(data);
            this.showIngredients(<[Object]>data);
        }))
    }
    private async fetchAPI<T>(query: string): Promise<T> {
        const response = fetch(this.URL + query, {
            method: 'GET'
        })
        return (await response).json() as Promise<T>;

    }
    private showIngredients(ingredients: [Object]) {
        let main: HTMLElement = document.getElementById("fridge_main");
        main.innerHTML = "";
        console.log(ingredients);
        ingredients.forEach(r => {
            main.innerHTML +=
                `
                <div class="card shadow-lg">
                    <div class="row">
                        <div class="col-4">
                            <img class="card-img-left" src="${r["image"]}" alt="Ingredient Icon">
                        </div>
                        <div class="card-body col-6">
                            <h4 class="card-title">${r["name"]}</h4>
                            <p class="card-text">Cost per Serving: ${r["cost"]}</p>
                            <p class="card-text">Aisle: ${r["aisle"]}</p>
                        </div>
                        <div class="btn-group-vertical col-2" role="group">
                            <button type="button" class="btn btn-primary addIngred" value="${r["id"]}" onclick="AddIngredient(this.value)">Add</button>
                        </div>
                    </div>
                </div>
            `;
        })
    }
}

function AddIngredient(id: string): void {
    let amount = prompt("Please enter the amount", "1")
    if (amount != '' && !isNaN(+amount)) {
        const response = fetch("Fridge/AddItem?id=" + id + "&amount=" + amount, {
            method: 'GET'           
        })
    }
}

function SearchByIngredientName(): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("ingredSearch");
    if (!search.value) {
        alert("Search can not be empty!");
        return;
    }
    let searcher = new IngredientSearch(search.value);
    searcher.getPossibleIngredients();
}

const inputSearchFridge: HTMLInputElement = <HTMLInputElement>document.getElementById("ingredSearch");
inputSearchFridge.addEventListener("keydown", (e) => {
    //checks whether the pressed key is "Enter"
    if (e.keyCode === 13) {
        SearchByIngredientName();
    }

});
