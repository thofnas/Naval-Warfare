using System;
using Data;
using EventBus;
using Events;
using Infrastructure;
using Map;
using Themes;
using UniRx;
using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundAnimator : MonoBehaviour, IDisposable
{
    private const int FrameRateInSeconds = 2;
    
    private static readonly int s_player1Background = Shader.PropertyToID("_Player1Background");
    private static readonly int s_player2Background = Shader.PropertyToID("_Player2Background");
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _backgroundBlendMaterial;
    [SerializeField] private Material _backgroundDefaultMaterial;
    [SerializeField] private Sprite _vertexSprite;
    private Sprite[] _player1Sprites;
    private Sprite[] _player2Sprites;
    private int _player1SpriteIndex;
    private int _player2SpriteIndex;
    private PersistentData _persistentData;
    private MapLibrary _mapLibrary;

    private EventBinding<OnThemeChanged> _onThemeChanged;
    private EventBinding<OnNewMapTypeSelected> _onNewMapTypeSelected;
    private IDisposable _observableInterval;
    private SelectedTheme _selectedTheme;

    [Inject]
    private void Construct(PersistentData persistentData, MapLibrary mapLibrary, SelectedTheme selectedTheme)
    {
        _persistentData = persistentData;
        _mapLibrary = mapLibrary;
        _selectedTheme = selectedTheme;

        _onThemeChanged = new EventBinding<OnThemeChanged>(Initialize);
        EventBus<OnThemeChanged>.Register(_onThemeChanged);
        _onNewMapTypeSelected = new EventBinding<OnNewMapTypeSelected>(Initialize);
        EventBus<OnNewMapTypeSelected>.Register(_onNewMapTypeSelected);
    }

    public void Dispose()
    {
        EventBus<OnThemeChanged>.Deregister(_onThemeChanged);
        EventBus<OnNewMapTypeSelected>.Deregister(_onNewMapTypeSelected);
        _observableInterval.Dispose();
    }

    private void Initialize()
    {
        _player1Sprites = _selectedTheme.PlayerTheme.BackgroundSprites;
        _player2Sprites = _mapLibrary.Maps[_persistentData.PlayerData.SelectedMapType].AITheme.BackgroundSprites;
        
        _player1SpriteIndex = (_player1SpriteIndex - 1) % _player1Sprites.Length;
        _player2SpriteIndex = (_player2SpriteIndex - 1) % _player2Sprites.Length;
        if (_player1SpriteIndex < 0) _player1SpriteIndex = 0;
        if (_player2SpriteIndex < 0) _player2SpriteIndex = 0;
        
        if (CharactersThemesAreSame())
        {
            _spriteRenderer.material = _backgroundDefaultMaterial;
            ChangeTexture();
            StartPeriodicTextureChange(ChangeTexture);
        }
        else
        {
            _spriteRenderer.sprite = _vertexSprite;
            _spriteRenderer.material = _backgroundBlendMaterial;
            ChangeTextures();
            StartPeriodicTextureChange(ChangeTextures);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private bool CharactersThemesAreSame()
    {
        return _persistentData.PlayerData.SelectedIslandsThemeType == IslandsThemeType.AI;
    }

    private void StartPeriodicTextureChange(Action changeAction)
    {
        _observableInterval?.Dispose();
        
        _observableInterval = Observable.Interval(TimeSpan.FromSeconds(FrameRateInSeconds))
            .Subscribe(_ => changeAction())
            .AddTo(this);
    }

    private void ChangeTextures()
    {
        _spriteRenderer.material.SetTexture(s_player1Background, _player1Sprites[_player1SpriteIndex].texture);
        _spriteRenderer.material.SetTexture(s_player2Background, _player2Sprites[_player2SpriteIndex].texture);
    
        _player1SpriteIndex = (_player1SpriteIndex + 1) % _player1Sprites.Length;
        _player2SpriteIndex = (_player2SpriteIndex + 1) % _player2Sprites.Length;
    }

    private void ChangeTexture()
    {
        _spriteRenderer.sprite = _player1Sprites[_player1SpriteIndex];
        _player1SpriteIndex = (_player1SpriteIndex + 1) % _player1Sprites.Length;
    }
}