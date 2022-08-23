using UnityEngine;
using UnityEngine.UI;

public class LimiterDisplay : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Player _player;
    [SerializeField] float _movementSpeed;
    [SerializeField] Vector3 _offsets;

    private void Update()
    {
        transform.position = _player.transform.position + _offsets;
    }

    public void Show()
    {
        _image.enabled = true;
    }

    public void Hide()
    {
        _image.enabled = false;
    }
}
