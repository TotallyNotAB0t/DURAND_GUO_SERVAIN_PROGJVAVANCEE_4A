using Interfaces;
using UnityEngine;

namespace Behaviour.Utils
{
    public class BoxCheck : MonoBehaviour, ICheck
    {
        [SerializeField] private float width = .1f;
        [SerializeField] private float height = .1f;

        [SerializeField] private LayerMask collisionMask;
        private ICheck _checkImplementation;

        public bool Check(Vector2 collider1, float radius1, Vector2 collider2, float radius2)
        {
            return Vector2.Distance(collider1, collider2) < (radius1/2)+(radius2/2);
        }
        public bool CheckGround(Vector2 collider1, float radius1, Vector2 ground, float size)
        {
            return Vector2.Distance(collider1, new Vector2(collider1.x, ground.y)) < (radius1/2)+(size/2);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, transform.localScale.x/2);
        }
        
    }
}