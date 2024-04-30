using System;
using System.Globalization;
using EventBus;
using Events;
using UnityEngine;
using Zenject;

public class GameSettings : IInitializable
{
    private const string FrameRateKey = "TargetFrameRate";
    private const int DefaultFrameRate = 60;
    private const string LanguageKey = "Language";
    private const string DefaultLanguage = "en";
    
    public void Initialize()
    {
        int targetFrameRate = GetTargetFrameRate();
        string language = GetLanguage(); 

        SetFrameRate(targetFrameRate);
        SetLanguage(new CultureInfo(language));
    }

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
            throw new ArgumentException(nameof(frameRate), "Wrong value " + frameRate);
        
        Application.targetFrameRate = frameRate;

        PlayerPrefs.SetInt(FrameRateKey, frameRate);
        PlayerPrefs.Save();
    }
}