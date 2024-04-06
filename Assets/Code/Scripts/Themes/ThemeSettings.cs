using System;
using Grid;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Utilities.Attributes;
using VInspector;

namespace Themes
{
    public abstract class ThemeSettings : ScriptableObject
    {
        public Color MainColor;
        public Color OutlineColor;
        public Sprite[] BackgroundSprites;
        public Font BaseFont;

        public Sprite[] GridCellSprites;
        public Color GridCellSpriteColor = Color.white;

        [Label("Placing Color")] public Color GridCellSpritePlacingColor = Color.white;
        [Label("Destroyed Ship Color")] public Color GridCellSpriteDestroyedShipColor = Color.red;

        public SerializedDictionary<CellIcon, Sprite> GridCellIcons;

        protected virtual void OnValidate()
        {
            Validation.CheckColor(this, MainColor, nameof(MainColor));
            Validation.CheckColor(this, OutlineColor, nameof(OutlineColor));
            Validation.CheckForNull(this, BackgroundSprites, nameof(BackgroundSprites));
            Validation.CheckForNull(this, BaseFont, nameof(BaseFont));
            Validation.CheckIfEmpty(this, GridCellIcons, nameof(GridCellIcons));
        }
    }
}