using System;
using UnityEngine;

namespace Core.Food
{
    public class FoodIconManager : MonoBehaviour, IFoodIconManager
    {
        [SerializeField] private Sprite _defaultFoodIcon;
        [SerializeField] private Sprite[] _foodIcons;

        public void SetDefaultIconToFood(Food food)
        {
            food.SetIcon(_defaultFoodIcon);
        }

        public void SetIconToFood(Food food)
        {
            try
            {
                food.SetIcon(_foodIcons[(int)food.FoodType]);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                SetDefaultIconToFood(food);
            }
        }
    }
}
