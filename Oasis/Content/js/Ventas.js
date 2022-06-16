
var empresa;
var sucursal;
var localidad;
var id_visitador;
var tipoCliente = [];
var fecha_desde;
var fecha_hasta;

function ConvertirFecha(fecha) {
    var date = fecha;
    var day = date.getDate();       // yields date
    var month = date.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
    var year = date.getFullYear();  // yields year
    var hour = date.getHours();     // yields hours
    var minute = date.getMinutes(); // yields minutes
    var second = date.getSeconds(); // yields seconds
    return day + "/" + month + "/" + year + " " + hour + ':' + minute + ':' + second;
}


function Parametros() {
    empresa = $("#empresa").val();
    fecha_desde = ConvertirFecha($('#fecha_corte').data('daterangepicker').startDate._d);
    fecha_hasta = ConvertirFecha($('#fecha_corte').data('daterangepicker').endDate._d);

}



$("#GenerarDatos").click(function () {
    console.log("Ingresa al js")
    Parametros();
    console.log("Sale parametros")
    iniciaLoading();
    console.log(empresa)
    console.log(fecha_desde)
    console.log(fecha_hasta)

    $.ajax({
        url: 'ObtenerDVP',
        type: 'GET',
        data: {
            //JSON.stringify({
            fecha_desde: fecha_desde,
            fecha_hasta: fecha_hasta,
            empresa: empresa
            
        },
        dataType: "JSON",
        contentType: "application/JSON",
        success: function (d) {
            $('#contenedorTabla').remove();
            var contenedorTabla = document.createElement("div");
            contenedorTabla.className = "col-md-12";
            contenedorTabla.id = "contenedorTabla"
            var row = document.createElement("div");
            row.className = "row";
            row.appendChild(contenedorTabla);
            $('#contenedorPrimario').append(row);

            console.log(d)

            d = JSON.parse(d);
            var col = [];
            var encabezado = ["Empresa",
                "Tipo documento", "Fecha factura", "Ciudad", "Provincia",
                "Parroquia", "Tipo cliente", "Canal", "RUC",
                "Cliente", "id_motivo_nota_credito_cliente",
                "Secuencial documento", "inidicador_afecta_devolucion",
                "Codigo producto", "Codigo MBA",
                "Producto", "Categoria", "Subcategoria",
                "UM", "Cantidad", "Valor total", "Tipo venta",
                "codigo", "Cod. Vendedor", "Vendedor", "NC",
                "Descripcion NC", "Fecha NC", "clave_acceso",
                "id_factura_cliente", "memo"];

            //console.log(d)
            console.log(encabezado)
            for (var i = 0; i < d.length; i++) {
                for (var key in d[i]) {
                    if (col.indexOf(key) === -1) {
                        col.push(key);
                    }
                }
            }

            console.log("sale setea los datos")
            // CREATE DYNAMIC TABLE.
            var div = document.createElement("div");
            var row = document.createElement("div");
            row.className = "row col-md-4";
            var card = document.createElement("div");
            card.className = "card";
            card.style = "font-size: 15px;overflow-x: scroll;";
            var cardbody = document.createElement("div");
            cardbody.className = "card-body";
            var table = document.createElement("table");
            table.className = 'table table-hover tableDetalle';
            table.id = "tableDetalle";
            table.style = '';

            // CREATE HTML TABLE HEADER ROW USING THE EXTRACTED HEADERS ABOVE
            var thead = document.createElement("thead");
            table.appendChild(thead);
            var tr_head = document.createElement("tr");
            for (var i = 0; i < encabezado.length; i++) {
                var th = document.createElement("th");      // TABLE HEADER.
                th.innerHTML = encabezado[i];
                tr_head.appendChild(th);
            }

            thead.appendChild(tr_head);

            var tbody = document.createElement("tbody");
            table.appendChild(tbody);
            var tr_body = document.createElement("tr");
            // ADD JSON DATA TO THE TABLE AS ROWS.
            for (var i = 0; i < d.length; i++) {
                tr_body = document.createElement("tr");
                for (var j = 0; j < col.length; j++) {
                    var tabCell = tr_body.insertCell(-1);
                    tabCell.innerHTML = d[i][col[j]];
                    tabCell.style = ' white-space: nowrap;';
                }
                tbody.appendChild(tr_body);
            }

            cardbody.appendChild(table);
            card.appendChild(cardbody);
            div.appendChild(card);
            //CrearTablaDetalle(div.outerHTML, titulo);
            $('#contenedorTabla').append(div);
            $('#tableDetalle').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    'copy', 'csv', 'excel', 
                    {
                        extend: 'pdfHtml5',
                        orientation: 'landscape',
                        pageSize: 'LEGAL',
                        exportOptions: {
                           columns: [0, 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,39]
                        }
                    }, 'print'
                ],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json'
                }
            });
            cierraLoading();
        },
        error: function (e) {
            Toast.fire({
                icon: 'error',
                title: 'Hubo un error al intentar generar.'
            })
        }
    })
    
});

