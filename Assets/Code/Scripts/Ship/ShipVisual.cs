using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventBus;
using Events;
using GameplayStates;
using Grid;
using UI;
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

        public IEnumerable<CellPosition> OccupiedCellPositions => _ship.OccupiedCellPositions;
        public CharacterType CharacterType => _ship.GetCharacterType();
        public bool IsHorizontal => _ship.IsHorizontal();
        
        [SerializeField] [SelfAndChildren] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _spriteOutline;
        [SerializeField] private Material _spriteFlash;
        private LevelManager _levelManager;
        private SpriteRenderer _placementPreviewRenderer;
        private List<CellPosition> _previewCellPositions = new();
        private Settings _settings;
        private Ship _ship;
        private Settings.ShipVisualData _shipVisualData;
        private bool _isDragging;
        private Vector3 _spriteRendererOffset;
        private RotateShipButton _rotateShipButton;

        private EventBinding<OnCellHit> _onCellHit;
        private EventBinding<OnShipDestroyed> _onShipDestroyed;
        private EventBinding<OnGameplayStateChanged> _onGameplayStateChanged;
        
        [Inject]
        private void Construct(LevelManager levelManager, Settings settings, RotateShipButton rotateShipButton)
        {
            _levelManager = levelManager;
            _settings = settings;
            _rotateShipButton = rotateShipButton;
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
            UpdatePlacementPreviewSprite();

            _placementPreviewRenderer.gameObject.SetActive(false);

            if (_ship.GetCharacterType() == CharacterType.Enemy)
                _spriteRenderer.gameObject.SetActive(false);

            _onCellHit = new EventBinding<OnCellHit>(Ship_OnHit);
            EventBus<OnCellHit>.Register(_onCellHit);

            _onShipDestroyed = new EventBinding<OnShipDestroyed>(Ship_OnDestroyed);
            EventBus<OnShipDestroyed>.Register(_onShipDestroyed);

            _onGameplayStateChanged = new EventBinding<OnGameplayStateChanged>(Gameplay_OnStateChanged);
            EventBus<OnGameplayStateChanged>.Register(_onGameplayStateChanged);

        }

        private void OnMouseDown()
        {
            _placementPreviewRenderer.transform.SetParent(_ship.transform.parent);
            _previewCellPositions = _ship.OccupiedCellPositions.ToList();
            
            if (!_ship.CanDragAndPlace()) return;

            UpdateSprite();
            UpdatePlacementPreviewSprite();

            _placementPreviewRenderer.transform.position = _ship.transform.position;
            _placementPreviewRenderer.gameObject.SetActive(true);

            _isDragging = true;
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
            
            EventBus<OnShipPlacementPreviewMoved>.Invoke(
                new OnShipPlacementPreviewMoved(_ship, _previewCellPositions, _ship.OccupiedCellPositions.ToList()));

            if (!_ship.CanDragAndPlace()) return;

            transform.position = _ship.transform.position;
            _placementPreviewRenderer.gameObject.SetActive(false);
            
            _isDragging = false;
        }

        private void OnValidate()
        {
            Validation.CheckIfNull(this, _spriteRenderer, nameof(_spriteRenderer));
            Validation.CheckIfNull(this, _spriteFlash, nameof(_spriteFlash));
            Validation.CheckIfNull(this, _spriteOutline, nameof(_spriteOutline));
        }
        
        private void OnDisable() => EventBus<OnCellHit>.Deregister(_onCellHit);

        public void UpdateSprite()
        {
            Sprite sprite = _ship.IsHorizontal()
                ? _shipVisualData.Horizontal
                : _shipVisualData.Vertical;

            _spriteRenderer.sprite = sprite;
        }

        public void UpdatePlacementPreviewSprite()
        {
            if (_isDragging) return;
            
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

        public void ShowInteractButtons() => _rotateShipButton.ShowFor(this, () => _ship.TryRotate());

        public void HideInteractButtons() => _rotateShipButton.Hide();

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
        

        private void Ship_OnDestroyed(OnShipDestroyed e)
        {
            if (e.Ship != _ship) return;
            
            _spriteRenderer.gameObject.SetActive(true);
        }
        
        private void Gameplay_OnStateChanged(OnGameplayStateChanged e)
        {
            if (e.NewState == typeof(BattleResults)) 
                _spriteRenderer.gameObject.SetActive(true);
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