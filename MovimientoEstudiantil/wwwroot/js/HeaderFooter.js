document.addEventListener("DOMContentLoaded", function () {
    header();
    footer();
});
function header() {
    const path = window.location.pathname.toLowerCase();
    let subTitulo = "Inicio";

    if (path.includes("/estudiante")) {
        subTitulo = "Registro de Estudiantes";
    } else if (path.includes("/grafico")) {
        subTitulo = "Gestion de Graficos";
    } else if (path.includes("/usuario")) {
        subTitulo = "Registro de Usuarios";
    }

    const rol = getCookie("rol") || "";
    const correo = getCookie("usuarioCorreo") || "";

    let enlacesRol = "";

    if (rol === "Administrador") {
        enlacesRol += `<a href="/Estudiante">Estudiante</a>`;
    }

    if (rol === "Coordinador") {
        enlacesRol += `<a href="/Usuario/Panel">Usuario</a>`;
    }

    let bienvenida = "";
    if (correo && rol) {
        bienvenida = `<span class="text-white me-3">Bienvenido, (${rol})</span>`;
    }

    const botonSesion = rol
        ? `<form method="post" action="/Auth/Logout" style="display:inline;">
                <button type="submit" class="login-btn btn-cerrar">Cerrar sesión</button>
           </form>`
        : `<a href="/Auth/Login" class="login-btn">Login</a>`;
   
    const head = `
    <header class="header-container">
        <div class="header-left">
            <img src="/img/logo_Movile.jpeg" alt="Logo Movile">
            <div class="header-title">
                <h1>Movil E</h1>
                <h2>Universidad de Costa Rica</h2>
            </div>
        </div>
        <div class="header-nav-container">
            <nav class="header-nav">
                <a href="/">Inicio</a>
                <a href="/Grafico">Estadísticas</a>
                ${enlacesRol}
            </nav>
            ${bienvenida}
            ${botonSesion}
        </div>
    </header>
    <div class="sub-header">
        ${subTitulo}
    </div>
    `;

    const headerEl = document.getElementById('headerGlobal');
    if (headerEl) headerEl.innerHTML = head;
}




function footer() {
    const foot = `
    <footer class="footer-container">
        <div class="footer-section">
            <img src="/img/logo_footer.png" alt="Logo UCR">
        </div>
        <div class="footer-section center">
            <p>Proyecto de Movilidad Estudiantil</p>
            <p>© 2025 UCR. Todos los derechos reservados.</p>
        </div>
        <div class="footer-section right">
            <strong>Contacto</strong><br>
            Correo: example@gmail.com<br>
            Teléfono: +506 1111 2222
        </div>
    </footer>
    `;

    document.getElementById('footerGlobal').innerHTML = foot;
}


function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null;
}