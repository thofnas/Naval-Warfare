using System;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace Themes.Store
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

        public StoreItemView()
        {
            this.AddClass("store-item");
            this.AddManipulator(new Clickable(_ => Clicked?.Invoke(this)));

            BackgroundImage = this.CreateChild<Image>("item-background-image");
            
            VisualElement priceContainer = this.CreateChild("item-price");
            PriceCoin = priceContainer.CreateChild<Image>("item-price-coin");
            PriceLabel = priceContainer.CreateChild<Label>("item-price-text");
            
            LockImage = this.CreateChild<Image>("item-lock-image");
            SelectedImage = this.CreateChild<Image>("item-selected-image");
        }

        public StoreItemView Initialize(StoreItem storeItem)
        {
            StoreItem = storeItem;
            
            BackgroundImage.image = storeItem.ThemeSettings.BackgroundSprites[0].texture;
            BackgroundImage.scaleMode = ScaleMode.ScaleAndCrop;
            PriceLabel.text = storeItem.Price > 0 ? storeItem.Price.ToString() : null;
            
            Lock();
            Deselect();
            
            return this;
        }

        public void Lock()
        {
            IsLocked = true;
            LockImage.visible = IsLocked;
            PriceLabel.visible = true;
        }

        public void Unlock()
        {
            IsLocked = false;
            LockImage.visible = IsLocked;
            PriceLabel.visible = false;
        }

        public void Select()
        {
            SelectedImage.visible = true;
        }

        public void Deselect()
        {
            SelectedImage.visible = false;
        }
    }
}