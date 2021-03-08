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
        });
    }
}
function AddIngredient(id) {
    let amount = prompt("Please enter the amount", "1");
    if (amount != '' && !isNaN(+amount)) {
        const response = fetch("Fridge/AddItem?id=" + id + "&amount=" + amount, {
            method: 'GET'
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
const inputSearchFridge = document.getElementById("ingredSearch");
inputSearchFridge.addEventListener("keydown", (e) => {
    if (e.keyCode === 13) {
        SearchByIngredientName();
    }
});
//# sourceMappingURL=Index.js.map