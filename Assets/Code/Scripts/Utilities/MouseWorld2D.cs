using UnityEngine;

namespace Utilities
{
    public static class MouseWorld2D
    {
        public static Vector3 GetPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}