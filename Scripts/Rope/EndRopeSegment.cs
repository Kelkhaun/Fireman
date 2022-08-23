using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EndRopeSegment : MonoBehaviour
{
    private RopeMover _ropeMover;
    private SphereCollider _sphereCollider;
    private float _delay = 2f;

    private IEnumerator Start()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);
        _ropeMover = GetComponentInParent<RopeMover>();
        _sphereCollider = GetComponent<SphereCollider>();

        yield return waitForSeconds;
        _ropeMover.Move(this);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Fireman fireman) && fireman.IsTaken == false)
        {
            _sphereCollider.enabled = false;
            fireman.TakeRope(this);
        }
    }
}
