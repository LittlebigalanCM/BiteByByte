var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#DT_Load').DataTable({
        "ajax": {
            "url": "/api/category",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { data: "name", width: "45%" },
            { data: "displayOrder", width: "15%" },
            {
                data: "id", width: "40%",
                render: function (data) {
                    return `
                        <div class="text-center d-flex gap-2 justify-content-center">
                            <a href="/Admin/Categories/Upsert?id=${data}"
                               class="btn btn-success text-white" style="width:100px;">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onClick="Delete('/api/category/${data}')"
                               class="btn btn-danger text-white" style="width:100px;">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>`;
                }
            }
        ],
        "language": { "emptyTable": "no data found." },
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
                    console.log(data);
                    if (data.success) {
                        toastr.success(data.message);
                        $('#DT_Load').DataTable().ajax.reload(null, false);
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
