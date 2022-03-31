$(document).ready(function () {
  
    $("#Estates").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": '/api/Api/GetEstate',
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": 0,
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "id", "name": "Id", "autowidth": true },
            { "data": "categoryID", "name": "categoryID", "autowidth": true },
            { "data": "typeID", "name": "TypeID", "autowidth": true },
            { "data": "stateID", "name": "StateID", "autowidth": true },
            { "data": "Longitude", "name": "Longitude", "autowidth": true },
            { "data": "Latitude", "name": "Latitude", "autowidth": true },
            { "data": "cityID", "name": "CityID", "autowidth": true },
            { "data": "currencyID", "name": "CurrencyID", "autowidth": true },
            {
                "render": function (data, type, row) { return '<a href="#" class="btn btn-outline-danger" onclick=DeleteCustomer("' + row.id + '"); > Delete </a>' },
                "orderable": false
            },
            {
                "render": function (data, type, row, meta) { return '<a href="#" data-bs-toggle="modal" data-bs-target="#staticBackdrop" class="btn btn-sm btn-outline-primary ms-2" data-toggle="modal" onclick=EditItem(' + JSON.stringify(row) + '); > Edit </a>' },
                "orderable": false
            },

        ]
    });
});