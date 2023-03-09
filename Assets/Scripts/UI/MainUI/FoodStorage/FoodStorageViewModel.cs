using System;
using System.Collections.Generic;
using Core.Food;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.MainUI.FoodStorage
{
    public class FoodStorageViewModel : UIBehaviour, IDisposable
    {
        [SerializeField] private RectTransform _foodStorageContent;
        [SerializeField] private FoodStorageElementView _foodStorageElementPrefab;

        private List<FoodStorageElementView> _foodViews;

        [Inject] private FoodStorageManager _foodStorageManager;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            _foodViews = new List<FoodStorageElementView>();

            _foodStorageManager.OnFoodSold += SellFood;
            _foodStorageManager.OnFoodAddedToStorage += AddFoodToStorage;
        }

        private void SellFood(Food foodToSell, int index)
        {
            var view = _foodViews[index];
            
            view.Dispose();
            _foodViews.Remove(_foodViews[index]);
            Destroy(view.gameObject);
        }

        private void AddFoodToStorage(Food food)
        {
            var view = Instantiate(_foodStorageElementPrefab, _foodStorageContent);
            view.Init(this, food.FoodIcon);
            _foodViews.Add(view);
        }

        public void OnSellButtonClicked(FoodStorageElementView view)
            => _foodStorageManager.SellFood(_foodViews.IndexOf(view));

        protected override void OnDestroy()
        {
            Dispose();
            base.OnDestroy();
        }

        public void Dispose()
        {
            _foodStorageManager.OnFoodSold -= SellFood;
            _foodStorageManager.OnFoodAddedToStorage -= AddFoodToStorage;
        }
    }
}