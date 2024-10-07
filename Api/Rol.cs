using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Rol
    {
        
        public int IdRol {get; set; }
        public required string Nombre { get; set; }
        public bool Habilitado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
