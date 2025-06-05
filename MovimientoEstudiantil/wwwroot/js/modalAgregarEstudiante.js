function busqueda() {
    const input = document.getElementById("busquedaCorreo");
    const tabla = document.getElementById("tablaEstudiantes");

    if (!input || !tabla) return;

    const filas = tabla.querySelectorAll("tbody tr");

    input.addEventListener("keyup", function () {
        const filtro = input.value.toLowerCase();
        filas.forEach(fila => {
            const correoTd = fila.querySelectorAll("td")[1];
            const texto = correoTd?.textContent.toLowerCase() || "";
            fila.style.display = texto.includes(filtro) ? "" : "none";
        });
    });
}
