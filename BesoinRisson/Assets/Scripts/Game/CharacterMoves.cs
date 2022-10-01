using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
// TODO superclass for movements
namespace DefaultNamespace
{
    public class CharacterMoves : MonoBehaviour
    {
        private enum SwordPositions
        {
            Up,
            Mid,
            Down
        }
        
        private Rigidbody2D _rigidbody;
        private IInputProvider _inputProvider;
        private ICheck _groundCheck;
        private Rigidbody2D _swordBody;
        private bool _facingRight = true;
        private bool _hasSword = true;
        private SwordPositions _swordState = SwordPositions.Mid;
        public bool IsBlocking { get; private set; }
        public bool IsSwordDown { get; private set; }


        [Header("Movement Config")] [SerializeField]
        private float walkspeed;

        [SerializeField] private Respawn _respawn;

        [SerializeField] private float jumpForce;
        [SerializeField] private Animator swordAnimator;
        [SerializeField] private Animation swordStabAnimation;
        [SerializeField] [NotNull] private GameObject groundCheckObject;
        [SerializeField] private GameObject sword;
    
        [SerializeField] private GameObject Adversary;
    }
}