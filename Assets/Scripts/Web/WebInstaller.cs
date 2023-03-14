using UnityEngine;
using Zenject;

namespace Web
{
    public class WebInstaller : MonoInstaller
    {
        [SerializeField] private string _endpoint = "http://localhost:5051/";

        public override void InstallBindings()
        {
            Container.BindInstance(_endpoint).WhenInjectedInto<HttpCommunicator>();
            Container.Bind<HttpCommunicator>().ToSelf().AsSingle().NonLazy();
        }
    }
}
