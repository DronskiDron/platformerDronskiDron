using General.Components;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerJumpChecker _playerJumpChecker;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpForce = 1;
        [SerializeField] private float _damageJumpForce = 10;
        [SerializeField] private float _interactionRadius = 5;
        [SerializeField] private LayerMask _interactionLayer;

        private Collider2D[] _interactionResult = new Collider2D[1];
        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private Animator _animator;
        private SpriteRenderer _sprite;
        private bool _isGrounded;
        private bool _allowDoubleJump;

        private static readonly int IsRunning = Animator.StringToHash("is-Running");
        private static readonly int IsGround = Animator.StringToHash("is-Ground");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-Velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Heal = Animator.StringToHash("heal");


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }


        private void FixedUpdate()
        {
            PlayerMover();
            AniimationSwitcher();
        }


        private void Update()
        {
            _isGrounded = IsGrounded();
        }


        public void SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction;
        }


        private bool IsGrounded()
        {
            var isGrounded = _playerJumpChecker.GetIsGrounded();
            return isGrounded;
        }


        public void SaySomething()
        {
            Debug.Log("ПАЛУНДРА!!!");
        }


        private void PlayerMover()
        {
            var xVelocity = _moveDirection.x * _speed;
            var yVelocity = CalculateVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        }


        private float CalculateVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _playerJumpChecker.GetIsPressingJump();

            if (_isGrounded) _allowDoubleJump = true;

            if (isJumpPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }


        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;

            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpForce;
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpForce;
                _allowDoubleJump = false;
            }

            return yVelocity;
        }


        private void AniimationSwitcher()
        {
            _animator.SetBool(IsRunning, _moveDirection.x != 0);
            _animator.SetBool(IsGround, _isGrounded);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

            if (_moveDirection.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_moveDirection.x < 0)
            {
                _sprite.flipX = true;
            }
        }


        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce);
        }


        public void Healing()
        {
            _animator.SetTrigger(Heal);
        }


        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                interactable?.Interact();
            }
        }
    }
}

