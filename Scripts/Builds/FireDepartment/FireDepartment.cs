using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FireDepartment : MonoBehaviour
{
    [SerializeField] private Fireman _firemanPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _delay;

    private Collider _collider;
    private List<Fireman> _firemans = new List<Fireman>();
    private int _maximumNumberOfFiremans = 10;

    public int FiremanCount => _firemans.Count;
    public int MaimumNumberOfFiremans => _maximumNumberOfFiremans;

    public event Action FiremansHaveBeenReduced;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        SpawnInitialFiremans();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            if (player.FiremanCount < player.MaximumNumberOfFirefighters)
            {
                StartCoroutine(ResetCollider());
                GiveTheFireman(player);
                FiremansHaveBeenReduced?.Invoke();
            }
        }
    }

    private void GiveTheFireman(Player player)
    {
        Fireman lastFireman = _firemans[_firemans.Count - 1];
        player.GetFireman(lastFireman);
        lastFireman.transform.parent = null;
        _firemans.Remove(lastFireman);
    }

    private void SpawnInitialFiremans()
    {
        int firemansInitialNumber = 10;

        for (int i = 0; i < firemansInitialNumber; i++)
        {
            Fireman fireman = Instantiate(_firemanPrefab, _spawnPoints[i].position, _firemanPrefab.transform.rotation, transform);
            _firemans.Add(fireman);
        }
    }

    private IEnumerator ResetCollider()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);

        _collider.enabled = false;
        yield return waitForSeconds;
        _collider.enabled = true;
        yield return waitForSeconds;
    }
}
