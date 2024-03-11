using Ship;
using Themes;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    [CreateAssetMenu(fileName = "ThemeSettingInstaller", menuName = "Installers/ThemeSettingsInstaller")]
    public class ThemeSettingsInstaller : ScriptableObjectInstaller<ThemeSettingsInstaller>
    {
        [SerializeField] private ThemeSettings _playerThemeSettings;
        [SerializeField] private ShipVisual.Settings _shipVisualSetting;

        public override void InstallBindings()
        {
            Container.BindInstance(_playerThemeSettings);
            Container.BindInstance(_shipVisualSetting);
        }
    }
}