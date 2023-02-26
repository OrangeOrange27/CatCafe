using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Core.Food
{
    public interface IFoodProducer
    {
        UniTask<Food> ProduceFood(FoodType foodType, List<FoodBonus> bonuses);
    }
}

