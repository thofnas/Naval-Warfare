using UnityEngine;

namespace Utilities
{
    public interface IComponent
    {
        Transform transform { get; }
        GameObject gameObject { get; }
    }
}