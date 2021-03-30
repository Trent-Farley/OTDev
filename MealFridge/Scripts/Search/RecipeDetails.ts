document.getElementById('print-list-button').addEventListener('click', e => {
    window.print();
})

class Details {
    private readonly URL: string = "/api/RecipeDetails/";
    private id: string;
    constructor(id: string) {
        this.id = id;
    }
    public async getDetails(): Promise<void | [string]> {
        return Promise.resolve(this.fetchAPI().then(data => {
            this.showRecipes(<[Object]>data);
        }))
    }

    private async fetchAPI<T>(): Promise<T> {

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
        })
        return (await response).json() as Promise<T>;

    }
    private modalDetails(image: string, recipeIngredients: Array<Object>, summary, title): void {
        let main: HTMLElement = document.getElementById('modal-main');
        main.innerHTML = "";
        let ingredients: Array<string> = [];
        let recipeDetails: string = `
            <h4 class="card-title"> ${title} </h4>
            <img class="card-img-top" src="${image}" alt="Recipe details image"></img>
            <div class="card-body">
                <h7 class="card-subtitle">Required Ingredients and amounts </h7>
                <ul class="list-group">
                    ${recipeIngredients.map(i => {
            ingredients.push(i["ingred"]["name"]);
            return `
                            <li class="list-group-item-light">
                                ${i["amount"]}
                            </li>
                        ` ;
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
        })
    }

    private showRecipes(rd: [Object]) {
        if (rd.length < 1) {
            $('modal-body').append(`<p class="text-center">><i> No details availible for this recipe</i> </p>`);
            return;
        }
        let rec: Object = rd[0];
        console.log(rec);
        this.modalDetails(rec["image"], rec["recipeingreds"], rec["summery"], rec["title"])

    }
}

function addToShoppingList(ingreds: Array<string>) {
    document.getElementById("shoppingListTable").innerHTML += `
    ${ingreds.map(i => {
        return `<tr> <td> ${i} </td> </tr>`
    }).join('')}
    `;
    $("#shoppingList").collapse("show");
}

function getDetails(id: string): void {

    let details = new Details(id);
    details.getDetails();
}


