using Grid;
using UnityEngine;
using Utilities;
using Utilities.Attributes;
using VInspector;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(ThemeSettings))]
    public class ThemeSettings : ScriptableObject
    {
        public Color MainColor;
        public Color OutlineColor;
        public Sprite[] BackgroundSprites;
        public Font BaseFont;

        public Sprite[] GridCellSprites;
        public Color GridCellSpriteColor;

        [Label("Placing Color")] public Color GridCellSpritePlacingColor = Color.white;
        [Label("Destroyed Ship Color")] public Color GridCellSpriteDestroyedShipColor = Color.red;

        public SerializedDictionary<CellIcon, Sprite> GridCellIcons;

        private void OnValidate()
        {
            Validation.CheckColor(this, MainColor, nameof(MainColor));
            Validation.CheckColor(this, OutlineColor, nameof(OutlineColor));
            Validation.CheckForNull(this, BackgroundSprites, nameof(BackgroundSprites));
            Validation.CheckForNull(this, BaseFont, nameof(BaseFont));
            Validation.CheckColor(this, GridCellSpriteColor, nameof(GridCellSpriteColor));
            Validation.CheckColor(this, GridCellSpritePlacingColor, nameof(GridCellSpritePlacingColor));
            Validation.CheckColor(this, GridCellSpriteDestroyedShipColor, nameof(GridCellSpriteDestroyedShipColor));
            Validation.CheckIfEmpty(this, GridCellIcons, nameof(GridCellIcons));
        }
    }
}