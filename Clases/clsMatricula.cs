using Examen_3.Models;
using System;
using System.Linq;

namespace Examen_3.Clases
{
    public class clsMatricula
    {
        private DBExamenEntities dbExamen = new DBExamenEntities(); // Contexto de la base de datos
        public Matricula matricula { get; set; }  // Propiedad para manipular la entidad Matricula

        // Método para insertar la matrícula
        public string Insertar(string documentoEstudiante)
        {
            // Buscar al estudiante por su Documento
            var est = dbExamen.Estudiantes
                .FirstOrDefault(e => e.Documento == documentoEstudiante); // Buscar estudiante por documento

            if (est == null)
                return "Estudiante no registrado"; // Si el estudiante no existe, retornar mensaje de error

            // Validar que los valores de los créditos y el valor del crédito sean mayores a cero
            if (matricula.NumeroCreditos <= 0)
                return "El número de créditos debe ser mayor que cero";

            if (matricula.ValorCredito <= 0)
                return "El valor del crédito debe ser mayor que cero";

            // Validar campos adicionales
            if (string.IsNullOrWhiteSpace(matricula.SemestreMatricula))
                return "El semestre de la matrícula es obligatorio";

            if (matricula.FechaMatricula == default)
                return "La fecha de matrícula es obligatoria";

            if (string.IsNullOrWhiteSpace(matricula.MateriasMatriculadas))
                return "Debe ingresar las asignaturas matriculadas";

            // Asignar la FK 'idEstudiante' de la matrícula, que se encuentra en la tabla Estudiantes
            matricula.idEstudiante = est.idEstudiante;

            // Calcular el total de la matrícula (Número de créditos * Valor del crédito)
            matricula.TotalMatricula = matricula.NumeroCreditos * matricula.ValorCredito;

            try
            {
                // Insertar la matrícula en la base de datos
                dbExamen.Matriculas.Add(matricula);
                dbExamen.SaveChanges();
                return "Matrícula insertada correctamente";
            }
            catch (Exception ex)
            {
                // En caso de error al insertar, retornar el mensaje de error
                return "Error al insertar la matrícula: " + ex.Message;
            }
        }

        // Método para consultar la matrícula por Documento del estudiante y semestre
        public Matricula Consultar(string documentoEstudiante, string semestre)
        {
            // Buscar al estudiante por su Documento
            var est = dbExamen.Estudiantes
                .FirstOrDefault(e => e.Documento == documentoEstudiante);

            if (est == null)
                return null; // Si no se encuentra el estudiante, retornar null

            // Buscar la matrícula del estudiante para el semestre específico
            var mat = dbExamen.Matriculas
                .FirstOrDefault(m => m.idEstudiante == est.idEstudiante && m.SemestreMatricula == semestre);

            return mat; // Retornar la matrícula encontrada
        }

        // Método para actualizar la matrícula de un estudiante
        public string Actualizar(string documentoEstudiante, string semestre)
        {
            // Buscar al estudiante por su Documento
            var est = dbExamen.Estudiantes
                .FirstOrDefault(e => e.Documento == documentoEstudiante);
            if (est == null)
                return "Estudiante no registrado"; // Si no se encuentra el estudiante, retornar mensaje de error

            // Buscar matrícula existente del estudiante para ese semestre
            var existente = dbExamen.Matriculas
                .FirstOrDefault(m => m.idEstudiante == est.idEstudiante && m.SemestreMatricula == semestre);

            if (existente == null)
                return "No se encontró matrícula para ese estudiante y semestre"; // Si no se encuentra la matrícula, retornar mensaje de error

            // Validaciones de los valores de los créditos y valor del crédito
            if (matricula.NumeroCreditos <= 0)
                return "El número de créditos debe ser mayor que cero";

            if (matricula.ValorCredito <= 0)
                return "El valor del crédito debe ser mayor que cero";

            // Actualizar los campos de la matrícula
            existente.NumeroCreditos = matricula.NumeroCreditos;
            existente.ValorCredito = matricula.ValorCredito;
            existente.TotalMatricula = matricula.NumeroCreditos * matricula.ValorCredito;
            existente.FechaMatricula = matricula.FechaMatricula;
            existente.MateriasMatriculadas = matricula.MateriasMatriculadas;

            try
            {
                // Guardar los cambios en la base de datos
                dbExamen.SaveChanges();
                return "Matrícula actualizada correctamente";
            }
            catch (Exception ex)
            {
                // En caso de error al actualizar, retornar mensaje de error
                return "Error al actualizar la matrícula: " + ex.Message;
            }
        }

        // Método para eliminar la matrícula de un estudiante
        public string Eliminar(string documentoEstudiante, string semestre)
        {
            // Buscar al estudiante por su Documento
            var est = dbExamen.Estudiantes
                .FirstOrDefault(e => e.Documento == documentoEstudiante);
            if (est == null)
                return "Estudiante no registrado"; // Si no se encuentra el estudiante, retornar mensaje de error

            // Buscar matrícula existente del estudiante para ese semestre
            var mat = dbExamen.Matriculas
                .FirstOrDefault(m => m.idEstudiante == est.idEstudiante && m.SemestreMatricula == semestre);

            if (mat == null)
                return "No se encontró matrícula para ese estudiante y semestre"; // Si no se encuentra la matrícula, retornar mensaje de error

            try
            {
                // Eliminar la matrícula de la base de datos
                dbExamen.Matriculas.Remove(mat);
                dbExamen.SaveChanges();
                return "Matrícula eliminada correctamente";
            }
            catch (Exception ex)
            {
                // En caso de error al eliminar, retornar mensaje de error
                return "Error al eliminar la matrícula: " + ex.Message;
            }
        }
    }
}
