using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mifichaje.Aplicacion.DTOs
{
    public class PutProductoDTO
    {
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
    //    public string FechaRegistro { get; set; }
    }
}
