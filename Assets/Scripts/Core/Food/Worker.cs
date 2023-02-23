using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Food
{
    public class Worker : MonoBehaviour, ILevelBased
    {
        [SerializeField] private List<FoodBonus> _foodBonusList;

        [SerializeField]
        private int _level = 0;
        public event Action<int> OnLevelUpgraded;

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
    }
}
