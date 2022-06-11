// static object
var Sortable = {
    baseURL: '',
    sortBy: 0,
    SearchTerm: '',
    Search() {
        var searchKey = $('#txtSearch').val();
        window.location.href = sort.baseURL + "?id=" + searchKey;
    },
    Sort(sortBy) {
        var isDesc = true;

        const urlParams = new URLSearchParams(window.location.search);

        var isDescOriginal = urlParams.get('isDesc');
        const sortByOriginal = urlParams.get('sortBy');

        if (sortByOriginal == sortBy) {
            if (isDescOriginal == 'true') {
                isDesc = false;
            }
        }

        window.location.href = Sortable.baseURL + "?sortBy=" + sortBy + "&isDesc=" + isDesc;
    }
};

var apiHandler = {
    GET(url) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (res) {
                debugger;
            }
        });
    },
    POST(url, object) {
        object = {
            Id: 5,
            Name: "asd",

        }

        $.ajax({
            url: url,
            type: 'GET',
            data: object,
            success: function (res) {
                debugger;
            }
        });
    },
    DELETE(url) {
        if (confirm("Are you sure you want to delete?")) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (res) {
                    debugger;
                }
            });
        } else {
            alert("Lucky we asked!");
        }
    }
}