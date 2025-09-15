var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#DT_Load').DataTable({
        "ajax": {
            "url": "/api/menuitem",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { data: "name", width: "25%" },
            { data: "price", render: $.fn.dataTable.render.number(',', '.', 2, "$"), width: "15%" },
            { data: "category.name", width: "15%" },
            { data: "foodType.name", width: "15%" },
            {
                data: "id", width: "30%",
                render: function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/MenuItems/Upsert?id=${data}"
                                class="btn btn-success text-white" style="cursor:pointer; width:100px;"> 
                                    <i class="bi bi-pencil-square"></i> Edit
                                </a>
                                <a onClick="Delete('/api/menuitem/${data}')"
                                class="btn btn-danger text-white" style="cursor:pointer; width:100px;"> 
                                    <i class="bi bi-trash"></i> Delete
                                </a>
                            </div>`;

                }
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore this data!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    console.log(data); // Debug log
                    if (data.success) {
                        toastr.success(data.message);
                        $('#DT_Load').DataTable().ajax.reload(null, false); // false = don't reset pagination
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (xhr, status, error) {
                    console.error(xhr.responseText);
                    toastr.error("An error occurred while deleting.");
                }
            });
        }
    });
}

