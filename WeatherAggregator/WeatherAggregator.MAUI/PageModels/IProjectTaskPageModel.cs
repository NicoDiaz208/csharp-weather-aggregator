using CommunityToolkit.Mvvm.Input;
using WeatherAggregator.MAUI.Models;

namespace WeatherAggregator.MAUI.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}