using Examen_3.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace Examen_3.Controllers
{
    [RoutePrefix("api/Matriculas")]
    [Authorize]
    public class MatriculasController : ApiController
    {
        // Contexto de la base de datos
        private DBExamenEntities dbExamen = new DBExamenEntities();

        [HttpPost]
        [Route("Insertar")]
        public IHttpActionResult Insertar([FromBody] Matricula matricula)
        {
            var est = dbExamen.Estudiantes
                .FirstOrDefault(e => e.Documento == matricula.Estudiante.Documento);
            if (est == null)
                return BadRequest("Estudiante no registrado");

            if (matricula.NumeroCreditos <= 0)
                return BadRequest("El número de créditos debe ser mayor que cero");
            if (matricula.ValorCredito <= 0)
                return BadRequest("El valor del crédito debe ser mayor que cero");
            if (string.IsNullOrWhiteSpace(matricula.SemestreMatricula))
                return BadRequest("El semestre de la matrícula es obligatorio");
            if (matricula.FechaMatricula == default)
                return BadRequest("La fecha de matrícula es obligatoria");
            if (string.IsNullOrWhiteSpace(matricula.MateriasMatriculadas))
                return BadRequest("Debe ingresar las asignaturas matriculadas");

            matricula.idEstudiante = est.idEstudiante;
            matricula.TotalMatricula = matricula.NumeroCreditos * matricula.ValorCredito;

            matricula.Estudiante = null; // 👈 para evitar que EF intente insertar estudiante nuevo

            try
            {
                dbExamen.Matriculas.Add(matricula);
                dbExamen.SaveChanges();
                return Ok("Matrícula insertada correctamente");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errores = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                var mensaje = string.Join("; ", errores);
                return BadRequest("Errores de validación: " + mensaje);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // GET: api/Matriculas/Consultar?documento=123&semestre=2025-1
        [HttpGet]
        [Route("Consultar")]
        public IHttpActionResult Consultar(string documento, string semestre)
        {
            var est = dbExamen.Estudiantes.FirstOrDefault(e => e.Documento == documento);
            if (est == null)
                return NotFound();

            var mat = dbExamen.Matriculas
                .FirstOrDefault(m => m.idEstudiante == est.idEstudiante && m.SemestreMatricula == semestre);

            if (mat == null)
                return NotFound();

            return Ok(mat);
        }

        // PUT: api/Matriculas/Actualizar
        [HttpPut]
        [Route("Actualizar")]
        public IHttpActionResult Actualizar([FromBody] Matricula matricula)
        {
            var est = dbExamen.Estudiantes.FirstOrDefault(e => e.Documento == matricula.Estudiante.Documento);
            if (est == null)
                return BadRequest("Estudiante no registrado");

            var existente = dbExamen.Matriculas
                .FirstOrDefault(m => m.idEstudiante == est.idEstudiante && m.SemestreMatricula == matricula.SemestreMatricula);
            if (existente == null)
                return NotFound();

            if (matricula.NumeroCreditos <= 0)
                return BadRequest("El número de créditos debe ser mayor que cero");
            if (matricula.ValorCredito <= 0)
                return BadRequest("El valor del crédito debe ser mayor que cero");
            if (string.IsNullOrWhiteSpace(matricula.MateriasMatriculadas))
                return BadRequest("Debe ingresar las asignaturas matriculadas");

            // Actualizar campos
            existente.NumeroCreditos = matricula.NumeroCreditos;
            existente.ValorCredito = matricula.ValorCredito;
            existente.TotalMatricula = matricula.NumeroCreditos * matricula.ValorCredito;
            existente.FechaMatricula = matricula.FechaMatricula;
            existente.MateriasMatriculadas = matricula.MateriasMatriculadas;

            try
            {
                dbExamen.SaveChanges();
                return Ok("Matrícula actualizada correctamente");
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/Matriculas/Eliminar
        [HttpDelete]
        [Route("Eliminar")]
        public IHttpActionResult Eliminar([FromBody] Matricula matricula)
        {
            var est = dbExamen.Estudiantes.FirstOrDefault(e => e.Documento == matricula.Estudiante.Documento);
            if (est == null)
                return BadRequest("Estudiante no registrado");

            var mat = dbExamen.Matriculas
                .FirstOrDefault(m => m.idEstudiante == est.idEstudiante && m.SemestreMatricula == matricula.SemestreMatricula);
            if (mat == null)
                return NotFound();

            try
            {
                dbExamen.Matriculas.Remove(mat);
                dbExamen.SaveChanges();
                return Ok("Matrícula eliminada correctamente");
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
