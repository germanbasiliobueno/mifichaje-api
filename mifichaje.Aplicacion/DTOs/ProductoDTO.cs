using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mifichaje.Aplicacion.DTOs
{
   public  class ProductoDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string FechaRegistro { get; set; }
       public string SimboloMoneda { get; set; }
    }
}
