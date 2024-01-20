using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CrudMvvm.Utilidades
{
    public class ProductoMensajeria : ValueChangedMessage<ProductoMensaje>
    {
        public ProductoMensajeria(ProductoMensaje value):base(value)
        {
            
        }
    }
}
