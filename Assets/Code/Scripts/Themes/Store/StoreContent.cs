using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Themes.Store
{
    [CreateAssetMenu(fileName = nameof(StoreContent), menuName = "Store/" + nameof(StoreContent))]
    public class StoreContent : ScriptableObject
    {
        public IReadOnlyList<IslandsThemeItem> IslandsThemeItems => _islandsThemeItems;
        
        [SerializeField] private List<IslandsThemeItem> _islandsThemeItems;

        private void OnValidate()
        {
            Validation.CheckForDuplicatesByProperty(_islandsThemeItems, item => item.IslandsType, nameof(_islandsThemeItems));
        }
    }
}