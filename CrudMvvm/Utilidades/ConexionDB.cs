

namespace CrudMvvm.Utilidades
{
    //Clase que devuelve la ruta para la base de datos
    public static class ConexionDB
    {
        public static string DevolverRuta(string nombreBaseDatos) { 
        
            string rutabaseDatos = string.Empty;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                rutabaseDatos = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                rutabaseDatos = Path.Combine(rutabaseDatos, nombreBaseDatos);
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS) {
                rutabaseDatos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                rutabaseDatos = Path.Combine(rutabaseDatos,"..","Library", nombreBaseDatos);
            }
            return rutabaseDatos;
        }
    }
}
