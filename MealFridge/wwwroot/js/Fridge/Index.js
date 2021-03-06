var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
window.onload = () => {
    const prevSearch = window.sessionStorage.getItem("prevSearch");
    if (prevSearch !== null) {
        let newSearch = document.getElementById("ingredSearch");
        newSearch.value = prevSearch;
        window.sessionStorage.clear();
        searchByName();
    }
};
class IngredientSearch {
    constructor(query) {
        this.URL = "/api/SearchByIngredientName/";
        this.query = query;
    }
    getPossibleIngredients() {
        return __awaiter(this, void 0, void 0, function* () {
            return Promise.resolve(this.fetchAPI(this.query).then(data => {
                console.log(data);
                this.showIngredients(data);
            }));
        });
    }
    fetchAPI(query) {
        return __awaiter(this, void 0, void 0, function* () {
            const response = fetch(this.URL + query, {
                method: 'GET'
            });
            return (yield response).json();
        });
    }
    showIngredients(ingredients) {
        let main = document.getElementById("fridge_main");
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
        });
    }
}
function SearchByIngredientName() {
    let search = document.getElementById("ingredSearch");
    if (!search.value) {
        alert("Search can not be empty!");
        return;
    }
    let searcher = new IngredientSearch(search.value);
    searcher.getPossibleIngredients();
}
//# sourceMappingURL=Index.js.map