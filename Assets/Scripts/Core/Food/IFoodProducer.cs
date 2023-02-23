using System.Collections.Generic;

namespace Core.Food
{
    public interface IFoodProducer
    {
        Food ProduceFood(FoodType foodType, List<FoodBonus> bonuses);
    }
}

