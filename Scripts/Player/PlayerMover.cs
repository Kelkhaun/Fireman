using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMover : MonoBehaviour
{
    private const float ZeroSpeed = 0f;

    [Range(1,10)][SerializeField] float _movementSpeed;
    [SerializeField] Joystick _joystick;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Turn()
    {
        float rotationSpeed = 3f;
        Vector3 direction = new Vector3(_joystick.Horizontal, ZeroSpeed, _joystick.Vertical) * rotationSpeed;
        direction = Vector3.ClampMagnitude(direction, rotationSpeed);

        if (direction != Vector3.zero)
            _rigidbody.MoveRotation(Quaternion.LookRotation(-direction));
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _movementSpeed, _rigidbody.velocity.y, _joystick.Vertical * _movementSpeed);

        if (_joystick.Horizontal != ZeroSpeed || _joystick.Vertical != ZeroSpeed)
        {
            Turn();
            _animator.SetBool(HumanAnimator.States.IsRun, true);
        }
        else
        {
            _animator.SetBool(HumanAnimator.States.IsRun, false);
        }
    }

}
