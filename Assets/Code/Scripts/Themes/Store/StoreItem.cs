using System;
using Unity.Collections;
using UnityEngine;
using Utilities;

namespace Themes.Store
{
    public abstract class StoreItem : ScriptableObject
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Theme Theme { get; private set; }
        [field: SerializeField] public bool IsPurchasable { get; private set; } = true;
        [field: SerializeField, Range(0, 1000)] public int Price { get; private set; }

        private void OnValidate()
        {
            Validation.CheckForNull(this, DisplayName, nameof(DisplayName));
            Validation.CheckForNull(this, Theme, nameof(Theme));
        }
    }
}