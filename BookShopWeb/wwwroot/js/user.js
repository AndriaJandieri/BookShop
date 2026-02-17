$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tableData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { data: "name", "width": "15%" },
            { data: "email", "width": "15%" },
            { data: "phoneNumber", "width": "15%" },
            { data: "company.name", "width": "15%" },
            { data: "role", "width": "10%" },
            {
                data: "status",
                "render": function (data, type, row) {
                    let color = data === "Active" ? "green" : "red";
                    return `<span style="color:${color}; font-weight:600">${data}</span>`;
                },
                "width": "5%"
            },

            {
                data: { id: 'id', lockoutEnd: "lockoutEnd", role: "role" },
                "render": function (data) {
                    var now = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (data.role === "Admin") {
                        return `
                        <div class="text-center">
                            <span style="color:gray; font-weight:600">Protected</span>
                        </div>`;
                    }


                    if (lockout > now) {
                        return `
                        <div class="text-center">
                           <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:120px;">
                              <i class="bi bi-unlock-fill"></i> UnLock</a>
                           <a/>
                           
                           <a class="btn btn-danger text-white" style="cursor:pointer; width:125px;">
                              <i class="bi bi-pencil-square"></i> Permission</a>
                           <a/>
                        </div>`
                    }
                    else {
                        return `
                        <div class="text-center">
                           
                           <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:120px;">
                              <i class="bi bi-lock-fill"></i> Lock</a>
                           <a/>
                           <a class="btn btn-danger text-white" style="cursor:pointer; width:125px;">
                              <i class="bi bi-pencil-square"></i> Permission</a>
                           <a/>
                        </div>`
                    }
                },
                "width": "25%"
            },
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/admin/user/lockunlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    })

}






