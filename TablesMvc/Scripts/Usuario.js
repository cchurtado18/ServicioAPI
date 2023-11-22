var tableHdr = null;
var IdRecord = "";

$(document).ready(function () {
    loadData();

    $('#btnnuevo').on('click', function (e) {
        e.preventDefault();
        NewRecord();
    });

    $('#btnguardar').on('click', function (e) {
        e.preventDefault();
        Guardar();
    });

    $('#dt-records').on('click', 'button.btn-edit', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        loadDtl(data);
        IdRecord = data.IdUsuario;
    });

    $('#dt-records').on('click', 'button.btn-delete', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        IdRecord = data.IdUsuario;
        if (confirm('¿Seguro de eliminar el registro?')) {
            Eliminar();
        }
    });

});

function loadData() {
    tableHdr = $('#dt-records').DataTable({
        responsive: true,
        destroy: true,
        ajax: "/Usuario/Lista",
        order: [],
        columns: [
            { "data": "IdUsuario" },
            { "data": "Nombre" },
            { "data": "Apellido" },
            { "data": "Ciudad" },

        ],
        processing: true,
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        columnDefs: [
            {
                width: "20%",
                targets: 0,
                data: "IdUsuario"
            },
            {
                width: "20%",
                targets: 1,
                data: "Nombre"
            },
            {
                width: "32%",
                targets: 2,
                data: "Apellido"
            },
            {
                width: "32%",
                targets: 3,
                data: "Ciudad"
            },
            {
                width: "1%",
                targets: 4,
                data: null,
                defaultContent: '<button type="button" class="btn btn-info btn-sm btn-edit" data-target="#modal-record"><i class="fa fa-pencil"></i></button>'
            },
            {
                width: "1%",
                targets: 5,
                data: null,
                defaultContent: '<button type="button" class="btn btn-danger btn-sm btn-delete"><i class="fa fa-trash"></i></button>'

            }
        ]
    });
}

function NewRecord() {
    $(".modal-header h3").text("Crear Usuario");

    $('#txtIdUsuario').val('');
    $('#txtIdUsuario').prop('disabled', false);
    $('#txtNombre').val('');
    $('#txtApellido').val('');
    $('#txtCiudad').val('');


    $('#modal-record').modal('toggle');
}

function loadDtl(data) {
    $(".modal-header h3").text("Editar Usuario");

    $('#txtIdUsuario').val(data.IdUsuario);
    $('#txtIdUsuario').prop('disabled', true);
    $('#txtNombre').val(data.Nombre);
    $("#txtApellido").val(data.Apellido);
    $("#txtCiudad").val(data.Ciudad);
    $('#modal-record').modal('toggle');
}

function Guardar() {
    var record = "'IdUsuario':'" + $.trim($('#txtIdUsuario').val()) + "'";
    record += ",'Nombre':'" + $.trim($('#txtNombre').val()) + "'";
    record += ",'Apellido':'" + $.trim($('#txtApellido').val()) + "'";
    record += ",'Ciudad':'" + $.trim($('#txtCiudad').val()) + "'";

    console.log(record);

    $.ajax({
        type: 'POST',
        url: '/Usuario/Guardar',
        data: eval('({' + record + '})'),
        success: function (response) {
            if (response.success) {
                console.log("success")
                $("#modal-record").modal('hide');
                //$.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            }
            else {
                $("#modal-record").modal('hide');
                console.log("no success")
                //$.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}

function Eliminar() {
    console.log(IdRecord)
    $.ajax({
        type: 'POST',
        url: '/Usuario/Eliminar/?IdUsuario=' + IdRecord,
        success: function (response) {
            if (response.success) {
                console.log("deleted")
                //$.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            } else {
                console.log("no success to delete")
                //$.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('AJAX Error:', textStatus, errorThrown);
        }
    });
}