using UnityEngine;

namespace Core.Food
{
    public enum FoodBonusType
    {
        //TODO implement
    }
    
    [CreateAssetMenu(menuName = "Food/FoodBonus", fileName = "New FoodBonus")]
    public class FoodBonus : ScriptableObject
    {
        public FoodBonusType FoodBonusType;
    }
}


    