var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
document.getElementById('print-list-button').addEventListener('click', e => {
    window.print();
});
class Details {
    constructor(id) {
        this.URL = "/api/RecipeDetails/";
        this.id = id;
    }
    getDetails() {
        return __awaiter(this, void 0, void 0, function* () {
            return Promise.resolve(this.fetchAPI().then(data => {
                this.showRecipes(data);
            }));
        });
    }
    fetchAPI() {
        return __awaiter(this, void 0, void 0, function* () {
            $('#recipe-modal').modal("show");
            document.getElementById('modal-main').innerHTML += `
                <div class="d-flex justify-content-center">
                   <div class="spinner-grow text-success" role="status">
                        <span class="sr-only">Loading...</span>
                    </div>
                </div>
        `;
            const response = fetch(this.URL + this.id, {
                method: 'GET'
            });
            return (yield response).json();
        });
    }
    modalDetails(image, recipeIngredients, summary, title) {
        let main = document.getElementById('modal-main');
        main.innerHTML = "";
        let ingredients = [];
        let recipeDetails = `
            <h4 class="card-title"> ${title} </h4>
            <img class="card-img-top" src="${image}" alt="Recipe details image"></img>
            <div class="card-body">
                <h7 class="card-subtitle">Required Ingredients and amounts </h7>
                <ul class="list-group">
                    ${recipeIngredients.map(i => {
            ingredients.push(i["ingred"]["name"]);
            return `
                            <li class="list-group-item-light">
                                ${i["amount"]}}
                            </li>
                        `;
        }).join('')}
                </ul>
                 <div class="card-footer">
                    Please check out the recipe <a href="${summary}"> here </a> for full instructions 
                </div>
            </div>

        `;
        main.innerHTML += recipeDetails;
        document.getElementById('button-cart').addEventListener('click', () => {
            addToShoppingList(ingredients);
        });
    }
    showRecipes(rd) {
        if (rd.length < 1) {
            $('modal-body').append(`<p class="text-center">><i> No details availible for this recipe</i> </p>`);
            return;
        }
        let rec = rd[0];
        console.log(rec);
        this.modalDetails(rec["image"], rec["recipeingreds"], rec["summery"], rec["title"]);
    }
}
function addToShoppingList(ingreds) {
    document.getElementById("shoppingListTable").innerHTML += `
    ${ingreds.map(i => {
        return `<tr> <td> ${i} </td> </tr>`;
    }).join('')}
    `;
    $("#shoppingList").collapse("show");
}
function getDetails(id) {
    let details = new Details(id);
    details.getDetails();
}
//# sourceMappingURL=RecipeDetails.js.map