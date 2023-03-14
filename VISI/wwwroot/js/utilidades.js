async function confirmarAccion({ titulo, icono, callBackAceptar, callBackCancelar }) {
    var retorno = false;
    await Swal.fire({
        title: titulo || 'Confirme por favor...',
        icon: icono || 'warning',
        showCancelButton: callBackCancelar,
        confirmButtonColor: '#27AE60',
        cancelButtonColor: '#E74C3C',
        confirmButtonText: 'Aceptar',
        focusConfirm: true
    }).then((respuesta) => {
        //console.log('entró en respuesta');
        if (respuesta.isConfirmed) {

            if (callBackAceptar) {
                callBackAceptar();
            }
            //console.log('entró en respuesta ACEPTADA');
            //console.log('RETORNARÁ TRUE');
            retorno = true;
        } else if (callBackCancelar) {
            callBackCancelar();
        //    console.log('entró en respuesta CANCELADA');
        } else {
        //    console.log('RETORNARÁ FALSE');
        }

    });
    return retorno;
}