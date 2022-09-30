using UnityEngine;

namespace Interfaces
{
    public interface ICheck
    {
        public bool Check(Vector2 collider1, float radius1, Vector2 collider2, float radius2);
        public bool CheckGround(Vector2 collider1, float radius1, Vector2 ground, float size);
    }
}