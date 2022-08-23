using UnityEngine;
using DG.Tweening;
using System;

public class Firetruck : MonoBehaviour
{
    private const int ZeroRotation = 0;

    [SerializeField] private Rope _ropePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Player _player;
    [SerializeField] private DOTweenPath _path;
    [SerializeField] private House _house;

    private float _delay = 0.7f;
    private float _yRotation = 150.1f;

    public event Action CarHasArrived;

    private void OnEnable()
    {
        _house.AllFiremansHaveArrived += MoveToHome;
    }

    private void OnDisable()
    {
        _house.AllFiremansHaveArrived -= MoveToHome;
    }

    public void OnCompletePath()
    {
        ChangeRotation();
        SpawnRope();
        CarHasArrived?.Invoke();
    }

    private void SpawnRope()
    {
        for (int i = 0; i < _player.FiremanCount; i++)
        {
            Instantiate(_ropePrefab, _spawnPoint.position, Quaternion.identity, transform);
        }
    }

    private void ChangeRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(ZeroRotation, _yRotation, ZeroRotation);
        transform.DORotateQuaternion(targetRotation, _delay);
    }

    private void MoveToHome()
    {
        _path.DOPlay();
    }
}
