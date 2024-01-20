using CommunityToolkit.Mvvm.ComponentModel;
namespace CrudMvvm.DTOs
{
    public partial class ProductoDTO : ObservableObject
    {
        [ObservableProperty]
        public int idProducto;
        [ObservableProperty]
        public string nombreProducto;
        [ObservableProperty]
        public string detalle;
        [ObservableProperty]
        public decimal precio;
        [ObservableProperty]
        public DateTime fechaCaducidad;
    }
}
