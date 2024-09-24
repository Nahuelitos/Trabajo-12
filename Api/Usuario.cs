using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Usuario
    {
        public int IdUsuario {get; set;}

        public required string Nombre {get; set;} 
        public required string  Email {get; set;}

        public required string NombredeUsuario {get; set;}

        public required string Contraseña {get; set;}

        public bool Habilitado {get; set;}

        public DateTime FechaCreacion {get; set;}

        public List<Rol> Roles { get; set; } = new List<Rol>();
    }
}