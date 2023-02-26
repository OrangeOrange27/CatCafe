using System.Collections.Generic;
using UnityEngine;

namespace Core.Food
{
    public class WorkplaceManager : MonoBehaviour
    {
        [SerializeField] 
        private List<BaseWorkplace> _workplaces;

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
        }
    }
}