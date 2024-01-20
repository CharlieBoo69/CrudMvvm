using System.ComponentModel.DataAnnotations;

namespace CrudMvvm.Modelos
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Detalle { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaCaducidad { get; set; }    
    }
}
