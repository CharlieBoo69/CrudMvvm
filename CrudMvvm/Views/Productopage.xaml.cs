using CrudMvvm.ViewModels;
namespace CrudMvvm.Views;

public partial class Productopage : ContentPage
{
	public Productopage(ProductoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}