using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Food
{
    public abstract class BaseWorkplace : MonoBehaviour, IFoodProducer, ILevelBased
    {
        [SerializeField] private FoodType _producingFoodType;
        [SerializeField] private float _productionTimeInSeconds;
        [SerializeField] private Worker _worker;
        [SerializeField] private int _level = 0;

        public event Action<int> OnLevelUpgraded;
        public event Action<Worker> OnWorkerChanged;

        private WorkplaceManager _workplaceManager;
        private FoodFactory _foodFactory;
        private bool _isProducingFood;

        private void FixedUpdate()
        {
            if (!_isProducingFood)
            {
                if (TryStartProducingFood() == false)
                {
                    Debug.LogError("Could not start producing food");
                }
            }
        }

        public async UniTask<Food> ProduceFood(FoodType foodType, List<FoodBonus> bonuses)
        {
            var food = _foodFactory.Create(new FoodData(foodType, bonuses));

            await UniTask.Delay(TimeSpan.FromSeconds(_productionTimeInSeconds));

            OnFoodProduced(food);
            _isProducingFood = false;
            
            return food;
        }

        public void UpgradeLevel()
        {
            _level++;
            OnLevelUpgraded?.Invoke(1);
        }

        public void UpgradeLevel(int amount)
        {
            _level += amount;
            OnLevelUpgraded?.Invoke(amount);
        }

        public void ChangeWorker(Worker worker)
        {
            if (worker == null)
                return;

            _worker = worker;
            OnWorkerChanged?.Invoke(_worker);
        }

        private bool TryStartProducingFood()
        {
            if (_worker == null || !_worker.CanWork)
            {
                return false;
            }

            _isProducingFood = true;
            ProduceFood(_producingFoodType, _worker.FoodBonuses).Forget();

            return true;
        }

        private void OnFoodProduced(Food food)
        {
            _workplaceManager.OnFoodProduced(food);
        }
    }
}