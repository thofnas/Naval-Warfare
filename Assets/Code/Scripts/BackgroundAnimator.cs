using System;
using Themes;
using UniRx;
using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundAnimator : MonoBehaviour
{
    private const int FrameRateInSeconds = 2;

    private static readonly int s_player1Background = Shader.PropertyToID("_Player1Background");
    private static readonly int s_player2Background = Shader.PropertyToID("_Player2Background");
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _backgroundDefaultMaterial;
    private Sprite[] _player1Sprites;
    private Sprite[] _player2Sprites;
    private int _player1SpriteIndex;
    private int _player2SpriteIndex;
    private CharactersThemes _charactersThemes;

    [Inject]
    private void Construct(CharactersThemes charactersThemes)
    {
        _charactersThemes = charactersThemes;
        _player1Sprites = charactersThemes.GetThemeSettings(CharacterType.Player).BackgroundSprites;
        _player2Sprites = charactersThemes.GetThemeSettings(CharacterType.Enemy).BackgroundSprites;
    }

    private void Start()
    {
        if (_charactersThemes.GetThemeSettings(CharacterType.Player) ==
            _charactersThemes.GetThemeSettings(CharacterType.Enemy))
        {
            _spriteRenderer.material = _backgroundDefaultMaterial;
            
            ChangeTexture();
            
            Observable.Interval(TimeSpan.FromSeconds(FrameRateInSeconds))
                .Subscribe(_ => ChangeTexture())
                .AddTo(this);
            return;
        }

        ChangeTextures();
        
        Observable.Interval(TimeSpan.FromSeconds(FrameRateInSeconds))
            .Subscribe(_ => ChangeTextures())
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