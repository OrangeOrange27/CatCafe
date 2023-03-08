using Core.Food;
using UI.MainUI.FoodStorage;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class FoodInstaller : MonoInstaller
    {
        [SerializeField] private WorkplaceManager _workplaceManager;
        [SerializeField] private FoodIconManager _foodIconManager;
        public override void InstallBindings()
        {
            Container.Bind<IFoodIconManager>().To<FoodIconManager>().FromInstance(_foodIconManager).AsSingle(); //TODO rework later

            Container.Bind<WorkplaceManager>().FromInstance(_workplaceManager).AsSingle();

            Container.Bind<IFoodPriceManager>().To<FoodPriceManager>().AsSingle();
            Container.Bind<FoodFactory>().ToSelf().AsSingle();
            
            Container.Bind<FoodStorageManager>().ToSelf().AsSingle().NonLazy();
        }
    }
}
