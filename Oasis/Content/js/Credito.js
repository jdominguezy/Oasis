
var OCDetalle = [];


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

function formatoValor(nStr) {
    nStr = nStr.toFixed(2);
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function CrearTablaDetalle(datos, titulo) {
    Swal.fire({
        title: '<strong>'+titulo+'</strong>',
        icon: 'info',
        width: 1100,
        html: datos,
        customClass: 'swal-wide',
        showCloseButton: true,
        focusConfirm: false,
        confirmButtonText:
            '<i class="fa fa-thumbs-up"></i> Ok!'
    })
}

function buildHtmlTable(selector) {
    var columns = addAllColumnHeaders(myList, selector);

    for (var i = 0; i < myList.length; i++) {
        var row$ = $('<tr/>');
        for (var colIndex = 0; colIndex < columns.length; colIndex++) {
            var cellValue = myList[i][columns[colIndex]];
            if (cellValue == null) cellValue = "";
            row$.append($('<td/>').html(cellValue));
        }
        $(selector).append(row$);
    }
}

$(".GenerarExcelDetalle").click(function () {
    $("#tableDetalle").table2excel({
        exclude: ".noExl",
        name: "Detalle OASIS",
        filename: "Detalle",//do not include extension
        //fileext: ".xlsx", // file extension
        exclude_img: true,
        exclude_links: true,
        exclude_inputs: true,
        preserveColors: true
    });
});

$("#GenerarExcel").click(function () {
    $("#tablePresupuesto").table2excel({
    exclude: ".noExl",
    name: "Consolidado OASIS",
    filename: "Consolidado",//do not include extension
    //fileext: ".xlsx", // file extension
    exclude_img: true,
    exclude_links: true,
    exclude_inputs: true,
    preserveColors: true
    });
});

function addAllColumnHeaders(myList, selector) {
    var columnSet = [];
    var headerTr$ = $('<tr/>');

    for (var i = 0; i < myList.length; i++) {
        var rowHash = myList[i];
        for (var key in rowHash) {
            if ($.inArray(key, columnSet) == -1) {
                columnSet.push(key);
                headerTr$.append($('<th/>').html(key));
            }
        }
    }
    $(selector).append(headerTr$);

    return columnSet;
}

function GenerarDatos(id_vendedor, direccionURL, titulo) {
    let spanOculto;
    
    spanOculto = $('.table-hover tbody tr td span.datosLinea[data-id_vendedor="' + id_vendedor + '"]')[0].dataset;

    var indicadorDI_ = window.location.href.includes("ConsolidadoDI");
   console.log(spanOculto.empresa)
            console.log(spanOculto.sucursal)
            //fecha_desde: spanOculto.fecha_desde,
            //fecha_hasta: spanOculto.fecha_hasta,
            //tipoCliente: JSON.stringify(spanOculto.tipocliente),
            //vendedor: spanOculto.id_vendedor, 
            //indicadorDI: indicadorDI_

    $.ajax({
        url: direccionURL,
        type: 'GET',
        data: {
            empresa: spanOculto.empresa,
            sucursal: spanOculto.sucursal,
            fecha_desde: spanOculto.fecha_desde,
            fecha_hasta: spanOculto.fecha_hasta,
            tipoCliente: JSON.stringify(spanOculto.tipocliente),
            vendedor: spanOculto.id_vendedor,
            indicadorDI: indicadorDI_
        },
        dataType: "JSON",
        contentType: "application/JSON",
        success: function (d) {
            d = JSON.parse(d);
            var col = [];
            for (var i = 0; i < d.length; i++) {
                for (var key in d[i]) {
                    if (col.indexOf(key) === -1) {
                        col.push(key);
                    }
                }
            }

            // CREATE DYNAMIC TABLE.
            var div = document.createElement("div");
            div.className = "col-md-12";

            var row = document.createElement("div");
            row.className = "row col-md-4";
            var card = document.createElement("div");
            card.className = "card";
            card.style = "font-size: 15px;overflow-x: scroll;";
            var table = document.createElement("table");
            table.className = 'table table-hover tableDetalle';
            table.id = "tableDetalle";
            table.style = '';

            var thead = document.createElement("thead");
            table.appendChild(thead);
            var tr_head = document.createElement("tr");
            for (var i = 0; i < col.length; i++) {
                var th = document.createElement("th");  
                th.innerHTML = (col[i].replace("_"," ")).toUpperCase();
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

            const cardBody = document.createElement("div");
            cardBody.className = "card-body";
            cardBody.appendChild(table);
            //row.appendChild(btnDescargar)
            card.appendChild(cardBody);
            //div.appendChild(row);
            div.appendChild(card);
            CrearTablaDetalle(div.outerHTML, titulo);
            $('#tableDetalle').DataTable({
                //"dom": '<"top"i>rt<"bottom"flp><"clear">',
                dom: 'Bfrtip',
                //"paging": true,
                //"ordering": true,
                //"info": true,
                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json'
                }
            });
        },
        error: function (e) {
            Toast.fire({
                icon: 'error',
                title: 'Hubo un error al intentar generar.'
            })
        }
    })

}

function verVentasXVendedor(id_vendedor) {
    console.log(id_vendedor)
    event.preventDefault();
    let direccionUrl = 'ObtenerVentasPorVendedor';
    GenerarDatos(id_vendedor, direccionUrl, "Detalle de ventas");
}

function verNCXVendedor(id_vendedor) {
    event.preventDefault();
    let direccionUrl = 'ObtenerNCPorVendedor';
    GenerarDatos(id_vendedor, direccionUrl, "Detalle de NC");
}

function verCobrosXVendedor(id_vendedor) {
    event.preventDefault();
    let direccionUrl = 'ObtenerCobrosPorVendedor';
    GenerarDatos(id_vendedor, direccionUrl, "Detalle de cobros");
}

$("botonVerDetalle").click(function () {
    event.preventDefault();
})


function GraficoBarras(identificador,tipoGrafico,presupuesto) {
    var ctx = document.getElementById(identificador).getContext('2d');

    const dataLength = presupuesto.length;
    /* Create color array */
    const colorScale = d3.interpolateInferno;

    const colorRangeInfo = {
        colorStart: 0.1,
        colorEnd: 1,
        useEndAsStart: false,
    };
    var COLORS = interpolateColors(dataLength, colorScale, colorRangeInfo);
    var myChart = new Chart(ctx, {
        type: tipoGrafico,
        data: {
            labels: presupuesto.map(presupuesto => presupuesto.nombre_vendedor),
            datasets: [{
                label: 'Ventas',
                data: presupuesto.map(presupuesto => presupuesto.ventas_neta),
                backgroundColor: COLORS,
                hoverBackgroundColor: COLORS,
                parsing: {
                    yAxisKey: 'ventas_neta'
                },
                borderWidth: 1
            },
            {
                label: 'Cobros',
                data: presupuesto.map(presupuesto => presupuesto.total_cobros),
                backgroundColor: COLORS,
                hoverBackgroundColor: COLORS,
                parsing: {
                    yAxisKey: 'total_cobros'
                },
                borderWidth: 1
            }]
        },
        options: {
            //responsive: false,
            scales: {
                x: {
                    stacked:true
                },
                y: {
                    beginAtZero: true,
                    stacked:true
                }
            }
        }
    });
}

function calculatePoint(i, intervalSize, colorRangeInfo) {
    var { colorStart, colorEnd, useEndAsStart } = colorRangeInfo;
    return (useEndAsStart
        ? (colorEnd - (i * intervalSize))
        : (colorStart + (i * intervalSize)));
}
/* Must use an interpolated color scale, which has a range of [0, 1] */
function interpolateColors(dataLength, colorScale, colorRangeInfo) {
    var { colorStart, colorEnd } = colorRangeInfo;
    var colorRange = colorEnd - colorStart;
    var intervalSize = colorRange / dataLength;
    var i, colorPoint;
    var colorArray = [];

    for (i = 0; i < dataLength; i++) {
        colorPoint = calculatePoint(i, intervalSize, colorRangeInfo);
        colorArray.push(colorScale(colorPoint));
    }

    return colorArray;
}

function GeneraGraficos(presupuesto) {
    $('#rowcontenedorGraficos').remove();
    const elementos = $.parseHTML('<h3 class="card-title"><i class="fas fa-th mr-1"></i>  Totales                </h3>        <div class="card-tools">            <button type="button" class="btn bg-defaultbtn-sm" data-card-widget="collapse">                <i class="fas fa-minus"></i>            </button>            <button type="button" class="btn bg-defaultbtn-sm" data-card-widget="remove">                <i class="fas fa-times"></i>            </button>');

    var canva = document.createElement("canvas");
    canva.id = "contenedorBarra";
    canva.width = 400;
    canva.height = 200;

    var canva2 = document.createElement("canvas");
    canva2.id = "contenedorPastel";
    canva2.width = 400;
    canva2.height = 200;

    var cardHeader = document.createElement("div");
    cardHeader.className = "card-header border-0";
    cardHeader.appendChild(elementos[0].cloneNode(true));
    cardHeader.appendChild(elementos[1].cloneNode(true));
    cardHeader.appendChild(elementos[2].cloneNode(true));

    var cardHeader2 = document.createElement("div");
    cardHeader2.className = "card-header border-0";
    cardHeader2.appendChild(elementos[0].cloneNode(true));
    cardHeader2.appendChild(elementos[1].cloneNode(true));
    cardHeader2.appendChild(elementos[2].cloneNode(true));

    var cardBody = document.createElement("div");
    cardBody.className = "card-body";
    cardBody.appendChild(canva);

    var cardBody2 = document.createElement("div");
    cardBody2.className = "card-body";
    cardBody2.appendChild(canva2);

    var cardBarra = document.createElement("div");
    cardBarra.className = "card";
    cardBarra.appendChild(cardHeader);
    cardBarra.appendChild(cardBody);

    var cardPastel = document.createElement("div");
    cardPastel.className = "card";
    cardPastel.appendChild(cardHeader2);
    cardPastel.appendChild(cardBody2);

    var contenedorGraficos2 = document.createElement("div");
    contenedorGraficos2.className = "col-md-12";
    contenedorGraficos2.appendChild(cardPastel);

    var contenedorGraficos = document.createElement("div");
    contenedorGraficos.className = "col-md-12";

    contenedorGraficos.appendChild(cardBarra);
    contenedorGraficos2.appendChild(cardPastel);

    var row2 = document.createElement("div");
    row2.className = "row";
    row2.appendChild(contenedorGraficos);
    row2.appendChild(contenedorGraficos2);
    row2.id = "rowcontenedorGraficos";

    $('#contenedorPrimario').append(row2);
    GraficoBarras('contenedorBarra', 'bar',presupuesto);
    GraficoBarras('contenedorPastel', 'pie',presupuesto);
}


$("#GenerarPresupuesto").click(function () {
    var tipoCliente = [];
    var empresa = $("#empresa").val();
    var sucursal = $("#sucursal").val();
    var localidad;
    if ($('#chkLocalidad').is(":checked")) {
        localidad = null
    } else {
        localidad = $("#localidad").val();
    }
    $('.tipoCliente:checkbox:checked').each(function () {
        tipoCliente.push($(this).attr('name'));
    });
    var fecha_desde = ConvertirFecha($('#fecha_presupuesto').data('daterangepicker').startDate._d);
    var fecha_hasta = ConvertirFecha($('#fecha_presupuesto').data('daterangepicker').endDate._d);
    iniciaLoading();
    console.log("Ingresa al js")

    var escogeDistribuidor = $(".DISTRIBUIDORES")[0].checked;
    if (escogeDistribuidor) {
        // PROCEDIMIENTO CON DISTRIBUIDOR
        var codigoVendedor = $(".js-data-vendedor-ajax").val();
        $.ajax({
            url: 'ObtenerPresupuestoDistribuidor',
            type: 'GET',
            data: {
                empresa: empresa,
                sucursal: sucursal,
                fecha_desde: fecha_desde,
                fecha_hasta: fecha_hasta,
                codigo_vendedor: codigoVendedor
            },
            dataType: "JSON",
            contentType: "application/JSON",
            success: function (d) {

                $('#rowcontenedorTabla').remove();
                var contenedorTabla = document.createElement("div");
                contenedorTabla.className = "col-md-12";
                contenedorTabla.id = "contenedorTabla"
                var row = document.createElement("div");
                row.className = "row";
                row.appendChild(contenedorTabla);
                row.id = "rowcontenedorTabla";
                $('#contenedorPrimario').append(row);

                var div = document.createElement("div");
                var row = document.createElement("div");
                row.className = "row col-md-4";
                var card = document.createElement("div");
                card.className = "card";
                card.style = "font-size: 15px;overflow-x: scroll;";
                var cardbody = document.createElement("div");
                cardbody.className = "card-body";

                var totalPresupuestoVentas = 0;
                var totalVentas = 0;
                var totalAlcanceVentas = 0;
                var totalPresupuestoCobros = 0;
                var totalCobros = 0;
                var totalAlcanceCobros = 0;


                var sTxt = '<table class="table table-hover" id="tablePresupuesto">';
                sTxt += '<thead><tr><th style="text-align:center">ID</th><th  style="text-align:center">Vendedor</th><th style="text-align:center">Cuota ventas</th>';
                sTxt += '<th style="text-align:center">Ventas</th><th style="text-align:center">%</th>';
                sTxt += '<th style="text-align:center">Cuota cobros</th><th style="text-align:center">Cobros</th>';
                sTxt += '<th style="text-align:center">%</th><th style="text-align:center"></th></tr></thead> ';
                sTxt += '<tbody>';
                $.each(JSON.parse(d), function (index, p) {
                //$.each(function (index, p) {
                    sTxt += '<tr>';
                    sTxt += '<td style="text-align:center">';
                    sTxt += '<span class="datosLinea" data-id_vendedor=' + p.id_vendedor + ' data-empresa="' + empresa + '" data-sucursal="' + sucursal + '" data-fecha_desde="' + fecha_desde + '" data-fecha_hasta="' + fecha_hasta + '" data-tipocliente="' + tipoCliente + '" hidden></span>';
                    //sTxt += '' + p.empresa + '</td>';
                    //sTxt += '' + p.sucursal + '</td>';
                    sTxt += '' + p.id_vendedor + '</td>';
                    sTxt += '<td style="text-align:center">' + p.nombre_vendedor + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.valor_venta) + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.ventas_neta) + '</td>';
                    sTxt += '<td style="text-align:center">' + p.alcance_venta.toFixed(2) + '%</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.valor_cobro) + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.total_cobros) + '</td>';
                    sTxt += '<td style="text-align:center">' + p.alcance_cobro.toFixed(2) + '%</td>';
                    sTxt += '<td style="text-align:center" class="noExl"> <div class="btn-group">';
                    sTxt += '<button type="button" class="btn btn-info" data-toggle="dropdown" id="botonVerDetalle"><i class="fas fa-search"></i></button>';
                    sTxt += '    <button type = "button" class="btn btn-info dropdown-toggle dropdown-hover dropdown-icon" data-toggle="dropdown" aria-expanded="false"> ';
                    sTxt += '    <span class="sr-only"> Toggle Dropdown</span> ';
                    sTxt += '</button> ';
                    sTxt += '    <div class="dropdown-menu" role = "menu" style = ""> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verVentasXVendedor(' + p.id_vendedor + ');" > Ventas </a> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verCobrosXVendedor(' + p.id_vendedor + ');" > Cobros </a> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verNCXVendedor(' + p.id_vendedor + ');"> N/C</a> ';
                    sTxt += '</div></td>';
                    sTxt += '</tr> ';
                    totalPresupuestoVentas += p.valor_venta;
                    totalVentas += p.ventas_neta;
                    totalPresupuestoCobros += p.valor_cobro;
                    totalCobros += p.total_cobros;
                });
                sTxt += '</tbody>';
                sTxt += '<tfoot style="font-weight: 800;"><tr><td></td><td class="centrar" >TOTAL</td><td class="centrar">' + formatoValor(totalPresupuestoVentas) + '</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalVentas) + '</td>';
                sTxt += '<td class="centrar">' + ((totalVentas / totalPresupuestoVentas) * 100).toFixed(2) + '%</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalPresupuestoCobros) + '</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalCobros) + '</td>';
                sTxt += '<td class="centrar">' + ((totalCobros / totalPresupuestoCobros) * 100).toFixed(2) + '%</td>';
                sTxt += '<td class="centrar"></td>';
                sTxt += '</tr></tfoot>';
                sTxt += '</table>';

                cardbody.append($.parseHTML(sTxt)[0]);
                card.appendChild(cardbody);
                div.appendChild(card);
                $('#contenedorTabla').append(div);
                $('#tablePresupuesto').DataTable({
                    "processing": true, // for show progress bar
                    "paging": false,
                    "bInfo": false,
                    //"scrollY": "500px",
                    dom: 'Bfrtip',
                    "buttons": [
                        {
                            "extend": 'copy', "text": 'Copiar', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'pdf', "text": 'PDF', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'excel', "text": 'Excel', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'print', "text": 'Imprimir', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                    ],
                    initComplete: function () {
                        $('.buttons-pdf').html('<i class="far fa-file-excel"></i>')
                    },
                    //buttons: [
                    //    'copy', 'csv', 'excel', 'pdf', 'print'
                    //],
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json'
                    }
                });
                var presupuesto = JSON.parse(d);
                var vendedores = presupuesto.map(presupuesto => presupuesto.nombre_vendedor);
                var ventas = presupuesto.map(presupuesto => presupuesto.ventas_neta);
                var cobros = presupuesto.map(presupuesto => presupuesto.total_cobros);
                GeneraGraficos(presupuesto);
                cierraLoading();
            },
            error: function (e) {
                Toast.fire({
                    icon: 'error',
                    title: 'Hubo un error al intentar generar.'
                })
            }
        })
    } else {

        $.ajax({
            url: 'ObtenerPresupuesto',
        type: 'GET',
        data: {
            empresa: empresa,
            sucursal: sucursal,
            //localidad: this.localidad,
            fecha_desde: fecha_desde,
            fecha_hasta: fecha_hasta,
            tipoCliente: JSON.stringify(tipoCliente)
        },
        dataType: "JSON",
        contentType: "application/JSON",
        success: function (d) {

            $('#rowcontenedorTabla').remove();
            var contenedorTabla = document.createElement("div");  
            contenedorTabla.className = "col-md-12";
            contenedorTabla.id = "contenedorTabla"
            var row = document.createElement("div");      
            row.className = "row";
            row.appendChild(contenedorTabla);
            row.id = "rowcontenedorTabla";
            $('#contenedorPrimario').append(row);

            console.log("Pasa la carga datos")

            var div = document.createElement("div");
            var row = document.createElement("div");
            row.className = "row col-md-4";
            var card = document.createElement("div");
            card.className = "card";
            card.style = "font-size: 15px;overflow-x: scroll;";
            var cardbody = document.createElement("div");
            cardbody.className = "card-body";

            var totalPresupuestoVentas = 0;
            var totalVentas = 0;
            var totalAlcanceVentas = 0;
            var totalPresupuestoCobros = 0;
            var totalCobros = 0;
            var totalAlcanceCobros = 0;

            console.log("Pasa variables")


            var sTxt = '<table class="table table-hover" id="tablePresupuesto">';
            sTxt += '<thead><tr><th style="text-align:center">ID</th><th  style="text-align:center">Vendedor</th><th style="text-align:center">Cuota ventas</th>';
            sTxt += '<th style="text-align:center">Ventas</th><th style="text-align:center">%</th>';
            sTxt += '<th style="text-align:center">Cuota cobros</th><th style="text-align:center">Cobros</th>';
            sTxt += '<th style="text-align:center">%</th><th style="text-align:center"></th></tr></thead> ';
            sTxt += '<tbody>';

            console.log(d)

            //if (empresa != '0'|| sucursal != '0') {
            //    var data = JSON.parse(d);
            //}
            //else {
            //    var data = d;
            //}
            
            //$.each(function (index, p) {
            $.each(JSON.parse(d), function (index, p) {
                sTxt += '<tr>';
                sTxt += '<td style="text-align:center">';
                sTxt += '<span class="datosLinea" data-id_vendedor=' + p.id_vendedor + ' data-empresa="' + empresa+'" data-sucursal="'+ sucursal+'" data-fecha_desde="'+fecha_desde+'" data-fecha_hasta="'+fecha_hasta+'" data-tipocliente="'+tipoCliente+'" hidden></span>';
                //sTxt += '' + p.empresa + '</td>';
                //sTxt += '' + p.sucursal + '</td>';
                sTxt += '' + p.id_vendedor + '</td>';
                sTxt += '<td style="text-align:center">' + p.nombre_vendedor + '</td>';
                sTxt += '<td style="text-align:center">' + formatoValor(p.valor_venta)  + '</td>';
                sTxt += '<td style="text-align:center">' + formatoValor(p.ventas_neta) + '</td>';
                sTxt += '<td style="text-align:center">' + p.alcance_venta.toFixed(2) + '%</td>';
                sTxt += '<td style="text-align:center">' + formatoValor(p.valor_cobro) + '</td>';
                sTxt += '<td style="text-align:center">' + formatoValor(p.total_cobros) + '</td>';
                sTxt += '<td style="text-align:center">' + p.alcance_cobro.toFixed(2) + '%</td>';
                sTxt += '<td style="text-align:center" class="noExl"> <div class="btn-group">';
                sTxt += '<button type="button" class="btn btn-info" data-toggle="dropdown" id="botonVerDetalle"><i class="fas fa-search"></i></button>';
                sTxt += '    <button type = "button" class="btn btn-info dropdown-toggle dropdown-hover dropdown-icon" data-toggle="dropdown" aria-expanded="false"> ';
                sTxt += '    <span class="sr-only"> Toggle Dropdown</span> ';
                sTxt += '</button> ';
                sTxt += '    <div class="dropdown-menu" role = "menu" style = ""> ';
                sTxt += '    <a class="dropdown-item" href = "#" onClick="verVentasXVendedor('+p.id_vendedor+');" > Ventas </a> ';
                sTxt += '    <a class="dropdown-item" href = "#" onClick="verCobrosXVendedor(' + p.id_vendedor +');" > Cobros </a> ';
                sTxt += '    <a class="dropdown-item" href = "#" onClick="verNCXVendedor(' + p.id_vendedor +');"> N/C</a> '; 
                sTxt += '</div></td>';
                sTxt += '</tr> ';
                totalPresupuestoVentas += p.valor_venta;
                totalVentas += p.ventas_neta;
                totalPresupuestoCobros += p.valor_cobro;
                totalCobros += p.total_cobros;
            });

            console.log("Pasa fn")
            console.log(sTxt)

            sTxt += '</tbody>';
            sTxt += '<tfoot style="font-weight: 800;"><tr><td></td><td class="centrar" >TOTAL</td><td class="centrar">' + formatoValor(totalPresupuestoVentas)+ '</td>';
            sTxt += '<td class="centrar">' + formatoValor(totalVentas) + '</td>';
            sTxt += '<td class="centrar">' + ((totalVentas / totalPresupuestoVentas) * 100).toFixed(2) + '%</td>';
            sTxt += '<td class="centrar">' + formatoValor(totalPresupuestoCobros) + '</td>';
            sTxt += '<td class="centrar">' + formatoValor(totalCobros) + '</td>';
            sTxt += '<td class="centrar">' + ((totalCobros / totalPresupuestoCobros) * 100).toFixed(2) + '%</td>';
            sTxt += '<td class="centrar"></td>';
            sTxt += '</tr></tfoot>';
            sTxt += '</table>';

            console.log("arma cabecera")
            console.log(sTxt)

            cardbody.append($.parseHTML(sTxt)[0]);
            card.appendChild(cardbody);
            div.appendChild(card);
            $('#contenedorTabla').append(div);
            $('#tablePresupuesto').DataTable({
                "processing": true, // for show progress bar
                "paging": false,
                "bInfo": false,
                //"scrollY": "500px",
                dom: 'Bfrtip',
                "buttons": [
                    {
                        "extend": 'copy', "text": 'Copiar', "className": 'btn btn-default btn-xs'
                        , exportOptions: { columns: [0,1,2,3,4,5,6,7]}
                    },
                    {
                        "extend": 'pdf', "text": 'PDF', "className": 'btn btn-default btn-xs'
                        , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                    },
                    {
                        "extend": 'excel', "text": 'Excel', "className": 'btn btn-default btn-xs'
                        , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                    },
                    {
                        "extend": 'print', "text": 'Imprimir', "className": 'btn btn-default btn-xs'
                        , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                    },
                ],
                initComplete: function () {
                    $('.buttons-pdf').html('<i class="far fa-file-excel"></i>')
                },
                //buttons: [
                //    'copy', 'csv', 'excel', 'pdf', 'print'
                //],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json'
                }
            });

            console.log("sale datos")
            var presupuesto = JSON.parse(d);

            var vendedores = presupuesto.map(presupuesto => presupuesto.nombre_vendedor);
            var ventas = presupuesto.map(presupuesto => presupuesto.ventas_neta);
            var cobros = presupuesto.map(presupuesto => presupuesto.total_cobros);

            console.log("entra a armar graficos")
            GeneraGraficos(presupuesto);

            console.log("sale de graficos")
            cierraLoading();
        },
        error: function (e) {
            Toast.fire({
                icon: 'error',
                title: 'Hubo un error al intentar generar.'
            })
        }
    })
    }
    
});

