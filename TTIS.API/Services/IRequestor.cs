using System.Net.Http;
using System.Threading.Tasks;
using TTIS.API.UsersModels;

namespace TTIS.API.Services
{
    public interface IRequestor
    {
        AspNetUsers IUser();
        string IpAddress();
    }
}
