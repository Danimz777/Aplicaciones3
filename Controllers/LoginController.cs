using Servicios_6_8.Clases;
using Examen_3.Models;  // Importa las clases necesarias del modelo de estudiantes
using System.Linq;
using System.Web.Http;
using Servicios_6_8.Models;

namespace Servicios_6_8.Controllers
{
    [RoutePrefix("api/Login")]
    
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("IngresarEstudiante")]
        public IQueryable<LoginRespuesta> IngresarEstudiante(Login login)
        {
            // Crear una instancia de clsEstudiante para manejar la autenticación del estudiante
            clsEstudiante _Estudiante = new clsEstudiante();
            _Estudiante.login = login;
            return _Estudiante.Ingresar();
        }
    }
}
