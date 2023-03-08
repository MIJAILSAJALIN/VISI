function confirmarAccion({ titulo, icono, callBackAceptar, callBackCancelar }) {
    //alert("estoy en confirmaraccion");
    Swal.fire({
        title: titulo || 'Confirme por favor...',
        icon: icono || 'warning',
        showCancelButton: callBackCancelar,
        confirmButtonColor: '#27AE60',
        cancelButtonColor: '#E74C3C',
        confirmButtonText: 'Aceptar',
        focusConfirm: true
    }).then((respuesta) => {
        console.log("estoy en el then de la función");
        if (respuesta.isConfirmed) {
            if (callBackAceptar) {
                callBackAceptar();
            }            
        } else if (callBackCancelar) {
            callBackCancelar();
        }

    });
}