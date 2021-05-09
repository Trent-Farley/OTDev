document.getElementById('print-list-button').addEventListener('click', e => {
    window.print();
})
document.getElementById('measure-swap').addEventListener('click', e => {
    $.ajax({
        url: "/Shopping/Swap",
        method: "POST",
        success: (data) => {
            $("#innerShopping").empty();
            $("#innerShopping").html(data);
        }
    })
})

function updateNeeded(id: string, amount: string): void {
    $.ajax({
        url: "/Shopping/AddItem",
        method: "POST",
        data: {
            id: id,
            amount: parseInt(amount)
        },
        success: (data) => {
            $("#innerShopping").empty();
            $("#innerShopping").html(data);
        },
        error: (err) => { console.log(err); }
    })
}

function updateQuant(id: string, amount: string): void {
    $.ajax({
        url: "/Shopping/AddFridgeItem",
        method: "POST",
        data: {
            id: id,
            amount: parseInt(amount)
        },
        success: (data) => {
            $("#innerShopping").empty();
            $("#innerShopping").html(data);
        },
        error: (err) => { console.log(err); }
    })
}