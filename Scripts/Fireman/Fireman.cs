using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FiremanMover))]
public class Fireman : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private FiremanMover _firemanMover;
    private Coroutine _followPlayer;

    private float _delay = 0.5f;
    private bool _isAtHouse = false;
    private bool _isTaken = false;

    public bool IsAtHouse => _isAtHouse;
    public bool IsTaken => _isTaken;

    public event Action SelectedRope;

    private void Start()
    {
        _firemanMover = GetComponent<FiremanMover>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void TakeRope(EndRopeSegment endRopeSegment)
    {
        _isTaken = true;
        endRopeSegment.transform.SetParent(transform);
        SelectedRope?.Invoke();
    }

    public void StopFollowPlayer()
    {
        StopCoroutine(_followPlayer);
    }

    public void MakeIsAtHouse()
    {
        _isAtHouse = true;
    }

    public void FollowPlayer(Transform transform)
    {
        _followPlayer = StartCoroutine(_firemanMover.FollowPlayer(transform));
    }

    public void MoveToPoint(Transform transform, Vector3 towards, Quaternion endRotation, float duration, bool isWorkLookAt = false)
    {
        StartCoroutine(_firemanMover.MoveToPoint(transform, towards, endRotation, duration, isWorkLookAt));
    }

    public IEnumerator ExtinguishFire()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);
        yield return waitForSeconds;
        _particleSystem.Play();
    }
}