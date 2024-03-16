using System;
using EventBus;
using Events;
using Ship;
using Themes;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    [CreateAssetMenu(fileName = "ThemeSettingInstaller", menuName = "Installers/ThemeSettingsInstaller")]
    public class ThemeSettingsInstaller : ScriptableObjectInstaller<ThemeSettingsInstaller>
    {
        [SerializeField] private SelectedThemeSettings _selectedThemeSettings;
        [SerializeField] private ShipVisual.Settings _shipVisualSetting;

        public override void InstallBindings()
        {
            Container.BindInstance(_selectedThemeSettings);
            Container.BindInstance(_selectedThemeSettings.PlayerThemeSettings);
            Container.BindInstance(_shipVisualSetting);
        }
    }
}

[Serializable]
public class SelectedThemeSettings
{
    [SerializeField] private ThemeSettings _playerThemeSettings;
    
    public ThemeSettings PlayerThemeSettings
    {
        get => _playerThemeSettings;
        set
        {
            _playerThemeSettings = value;
            EventBus<OnThemeChanged>.Invoke(new OnThemeChanged());
        }
    }
}