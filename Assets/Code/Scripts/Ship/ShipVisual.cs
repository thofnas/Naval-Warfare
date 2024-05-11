using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventBus;
using Events;
using Grid;
using UnityEngine;
using Utilities;
using Utilities.Attributes.SelfAndChildren;
using Utilities.Extensions;
using VInspector;
using Zenject;

namespace Ship
{
    [RequireComponent(typeof(Ship))]
    [DisallowMultipleComponent]
    public class ShipVisual : MonoBehaviour
    {
        private static readonly int s_flashAmount = Shader.PropertyToID("_FlashAmount");

        [SerializeField] [SelfAndChildren] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _spriteOutline;
        [SerializeField] private Material _spriteFlash;
        private LevelManager _levelManager;
        private EventBinding<OnCellHit> _onCellHit;
        private SpriteRenderer _placementPreviewRenderer;
        private List<CellPosition> _previewCellPositions = new();
        private Settings _settings;
        private Ship _ship;
        private Settings.ShipVisualData _shipVisualData;

        private Vector3 _spriteRendererOffset;

        private void OnDisable() => EventBus<OnCellHit>.Deregister(_onCellHit);

        private void OnMouseDown()
        {
            _placementPreviewRenderer.transform.SetParent(_ship.transform.parent);

            if (!_ship.CanDragAndPlace()) return;

            UpdateSprite();
            UpdatePlacementSprite();

            _placementPreviewRenderer.transform.position = _ship.transform.position;
            _placementPreviewRenderer.gameObject.SetActive(true);
        }

        private void OnMouseDrag()
        {
            if (!_ship.CanDragAndPlace()) return;

            if (!_levelManager.TryGetValidGridCellPositions(_ship.GetCharacterType(), transform.position, _ship,
                    out List<CellPosition> cellPositions))
                return;
            
            if (cellPositions.SequenceEqual(_previewCellPositions)) return;
            
            EventBus<OnShipPlacementPreviewMoved>.Invoke(
                new OnShipPlacementPreviewMoved(_ship, _previewCellPositions, cellPositions));
            
            _previewCellPositions = cellPositions;
        }

        private void OnMouseUp()
        {
            _placementPreviewRenderer.transform.SetParent(_ship.transform);

            if (!_ship.CanDragAndPlace()) return;

            transform.position = _ship.transform.position;
            _placementPreviewRenderer.gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            Validation.CheckIfNull(this, _spriteRenderer, nameof(_spriteRenderer));
            Validation.CheckIfNull(this, _spriteFlash, nameof(_spriteFlash));
            Validation.CheckIfNull(this, _spriteOutline, nameof(_spriteOutline));
        }

        [Inject]
        private void Construct(LevelManager levelManager, Settings settings)
        {
            _levelManager = levelManager;
            _settings = settings;
        }

        public void Init(Ship ship)
        {
            _ship = ship;
            _shipVisualData = _settings.Ships[ship.GetShipLength()];

            _spriteRenderer.materials = new[] { _spriteOutline, _spriteFlash };

            var placement = new GameObject("PlacementPreviewSprite") { transform = { parent = ship.transform } };

            _placementPreviewRenderer = placement.AddComponent<SpriteRenderer>();

            _placementPreviewRenderer.transform.position = ship.transform.position;

            UpdateSprite();
            UpdatePlacementSprite();

            _placementPreviewRenderer.gameObject.SetActive(false);

            if (_ship.GetCharacterType() == CharacterType.Enemy)
                _spriteRenderer.gameObject.SetActive(false);

            _onCellHit = new EventBinding<OnCellHit>(Ship_OnHit);
            EventBus<OnCellHit>.Register(_onCellHit);
        }

        public void UpdateSprite()
        {
            Sprite sprite = _ship.IsHorizontal()
                ? _shipVisualData.Horizontal
                : _shipVisualData.Vertical;

            _spriteRenderer.sprite = sprite;
        }

        public void UpdatePlacementSprite()
        {
            Sprite sprite = _ship.IsHorizontal()
                ? _shipVisualData.Horizontal
                : _shipVisualData.Vertical;

            Quaternion rotation = _placementPreviewRenderer.transform.rotation;
            rotation.eulerAngles = rotation.eulerAngles.With(z: _ship.GetZRotation());
            _placementPreviewRenderer.transform.rotation = rotation;

            _placementPreviewRenderer.sprite = sprite;
            _placementPreviewRenderer.transform.localScale = _spriteRenderer.transform.localScale;
            _placementPreviewRenderer.color = _placementPreviewRenderer.color.With(a: 0.5f);
        }

        private void Ship_OnHit(OnCellHit e)
        {
            if (e.Ship != _ship) return;

            Sequence flashSequence = DOTween.Sequence();
            Material spriteFlashMaterial = _spriteRenderer.materials[1];

            flashSequence.Append(spriteFlashMaterial.DOFloat(_settings.MaxSpriteFlashAmount, s_flashAmount, 
                _settings.SpriteFlashDuration * 0.5f));
            
            flashSequence.Append(spriteFlashMaterial.DOFloat(0f, s_flashAmount,
                _settings.SpriteFlashDuration));
            
            Instantiate(GameResources.Instance.ShipHitExplosionPrefab, _levelManager.GetWorldCellPosition(e.WoundedCharacterType, e.HitCellPosition), Quaternion.identity);
        }

        [Serializable]
        public class Settings
        {
            public SerializedDictionary<int, ShipVisualData> Ships;
            [Range(0f, 1f)] public float MaxSpriteFlashAmount = 0.3f;
            [Range(0f, 0.2f)] public float SpriteFlashDuration = 0.1f;


            [Serializable]
            public class ShipVisualData
            {
                public Sprite Vertical;
                public Sprite Horizontal;
            }
        }
    }
}