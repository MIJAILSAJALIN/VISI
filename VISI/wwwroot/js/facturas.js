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
    console.log("estoy en el evento");
    console.log(factura.subtotal());
   // facturaListadoViewModel.cargando(true);
    return factura.subtotal();
}

async function grabarFactura() {
    //facturaListadoViewModel.facturas().forEach(f => console.log(f.descripcion()));
    if (facturaListadoViewModel.facturas().length == 0) {
        alert("La factura no tiene líneas de detalle, no se puede grabar");
        return;
    }
   

    var listado = [];
    var a = 0;
    facturaListadoViewModel.facturas().forEach(x => listado.push({
        descripcion: x.descripcion(), precio: x.precio(), cantidad: x.cantidad(),
        lineanumero: ++a
    }));

    const data = JSON.stringify({
        LineasFacturas: listado,
        
        clienteId: clienteIdOculto.value,
        Fecha: Fechafac.value
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
        alert("Se ha grabado la factura nº: "+json);
        //console.log(json);        
    } else {
        alert("E R R O R");
        alert(respuesta.status);
    }


}