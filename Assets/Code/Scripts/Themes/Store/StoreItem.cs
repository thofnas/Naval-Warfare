using System;
using Unity.Collections;
using UnityEngine;
using Utilities;

namespace Themes.Store
{
    public abstract class StoreItem : ScriptableObject
    {
        [field: ReadOnly] public static Guid ID { get; private set; } = new();
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ThemeSettings ThemeSettings { get; private set; }
        [field: SerializeField] public bool IsPurchasable { get; private set; } = true;
        [field: SerializeField, Range(0, 1000)] public int Price { get; private set; }

        private void OnValidate()
        {
            Validation.CheckForNull(this, Name, nameof(Name));
            Validation.CheckForNull(this, ThemeSettings, nameof(ThemeSettings));
        }
    }
}