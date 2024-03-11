using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utilities.Extensions
{
    public static class CollidersExtensions
    {
        #region BoxCollider

        public static void Enable(this BoxCollider boxCollider) => boxCollider.enabled = true;

        public static void Disable(this BoxCollider boxCollider) => boxCollider.enabled = false;

        #endregion

        #region SphereCollider

        public static void Enable(this SphereCollider sphereCollider) => sphereCollider.enabled = true;

        public static void Disable(this SphereCollider sphereCollider) => sphereCollider.enabled = false;

        #endregion

        #region MeshCollider

        public static void Enable(this MeshCollider meshCollider) => meshCollider.enabled = true;

        public static void Disable(this MeshCollider meshCollider) => meshCollider.enabled = false;

        #endregion

        #region CapsuleCollider

        public static void Enable(this CapsuleCollider capsuleCollider) => capsuleCollider.enabled = true;

        public static void Disable(this CapsuleCollider capsuleCollider) => capsuleCollider.enabled = false;

        #endregion

        #region BoxCollider2D

        public static void Enable(this BoxCollider2D boxCollider2D) => boxCollider2D.enabled = true;

        public static void Disable(this BoxCollider2D boxCollider2D) => boxCollider2D.enabled = false;

        #endregion

        #region CircleCollider2D

        public static void Enable(this CircleCollider2D circleCollider2D) => circleCollider2D.enabled = true;

        public static void Disable(this CircleCollider2D circleCollider2D) => circleCollider2D.enabled = false;

        #endregion

        #region CapsuleCollider2D

        public static void Enable(this CapsuleCollider2D capsuleCollider2D) => capsuleCollider2D.enabled = true;

        public static void Disable(this CapsuleCollider2D capsuleCollider2D) => capsuleCollider2D.enabled = false;

        #endregion

        #region CustomCollider2D

        public static void Enable(this CustomCollider2D customCollider2D) => customCollider2D.enabled = true;

        public static void Disable(this CustomCollider2D customCollider2D) => customCollider2D.enabled = false;

        #endregion

        #region PolygonCollider2D

        public static void Enable(this PolygonCollider2D polygonCollider2D) => polygonCollider2D.enabled = true;

        public static void Disable(this PolygonCollider2D polygonCollider2D) => polygonCollider2D.enabled = false;

        #endregion

        #region TilemapCollider2D

        public static void Enable(this TilemapCollider2D tilemapCollider2D) => tilemapCollider2D.enabled = true;

        public static void Disable(this TilemapCollider2D tilemapCollider2D) => tilemapCollider2D.enabled = false;

        #endregion
    }
}