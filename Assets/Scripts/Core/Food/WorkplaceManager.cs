using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Food
{
    public class WorkplaceManager : MonoBehaviour
    {
        [SerializeField] private List<BaseWorkplace> _workplaces;

        [Inject] private FoodStorageManager _foodStorageManager;

        public void AddWorkplace(BaseWorkplace workplace)
        {
            _workplaces.Add(workplace);
        }

        public void RemoveWorkplace(BaseWorkplace workplace)
        {
            _workplaces.Remove(workplace);
        }

        public void OnFoodProduced(Food food)
        {
            //TODO add to food temp storage after playing animations
            _foodStorageManager.AddFood(food);
            Debug.Log($"Produced: {food.FoodType}");
        }
    }
}