using Examen_3.Models;
using Servicios_6_8.Models;
using System.Collections.Generic;
using System.Linq;
using Examen_3.Clases; // Asegúrate de importar la clase del TokenGenerator

namespace Servicios_6_8.Clases
{
    public class clsEstudiante
    {
        private DBExamenEntities dbExamen = new DBExamenEntities();
        public Login login { get; set; }

        public IQueryable<LoginRespuesta> Ingresar()
        {
            // Buscamos al estudiante por el Usuario
            Estudiante estudiante = dbExamen.Estudiantes.FirstOrDefault(e => e.Usuario == login.Usuario);

            if (estudiante == null)
            {
                return new List<LoginRespuesta>
                {
                    new LoginRespuesta
                    {
                        Autenticado = false,
                        Mensaje = "Estudiante no existe"
                    }
                }.AsQueryable();
            }

            // Verificamos la clave
            if (estudiante.Clave != login.Clave)
            {
                return new List<LoginRespuesta>
                {
                    new LoginRespuesta
                    {
                        Autenticado = false,
                        Mensaje = "La clave no coincide"
                    }
                }.AsQueryable();
            }

            // Generamos el token
            string token = TokenGenerator.GenerateTokenJwt(estudiante.Usuario);

            // Retornamos la respuesta con el token generado
            return new List<LoginRespuesta>
            {
                new LoginRespuesta
                {
                    Usuario = estudiante.Usuario,
                    Autenticado = true,
                    Perfil = "Estudiante",
                    PaginaInicio = "/inicio",
                    Token = token,
                    Mensaje = ""
                }
            }.AsQueryable();
        }
    }
}﻿
