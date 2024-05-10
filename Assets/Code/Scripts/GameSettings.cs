using System;
using System.Globalization;
using EventBus;
using Events;
using UnityEngine;
using Utilities.Extensions;
using Zenject;

public class GameSettings : IInitializable
{
    private const string FrameRateKey = "TargetFrameRate";
    private const int DefaultFrameRate = 60;
    private const string LanguageKey = "Language";
    private const string DefaultLanguage = "en";
    private const string MusicKey = "Music";
    private const string SfxKey = "Sfx";
    private const string UISoundKey = "UISound";
    
    public void Initialize()
    {
        int targetFrameRate = GetTargetFrameRate();
        string language = GetLanguage();
        bool isMusicEnabled = IsMusicEnabled();
        bool isSfxEnabled = IsSfxEnabled();

        SetFrameRate(targetFrameRate);
        SetLanguage(new CultureInfo(language));
        SetMusic(isMusicEnabled);
        SetSfx(isSfxEnabled);
    }

    public static bool IsMusicEnabled() => PlayerPrefs.GetInt(MusicKey, true.ToInt()).ToBoolean();

    public static void SetMusic(bool value) => PlayerPrefs.SetInt(MusicKey, value.ToInt());

    public static bool IsSfxEnabled() => PlayerPrefs.GetInt(SfxKey, true.ToInt()).ToBoolean();

    public static void SetSfx(bool value) => PlayerPrefs.SetInt(SfxKey, value.ToInt());

    public static bool IsUISoundEnabled() => PlayerPrefs.GetInt(UISoundKey, true.ToInt()).ToBoolean();

    public static void SetUISound(bool value) => PlayerPrefs.SetInt(UISoundKey, value.ToInt());

    public static int GetTargetFrameRate() => PlayerPrefs.GetInt(FrameRateKey, DefaultFrameRate);

    public static string GetLanguage() => PlayerPrefs.GetString(LanguageKey, DefaultLanguage);

    public static void ResetLanguage() => SetLanguage(new CultureInfo(DefaultLanguage));

    public static void SetLanguage(CultureInfo cultureInfo)
    {
        string oldLanguage = GetLanguage();
        string newLanguage = cultureInfo.Name;
        
        if (oldLanguage == newLanguage)
            return;
        
        PlayerPrefs.SetString(LanguageKey, newLanguage);
        
        EventBus<OnPlayerPrefsLanguageChanged>.Invoke(new OnPlayerPrefsLanguageChanged(from: oldLanguage, to: newLanguage));
    }

    public static void SetFrameRate(int frameRate)
    {
        if (frameRate is 0 or < -1)
            throw new ArgumentException("Wrong value " + frameRate, nameof(frameRate));
        
        Application.targetFrameRate = frameRate;

        PlayerPrefs.SetInt(FrameRateKey, frameRate);
        PlayerPrefs.Save();
    }
}