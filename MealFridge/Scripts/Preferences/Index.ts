function removeRestrictedIngredient(id: string): void {
    $.ajax({
        url: "/AccountManagement/RemoveRestrictedIngredient",
        method: "POST",
        data: {
            id: parseInt(id)
        },
        success: (data) => {
            location.reload();
        },
        error: (err) => { console.log(err); }
    })
}