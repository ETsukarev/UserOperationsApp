// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function prepareServerSideParams(params) {

    var serverSideParams = {};

    serverSideParams.draw = params.draw;
    serverSideParams.start = params.start;
    serverSideParams.length = params.length;
    serverSideParams.searchValue = params.search.value;
    serverSideParams.searchRegex = params.search.regex;

    // order[i][column]
    var i;
    var separator = "|";
    var orderColumns = "";
    for (i = 0; i < params.order.length; i++) {
        orderColumns += params.order[i].column.toString(10);
        orderColumns += separator;
    }
    serverSideParams.orderColumns = orderColumns;

    // order[i][dir]
    var orderDirs = "";
    for (i = 0; i < params.order.length; i++) {
        orderDirs += params.order[i].dir;
        orderDirs += separator;
    }
    serverSideParams.orderDirs = orderDirs;

    // columns[i][data], searchable, orderable, [search][value], [search][regex]

    var columnsDatas = "";
    var columnsSearchable = "";
    var columnsOrderable = "";
    var columnsSearchValue = "";
    var columnsSearchRegex = "";
    for (i = 0; i < params.columns.length; i++) {
        columnsDatas += params.columns[i].data;
        columnsDatas += separator;

        columnsSearchable += params.columns[i].searchable.toString();
        columnsSearchable += separator;

        columnsOrderable += params.columns[i].orderable.toString();
        columnsOrderable += separator;

        columnsSearchValue += params.columns[i].search.value;
        columnsSearchValue += separator;

        columnsSearchRegex += params.columns[i].search.regex.toString();
        columnsSearchRegex += separator;
    }
    serverSideParams.columnsDatas = columnsDatas;
    serverSideParams.columnsSearchable = columnsSearchable;
    serverSideParams.columnsOrderable = columnsOrderable;
    serverSideParams.columnsSearchValue = columnsSearchValue;
    serverSideParams.columnsSearchRegex = columnsSearchRegex;

    return serverSideParams;
}

function filterResponseData(data) {

    var res = {};

    var json = jQuery.parseJSON(data);

    if (json.users.error !== "") {
        res.error = json.users.error;
        res.data = [];
        res.recordsTotal = 0;
        res.recordsFiltered = 0;
    } else {
        res.recordsTotal = json.users.recordsTotal;
        res.recordsFiltered = json.users.recordsFiltered;
        res.data = json.users.data;
    }
    //return res;
    return JSON.stringify(res);
}
