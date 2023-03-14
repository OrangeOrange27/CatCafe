using SharedLibrary;
using UnityEngine;
using Web;
using Zenject;

public class DebugClientServer : MonoBehaviour
{
    [Inject]
    HttpCommunicator HttpCommunicator;
    async void Start()
    {
        var player = await HttpCommunicator.Get<Player>();
    }

}
