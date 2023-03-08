using System;
using System.Collections.Generic;
using Zenject;

namespace Core.Food
{
    public class FoodStorageManager
    {
        public event Action<Food> OnFoodSold;
        public event Action<Food> OnFoodAddedToStorage;
        
        private readonly Queue<Food> _foodQueue;

        [Inject]
        public FoodStorageManager()
        {
            _foodQueue = new Queue<Food>();
        }

        public void AddFood(Food food)
        {
            _foodQueue.Enqueue(food);
            
            OnFoodAddedToStorage?.Invoke(food);
            
            if (_foodQueue.Count >= GlobalConstants.FOOD_QUEUE_MAX_SIZE)
            {
                SellFood();
            }
        }

        public void SellFood()
        {
            OnFoodSold?.Invoke(_foodQueue.Dequeue());
        }
    }
}
