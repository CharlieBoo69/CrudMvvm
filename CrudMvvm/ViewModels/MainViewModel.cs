using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using CrudMvvm.DataAccess;
using CrudMvvm.DTOs;
using CrudMvvm.Utilidades;
using CrudMvvm.Modelos;
using System.Collections.ObjectModel;
using CrudMvvm.Views;
using Microsoft.Maui.Controls.Platform.Compatibility;


namespace CrudMvvm.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        //representa hacia la base de datos 
        private readonly ProductoDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<ProductoDTO> listaProductos = new ObservableCollection<ProductoDTO>();

        public MainViewModel(ProductoDbContext context)
        {
            _dbContext = context;

            //El metodo de obtener el listado de productos lo ejecuto en el constructor yendo al hilo principal,
            //al momento de cargar la pagina obtien la lista inmediatamnete
            MainThread.BeginInvokeOnMainThread(new Action(async () => await Obtener()));

            //Para que cada que inicie la aplicacion pueda estar subscrito a recibir los mensajes 

            WeakReferenceMessenger.Default.Register<ProductoMensajeria>(this, (r, m) => {

                ProductoMensajeRecibido(m.Value);


            });
        }

        //Metodo para obetner la lista de productos desde la base de datos 

        public async Task Obtener() {

            //consultar la base de datos y obtener la lista de manera async
            var lista = await _dbContext.Productos.ToListAsync();
            //Validar si tiene elemenots o no
            if (lista.Any()) {

                foreach (var item in lista)
                {
                    ListaProductos.Add(new ProductoDTO {
                        IdProducto = item.IdProducto,
                        NombreProducto = item.NombreProducto,
                        Detalle = item.Detalle,
                        Precio = item.Precio,
                        FechaCaducidad = item.FechaCaducidad,
                    });
                }

            }

        }

        private void ProductoMensajeRecibido(ProductoMensaje productoMensaje)
        {
            var productoDto = productoMensaje.ProductoDTO;

            //en base al mensaje que recibe si es para crear
            if (productoMensaje.EsCrear)
            {

                ListaProductos.Add(productoDto);
            }
            else //en base al mensaje que recibe si es para editar
            {
                var encontrado = ListaProductos
                        .First(e => e.IdProducto == productoDto.IdProducto);
                encontrado.NombreProducto = productoDto.NombreProducto;
                encontrado.Detalle = productoDto.Detalle;
                encontrado.Precio = productoDto.Precio;
                encontrado.FechaCaducidad = productoDto.FechaCaducidad;


            }
        }

        //Metodo para crear nuevo producto

        [RelayCommand]
        private async Task Crear() {
            //voy a la pagoina productos page y como parametro el id le paso 0
            var uri = $"{nameof(Productopage)}?id=0";
            //redirigir hacua la pagna 
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Editar(ProductoDTO productoDTO)
        {
            var uri = $"{nameof(Productopage)}?id={productoDTO.IdProducto}";
            //redirigir hacua la pagna 
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Eliminar(ProductoDTO productoDTO)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Desea eliminar el producto?", "Si","No");
            if (answer)
            {
                //Encontrar el producto
                var encontrado = await _dbContext.Productos.
                    FirstAsync(e => e.IdProducto == productoDTO.IdProducto);
                //elimianar el producto
                _dbContext.Productos.Remove(encontrado);
                
                await _dbContext.SaveChangesAsync();
                ListaProductos.Remove(productoDTO);

            }
        }


    }
}
