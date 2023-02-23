using System.Collections.Generic;
using UnityEngine;

namespace Core.Food
{
    public enum FoodType
    {
        //TODO implement

        Kebab,
        Soup,
        Tea,
        Coffee,
    }

    [CreateAssetMenu(menuName = "Food/Food", fileName = "New Food")]
    public class Food : ScriptableObject
    {
        public FoodType FoodType;
        public int Price;
        public List<FoodBonus> FoodBonusList;
    }
}
