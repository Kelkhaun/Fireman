using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TutorialCamera : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint1;
    [SerializeField] private Transform _targetPoint2;
    [SerializeField] private float _delayBeforeShowing;
    [SerializeField] private float _movementDuration;
    [SerializeField] private float _turnDuration;

    private Quaternion _targetRotation1 = Quaternion.Euler(55, 0, 0);
    private Quaternion _targetRotation2 = Quaternion.Euler(55, -15, 0);
    private Sequence _moveAnimation;
    private int _delayMultiplier = 2;

    public event Action ShowIsOver;

    private IEnumerator Start()
    {
        WaitForSeconds waitForSeconds1 = new WaitForSeconds(_delayBeforeShowing);
        yield return waitForSeconds1;

        ShowTerritory();
    }

    private void ShowTerritory()
    {
        _moveAnimation = DOTween.Sequence();

        transform.DORotateQuaternion(_targetRotation1, _turnDuration);

        _moveAnimation
            .Append(transform.DOMove(_targetPoint1.position, _movementDuration)).SetEase(Ease.Linear)
            .Append(transform.DOMove(_targetPoint2.position, _movementDuration)).SetEase(Ease.Linear);

        transform.DORotateQuaternion(_targetRotation2, _turnDuration).SetDelay(_movementDuration * _delayMultiplier);

        ShowIsOver?.Invoke();
    }
}
