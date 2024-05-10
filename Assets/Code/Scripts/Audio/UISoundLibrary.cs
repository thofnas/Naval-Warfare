using System;
using FMODUnity;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public struct UISoundLibrary
    {
        [field: SerializeField] public EventReference ButtonClick { get; private set; }
        [field: SerializeField] public EventReference SwitchButtonClick { get; private set; }
    }
}