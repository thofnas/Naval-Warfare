using System;
using EventBus;
using Events;
using Ship;
using Themes;
using UnityEngine;
using UnityEngine.Serialization;
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
            Container.BindInstance(_selectedThemeSettings.PlayerTheme);
            Container.BindInstance(_shipVisualSetting);
        }
    }
}

[Serializable]
public class SelectedThemeSettings
{
    [FormerlySerializedAs("_playerThemeSettings")] [SerializeField] private Theme _playerTheme;
    
    public Theme PlayerTheme
    {
        get => _playerTheme;
        set
        {
            _playerTheme = value;
            EventBus<OnThemeChanged>.Invoke(new OnThemeChanged());
        }
    }
}