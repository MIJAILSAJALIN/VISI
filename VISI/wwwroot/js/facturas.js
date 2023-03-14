
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
// *********************************************************************************************
function AgregarLinea() {
    
    var lon = facturaListadoViewModel.facturas().length - 1;
    var indi = 1;
    if (lon >= 0) {
        indi = facturaListadoViewModel.facturas()[lon].id();        
        ++indi;
    };
    
    facturaListadoViewModel.facturas.push(new facturaViewModel({ id: indi, numero: 0, descripcion: `nueva línea ${indi}`, precio: 0, cantidad: 0 }));
    
    $("[name=descri]").last().focus();
}
// *********************************************************************************************
function manejarSalidaFocus(factura) {
    alert("estoy en el evento");
    console.log(factura.subtotal());
   // facturaListadoViewModel.cargando(true);
    return factura.subtotal();
}
// *********************************************************************************************
 async function grabarFactura() {


    if (facturaListadoViewModel.facturas().length == 0) {
        Swal.fire(
            'ERROR',
            'La factura no tiene líneas de detalle, no se puede grabar.',
            'error'
        );
        return;
    }
    if (clienteIdOculto.value == 0) {
        Swal.fire(
            'ERROR',
            'Debe seleccionar un cliente.',
            'error'
        );
        return;
    }

     let titulo = "creación";
     if (numeroFactura.value > 0) {
         //se puede controlar que si no ha habido cambios no se haga la llamada a la API
         titulo = "modificación";
     }


     let accion = await confirmarAccion({
         titulo: `Confirme la ${titulo} de la factura...`,
         icono: 'question',
         callBackAceptar: () => {
         },
         callBackCancelar: () => {
         }
     });
     if (!accion) {
         swal.fire("Atención", `Se ha cancelado la acción`, "warning");
         return;
     }

    let listado = [];
    let a = 0;
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
    if (respuesta.ok) {
        const json = await respuesta.json();
        numeroFactura.value = json;
        swmensaje("Se ha grabado la factura nº: " + json,'success');       
    } else {
        Swal.fire(
            'Se ha producido un error inesperado',
            `Código: ${ respuesta.status }`,
            'error'
        );
        console.log(respuesta.status);
    }

}
// *********************************************************************************************
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
