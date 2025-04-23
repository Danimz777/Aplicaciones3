namespace Servicios_6_8.Models
{
    public class Login
    {
        public string Usuario { get; set; }  // El documento del estudiante
        public string Clave { get; set; }    // La clave que ingresa el estudiante (sin cifrado)
        public string PaginaSolicitud { get; set; }
    }

    public class LoginRespuesta
    {
        public string Usuario { get; set; }
        public string Perfil { get; set; }
        public string PaginaInicio { get; set; }
        public bool Autenticado { get; set; }
        public string Token { get; set; }
        public string Mensaje { get; set; }
    }
}
