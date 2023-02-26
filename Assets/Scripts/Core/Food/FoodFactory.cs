using Zenject;

namespace Core.Food
{
    public class FoodFactory : IFactory<FoodData,Food>
    {
        [Inject]
        private IFoodPriceManager _foodPriceManager;
        [Inject]
        private IFoodIconManager _foodIconManager;
        
        public Food Create(FoodData foodData)
        {
            var food = new Food(foodData.FoodType, _foodPriceManager.CalculatePrice(foodData), foodData.FoodBonusList);
            
            _foodIconManager.SetIconToFood(food);
            
            return food;
        }
    }
}
