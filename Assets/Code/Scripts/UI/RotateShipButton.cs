using System;
using System.Linq;
using DG.Tweening;
using EventBus;
using Events;
using Grid;
using Infrastructure;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;
using Image = UnityEngine.UI.Image;

namespace UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class RotateShipButton : MonoBehaviour
    {
        private const float AnimationDuration = 0.2f;
        
        private Button _button;
        private LevelManager _levelManager;
        private Vector2 _buttonStartPosition;

        [Inject]
        private void Construct(LevelManager levelManager, SelectedTheme selectedTheme)
        {
            _levelManager = levelManager;
            
            GetComponent<Image>().color = selectedTheme.PlayerTheme.MainColor;
            _button = GetComponent<Button>();
            
        }
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void ShowFor(Ship.ShipVisual shipVisual, UnityAction onClick)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(onClick);
            
            int shipCellCount = shipVisual.OccupiedCellPositions.Count();
            int index = Mathf.FloorToInt(shipCellCount * 0.5f);
            CellPosition cellPosition = shipVisual.OccupiedCellPositions.ToList()[index];
            CellPosition offset =  shipVisual.IsHorizontal ?  new CellPosition(0, 1) : new CellPosition(1, 0);
            _buttonStartPosition = _levelManager.GetWorldCellPosition(shipVisual.CharacterType, cellPosition);
            Vector2 buttonEndPosition = _levelManager.GetWorldCellPosition(shipVisual.CharacterType, cellPosition + offset);
            
            transform.position = Camera.main.WorldToScreenPoint(_buttonStartPosition);
            transform.localScale = Vector3.zero;
            
            transform.DOMove(Camera.main.WorldToScreenPoint(buttonEndPosition), AnimationDuration).SetEase(Ease.InCubic);
            transform.DOScale(Vector3.one, AnimationDuration).SetEase(Ease.InCubic);
            
            Show();
        }

        public void Hide()
        {
            transform.DOMove(Camera.main.WorldToScreenPoint(_buttonStartPosition), AnimationDuration).SetEase(Ease.InCubic);
            transform.DOScale(Vector3.zero, AnimationDuration).SetEase(Ease.InCubic)
                .onComplete += () => gameObject.SetActive(false);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
    }
}