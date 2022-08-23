using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class FiremansCounterDisplay : MonoBehaviour
{
    [SerializeField] FireDepartment _fireDepartment;

    private TMP_Text _text;

    private void OnEnable()
    {
        _fireDepartment.FiremansHaveBeenReduced += ChangeCountFireman;
    }
    private void OnDisable()
    {
        _fireDepartment.FiremansHaveBeenReduced -= ChangeCountFireman;
    }

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void ChangeCountFireman()
    {
        _text.text = _fireDepartment.FiremanCount + "/" + _fireDepartment.MaimumNumberOfFiremans;
    }
}
