using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MainUI.FoodStorage
{
    public class FoodStorageElementView : UIBehaviour, IDisposable
    {
        [SerializeField] private Image _foodIcon;
        [SerializeField] private Button _sellButton;
        
        private FoodStorageViewModel _viewModel;

        public void Init(FoodStorageViewModel viewModel,Sprite foodIcon)
        {
            _viewModel = viewModel;
            
            _foodIcon.sprite = foodIcon;
            
            _sellButton.onClick.AddListener(SellButtonClicked);
            _sellButton.interactable = true;
        }

        private void SellButtonClicked()
        {
            _viewModel.OnSellButtonClicked(this);
        }

        protected override void OnDestroy()
        {
            Dispose();
            base.OnDestroy();
        }

        public void Dispose()
        {
            _sellButton.onClick.RemoveListener(SellButtonClicked);
        }
    }
}
