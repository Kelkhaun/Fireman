using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(TutorialCamera))]
public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Rigidbody _player;
    [SerializeField] private Vector3 _forwardDirection;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _xAngle;
    [SerializeField] private float _distance;
    [SerializeField] private float _maxVectorLenght;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] FireDetector _fireDetector;

    private TutorialCamera _tutorialCamera;
    private Quaternion _targetRotation1 = Quaternion.Euler(55, 0, 0);
    private Quaternion _targetRotation2 = Quaternion.Euler(55, -15, 0);
    private Vector3 _nextPosition;
    private float _turnDuration = 0.5f;
    private bool _isMonitorsThePlayer = false;

    private void Awake()
    {
        _tutorialCamera = GetComponent<TutorialCamera>();
    }

    private void OnEnable()
    {
        _tutorialCamera.ShowIsOver += StartFollowingPlayer;
        _fireDetector.FireFound += ActivateShowFire;
    }

    private void OnDisable()
    {
        _tutorialCamera.ShowIsOver -= StartFollowingPlayer;
        _fireDetector.FireFound -= ActivateShowFire;
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(_xAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void StartFollowingPlayer()
    {
        _isMonitorsThePlayer = true;
    }

    private void FixedUpdate()
    {
        if (_isMonitorsThePlayer == true)
        {
            _nextPosition = _player.position + Vector3.ClampMagnitude(_player.velocity, _maxVectorLenght);
            _nextPosition += Vector3.up * Mathf.Cos(Mathf.Deg2Rad * _xAngle) * _distance;
            _nextPosition += -_forwardDirection * Mathf.Sin(Mathf.Deg2Rad * _xAngle) * _distance;
            transform.position = Vector3.Lerp(transform.position, _nextPosition, _movementSpeed * Time.fixedDeltaTime);
        }
    }

    private void ActivateShowFire()
    {
        StartCoroutine(ShowFire());
    }

    private IEnumerator ShowFire()
    {
        _isMonitorsThePlayer = false;
        float delay = 1.7f;
        float duration = 1.2f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        transform.DORotateQuaternion(_targetRotation1, _turnDuration);
        transform.DOMove(_targetPoint.position, duration);

        yield return waitForSeconds;
        transform.DORotateQuaternion(_targetRotation2, _turnDuration);
        _isMonitorsThePlayer = true;
    }
}
