using System;
using System.Collections.Generic;
using Zenject;

namespace Core.Food
{
    public class FoodStorageManager
    {
        public event Action<Food, int> OnFoodSold;
        public event Action<Food> OnFoodAddedToStorage;
        public int ItemsInStorage => _foodList.Count;

        private readonly List<Food> _foodList;

        [Inject]
        public FoodStorageManager()
        {
            _foodList = new List<Food>();
        }

        public void AddFood(Food food)
        {
            if (_foodList.Count >= GlobalConstants.FOOD_STORAGE_MAX_SIZE)
            {
                SellFood(0);
            }

            _foodList.Add(food);
            OnFoodAddedToStorage?.Invoke(food);
        }

        public void SellFood(int index)
        {
            var foodToSell = _foodList[index];
            _foodList.Remove(foodToSell);
            OnFoodSold?.Invoke(foodToSell, index);
        }
    }
}
