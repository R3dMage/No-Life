using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


namespace NoLifeUWP
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton(new MainViewModel())
                .BuildServiceProvider());
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return Ioc.Default.GetService<MainViewModel>();            
            }
        }
    }
}
