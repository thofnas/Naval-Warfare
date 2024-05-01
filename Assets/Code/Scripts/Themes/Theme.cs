using Grid;
using UnityEngine;
using Utilities;
using Utilities.Attributes;
using VInspector;

namespace Themes
{
    [CreateAssetMenu(fileName = nameof(Theme))]
    public class Theme : ScriptableObject
    {
        public Color MainColor;
        public Color OutlineColor;
        public Sprite[] BackgroundSprites;

        public Sprite[] GridCellSprites;
        public Color GridCellSpriteColor;

        [Label("Placing Color")] public Color GridCellSpritePlacingColor = Color.white;
        [Label("Destroyed Ship Color")] public Color GridCellSpriteDestroyedShipColor = Color.red;

        public SerializedDictionary<CellIcon, Sprite> GridCellIcons;

        private void OnValidate()
        {
            Validation.CheckColor(this, MainColor, nameof(MainColor));
            Validation.CheckColor(this, OutlineColor, nameof(OutlineColor));
            Validation.CheckIfNull(this, BackgroundSprites, nameof(BackgroundSprites));
            Validation.CheckColor(this, GridCellSpriteColor, nameof(GridCellSpriteColor));
            Validation.CheckColor(this, GridCellSpritePlacingColor, nameof(GridCellSpritePlacingColor));
            Validation.CheckColor(this, GridCellSpriteDestroyedShipColor, nameof(GridCellSpriteDestroyedShipColor));
            Validation.CheckIfEmpty(this, GridCellIcons, nameof(GridCellIcons));
        }
    }
}