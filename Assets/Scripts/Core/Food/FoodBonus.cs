using UnityEngine;

namespace Core.Food
{
    public enum FoodBonusType
    {
        PriceFromWorkplaceLevel,
        PriceFromWorkerLevel,
        AdditionalProfit,
        AdditionalSpeed,
    }
    
    [CreateAssetMenu(menuName = "Food/FoodBonus", fileName = "New FoodBonus")]
    public class FoodBonus : ScriptableObject
    {
        public FoodBonusType FoodBonusType;
    }
}


    