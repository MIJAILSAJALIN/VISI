
//document.addEventListener("keyup", function (event) {
//    if (event.keyCode === 13) {
//        alert('Enter is pressed!');
//    }
//});

//document.addEventListener('keypress', function (evt) {
//    // Si el evento NO es una tecla Enter
//    if (evt.key !== 'Enter') {
//        return;
//    }
    
 

//    let element = evt.target;

//    // Si el evento NO fue lanzado por un elemento con class "focusNext"
//    if (!element.classList.contains('focusNext')) {
//        return;
//    }
//    console.log("pulsada enter");

//    // AQUI logica para encontrar el siguiente
//    let tabIndex = element.tabIndex + 1;
//    var next = document.querySelector('[tabindex="' + tabIndex + '"]');

//    // Si encontramos un elemento
//    if (next) {
//        console.log("debería pasar el foco");
//        next.focus();
//        event.preventDefault();
//    }
//});








function AgregarLinea() {
    
    var lon = facturaListadoViewModel.facturas().length - 1;
    var indi = 1;
    if (lon >= 0) {
        indi = facturaListadoViewModel.facturas()[lon].id();        
        ++indi;
    };
    
    //console.log(indi);
    facturaListadoViewModel.facturas.push(new facturaViewModel({ id: indi, numero: 0, descripcion: `nueva línea ${indi}`, precio: 0, cantidad: 0 }));
    
    $("[name=descri]").last().focus();

}
function manejarSalidaFocus(factura) {
    alert("estoy en el evento");
    console.log(factura.subtotal());
   // facturaListadoViewModel.cargando(true);
    return factura.subtotal();
}

async function grabarFactura() {

    if (facturaListadoViewModel.facturas().length == 0) {
        confirmarAccion({
            titulo: '¿ Quiere modificar la factura ?',
            icono: 'question',
            callBackAceptar: () => {
                console.log("bien...");
                return;
            },
            callBackCancelar: () => {
                console.log("bien CANCELAR");
                return;
            }
        });


    }


    if (facturaListadoViewModel.facturas().length == 0) {
        console.log("No hay líneas de detalle...");
        Swal.fire(
            'ERROR',
            'La factura no tiene líneas de detalle, no se puede grabar.',
            'error'
        );
        return;
    }
    if (clienteIdOculto.value == 0) {
        console.log("No ha seleccionado ningún cliente...");
        Swal.fire(
            'ERROR',
            'Debe seleccionar un cliente.',
            'error'
        );
        return;
    }
   

    confirmarAccion({
        titulo: '¿ Quiere modificar la factura ?',       
        icono: 'question',
        callBackAceptar: () => {
                console.log("Modificando la factura...");
                return;
        },
        callBackCancelar: () => {
                console.log("CANCELAR");
                return;
        }
    });

    var listado = [];
    var a = 0;
    facturaListadoViewModel.facturas().forEach(x => listado.push({
        descripcion: x.descripcion(), precio: x.precio(), cantidad: x.cantidad(),
        lineanumero: ++a
    }));

    const data = JSON.stringify({
        LineasFacturas: listado,
        clienteId: clienteIdOculto.value,
        Fecha: Fechafac.value,
        Numero: numeroFactura.value
        //Subtotal: sumaTotal.value        
    });

    const respuesta = await fetch(urlFactura, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    //alert(respuesta);
    if (respuesta.ok) {
        const json = await respuesta.json();
        numeroFactura.value = json;
        swmensaje("Se ha grabado la factura nº: " + json,'success');
        
        //console.log(json);        
    } else {
        alert("E R R O R");
        alert(respuesta.status);
    }

}
function swmensaje(mensaje, icono) {
    Swal.fire({
        title: mensaje,
        icon: icono,
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        }
    })
}
