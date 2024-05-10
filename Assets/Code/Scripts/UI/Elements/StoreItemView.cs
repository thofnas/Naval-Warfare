using System;
using EventBus;
using Events;
using Map;
using Themes;
using Themes.Store;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StoreItemView : VisualElement
    {
        private static readonly Color s_notEnoughMoneyColor = new(1f,0.5f,0.4f);

        private readonly Image _backgroundImage;
        private readonly VisualElement _priceWrapper;
        private readonly Image _lockImage;
        private readonly Image _selectedImage;
        private readonly Label _priceLabel;

        private bool _isLocked;
        
        public StoreItem StoreItem { get; private set; }

        private StoreItemView(Theme theme)
        {
            this.AddClass("store-item");
            
            VisualElement imageWrapper = this.CreateChild("item-image-wrapper");
            _backgroundImage = imageWrapper.CreateChild<Image>("item-image");
            
            StyledPanel styledPanel = new(theme, false);
            Add(styledPanel);
            
            _priceWrapper = this.CreateChild("item-price-wrapper");
            VisualElement priceContainer = _priceWrapper.CreateChild("item-price");
            Image priceCoin = priceContainer.CreateChild<Image>("item-price-coin");
            _priceLabel = priceContainer.CreateChild<Label>("item-price-text");
            
            _lockImage = this.CreateChild<Image>("item-lock-image");
            _selectedImage = this.CreateChild<Image>("item-selected-image");
            
            Lock();
            Deselect();
        }

        public void Lock()
        {
            _isLocked = true;
            _lockImage.visible = _isLocked;
            _priceWrapper.visible = true;
        }

        public void Unlock()
        {
            _isLocked = false;
            _lockImage.visible = _isLocked;       
            _priceWrapper.visible = false;
        }

        public void Select()
        {
            _backgroundImage.style.opacity = 1f;
            _selectedImage.visible = true;
        }

        public void Deselect()
        {
            _backgroundImage.style.opacity = 0.8f;
            _selectedImage.visible = false;
        }
        
        public void SetNotEnoughMoneyStyles()
        {
            _priceLabel.style.color = s_notEnoughMoneyColor;
        }

        public void AnimateNotEnoughMoney()
        {
            const int durationMs = 100;
            
            StyleValues blinkLabelStyles = new()
            {
                color = s_notEnoughMoneyColor.gamma.gamma
            };
            
            StyleValues originalLabelStyles = new()
            {
                color = s_notEnoughMoneyColor
            };

            _priceLabel.experimental.animation.Start(blinkLabelStyles, durationMs).onAnimationCompleted += () =>
                _priceLabel.experimental.animation.Start(originalLabelStyles, durationMs).onAnimationCompleted += () => 
                    _priceLabel.experimental.animation.Start(blinkLabelStyles, durationMs).onAnimationCompleted += () =>
                        _priceLabel.experimental.animation.Start(originalLabelStyles, durationMs);
        }

        public static class Factory
        {
            public static StoreItemView Create(StoreItem storeItem, MapType mapType, VisualElement parentContainer, Action<StoreItemView> onClick)
            {
                StoreItemView instance = new(storeItem.Theme);
                instance.AddManipulator(new Clickable(_ => onClick?.Invoke(instance)));
                
                instance.StoreItem = storeItem;
                
                SetStyles(instance, storeItem);

                parentContainer.Add(instance);
                
                return instance;
            }

            private static void SetStyles(StoreItemView storeItemView, StoreItem storeItem)
            {
                storeItemView._backgroundImage.image = storeItem.Theme.BackgroundSprites[0].texture;
                storeItemView._backgroundImage.scaleMode = ScaleMode.ScaleAndCrop;
                storeItemView._priceLabel.text = storeItem.Price > 0 ? storeItem.Price.ToString() : null;
            }
        }
    }
}