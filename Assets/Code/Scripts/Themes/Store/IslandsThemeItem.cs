using UnityEngine;

namespace Themes.Store
{
    [CreateAssetMenu(fileName = nameof(IslandsThemeItem), menuName = "Store/" + nameof(IslandsThemeItem))]
    public class IslandsThemeItem : StoreItem
    {
        [field: SerializeField] public IslandsThemeType IslandsType { get; private set; }
    }
}