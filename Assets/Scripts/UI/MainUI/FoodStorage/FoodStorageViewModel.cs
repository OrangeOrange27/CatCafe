using System.Collections.Generic;
using Core.Food;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.MainUI.FoodStorage
{
    public class FoodStorageViewModel : UIBehaviour
    {
        [SerializeField] public List<FoodStorageElementView> _foodViews;

        [Inject] private FoodStorageManager _foodStorageManager;

        protected override void Awake()
        {
            base.Awake();
            
            _foodStorageManager.OnFoodSold += OnFoodSold;
            _foodStorageManager.OnFoodAddedToStorage += OnFoodAddedToStorage;
        }

        private void OnFoodSold(Food food)
        {
            
        }
        
        private void OnFoodAddedToStorage(Food food)
        {
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _foodStorageManager.OnFoodSold -= OnFoodSold;
            _foodStorageManager.OnFoodAddedToStorage -= OnFoodAddedToStorage;
        }
    }
}
