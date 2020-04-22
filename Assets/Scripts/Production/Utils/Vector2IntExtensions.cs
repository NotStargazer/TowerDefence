using UnityEngine;

namespace Tools
{
    public static class Vector2IntExtensions
    {
        public static Vector3 ToVector3(this Vector2Int v2i)
        {
            return new Vector3(v2i.x, v2i.y);
        }
    }
}