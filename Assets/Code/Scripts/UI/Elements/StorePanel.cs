﻿using System.Collections.Generic;
using Data;
using Infrastructure;
using Map;
using Themes;
using Themes.Store;
using UnityEngine.UIElements;
using Utilities.Extensions;
using Zenject;

namespace UI.Elements
{
    public class StorePanel : StyledPanel
    {
        private readonly SelectedTheme _selectedTheme;
        private readonly ThemeSelector _themeSelector;
        private readonly ThemeUnlocker _themeUnlocker;
        private readonly OwnedThemesChecker _ownedThemesChecker;
        private readonly SelectedThemeChecker _selectedThemeChecker;
        private readonly Wallet _wallet;
        private readonly MapType _mapType;
        private readonly MapSelector _mapSelector;

        public StorePanel(IEnumerable<StoreItem> storeItems,
            MapType mapType,
            SelectedTheme selectedTheme,
            ThemeSelector themeSelector, 
            ThemeUnlocker themeUnlocker,
            OwnedThemesChecker ownedThemesChecker, 
            SelectedThemeChecker selectedThemeChecker, 
            Wallet wallet, 
            MapSelector mapSelector) : base(selectedTheme.PlayerTheme)
        {
            _selectedTheme = selectedTheme;
            _themeSelector = themeSelector;
            _themeUnlocker = themeUnlocker;
            _ownedThemesChecker = ownedThemesChecker;
            _selectedThemeChecker = selectedThemeChecker;
            _wallet = wallet;
            _mapSelector = mapSelector;
            _mapType = mapType;

            this.AddClass("store-panel");
            
            RenderStoreItems(storeItems);
        }

        private void RenderStoreItems(IEnumerable<StoreItem> storeItems)
        {
            Clear();
            
            VisualElement nameContainer = this.CreateChild("panel-name-container");
            ScrollView itemsContainer = this.CreateChild<ScrollView>("panel-items-container");

            itemsContainer.mode = ScrollViewMode.Horizontal;
            itemsContainer.elasticity = 1;
            itemsContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic;
            itemsContainer.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            itemsContainer.scrollDecelerationRate = 1;
            
            Label nameLabel = nameContainer.CreateChild<Label>();
            nameLabel.text = _mapType.ToString();
            
            foreach (StoreItem storeItem in storeItems)
            {
                _ownedThemesChecker.Visit(storeItem);
                
                if (!storeItem.IsPurchasable && !_ownedThemesChecker.IsOwned)
                {
                    continue;
                }
                
                StoreItemView storeItemView = StoreItemView.Factory.Create(storeItem, _mapType, itemsContainer, StoreItemView_OnClicked);

                if (_ownedThemesChecker.IsOwned)
                {
                    if (IsStoreItemSelected(storeItem))
                    {
                        _themeSelector.Visit(storeItem);
                        storeItemView.Select();
                    }
                    else
                        storeItemView.Deselect();

                    storeItemView.Unlock();
                }
                else
                {
                    if (!_wallet.IsEnough(storeItem.Price))
                    {
                        storeItemView.SetNotEnoughMoneyStyles();
                    }
                    
                    storeItemView.Lock();
                }
            } 
        }

        private bool IsStoreItemSelected(StoreItem storeItem)
        {
            _selectedThemeChecker.Visit(storeItem);
            return _selectedThemeChecker.IsSelected;
        }

        private void SelectTheme(StoreItemView storeItemView)
        {
            _themeSelector.Visit(storeItemView.StoreItem);
            storeItemView.Select();
            _mapSelector.Select(_mapType);
            _selectedTheme.PlayerTheme = storeItemView.StoreItem.Theme;
        }

        private void StoreItemView_OnClicked(StoreItemView storeItemView)
        {
            _selectedThemeChecker.Visit(storeItemView.StoreItem);

            if (_selectedThemeChecker.IsSelected)
            {
                SelectTheme(storeItemView);
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
            else
            {
                storeItemView.AnimateNotEnoughMoney();
            }
        }

        public class Factory : PlaceholderFactory<IEnumerable<StoreItem>, MapType, StorePanel>
        {
        }
    }
}