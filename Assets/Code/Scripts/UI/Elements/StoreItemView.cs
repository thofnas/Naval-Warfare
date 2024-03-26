using System;
using Themes.Store;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.Elements
{
    public class StoreItemView : VisualElement
    {
        public readonly Image BackgroundImage;
        public readonly Label PriceLabel;
        public readonly Image LockImage;
        public readonly Image SelectedImage;
        public readonly Image PriceCoin;
        
        public StoreItem StoreItem { get; private set; }
        public bool IsLocked { get; private set; }
        
        public event Action<StoreItemView> Clicked;

        private StoreItemView()
        {
            this.AddClass("store-item");

            BackgroundImage = this.CreateChild<Image>("item-background-image");
            
            VisualElement priceContainer = this.CreateChild("item-price");
            PriceCoin = priceContainer.CreateChild<Image>("item-price-coin");
            PriceLabel = priceContainer.CreateChild<Label>("item-price-text");
            
            LockImage = this.CreateChild<Image>("item-lock-image");
            SelectedImage = this.CreateChild<Image>("item-selected-image");
            
            Lock();
            Deselect();
        }
        
        public void Lock()
        {
            IsLocked = true;
            LockImage.visible = IsLocked;
            PriceLabel.visible = true;
            PriceCoin.visible = true;
        }

        public void Unlock()
        {
            IsLocked = false;
            LockImage.visible = IsLocked;
            PriceLabel.visible = false;
            PriceCoin.visible = false;
        }

        public void Select()
        {
            SelectedImage.visible = true;
        }

        public void Deselect()
        {
            SelectedImage.visible = false;
        }

        public static class Factory
        {
            public static StoreItemView Create(StoreItem storeItem, VisualElement parentContainer)
            {
                StoreItemView instance = new();
                instance.AddManipulator(new Clickable(_ => instance.Clicked?.Invoke(instance)));
                instance.StoreItem = storeItem;
                
                SetStyles(instance, storeItem);

                parentContainer.Add(instance);
                
                return instance;
            }

            private static void SetStyles(StoreItemView storeItemView, StoreItem storeItem)
            {
                storeItemView.BackgroundImage.image = storeItem.Theme.BackgroundSprites[0].texture;
                storeItemView.BackgroundImage.scaleMode = ScaleMode.ScaleAndCrop;
                storeItemView.PriceLabel.text = storeItem.Price > 0 ? storeItem.Price.ToString() : null;
            }
        }
    }
}