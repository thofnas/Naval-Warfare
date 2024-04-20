using System;
using UnityEngine;
using Zenject;

public class GameSettings : IInitializable
{
    public const int DefaultFrameRate = 60;
    private const string FrameRateKey = "TargetFrameRate";
    
    public void Initialize()
    {
        int targetFrameRate = PlayerPrefs.GetInt(FrameRateKey, DefaultFrameRate);

        SetFrameRate(targetFrameRate);
    }

    public void SetFrameRate(int frameRate)
    {
        if (frameRate <= 0)
            throw new ArgumentException(nameof(frameRate), "Wrong value " + frameRate);
        
        Application.targetFrameRate = frameRate;

        PlayerPrefs.SetInt(FrameRateKey, frameRate);
        PlayerPrefs.Save();
    }
}