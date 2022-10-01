using UnityEngine;

namespace Behaviour.Utils
{
    public class BoxCheck : MonoBehaviour
    {
        [SerializeField] private float diameter = .1f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, diameter/2);
        }
        
    }
}