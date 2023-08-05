function toggleComments() {
    var commentsContainer = document.getElementById("comments-container");
    var toggleCheckbox = document.getElementById("toggle-comments-checkbox");

    if (toggleCheckbox.checked) {
        commentsContainer.style.display = "block";
    } else {
        commentsContainer.style.display = "none";
    }
}
function toggleDocuments() {
    var documentsContainer = document.getElementById("documents-container");
    var toggleCheckboxD = document.getElementById("toggle-document-checkbox");

    if (toggleCheckboxD.checked) {
        documentsContainer.style.display = "block";
    } else {
        documentsContainer.style.display = "none";
    }
}

function guardarComentario() {
    var ticketId = document.getElementById("ticketId").value;
    var comment = document.getElementById("comment").value;

    // Objeto de datos para enviar al servidor
    var data = {
        mjs: comment,
        id: ticketId
    };

    $.ajax({
        url: '/Comment/AddComment', // Reemplaza 'ControllerName' con el nombre real de tu controlador
        type: 'POST',
        data: data,
        success: function (response) {
            // Manejar la respuesta del servidor
            if (response) {
                // Comentario agregado correctamente
                Swal.fire({
                    icon: 'success',
                    title: 'Comentario agregado',
                    text: 'El comentario se ha agregado correctamente.',
                    showConfirmButton: false,
                    timer: 2000
                }).then(function () {
                    // Realizar las acciones necesarias, como actualizar la interfaz de usuario
                    console.log('Comentario agregado correctamente');
                    // Cerrar el modal después de guardar el comentario
                    $('#addCommentModal').modal('hide');
                    location.reload();
                });
            } else {
                // Ocurrió un error al agregar el comentario
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ocurrió un error al agregar el comentario. Inténtalo de nuevo más tarde.',
                    showConfirmButton: false,
                    timer: 2000
                }).then(function () {
                    console.log('Error al agregar el comentario');
                });
            }
        },
        error: function () {
            // Ocurrió un error al realizar la solicitud AJAX
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Ocurrió un error al realizar la solicitud. Inténtalo de nuevo más tarde.',
                showConfirmButton: false,
                timer: 2000
            }).then(function () {
                console.log('Error al realizar la solicitud AJAX');
            });
        }
    });
}
