using News.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace News
{
    public class Navigator : INavigate
    {
        public async Task NavigateTo(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        public async Task PushModal(Page page)
        {
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        public async Task PopModal()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
