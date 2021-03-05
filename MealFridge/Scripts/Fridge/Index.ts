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
                    <img class="card-img-top" src="${r["image"]}" alt="Ingredient Icon">
                    <div class="card-body">
                        <h4 class="card-title">${r["name"]}</h4>
                       
                    </div>
                </div>
            `;
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
