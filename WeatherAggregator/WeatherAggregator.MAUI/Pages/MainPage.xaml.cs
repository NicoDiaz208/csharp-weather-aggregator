using WeatherAggregator.MAUI.Models;
using WeatherAggregator.MAUI.PageModels;

namespace WeatherAggregator.MAUI.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}