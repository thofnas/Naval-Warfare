using System;
using Data;
using EventBus;
using Events;
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
    private ThemeLibrary _themeLibrary;

    private EventBinding<OnThemeChanged> _onThemeChanged;
    private IDisposable _observableInterval;

    [Inject]
    private void Construct(PersistentData persistentData, ThemeLibrary themeLibrary)
    {
        _persistentData = persistentData;
        _themeLibrary = themeLibrary;

        _player2Sprites = themeLibrary.GetTheme(IslandsTheme.AI).BackgroundSprites;

        _onThemeChanged = new EventBinding<OnThemeChanged>(Initialize);
        EventBus<OnThemeChanged>.Register(_onThemeChanged);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _player1Sprites = _themeLibrary.GetTheme(_persistentData.PlayerData.SelectedIslandsTheme).BackgroundSprites;
        
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

    private bool CharactersThemesAreSame()
    {
        return _persistentData.PlayerData.SelectedIslandsTheme == IslandsTheme.AI;
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

    public void Dispose()
    {
        EventBus<OnThemeChanged>.Deregister(_onThemeChanged);
        _observableInterval.Dispose();
    }
}