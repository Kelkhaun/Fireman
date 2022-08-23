using System.Collections;
using UnityEngine;
using DG.Tweening;
using static DG.Tweening.DOTweenCYInstruction;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class FiremanMover : MonoBehaviour
{
    [SerializeField] private float _movementDuration;
    [SerializeField] private Transform _targetPoint;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Tween _move;
    private Tweener _moveToPlayer;
    private float _duration1 = 0.2f;
    private float _duration2 = 0.5f;
    private float _minimalDistance = 0.008f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void ChangeAnimationState(Transform target)
    {
        if (Vector3.Distance(transform.position, target.position) > _minimalDistance)
            _animator.SetBool(HumanAnimator.States.IsRun, true);
        else
            _animator.SetBool(HumanAnimator.States.IsRun, false);
    }

    public IEnumerator FollowPlayer(Transform target)
    {
        _moveToPlayer = _rigidbody.DOMove(target.position, _movementDuration).SetAutoKill(false);

        while (true)
        {
            _moveToPlayer.ChangeEndValue(target.position, true).Restart();
            transform.rotation = target.rotation;
            ChangeAnimationState(target);
            yield return null;
        }
    }

    public IEnumerator MoveToPoint(Transform target, Vector3 towards, Quaternion endRotation, float duration, bool isLookAt = true)
    {
        _animator.SetBool(HumanAnimator.States.IsRun, true);

        if (isLookAt == true)
            transform.DOLookAt(towards, _duration1, AxisConstraint.Y, Vector3.up);

        _move = _rigidbody.DOMove(target.position, duration);
        yield return new WaitForCompletion(_move);
        _animator.SetBool(HumanAnimator.States.IsRun, false);
        transform.DORotateQuaternion(endRotation, _duration2);
    }
}
