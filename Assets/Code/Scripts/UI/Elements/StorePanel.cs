using System.Collections.Generic;
using Themes.Store;
using UnityEngine.UIElements;
using Utilities.Extensions;
using Zenject;

namespace UI.Elements
{
    public class StorePanel : VisualElement
    {
        private List<StoreItemView> _storeItemViews = new();
        
        public StorePanel(IEnumerable<StoreItem> storeItems, SelectedThemeSettings selectedThemeSettings)
        {
            Clear();
            this.AddClass("store-panel");
            
            foreach (StoreItem storeItem in storeItems)
            {
                StoreItemView storeItemView = StoreItemView.Factory.Create(storeItem, this);
                _storeItemViews.Add(storeItemView);
                
                storeItemView.Clicked += view =>
                {
                    selectedThemeSettings.PlayerTheme = view.StoreItem.Theme;
                };
                
                if (storeItem.Theme == selectedThemeSettings.PlayerTheme)
                {
                    storeItemView.Unlock();
                    storeItemView.Select();
                }
            }
        }

        public class Factory : PlaceholderFactory<IEnumerable<StoreItem>, StorePanel>
        {
        }
    }
}