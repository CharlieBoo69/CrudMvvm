using CrudMvvm.ViewModels;
namespace CrudMvvm
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

        }

    }

}
