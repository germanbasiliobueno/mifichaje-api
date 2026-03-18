using mifichaje.Aplicacion.Interfaces;
using mifichaje.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mifichaje.Aplicacion.DTOs;


namespace mifichaje.Aplicacion.Services
{
    public class ProductoService
    {
        private readonly IProductoRepository _repository;
        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllAsync()
        {
            var productos = await _repository.ListaTotalAsync();
            return productos.Select(e => new ProductoDTO { IdProducto = e.IdProducto, Nombre = e.Nombre, Precio = e.Precio, FechaRegistro = e.FechaRegistro });

        }
        public async Task AñadirAsync(PostProductoDTO dto)
        {
            var Producto = new Producto { Nombre = dto.Nombre, Precio = dto.Precio };
            await _repository.AñadirAsync(Producto);
        }

        public async Task<bool> ActualizarAsync(int id, PutProductoDTO dto)
        {           
            var Producto = new Producto { IdProducto = id, Nombre = dto.Nombre, Precio = dto.Precio };
           var actualizado = await _repository.ActualizarAsync(id, Producto);
            return actualizado;
            
        }

        public async Task EliminarAsync(int id)
        {
            await _repository.EliminarAsync(id);
        }

        public async Task<ProductoDTO?> GetPorIdAsync(int id)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);
            if (producto == null) return null;

            return new ProductoDTO
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                FechaRegistro = producto.FechaRegistro
            };
        }


    }
}
