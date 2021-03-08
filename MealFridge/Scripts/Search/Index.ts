
window.onload = () => {

    const prevSearch = window.sessionStorage.getItem("prevSearch");
    if (prevSearch !== null) {
        let newSearch = <HTMLInputElement>document.getElementById("sbn");
        newSearch.value = prevSearch;
        window.sessionStorage.clear();
        searchByName()
    }
};


class Search {
    private readonly URL: string = "/api/SearchByName/"
    private readonly query: string;

    private readonly type: string;
    public recipes: [string];
    constructor(query: string, type: string) {
        this.query = query;
        this.type = type;
    }

    public async getPossibleRecipes(): Promise<void | [string]> {
        return Promise.resolve(this.fetchAPI(this.query, this.type).then(data => {
            this.showRecipes(<[Object]>data);
        }))
    }
    private async fetchAPI<T>(query: string, type: string): Promise<T> {
        const response = fetch(this.URL + query + "/" + type, {
            method: 'GET'
        })
        return (await response).json() as Promise<T>;

    }
    private showRecipes(recipes: [Object]) {
        let main: HTMLElement = document.getElementById("main");
        document.getElementById("spinner").innerHTML = "";
        main.innerHTML = "";
        if (recipes.length < 1) {
            main.innerHTML += "<p> No results found! </p>";
            return;
        }
        recipes.forEach(r => {

            main.innerHTML +=
                `
                <button class="btn" type="button" onClick="getDetails(${r["id"]})" data-toggle="collapse" data-target="#info-${r["id"]}" aria-expanded="false" aria-controls="info-${r["id"]}">
                    <div class="card shadow-lg">
                        <img class="card-img-top" src = "${r["image"]}" alt = "Recipe Image" >
                        <div class="card-body" >
                            <h4 class="card-title" > ${r["title"]} </h4>
                         </div>  
                    </div>
                </button>
                `;
        })
    }
}
function inventorySearch(): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("inventorySearch");
    if (search.value == "") {
        alert("You have no saved ingredients. Visit the Inventory page to add ingredients to your fridge.")
        return;
    }
    let searcher = new Search(search.value, "Ingredient");
    searcher.getPossibleRecipes();
}
function searchByName(): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
    let type: HTMLInputElement = <HTMLInputElement>document.getElementById("searchType");
    if (!search.value) {
        alert("Search can not be empty!");
        return;
    }
    let searcher = new Search(search.value, type.value);
    searcher.getPossibleRecipes();
}

const inputSearch: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
inputSearch.addEventListener("keydown", (e) => {
    //checks whether the pressed key is "Enter"
    if (e.keyCode === 13) {
        searchByName();
    }

});
