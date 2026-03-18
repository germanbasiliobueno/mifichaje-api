using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using mifichaje.Dominio;

namespace mifichaje.Aplicacion.DTOs
{
    public class PostFichajeDTO
    {
        public int IdUsuario { get; set; }
        public TipoFichaje TipoFichaje { get; set; }
        public double Latitud { get; set; }      
        public double Longitud { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;

    }
}
