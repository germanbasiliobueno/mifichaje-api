using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mifichaje.Dominio;

namespace mifichaje.Aplicacion.DTOs
{
    public class FichajeDTO
    {
        public int IdFichaje { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public TipoFichaje TipoFichaje { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
