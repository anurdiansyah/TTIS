using System.Net.Http;
using System.Threading.Tasks;

namespace TTIS.WebUi.Services
{
    public interface IGitternsHttpClient
    { 
        Task<HttpClient> GetClient();
    }
}
