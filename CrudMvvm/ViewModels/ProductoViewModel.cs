using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using CrudMvvm.DataAccess;
using CrudMvvm.DTOs;
using CrudMvvm.Utilidades;
using CrudMvvm.Modelos;

namespace CrudMvvm.ViewModels
{
    public partial class ProductoViewModel : ObservableObject, IQueryAttributable
    {
        //representa hacia la base de datos 
        private readonly ProductoDbContext _dbContext;

        [ObservableProperty]
        private ProductoDTO productoDTO = new ProductoDTO();

        [ObservableProperty]
        private string tituloPagina;

        private int IdProducto;

        [ObservableProperty]
        private bool loadingEsVisible = false;

        //recivo por inyeccion de dependencias el contexto de la base de datos
        public ProductoViewModel(ProductoDbContext context)
        {
            _dbContext = context;
            ProductoDTO.FechaCaducidad = DateTime.Now;
        }

        //meodo para cuanod recivo los parametros de la pagina para nuevo producto o editar
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse( query["id"].ToString());
            IdProducto = id;

            if (IdProducto == 0)
            {
                TituloPagina = "Nuevo Producto";
            }
            else {
                TituloPagina = "Editar Producto";
                LoadingEsVisible = true;
                //Especificar que pueda ejecutar una funcionalidad 
                await Task.Run(async () =>
                {
                    var encontrado= await _dbContext.Productos.FirstAsync(e=>e.IdProducto == IdProducto);
                    ProductoDTO.IdProducto = encontrado.IdProducto;
                    ProductoDTO.NombreProducto = encontrado.NombreProducto;
                    ProductoDTO.Detalle = encontrado.Detalle;
                    ProductoDTO.Precio = encontrado.Precio;
                    ProductoDTO.FechaCaducidad = encontrado.FechaCaducidad;

                    //regresar al hilo principal del proyecto
                    MainThread.BeginInvokeOnMainThread(() => { LoadingEsVisible=false; });
                });
            }
        }

        [RelayCommand]
        private async Task Guardar() {
            LoadingEsVisible = true;
            ProductoMensaje mensaje = new ProductoMensaje();
            //Toda la operacoin esta dentro de un hilo y se corre diferente al hilo principal
            await Task.Run(async () =>
            {
                //Logica para crear
                if (IdProducto == 0)
                {
                    var tbProducto = new Producto
                    {

                        NombreProducto = ProductoDTO.NombreProducto,
                        Detalle = ProductoDTO.Detalle,
                        Precio = ProductoDTO.Precio,
                        FechaCaducidad = ProductoDTO.FechaCaducidad,
                    };

                    _dbContext.Productos.Add(tbProducto);
                    await _dbContext.SaveChangesAsync();

                    ProductoDTO.IdProducto = tbProducto.IdProducto;
                    mensaje = new ProductoMensaje()
                    {
                        EsCrear = true,
                        ProductoDTO = ProductoDTO
                    };

                }//Para Editar
                else { 
                    var encontrado = await _dbContext.Productos.FirstAsync(e=>e.IdProducto==IdProducto);
                    encontrado.NombreProducto= ProductoDTO.NombreProducto;
                    encontrado.Detalle= ProductoDTO.Detalle;
                    encontrado.Precio= ProductoDTO.Precio;
                    encontrado.FechaCaducidad = ProductoDTO.FechaCaducidad;

                    await _dbContext.SaveChangesAsync();

                    mensaje = new ProductoMensaje()
                    {
                        EsCrear = false,
                        ProductoDTO = ProductoDTO
                    };
                }

                //regresar al hilo principal del proyecto
                MainThread.BeginInvokeOnMainThread( async () => 
                {
                    LoadingEsVisible = false;
                    WeakReferenceMessenger.Default.Send(new ProductoMensajeria(mensaje)); //Devolver el mensaje que hemos creado
                    await Shell.Current.Navigation.PopAsync();


                
                });

            });
        }
    }
}
