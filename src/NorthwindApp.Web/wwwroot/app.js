// specify the columns
var columnDefs = [
    { headerName: "#", field: "id" },
    { headerName: "Name", field: "productName" },
    { headerName: "Supplier", field: "supplierId" },
    { headerName: "Quantity Per Unit", field: "quantityPerUnit" },
    { headerName: "Unit Price", field: "unitPrice" },
    { headerName: "Units In Stock", field: "unitsInStock" },
    { headerName: "Units On Order", field: "unitsOnOrder" },
    { headerName: "Reorder Level", field: "reorderLevel" },
    { headerName: "Discontinued", field: "discontinued" }
];

// let the grid know which columns and what data to use
var gridOptions = {
    columnDefs: columnDefs,
    enableCellChangeFlash: true,
    rowData: [],
    onFirstDataRendered: function (params) {
        params.api.sizeColumnsToFit();
    },
    onGridReady: function (params) {
        params.api.setRowData([]);
        params.api.sizeColumnsToFit();
    }
};

function submit() {
    var url = '/api/product';
    var query = document.querySelector("textarea[name=query]").value;

    axios.get(url, {
        params: {
            q: query
        }
    }).then(function (resp) {
        gridOptions.api.setRowData(resp.data);
        gridOptions.api.sizeColumnsToFit();
    }).catch(err => {
        throw err;
    })
}

function clear() {
    document.querySelector("textarea[name=query]").value = null;
}

function updateQuery(el) {
    document.querySelector("textarea[name=query]").value = el.getAttribute('data-query');
}


document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll("button.button[data-query]").forEach(el => {
        el.innerText = el.getAttribute('data-query');
    });

    // lookup the container we want the Grid to use
    var eGridDiv = document.querySelector('#myGrid');

    // create the grid passing in the div to use together with the columns & data we want to use
    new agGrid.Grid(eGridDiv, gridOptions);
})