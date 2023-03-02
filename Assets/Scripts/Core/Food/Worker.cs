using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Food
{
    public class Worker : MonoBehaviour, ILevelBased
    {
        [SerializeField] private List<FoodBonus> _foodBonusList;
        [SerializeField] private int _level = 0;
        [SerializeField] private Animator _workerAnimator;
        
        public bool CanWork { get; private set; }

        public bool IsWorking
        {
            get => _isWorking;
            private set
            {
                if(_isWorking == value)
                    return;

                _workerAnimator.SetTrigger(value
                    ? GlobalConstants.DEBUG_CHARACTER_COOK_ANIMATION_NAME
                    : GlobalConstants.DEBUG_CHARACTER_IDLE_ANIMATION_NAME);
                
                _isWorking = value;
            }
        }

        public List<FoodBonus> FoodBonuses => _foodBonusList;
        public event Action<int> OnLevelUpgraded;

        private bool _isWorking;

        private void Awake()
        {
            CanWork = true;
        }

        public void StartWorking()
            => IsWorking = true;
        public void StopWorking()
            => IsWorking = false;

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
