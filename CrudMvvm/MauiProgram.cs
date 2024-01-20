using Microsoft.Extensions.Logging;
using CrudMvvm.DataAccess;
using CrudMvvm.ViewModels;
using CrudMvvm.Views;

namespace CrudMvvm
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //confi de base de datos
            var dbContext = new ProductoDbContext();
            dbContext.Database.EnsureCreatedAsync();
            dbContext.Dispose(); //Para que sea liberada

            builder.Services.AddDbContext<ProductoDbContext>();

            builder.Services.AddTransient<Productopage> ();
            builder.Services.AddTransient<ProductoViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            Routing.RegisterRoute(nameof(Productopage), typeof(Productopage));



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
