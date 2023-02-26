using System.Collections.Generic;
using UnityEngine;

namespace Core.Food
{
    public enum FoodType
    {
        Kebab,
        Soup,
        Tea,
        Coffee,
    }

    public struct FoodData
    {
        public readonly FoodType FoodType;
        public readonly List<FoodBonus> FoodBonusList;

        public FoodData(FoodType type, List<FoodBonus> foodBonusList)
        {
            FoodType = type;
            FoodBonusList = foodBonusList;
        }
    }
    
    public class Food
    {
        public FoodType FoodType { get; private set; }
        public int Price { get; private set; }
        public List<FoodBonus> FoodBonusList { get; private set; }
        public Sprite FoodIcon { get; private set; }

        public Food(FoodType type, int price, List<FoodBonus> foodBonusList)
        {
            FoodType = type;
            Price = price;
            FoodBonusList = foodBonusList;
        }

        public void SetIcon(Sprite icon)
        {
            FoodIcon = icon;
        }
    }
}
