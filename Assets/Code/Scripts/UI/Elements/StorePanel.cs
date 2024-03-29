using System.Collections.Generic;
using Data;
using EventBus;
using Events;
using Themes;
using Themes.Store;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;
using Zenject;

namespace UI.Elements
{
    public class StorePanel : VisualElement
    {
        private readonly SelectedThemeSettings _selectedThemeSettings;
        private readonly LocalDataProvider _localDataProvider;
        private readonly ThemeSelector _themeSelector;
        private readonly ThemeUnlocker _themeUnlocker;
        private readonly OwnedThemesChecker _ownedThemesChecker;
        private readonly SelectedThemeChecker _selectedThemeChecker;
        private readonly Wallet _wallet;
        private readonly List<StoreItemView> _storeItemViews = new();

        public StorePanel(IEnumerable<StoreItem> storeItems, 
            SelectedThemeSettings selectedThemeSettings,
            LocalDataProvider localDataProvider,
            ThemeSelector themeSelector, 
            ThemeUnlocker themeUnlocker,
            OwnedThemesChecker ownedThemesChecker, 
            SelectedThemeChecker selectedThemeChecker, 
            Wallet wallet)
        {
            _selectedThemeSettings = selectedThemeSettings;
            _localDataProvider = localDataProvider;
            _themeSelector = themeSelector;
            _themeUnlocker = themeUnlocker;
            _ownedThemesChecker = ownedThemesChecker;
            _selectedThemeChecker = selectedThemeChecker;
            _wallet = wallet;
            
            Clear();
            this.AddClass("store-panel");
            
            foreach (StoreItem storeItem in storeItems)
            {
                StoreItemView storeItemView = StoreItemView.Factory.Create(storeItem, this);
                
                storeItemView.Clicked += StoreItemView_OnClicked;
                
                ownedThemesChecker.Visit(storeItemView.StoreItem);

                if (ownedThemesChecker.IsOwned)
                {
                    selectedThemeChecker.Visit(storeItemView.StoreItem);

                    if (selectedThemeChecker.IsSelected)
                    {
                        _themeSelector.Visit(storeItemView.StoreItem);
                        storeItemView.Select();
                    }
                    else
                        storeItemView.Deselect();

                    storeItemView.Unlock();
                }
                else
                {
                    if (!storeItem.IsPurchasable)
                    {
                        Remove(storeItemView);
                        return;
                    }
                    
                    storeItemView.Lock();
                }

                _storeItemViews.Add(storeItemView);
            }
        }
        
        private void SelectTheme(StoreItemView storeItemView)
        {
            _themeSelector.Visit(storeItemView.StoreItem);
            storeItemView.Select();
            _localDataProvider.Save();
            _selectedThemeSettings.PlayerTheme = storeItemView.StoreItem.Theme;
        }

        private void StoreItemView_OnClicked(StoreItemView storeItemView)
        {
            _selectedThemeChecker.Visit(storeItemView.StoreItem);

            if (_selectedThemeChecker.IsSelected)
            {
                return;
            }

            _ownedThemesChecker.Visit(storeItemView.StoreItem);
            
            if (_ownedThemesChecker.IsOwned)
            {
                SelectTheme(storeItemView);
                return;
            }

            if (_wallet.IsEnough(storeItemView.StoreItem.Price))
            {
                _wallet.SpendMoney(storeItemView.StoreItem.Price);
                
                _themeUnlocker.Visit(storeItemView.StoreItem);

                SelectTheme(storeItemView);
                
                storeItemView.Unlock();
            }
        }

        public class Factory : PlaceholderFactory<IEnumerable<StoreItem>, StorePanel>
        {
        }
    }
}