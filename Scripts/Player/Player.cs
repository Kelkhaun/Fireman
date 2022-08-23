using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private const int ZeroAngle = 0;

    [SerializeField] private Transform[] _targetPoints;
    [SerializeField] private Transform[] _targetPointsNearHouse;
    [SerializeField] private FireDepartment _fireDepartment;
    [SerializeField] private Firetruck _firetruck;
    [SerializeField] private LimiterDisplay _limiterDisplay;
    [SerializeField] private Transform _targetPoint;

    private List<Fireman> _firemans = new List<Fireman>();
    private int _index = 0;
    private int _numbersOfRopeTaken = 0;
    private int _maximumNumberOfFirefighters = 3;
    private bool _isWork = true;

    public int FiremanCount => _firemans.Count;
    public int MaximumNumberOfFirefighters => _maximumNumberOfFirefighters;

    private void OnEnable()
    {
        _fireDepartment.FiremansHaveBeenReduced += ActivateGiveOrderFollow;
        _firetruck.CarHasArrived += ActivateGiveOrderToRunCar;
    }

    private void OnDisable()
    {
        _fireDepartment.FiremansHaveBeenReduced -= ActivateGiveOrderFollow;
        _firetruck.CarHasArrived -= ActivateGiveOrderToRunCar;

        for (int i = 0; i < _firemans.Count; i++)
        {
            _firemans[i].SelectedRope -= IncreaseNumberOfRopesTaken;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out FireDepartment fireDepartment))
        {
            if (FiremanCount == MaximumNumberOfFirefighters && _isWork == true)
            {
                _limiterDisplay.Show();
                _isWork = false;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (FiremanCount == MaximumNumberOfFirefighters)
        {
            if (collider.gameObject.TryGetComponent(out FireDepartment fireDepartment))
            {
                _limiterDisplay.Hide();
                _isWork = true;
            }
        }
    }

    public void GetFireman(Fireman fireman)
    {
        _firemans.Add(fireman);
        fireman.SelectedRope += IncreaseNumberOfRopesTaken;
    }

    private void IncreaseNumberOfRopesTaken()
    {
        _numbersOfRopeTaken++;

        if (_numbersOfRopeTaken == _firemans.Count)
            StartCoroutine(GiveOrderMoveToHome());
    }

    private void ActivateGiveOrderToRunCar()
    {
        StartCoroutine(GiveOrderToRunToCar());
    }

    private void ActivateGiveOrderFollow()
    {
        StartCoroutine(GiveOrderFollow());
    }

    private IEnumerator GiveOrderToRunToCar()
    {
        float duration = 1.5f;
        float delay1 = 0.45f;
        float delay2 = 0.6f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay1);
        WaitForSeconds waitForSeconds2 = new WaitForSeconds(delay2);

        yield return waitForSeconds2;

        for (int i = _firemans.Count - 1; i >= 0; i--)
        {
            _firemans[i].MoveToPoint(_targetPoint.transform, -_targetPoint.position, _targetPoint.rotation, duration, true);
            yield return waitForSeconds;
        }
    }

    private IEnumerator GiveOrderFollow()
    {
        float duration = 0.55f;
        float delay = 0.6f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        _firemans[_index].MoveToPoint(_targetPoints[_index], -_targetPoints[_index].position, _targetPoints[_index].rotation, duration, true);
        yield return waitForSeconds;
        _firemans[_index].FollowPlayer(_targetPoints[_index]);
        _index++;
    }

    private IEnumerator GiveOrderMoveToHome()
    {
        float duration1 = 1f;
        float duration2 = 0.05f;
        float delay = 0.5f;
        int yRotation = 260;
        Quaternion rotation = Quaternion.Euler(ZeroAngle, yRotation, ZeroAngle);
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        for (int i = _firemans.Count - 1; i >= 0; i--)
        {
            _firemans[i].MoveToPoint(_targetPointsNearHouse[i].transform, _targetPointsNearHouse[i].transform.position, _targetPointsNearHouse[i].transform.rotation, duration1, false);
            _firemans[i].transform.DORotateQuaternion(rotation, duration2);
            yield return waitForSeconds;
            StartCoroutine(_firemans[i].ExtinguishFire());
        }
    }
}
