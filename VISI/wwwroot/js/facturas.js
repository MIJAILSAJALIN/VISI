function AgregarLinea() {
    
    var lon = facturaListadoViewModel.facturas().length - 1;
    var indi = 1;
    if (lon >= 0) {
        indi = facturaListadoViewModel.facturas()[lon].id();        
        ++indi;
    };
    
    console.log(indi);
    facturaListadoViewModel.facturas.push(new facturaViewModel({ id: indi, numero: 0, descripcion: `nueva línea ${indi}`, precio: 0, cantidad: 0 }));
    
    $("[name=descri]").last().focus();

}
function manejarSalidaFocus(factura) {
    console.log("estoy en el evento");
    console.log(factura.subtotal());
   // facturaListadoViewModel.cargando(true);
    return factura.subtotal();
}

async function grabarFactura() {
    facturaListadoViewModel.facturas().forEach(f => console.log(f.descripcion()));
    if (facturaListadoViewModel.facturas().length == 0) {
        alert("La factura no tiene líneas de detalle, no se puede grabar");
    }
    else {
        alert("voy a grabar la factura");
    }

    //const data = JSON.stringify(facturaListadoViewModel.facturas()[0].descripcion());
    const data = (JSON.stringify({ "misdatos":"una mierda"}));
    
    //const data = JSON.stringify("hola");
    alert(data);
    const respuesta = await fetch(urlFactura, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    //alert(respuesta);
    if (respuesta.ok) {
        const json = respuesta.json();
    } else {
        alert("E R R O R");
        alert(respuesta.status);
    }


}