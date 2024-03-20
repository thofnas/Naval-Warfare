using System.Collections.Generic;
using Themes;
using Themes.Store;
using UnityEngine.UIElements;
using Utilities.Extensions;
using Zenject;

namespace UI.Elements
{
    public class StorePanel : VisualElement
    {
        private List<StoreItemView> _storeItemViews = new();

        private OwnedThemesChecker _ownedThemesChecker;
        private SelectedThemeChecker _selectedThemeChecker;
        
        public StorePanel(IEnumerable<StoreItem> storeItems, SelectedThemeSettings selectedThemeSettings)
        {
            Clear();
            this.AddClass("store-panel");
            
            foreach (StoreItem storeItem in storeItems)
            {
                StoreItemView storeItemView = StoreItemView.Factory.Create(storeItem, this);
                
                storeItemView.Clicked += view =>
                {
                    selectedThemeSettings.PlayerTheme = view.StoreItem.Theme;
                };
                
                _ownedThemesChecker.Visit(storeItemView.StoreItem);

                if (!_ownedThemesChecker.IsOwned)
                    storeItemView.Lock();
                else
                {
                    _selectedThemeChecker.Visit(storeItemView.StoreItem);

                    if (_selectedThemeChecker.IsSelected)
                        storeItemView.Select();
                    else
                        storeItemView.Deselect();

                    storeItemView.Unlock();
                }

                _storeItemViews.Add(storeItemView);
            }
        }

        public class Factory : PlaceholderFactory<IEnumerable<StoreItem>, StorePanel>
        {
        }
    }
}