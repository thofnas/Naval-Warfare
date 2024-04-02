using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Themes.Store
{
    [CreateAssetMenu(fileName = nameof(StoreContent), menuName = "Store/" + nameof(StoreContent))]
    public class StoreContent : ScriptableObject
    {
        public IEnumerable<IslandsThemeItem> IslandsThemeItems => _islandsThemeItems;
        public IEnumerable<OceanThemeItem> OceanThemeItems => _oceanThemeItems;
        
        [SerializeField] private List<IslandsThemeItem> _islandsThemeItems;
        [SerializeField] private List<OceanThemeItem> _oceanThemeItems;

        private void OnValidate()
        {
            Validation.CheckIfEmpty(this, _islandsThemeItems, nameof(_islandsThemeItems));
            Validation.CheckForDuplicatesByProperty(_islandsThemeItems, item => item.IslandsType, nameof(_islandsThemeItems));
            Validation.CheckIfEmpty(this, _oceanThemeItems, nameof(_oceanThemeItems));
            Validation.CheckForDuplicatesByProperty(_oceanThemeItems, item => item.OceanType, nameof(_oceanThemeItems));
        }
    }
}