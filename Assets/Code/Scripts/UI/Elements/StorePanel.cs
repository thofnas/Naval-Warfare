using System.Collections.Generic;
using EventBus;
using Events;
using Themes;
using Themes.Store;
using UnityEngine.UIElements;
using Utilities.Extensions;
using Zenject;

namespace UI.Elements
{
    public class StorePanel : VisualElement
    {
        private readonly SelectedThemeSettings _selectedThemeSettings;
        private readonly List<StoreItemView> _storeItemViews = new();

        public StorePanel(IEnumerable<StoreItem> storeItems, SelectedThemeSettings selectedThemeSettings, ThemeSelector themeSelector, ThemeUnlocker themeUnlocker, OwnedThemesChecker ownedThemesChecker, SelectedThemeChecker selectedThemeChecker)
        {
            _selectedThemeSettings = selectedThemeSettings;
            Clear();
            this.AddClass("store-panel");
            
            foreach (StoreItem storeItem in storeItems)
            {
                StoreItemView storeItemView = StoreItemView.Factory.Create(storeItem, this);
                
                storeItemView.Clicked += StoreItemView_OnClicked;
                
                ownedThemesChecker.Visit(storeItemView.StoreItem);

                if (!ownedThemesChecker.IsOwned)
                    storeItemView.Lock();
                else
                {
                    selectedThemeChecker.Visit(storeItemView.StoreItem);

                    if (selectedThemeChecker.IsSelected)
                        storeItemView.Select();
                    else
                        storeItemView.Deselect();

                    storeItemView.Unlock();
                }

                _storeItemViews.Add(storeItemView);
            }
        }

        private void StoreItemView_OnClicked(StoreItemView storeItemView)
        {
            _selectedThemeSettings.PlayerTheme = storeItemView.StoreItem.Theme;
            EventBus<OnStoreItemViewClicked>.Invoke(new OnStoreItemViewClicked(storeItemView));
        }

        public class Factory : PlaceholderFactory<IEnumerable<StoreItem>, StorePanel>
        {
        }
    }
}