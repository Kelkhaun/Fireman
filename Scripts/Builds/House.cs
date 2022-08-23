using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class House : MonoBehaviour
{
    [SerializeField] private int _firemansNumberRequired;
    [SerializeField] private Transform[] _targetPoints;
    [SerializeField] private Transform _targetPoint;

    private BoxCollider _collider;
    private Coroutine _resetCollider;
    private int _index = 0;
    private int _firemanCount;
    private float _duration = 0.7f;

    public int FiremansNumberRequired => _firemansNumberRequired;
    public int FiremanCount => _firemanCount;

    public event Action FiremanIsAssembled;
    public event Action AllFiremansHaveArrived;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Fireman fireman) && fireman.IsAtHouse == false)
        {
            fireman.MakeIsAtHouse();
            fireman.StopFollowPlayer();
            fireman.MoveToPoint(_targetPoints[_index], -_targetPoints[_index].position, _targetPoints[_index].rotation, _duration);
            _resetCollider = StartCoroutine(ResetCollider());

            if (_firemanCount < _firemansNumberRequired)
                _index++;

            _firemanCount++;
            FiremanIsAssembled?.Invoke();

            if (_firemanCount == _firemansNumberRequired)
            {
                AllFiremansHaveArrived?.Invoke();
                DeactivateCollider();
            }
        }
    }

    private void DeactivateCollider()
    {
        StopCoroutine(_resetCollider);
        _collider.enabled = false;
    }

    private IEnumerator ResetCollider()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_duration);

        _collider.enabled = false;
        yield return waitForSeconds;
        _collider.enabled = true;
        yield return waitForSeconds;
    }
}