$("#GenerarPresupuestoEmpresa").click(function () {

    var tipoCliente = [];
    var empresa = $("#empresa").val();
    var sucursal = $("#sucursal").val();
    var localidad;
    if ($('#chkLocalidad').is(":checked")) {
        localidad = null
    } else {
        localidad = $("#localidad").val();
    }
    $('.tipoCliente:checkbox:checked').each(function () {
        tipoCliente.push($(this).attr('name'));
    });
    var fecha_desde = ConvertirFecha($('#fecha_presupuesto').data('daterangepicker').startDate._d);
    var fecha_hasta = ConvertirFecha($('#fecha_presupuesto').data('daterangepicker').endDate._d);

    iniciaLoading();

    var escogeDistribuidor = $(".DISTRIBUIDORES")[0].checked;

    if (escogeDistribuidor) {
        // PROCEDIMIENTO CON DISTRIBUIDOR
        var codigoVendedor = $(".js-data-vendedor-ajax").val();
        $.ajax({
            url: 'ObtenerPresupuestoDistribuidor',
            type: 'GET',
            data: {
                empresa: empresa,
                sucursal: sucursal,
                fecha_desde: fecha_desde,
                fecha_hasta: fecha_hasta,
                codigo_vendedor: codigoVendedor
            },
            dataType: "JSON",
            contentType: "application/JSON",
            success: function (d) {

                $('#rowcontenedorTabla').remove();
                var contenedorTabla = document.createElement("div");
                contenedorTabla.className = "col-md-12";
                contenedorTabla.id = "contenedorTabla"
                var row = document.createElement("div");
                row.className = "row";
                row.appendChild(contenedorTabla);
                row.id = "rowcontenedorTabla";
                $('#contenedorPrimario').append(row);

                var div = document.createElement("div");
                var row = document.createElement("div");
                row.className = "row col-md-4";
                var card = document.createElement("div");
                card.className = "card";
                card.style = "font-size: 15px;overflow-x: scroll;";
                var cardbody = document.createElement("div");
                cardbody.className = "card-body";

                var totalPresupuestoVentas = 0;
                var totalVentas = 0;
                var totalAlcanceVentas = 0;
                var totalPresupuestoCobros = 0;
                var totalCobros = 0;
                var totalAlcanceCobros = 0;


                var sTxt = '<table class="table table-hover" id="tablePresupuesto">';
                sTxt += '<thead><tr><th style="text-align:center">ID</th><th  style="text-align:center">Vendedor</th><th style="text-align:center">Cuota ventas</th>';
                sTxt += '<th style="text-align:center">Ventas</th><th style="text-align:center">%</th>';
                sTxt += '<th style="text-align:center">Cuota cobros</th><th style="text-align:center">Cobros</th>';
                sTxt += '<th style="text-align:center">%</th><th style="text-align:center"></th></tr></thead> ';
                sTxt += '<tbody>';

                $.each(JSON.parse(d), function (index, p) {
                //$.each(function (index, p) {
                    sTxt += '<tr>';
                    sTxt += '<td style="text-align:center">';
                    sTxt += '<span class="datosLinea" data-id_vendedor=' + p.id_vendedor + ' data-empresa="' + empresa + '" data-sucursal="' + sucursal + '" data-fecha_desde="' + fecha_desde + '" data-fecha_hasta="' + fecha_hasta + '" data-tipocliente="' + tipoCliente + '" hidden></span>';
                    sTxt += '' + p.empresa + '</td>';
                    sTxt += '' + p.sucursal + '</td>';
                    sTxt += '' + p.id_vendedor + '</td>';
                    sTxt += '<td style="text-align:center">' + p.nombre_vendedor + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.valor_venta) + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.ventas_neta) + '</td>';
                    sTxt += '<td style="text-align:center">' + p.alcance_venta.toFixed(2) + '%</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.valor_cobro) + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.total_cobros) + '</td>';
                    sTxt += '<td style="text-align:center">' + p.alcance_cobro.toFixed(2) + '%</td>';
                    sTxt += '<td style="text-align:center" class="noExl"> <div class="btn-group">';
                    sTxt += '<button type="button" class="btn btn-info" data-toggle="dropdown" id="botonVerDetalle"><i class="fas fa-search"></i></button>';
                    sTxt += '    <button type = "button" class="btn btn-info dropdown-toggle dropdown-hover dropdown-icon" data-toggle="dropdown" aria-expanded="false"> ';
                    sTxt += '    <span class="sr-only"> Toggle Dropdown</span> ';
                    sTxt += '</button> ';
                    sTxt += '    <div class="dropdown-menu" role = "menu" style = ""> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verVentasXVendedor(' + p.id_vendedor + ');" > Ventas </a> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verCobrosXVendedor(' + p.id_vendedor + ');" > Cobros </a> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verNCXVendedor(' + p.id_vendedor + ');"> N/C</a> ';
                    sTxt += '</div></td>';
                    sTxt += '</tr> ';
                    totalPresupuestoVentas += p.valor_venta;
                    totalVentas += p.ventas_neta;
                    totalPresupuestoCobros += p.valor_cobro;
                    totalCobros += p.total_cobros;
                });
                sTxt += '</tbody>';
                sTxt += '<tfoot style="font-weight: 800;"><tr><td></td><td class="centrar" >TOTAL</td><td class="centrar">' + formatoValor(totalPresupuestoVentas) + '</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalVentas) + '</td>';
                sTxt += '<td class="centrar">' + ((totalVentas / totalPresupuestoVentas) * 100).toFixed(2) + '%</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalPresupuestoCobros) + '</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalCobros) + '</td>';
                sTxt += '<td class="centrar">' + ((totalCobros / totalPresupuestoCobros) * 100).toFixed(2) + '%</td>';
                sTxt += '<td class="centrar"></td>';
                sTxt += '</tr></tfoot>';
                sTxt += '</table>';

                cardbody.append($.parseHTML(sTxt)[0]);
                card.appendChild(cardbody);
                div.appendChild(card);
                $('#contenedorTabla').append(div);
                $('#tablePresupuesto').DataTable({
                    "processing": true, // for show progress bar
                    "paging": false,
                    "bInfo": false,
                    //"scrollY": "500px",
                    dom: 'Bfrtip',
                    "buttons": [
                        {
                            "extend": 'copy', "text": 'Copiar', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'pdf', "text": 'PDF', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'excel', "text": 'Excel', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'print', "text": 'Imprimir', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                    ],
                    initComplete: function () {
                        $('.buttons-pdf').html('<i class="far fa-file-excel"></i>')
                    },
                    //buttons: [
                    //    'copy', 'csv', 'excel', 'pdf', 'print'
                    //],
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json'
                    }
                });
                var presupuesto = JSON.parse(d);
                var vendedores = presupuesto.map(presupuesto => presupuesto.nombre_vendedor);
                var ventas = presupuesto.map(presupuesto => presupuesto.ventas_neta);
                var cobros = presupuesto.map(presupuesto => presupuesto.total_cobros);
                GeneraGraficos(presupuesto);
                cierraLoading();
            },
            error: function (e) {
                Toast.fire({
                    icon: 'error',
                    title: 'Hubo un error al intentar generar.'
                })
            }
        })
    } else {

    $.ajax({
        url: 'ObtenerPresupuestoEmpresa',
        type: 'GET',
        data: {
            empresa: empresa,
            sucursal: sucursal,
            //localidad: this.localidad,
            fecha_desde: fecha_desde,
            fecha_hasta: fecha_hasta,
            tipoCliente: JSON.stringify(tipoCliente)
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

            var col = [];
            var encabezado = ["Empresa",
                "Sucursal", "ID", "Vendedor", "Cuota Ventas",
                "Ventas", "%", "Cuota Cobros", "Cobros", "%", ""];

            console.log(encabezado)
            console.log(d)

            d = JSON.parse(d);

            for (var i = 0; i < d.length; i++) {
                for (var key in d[i]) {
                    if (col.indexOf(key) === -1) {
                        col.push(key);
                    }
                }
            }

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
            var cod_vendedor;
            var cod_empresa;
            var cod_sucursal;
            //var linea;

            // ADD JSON DATA TO THE TABLE AS ROWS.
            for (var i = 0; i < d.length; i++) {
                tr_body = document.createElement("tr");
                for (var j = 0; j < col.length; j++) {
                    var tabCell = tr_body.insertCell(-1);
                    tabCell.innerHTML = d[i][col[j]];

                    tabCell.style = ' white-space: nowrap;';

                    if (j == 0) {
                        cod_empresa = d[i][col[j]];
                    }

                    if (j == 1) {
                        cod_sucursal = d[i][col[j]];
                    }

                    if (j == 2) {
                        cod_vendedor = d[i][col[j]];
                    }
                    
                }

                var linea = '<span class="datosLinea" data-id_vendedor=' + cod_vendedor + ' data-empresa="' + cod_empresa + '" data-sucursal="' + cod_sucursal + '" data-fecha_desde="' + fecha_desde + '" data-fecha_hasta="' + fecha_hasta + '" data-tipocliente="' + tipoCliente + '" hidden></span>';

                var tabCell = tr_body.insertCell(-1);
                var cadenaB;
                cadenaB = '<button type="button" class="btn btn-info" data-toggle="dropdown" id="botonVerDetalle"><i class="fas fa-search"></i></button>';
                cadenaB = cadenaB + '<button type="button" class="btn btn-info dropdown-toggle dropdown-hover dropdown-icon" data-toggle="dropdown" aria-expanded="false"> ';
                cadenaB = cadenaB + '<span class="sr-only"> Toggle Dropdown</span> ';
                cadenaB = cadenaB + '</button> ';
                cadenaB = cadenaB + '<div class="dropdown-menu" role = "menu" style = ""> ';
                cadenaB = cadenaB + '    <a class="dropdown-item" href = "#" onClick="verVentasXVendedor(' + cod_vendedor + ');" > Ventas </a> ';
                cadenaB = cadenaB + '    <a class="dropdown-item" href = "#" onClick="verCobrosXVendedor(' + cod_vendedor +  ');" > Cobros </a> ';
                cadenaB = cadenaB + '    <a class="dropdown-item" href = "#" onClick="verNCXVendedor(' + cod_vendedor + ');"> N/C</a> ';
                cadenaB = cadenaB + '</div>';
                
                tabCell.innerHTML = cadenaB;
                //tabCell.innerHTML = '<button class="btn btn-info imprimir" onClick="imprimirOrden(\'' + d[i][col[0]] + '\')" >Imprimir</button>';
                //tabCell.innerHTML = '<button type="button" class="btn btn-info" data-toggle="dropdown" id="botonVerDetalle"><i class="fas fa-search"></i></button>'+
                tabCell.style = ' white-space: nowrap;';
                
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
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json'
                }
            });

            console.log("sale datos")
            //var presupuesto = JSON.parse(d);
            var presupuesto = d;
            var vendedores = presupuesto.map(presupuesto => presupuesto.nombre_vendedor);
            var ventas = presupuesto.map(presupuesto => presupuesto.ventas_neta);
            var cobros = presupuesto.map(presupuesto => presupuesto.total_cobros);

            console.log("entra a armar graficos")
            GeneraGraficos(presupuesto);

            cierraLoading();
        },
        error: function (e) {
            cierraLoading();
            Toast.fire({
                icon: 'error',
                title: 'Hubo un error al intentar generar.'
            })
        }
    })
    }
});

