using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mifichaje.Dominio
{
   public  class Fichaje
    {
        public int IdFichaje { get;  set; }
        public int IdUsuario { get; set; }
        public DateTime FechaHora { get;  set; }
        public int TipoFichaje { get;  set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public DateTime FechaRegistro { get;  set; }
    }
}
