using System;
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
        [SerializeField] private ShipVisual.Settings _shipVisualSetting;

        public override void InstallBindings()
        {
            Container.BindInstance(_shipVisualSetting);
        }
    }
}