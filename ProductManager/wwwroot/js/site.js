// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function myFunction(button, hasBusket, url) {
    if (hasBusket) {
        $.ajax({
            type: "Get",
            url: url,
            //data: '{id: ' + JSON.stringify(id) + '}',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                // alert("Data has been added successfully.");
                LoadData();
            },
            error: function () {
                console.log("Error while inserting data");
            }
        });

        button.disabled = true;
        button.style.backgroundColor = "grey";
    }
}