using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mifichaje.Dominio
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Documento { get; set; }
        public string  NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public int IdRol{ get; set; }
        public bool Estado { get; set; }
        public string FechaRegistro { get; set; }
    }
}
