using System.Threading.Tasks;

namespace Web
{
    public enum RequestType
    {
        GET = 0,
        POST = 1,
    }
    public interface IHttpCommunicator<in TRequest,TResponse>
    where TResponse : class
    where TRequest : class
    {
        Task<TResponse> Get<TResponse>(TRequest request);
        Task<TResponse> Post<TResponse>(TRequest request);
    }
}
