// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {


    GetData();


});


function GetData() {
    let apiUrl = "https://localhost:7192/AdventureWorks/";

    $.ajax({
        url: apiUrl + 'GetPeople',
        type: 'GET',
        dataType: 'json',
        beforeSend: function () {
            $('#MainTable').hide();
            $('.spinner-border').show();
        },
        success: function (data) {

            let tableBody = "";

            $.each(data, function (i, val) {

                console.log(val);

                tableBody += `
                                <tr>
                                    <td>${val.businessEntityID}</td>
                                    <td>${val.firstName}</td>
                                    <td>${val.lastName}</td>
                                    <td>${val.jobTitle}</td>
                                    <td>${val.middleName}</td>
                                    <td>${val.suffix}</td>
                                    <td>${val.title}</td>
                                    
                                    <td>${val.phoneNumber}</td>
                                    <td>${val.emailAddress}</td>
                                    <td>${val.phoneNumberType}</td>
                                                    
                                    <td><button Id="Updatebtn" class="btn btn-primary btn-sm" data-id="${val.businessEntityID}">Edit</button></td>
                                    <td><button Id="Deletebtn" class="btn btn-danger btn-sm " data-id="${val.businessEntityID}">Delete</button></td>
                                                    
                                </tr>`;

            });
            $('#MainTable > tbody').replaceWith(tableBody);

            $('.spinner-border').hide();
            $('#MainTable').fadeIn(1400);
        },
        error: function () {
            alert('Error!');
        }
    });

};

// Create

$(function () {
    const url = '/Home/Add/';
    const placeHolderDiv = $('#modalPlaceHolder');
    $('#createbutton').click(function () {
        $.get(url).done(function (data) { // Jquery Ajax Get işlemi ile Partial Modal alınıyor. Controllerdan view dönüyor
            placeHolderDiv.html(data);
            placeHolderDiv.find(".modal").modal('show');
        });
    });

    placeHolderDiv.on('click',
        '#btnSave',
        function (event) {
            event.preventDefault(); // button submit ise veya farklı bir davranışı varsa bunu engelliyoruz.kıscası buttonun kendi click eventi engelleniyor
            const form = $('#form-person-add');
            const actionUrl = "https://localhost:7192/AdventureWorks/AddPerson"
            const dataToSend = form.serialize(); // 1. yöntem form datalaını almak için.

            // 2. yöntem
            var data2 = {
                FirstName: $("#FirstName").val(),
                LastName: $("#LastName").val(),
                Title: $("#Title").val(),
                Suffix: $("#Suffix").val(),
                PersonType: $("#PersonType").val(),
                MiddleName: $("#MiddleName").val(),
            }

            console.log("Data2" + data2.FirstName + " " + data2.LastName + " " + data2.MiddleName + " " + data2.Suffix + " " + data2.PersonType + " " + data2.Title);

            $.ajax({
                type: "POST",
                url: actionUrl,
                data: JSON.stringify(data2),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Accept': 'application/json',
                },
                success: function (msg) {

                    placeHolderDiv.find('.modal').modal('hide');

                    GetData();
                },
                error: function (error) {
                    alert(error);
                    console.log(error);
                }

            });

        });
});


// Update

$(function () {

    const url = '/Home/Update/';
    const placeHolderDiv = $('#modalPlaceHolder');

    $(document).on('click', '#Updatebtn', function (event) {
        event.preventDefault();
        const id = $(this).attr('data-id');
        $.get(url, { Id: id }).done(function (data) { // Partial alınıor
            placeHolderDiv.html(data);
            placeHolderDiv.find('.modal').modal('show');
        }).fail(function () {
            alert("Hata");
        });
    });

    placeHolderDiv.on('click',
        '#btnUpdate',
        function (event) {
            event.preventDefault(); // button submit ise veya farklı bir davranışı varsa bunu engelliyoruz.kıscası buttonun kendi click eventi engelleniyor
            const form = $('#form-person-update');
            const actionUrl = "https://localhost:7192/AdventureWorks/UpdatePerson"
            const dataToSend = form.serialize(); // 1. yöntem form datalaını almak için.

            // 2. yöntem
            var data2 = {
                FirstName: $("#FirstName").val(),
                LastName: $("#LastName").val(),
                Title: $("#Title").val(),
                Suffix: $("#Suffix").val(),
                PersonType: $("#PersonType").val(),
                MiddleName: $("#MiddleName").val(),
                businessEntityID: $("#hiddenId").val(),
            }

            console.log("Data2" + data2.FirstName + " " + data2.LastName + " " + data2.MiddleName + " " + data2.Suffix + " " + data2.PersonType + " " + data2.Title);

            $.ajax({
                type: "PUT",
                url: actionUrl,
                data: JSON.stringify(data2),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Accept': 'application/json',
                },
                success: function (msg) {

                    placeHolderDiv.find('.modal').modal('hide');
                    location.reload();

                    GetData();

                },
                error: function (error) {
                    alert(error);
                    console.log(error);
                }

            });

        });


});

// Delete

$(document).on('click', '#Deletebtn', function (event) {
    event.preventDefault();
    const id = $(this).attr('data-id');
    console.log(id);
    const actionUrl = "https://localhost:7192/AdventureWorks/DeletePerson/" + id;
    $.ajax({
        type: "Delete",
        url: actionUrl,
        /*data: {Id:id},*/
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        headers: {
            'Accept': 'application/json',
        },
        success: function (msg) {

            placeHolderDiv.find('.modal').modal('hide');
            location.reload();

            GetData();

        },
        error: function (error) {
            alert(error);
            console.log(error);
        }

    });


});

