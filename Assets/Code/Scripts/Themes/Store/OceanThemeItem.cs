using UnityEngine;

namespace Themes.Store
{
    [CreateAssetMenu(fileName = nameof(OceanThemeItem), menuName = "Store/" + nameof(OceanThemeItem))]
    public class OceanThemeItem : StoreItem
    {
        [field: SerializeField] public OceanThemeType OceanType { get; private set; }
    }
}