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
    public recipes: [string];
    constructor(query: string) {
        this.query = query;
    }

    public async getPossibleRecipes(): Promise<void | [string]> {
        return Promise.resolve(this.fetchAPI(this.query).then(data => {
            this.showRecipes(<[Object]>data);
        }))
    }

    private async fetchAPI<T>(query: string): Promise<T> {
        const response = fetch(this.URL + query, {
            method: 'GET'
        })
        return (await response).json() as Promise<T>;

    }
    private showRecipes(recipes: [Object]) {
        let main: HTMLElement = document.getElementById("main");
        main.innerHTML = "";
        if (recipes.length < 1) {
            main.innerHTML += "<p> No results found! </p>";
            return;
        }
        recipes.forEach(r => {

            main.innerHTML +=
                `
                <div class="card shadow-lg">
                    <img class="card-img-top" src="${r["image"]}" alt="Recipe Image">
                    <div class="card-body">
                        <h4 class="card-title">${r["title"]}</h4>
                        <a href="#!" class="btn btn-primary">Recipe Details</a>
                    </div>
                </div>
            `;
        })
    }
}

function searchByName(): void {
    let search: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
    if (!search.value) {
        alert("Search can not be empty!");
        return;
    }
    let searcher = new Search(search.value);
    searcher.getPossibleRecipes();
}

const inputSearch: HTMLInputElement = <HTMLInputElement>document.getElementById("sbn");
inputSearch.addEventListener("keydown", (e) => {
    //checks whether the pressed key is "Enter"
    if (e.keyCode === 13) {
        searchByName();
    }

});
