import { post } from "jquery";

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

document.getElementById('remove-obtained').addEventListener('click', e => {
    let obtained = "";
    $(".strikeout").each(function (i, el) {
        obtained += el.id.substring(0, el.id.length - 3) + ',';
    })
    $.ajax({
        url: "/Shopping/Obtained",
        method: "POST",
        data: {
            ids: obtained,
            obtain: false
        },
        success: (data) => {
            $("#innerShopping").empty();
            $("#innerShopping").html(data);
        }
    })
})

document.getElementById('add-obtained').addEventListener('click', e => {
    let obtained = "";
    $(".strikeout").each(function (i, el) {
        obtained += el.id.substring(0, el.id.length - 3) + ',';
    })
    $.ajax({
        url: "/Shopping/Obtained",
        method: "POST",
        data: {
            ids: obtained,
            obtain: true
        },
        success: (data) => {
            $("#innerShopping").empty();
            $("#innerShopping").html(data);
        }
    })
})

function checkedEvent(id: string): void {
    let eleRow = "#" + id + "row";
    let row = $(eleRow);
    if (row.hasClass("strikeout"))
        row.removeClass("strikeout");
    else
        row.addClass("strikeout");
}

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