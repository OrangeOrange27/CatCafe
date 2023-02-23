using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Food
{
    public abstract class BaseWorkplace : MonoBehaviour, IFoodProducer, ILevelBased
    {
        [SerializeField] private List<FoodType> _producingFoodTypes;
        [SerializeField] private float _speed;
        [SerializeField] private Worker _worker;

        private int _level = 0;

        public event Action<int> OnLevelUpgraded;
        public event Action<Worker> OnWorkerChanged;
        

        public Food ProduceFood(FoodType foodType, List<FoodBonus> bonuses)
        {
            //TODO implementation
            throw new System.NotImplementedException();
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
    }
}