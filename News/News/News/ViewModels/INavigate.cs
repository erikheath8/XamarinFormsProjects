using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace News.ViewModels
{
    public interface INavigate
    {
        Task NavigateTo(String route);
        Task PushModal(Page page);
        Task PopModal();
    }
}