$("#GenerarPresupuestoConsolidado").click(function () {
    var tipoCliente = [];
    var empresa = $("#empresa").val();
    var sucursal = $("#sucursal").val();
    var localidad;

    if ($('#chkLocalidad').is(":checked")) {
        localidad = null
    } else {
        localidad = $("#localidad").val();
    }
    $('.tipoCliente:checkbox:checked').each(function () {
        tipoCliente.push($(this).attr('name'));
    });
    var fecha_desde = ConvertirFecha($('#fecha_presupuesto').data('daterangepicker').startDate._d);
    var fecha_hasta = ConvertirFecha($('#fecha_presupuesto').data('daterangepicker').endDate._d);
    var titulo = "REPORTE CONSOLIDADO \n\t"+
                "Empresa: " + empresa +"\t\n" +
                "Periodo: " + fecha_desde + " Al  " + fecha_hasta + "\t\r";
    iniciaLoading();
    console.log("Ingresa al js")

    var escogeDistribuidor = $(".DISTRIBUIDORES")[0].checked;
    if (escogeDistribuidor) {
        // PROCEDIMIENTO CON DISTRIBUIDOR
        var codigoVendedor = $(".js-data-vendedor-ajax").val();
        $.ajax({
            url: 'ObtenerPresupuestoDistribuidor',
            type: 'GET',
            data: {
                empresa: empresa,
                sucursal: sucursal,
                fecha_desde: fecha_desde,
                fecha_hasta: fecha_hasta,
                codigo_vendedor: codigoVendedor
            },
            dataType: "JSON",
            contentType: "application/JSON",
            success: function (d) {

                $('#rowcontenedorTabla').remove();
                var contenedorTabla = document.createElement("div");
                contenedorTabla.className = "col-md-12";
                contenedorTabla.id = "contenedorTabla"
                var row = document.createElement("div");
                row.className = "row";
                row.appendChild(contenedorTabla);
                row.id = "rowcontenedorTabla";
                $('#contenedorPrimario').append(row);

                var div = document.createElement("div");
                var row = document.createElement("div");
                row.className = "row col-md-4";
                var card = document.createElement("div");
                card.className = "card";
                card.style = "font-size: 15px;overflow-x: scroll;";
                var cardbody = document.createElement("div");
                cardbody.className = "card-body";

                var totalPresupuestoVentas = 0;
                var totalVentas = 0;
                var totalAlcanceVentas = 0;
                var totalPresupuestoCobros = 0;
                var totalCobros = 0;
                var totalAlcanceCobros = 0;


                var sTxt = '<table class="table table-hover" id="tablePresupuesto">';
                sTxt += '<thead><tr><th style="text-align:center">ID</th><th  style="text-align:center">Vendedor</th><th style="text-align:center">Cuota ventas</th>';
                sTxt += '<th style="text-align:center">Ventas</th><th style="text-align:center">%</th>';
                sTxt += '<th style="text-align:center">Cuota cobros</th><th style="text-align:center">Cobros</th>';
                sTxt += '<th style="text-align:center">%</th><th style="text-align:center"></th></tr></thead> ';
                sTxt += '<tbody>';
                $.each(JSON.parse(d), function (index, p) {
                    //$.each(function (index, p) {
                    sTxt += '<tr>';
                    sTxt += '<td style="text-align:center">';
                    sTxt += '<span class="datosLinea" data-id_vendedor=' + p.id_vendedor + ' data-empresa="' + empresa + '" data-sucursal="' + sucursal + '" data-fecha_desde="' + fecha_desde + '" data-fecha_hasta="' + fecha_hasta + '" data-tipocliente="' + tipoCliente + '" hidden></span>';
                    //sTxt += '' + p.empresa + '</td>';
                    //sTxt += '' + p.sucursal + '</td>';
                    sTxt += '' + p.id_vendedor + '</td>';
                    sTxt += '<td style="text-align:center">' + p.nombre_vendedor + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.valor_venta) + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.ventas_neta) + '</td>';
                    sTxt += '<td style="text-align:center">' + p.alcance_venta.toFixed(2) + '%</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.valor_cobro) + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.total_cobros) + '</td>';
                    sTxt += '<td style="text-align:center">' + p.alcance_cobro.toFixed(2) + '%</td>';
                    sTxt += '<td style="text-align:center" class="noExl"> <div class="btn-group">';
                    sTxt += '<button type="button" class="btn btn-info" data-toggle="dropdown" id="botonVerDetalle"><i class="fas fa-search"></i></button>';
                    sTxt += '    <button type = "button" class="btn btn-info dropdown-toggle dropdown-hover dropdown-icon" data-toggle="dropdown" aria-expanded="false"> ';
                    sTxt += '    <span class="sr-only"> Toggle Dropdown</span> ';
                    sTxt += '</button> ';
                    sTxt += '    <div class="dropdown-menu" role = "menu" style = ""> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verVentasXVendedor(' + p.id_vendedor + ');" > Ventas </a> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verCobrosXVendedor(' + p.id_vendedor + ');" > Cobros </a> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verNCXVendedor(' + p.id_vendedor + ');"> N/C</a> ';
                    sTxt += '</div></td>';
                    sTxt += '</tr> ';
                    totalPresupuestoVentas += p.valor_venta;
                    totalVentas += p.ventas_neta;
                    totalPresupuestoCobros += p.valor_cobro;
                    totalCobros += p.total_cobros;
                });
                sTxt += '</tbody>';
                sTxt += '<tfoot style="font-weight: 800;"><tr><td></td><td class="centrar" >TOTAL</td><td class="centrar">' + formatoValor(totalPresupuestoVentas) + '</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalVentas) + '</td>';
                sTxt += '<td class="centrar">' + ((totalVentas / totalPresupuestoVentas) * 100).toFixed(2) + '%</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalPresupuestoCobros) + '</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalCobros) + '</td>';
                sTxt += '<td class="centrar">' + ((totalCobros / totalPresupuestoCobros) * 100).toFixed(2) + '%</td>';
                sTxt += '<td class="centrar"></td>';
                sTxt += '</tr></tfoot>';
                sTxt += '</table>';

                cardbody.append($.parseHTML(sTxt)[0]);
                card.appendChild(cardbody);
                div.appendChild(card);
                $('#contenedorTabla').append(div);
                $('#tablePresupuesto').DataTable({
                    "processing": true, // for show progress bar
                    "paging": false,
                    "bInfo": false,
                    //"scrollY": "500px",
                    dom: 'Bfrtip',
                    "buttons": [
                        {
                            "extend": 'copy', "text": 'Copiar', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'pdf', "text": 'PDF', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'excel', "text": 'Excel', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            "extend": 'print', "text": 'Imprimir', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }
                        },
                    ],
                    initComplete: function () {
                        $('.buttons-pdf').html('<i class="far fa-file-excel"></i>')
                    },
                    //buttons: [
                    //    'copy', 'csv', 'excel', 'pdf', 'print'
                    //],
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json'
                    }
                });
                var presupuesto = JSON.parse(d);
                var vendedores = presupuesto.map(presupuesto => presupuesto.nombre_vendedor);
                var ventas = presupuesto.map(presupuesto => presupuesto.ventas_neta);
                var cobros = presupuesto.map(presupuesto => presupuesto.total_cobros);
                GeneraGraficos(presupuesto);
                cierraLoading();
            },
            error: function (e) {
                Toast.fire({
                    icon: 'error',
                    title: 'Hubo un error al intentar generar.'
                })
            }
        })
    } else {

        $.ajax({
            url: 'ObtenerPresupuestoEmpresa',
            type: 'GET',
            data: {
                empresa: empresa,
                sucursal: sucursal,
                //localidad: this.localidad,
                fecha_desde: fecha_desde,
                fecha_hasta: fecha_hasta,
                tipoCliente: JSON.stringify(tipoCliente)
            },
            dataType: "JSON",
            contentType: "application/JSON",
            success: function (d) {

                $('#rowcontenedorTabla').remove();
                var contenedorTabla = document.createElement("div");
                contenedorTabla.className = "col-md-12";
                contenedorTabla.id = "contenedorTabla"
                var row = document.createElement("div");
                row.className = "row";
                row.appendChild(contenedorTabla);
                row.id = "rowcontenedorTabla";
                $('#contenedorPrimario').append(row);

                console.log("Pasa la carga datos")

                var div = document.createElement("div");
                var row = document.createElement("div");
                row.className = "row col-md-4";
                var card = document.createElement("div");
                card.className = "card";
                card.style = "font-size: 15px;overflow-x: scroll;";
                var cardbody = document.createElement("div");
                cardbody.className = "card-body";

                var totalPresupuestoVentas = 0;
                var totalVentas = 0;
                var totalAlcanceVentas = 0;
                var totalPresupuestoCobros = 0;
                var totalCobros = 0;
                var totalAlcanceCobros = 0;

                console.log("Pasa variables")

                var sTxt = '<table class="table table-hover" id="tablePresupuesto">';
                sTxt += '<thead><tr><th style="text-align:center">ID</th><th style="text-align:center">Sucursal</th><th  style="text-align:center">Vendedor</th><th style="text-align:center">Cuota ventas</th>';
                sTxt += '<th style="text-align:center">Ventas</th><th style="text-align:center">%</th>';
                sTxt += '<th style="text-align:center">Cuota cobros</th><th style="text-align:center">Cobros</th>';
                sTxt += '<th style="text-align:center">%</th><th style="text-align:center"></th></tr></thead> ';
                sTxt += '<tbody>';

                console.log(d)

                //d = JSON.parse(d);

                //$.each(function (index, p) {
                $.each(JSON.parse(d), function (index, p) {
                    sTxt += '<tr>';
                    sTxt += '<td style="text-align:center">';
                    sTxt += '<span class="datosLinea" data-id_vendedor=' + p.id_vendedor + ' data-empresa="' + p.empresa + '" data-sucursal="' + p.sucursal + '" data-fecha_desde="' + fecha_desde + '" data-fecha_hasta="' + fecha_hasta + '" data-tipocliente="' + tipoCliente + '" hidden></span>';
                    //sTxt += '' + p.empresa + '</td>';
                    //sTxt += '' + p.sucursal + '</td>';
                    sTxt += '' + p.id_vendedor + '</td>';
                    //sTxt += '<td style="text-align:center">' + p.empresa + '</td>';
                    sTxt += '<td style="text-align:center">' + p.sucursal + '</td>';
                    sTxt += '<td style="text-align:center">' + p.nombre_vendedor + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.valor_venta) + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.ventas_neta) + '</td>';
                    sTxt += '<td style="text-align:center">' + p.alcance_venta.toFixed(2) + '%</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.valor_cobro) + '</td>';
                    sTxt += '<td style="text-align:center">' + formatoValor(p.total_cobros) + '</td>';
                    sTxt += '<td style="text-align:center">' + p.alcance_cobro.toFixed(2) + '%</td>';
                    sTxt += '<td style="text-align:center" class="noExl"> <div class="btn-group">';
                    sTxt += '<button type="button" class="btn btn-info" data-toggle="dropdown" id="botonVerDetalle"><i class="fas fa-search"></i></button>';
                    sTxt += '    <button type = "button" class="btn btn-info dropdown-toggle dropdown-hover dropdown-icon" data-toggle="dropdown" aria-expanded="false"> ';
                    sTxt += '    <span class="sr-only"> Toggle Dropdown</span> ';
                    sTxt += '</button> ';
                    sTxt += '    <div class="dropdown-menu" role = "menu" style = ""> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verVentasXVendedor(' + p.id_vendedor + ');" > Ventas </a> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verCobrosXVendedor(' + p.id_vendedor + ');" > Cobros </a> ';
                    sTxt += '    <a class="dropdown-item" href = "#" onClick="verNCXVendedor(' + p.id_vendedor + ');"> N/C</a> ';
                    sTxt += '</div></td>';
                    sTxt += '</tr> ';
                    totalPresupuestoVentas += p.valor_venta;
                    totalVentas += p.ventas_neta;
                    totalPresupuestoCobros += p.valor_cobro;
                    totalCobros += p.total_cobros;
                });

                console.log("Pasa fn")
                console.log(sTxt)

                sTxt += '</tbody>';
                sTxt += '<tfoot style="font-weight: 900;"><tr><td></td><td class="centrar" >TOTAL</td><td class="centrar">' + formatoValor(totalPresupuestoVentas) + '</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalVentas) + '</td>';
                sTxt += '<td class="centrar">' + ((totalVentas / totalPresupuestoVentas) * 100).toFixed(2) + '%</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalPresupuestoCobros) + '</td>';
                sTxt += '<td class="centrar">' + formatoValor(totalCobros) + '</td>';
                sTxt += '<td class="centrar">' + ((totalCobros / totalPresupuestoCobros) * 100).toFixed(2) + '%</td>';
                sTxt += '<td class="centrar"></td>';
                sTxt += '</tr></tfoot>';
                sTxt += '</table>';

                console.log("arma totales")
                console.log(sTxt)

                cardbody.append($.parseHTML(sTxt)[0]);
                card.appendChild(cardbody);
                div.appendChild(card);
                $('#contenedorTabla').append(div);
                $('#tablePresupuesto').DataTable({
                    "processing": true, // for show progress bar
                    "paging": true,
                    "bInfo": false,
                    //"scrollY": "500px",
                    dom: 'Bfrtip',
                    "buttons": [
                        {
                            "extend": 'copy', "text": 'Copiar', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                        },
                        {
                            "extend": 'pdf', "text": 'PDF', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                        },
                        {
                            "extend": 'excel', "text": 'Excel', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                            , messageTop: titulo
                        },
                        {
                            "extend": 'print', "text": 'Imprimir', "className": 'btn btn-default btn-xs'
                            , exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                        },
                    ],
                    initComplete: function () {
                        $('.buttons-pdf').html('<i class="far fa-file-excel"></i>')
                    },
                    //buttons: [
                    //    'copy', 'csv', 'excel', 'pdf', 'print'
                    //],
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Spanish.json'
                    }
                });

                console.log("sale datos")
                var presupuesto = JSON.parse(d);
                //var presupuesto = d;
                var vendedores = presupuesto.map(presupuesto => presupuesto.nombre_vendedor);
                var ventas = presupuesto.map(presupuesto => presupuesto.ventas_neta);
                var cobros = presupuesto.map(presupuesto => presupuesto.total_cobros);

                console.log("entra a armar graficos")
                GeneraGraficos(presupuesto);

                console.log("sale de graficos")
                cierraLoading();
            },
            error: function (e) {
                Toast.fire({
                    icon: 'error',
                    title: 'Hubo un error al intentar generar.'
                })
            }
        })
    }

});


function SugerirFecha() {
    var empresa = $("#empresa").val();
    var sucursal = $("#sucursal").val();
    $.ajax({
        url: 'SugerirFechas',
        type: 'GET',
        data: {
            empresa: empresa,
            sucursal: sucursal
        },
        dataType: "JSON",
        contentType: "application/JSON",
        success: function (d) {

            if (d.MensajeError != null) {
                Toast.fire({
                    icon: 'error',
                    title: d.MensajeError
                })
            } else {
                const datos = JSON.parse(d);
                Toast.fire({
                    icon: 'success',
                    title: 'El corte sugerido es desde ' + datos.fecha_desde + ' hasta ' +datos.fecha_hasta
                })
                $('#fecha_presupuesto').data('daterangepicker').setStartDate(datos.fecha_desde);
                $('#fecha_presupuesto').data('daterangepicker').setEndDate(datos.fecha_hasta);
            }
            console.log(d);

        },
        error: function (e) {
            Toast.fire({
                icon: 'error',
                title: 'No se encuentra un corte de fechas sugerido'
            })
        }
    });
}
