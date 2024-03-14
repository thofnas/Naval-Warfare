using System;
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
        public StoreItem StoreItem { get; private set; }
        
        public event Action<StoreItemView> Clicked;

        public StoreItemView()
        {
            this.AddManipulator(new Clickable(_ => Clicked?.Invoke(this)));

            BackgroundImage = this.CreateChild<Image>("item-background-image");
            
            PriceLabel = this.CreateChild<Label>("item-price-text");
            LockImage = this.CreateChild<Image>("item-lock-image");
            SelectedImage = this.CreateChild<Image>("item-selected-image");
        }

        public StoreItemView Initialize(StoreItem storeItem)
        {
            StoreItem = storeItem;
            
            BackgroundImage.image = storeItem.ThemeSettings.BackgroundSprites[0].texture;
            PriceLabel.text = storeItem.Price > 0 ? storeItem.Price.ToString() : null;
            
            return this;
        }
    }
}