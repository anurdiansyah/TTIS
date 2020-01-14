using System.Net.Http;
using System.Threading.Tasks;
using TTIS.WebUi.Common;
using TTIS.WebUi.Models;

namespace TTIS.WebUi.Services
{
    public interface IAppUser
    {
        Task<AspNetUsers> CurrentUser();
    }
}
