using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Web
{
    public class HttpCommunicator
    {
        private readonly string _endpoint;

        protected HttpCommunicator(string endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task<T> Get<T>()
        {
            var getRequest = CreateRequest(_endpoint);
            getRequest.SendWebRequest();

            while (!getRequest.isDone) await Task.Delay(10);
            return JsonUtility.FromJson<T>(getRequest.downloadHandler.text);
        }

        public async Task<T> Post<T>(object request)
        {
            var postRequest = CreateRequest(_endpoint, RequestType.POST, request);
            postRequest.SendWebRequest();

            while (!postRequest.isDone) await Task.Delay(10);
            return JsonUtility.FromJson<T>(postRequest.downloadHandler.text);
        }


        protected UnityWebRequest CreateRequest(string path, RequestType requestType = RequestType.GET,
            object data = null)
        {
            var request = new UnityWebRequest(path, requestType.ToString());

            if (data != null)
            {
                var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }

        protected void AttachHeader(UnityWebRequest request, string key, string value)
        {
            request.SetRequestHeader(key, value);
        }
    }
}
