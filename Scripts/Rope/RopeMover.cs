using UnityEngine;
using DG.Tweening;

public class RopeMover : MonoBehaviour
{
    private float _duration = 0.1f;
    private float _xOffset = 0.9f;

    public void Move(EndRopeSegment endRopeSegment)
    {
        endRopeSegment.transform.DOMoveX(endRopeSegment.transform.position.x + _xOffset, _duration);
    }
}