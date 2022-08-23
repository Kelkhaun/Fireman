using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(CanvasGroup))]
public class FiremanCounterAtHouse : MonoBehaviour
{
    [SerializeField] House _house;
    [SerializeField] private Image _slider;

    private CanvasGroup _canvasGroup;
    private TMP_Text _text;
    private float _delay = 2f;

    private void OnEnable()
    {
        _house.FiremanIsAssembled += ChangeCountFireman;
    }

    private void OnDisable()
    {
        _house.FiremanIsAssembled -= ChangeCountFireman;
    }

    private IEnumerator Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _text = GetComponentInChildren<TMP_Text>();
        WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);

        yield return waitForSeconds;
        _canvasGroup.alpha = 1;
    }

    private void ChangeCountFireman()
    {
        _text.text = _house.FiremanCount + "/" + _house.FiremansNumberRequired;
        FillingSlider();

        if (_house.FiremanCount == _house.FiremansNumberRequired)
            StartCoroutine(Deactivate());
    }

    private void FillingSlider()
    {
        float maximimumSliderValue = 1f;
        float targetValue = maximimumSliderValue / _house.FiremansNumberRequired;
        float duration = 0.2f;
        _slider.DOFillAmount((_slider.fillAmount + targetValue), duration).SetEase(Ease.Linear);
    }

    private IEnumerator Deactivate()
    {
        float delay = 0.5f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        yield return waitForSeconds;
        gameObject.active = false;
    }
}
