using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerJumpChecker _playerJumpChecker;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpForce = 1;

        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            PlayerMover();
            PlayerJumper();
        }


        public void SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction;
        }


        public void SaySomething()
        {
            Debug.Log("ПАЛУНДРА!!!");
        }


        private void PlayerMover()
        {
            _rigidbody.velocity = new Vector2(_moveDirection.x * _speed, _rigidbody.velocity.y);
        }


        public void PlayerJumper()
        {
            if (_playerJumpChecker.GetIsJumping())
            {

                if (_playerJumpChecker.GetIsGrounded() && _rigidbody.velocity.y <= 0.01f)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                }
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }
        }

    }
}

