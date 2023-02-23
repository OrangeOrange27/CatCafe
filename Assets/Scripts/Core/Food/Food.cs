using System.Collections.Generic;

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

    public struct Food
    {
        public FoodType FoodType;
        public int Price;
        public List<FoodBonus> FoodBonusList;
    }
}
