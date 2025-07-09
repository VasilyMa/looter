using UnityEngine;
using UnityEngine.UI;

public class StatusEffectSlotView : SourceSlot
{
    public EffectViewData Data;
    [SerializeField] private Image _progressFill;
    [SerializeField] private Image _iconNegative;
    [SerializeField] private Image _iconPositive;
    [SerializeField] private Transform _positiveView;
    [SerializeField] private Transform _negativeView;

    public bool IsPositive { get; private set; }
    public bool IsOccupied { get; private set; }


    public override T GetSlot<T>()
    {
        return this as T;
    }

    public override void OnClick()
    {
        return;
    }

    public override void UpdateView()
    {
        IsPositive = Data.IsPositive;
        IsOccupied = true;

        if (IsPositive)
        {
            _iconPositive.gameObject.SetActive(true);
            _iconNegative.gameObject.SetActive(false);
            _iconPositive.sprite = Data.Icon;
            _positiveView.gameObject.SetActive(true);
            _negativeView.gameObject.SetActive(false);
        }
        else
        {
            _iconPositive.gameObject.SetActive(false);
            _iconNegative.gameObject.SetActive(true);
            _iconNegative.sprite = Data.Icon;
            _positiveView.gameObject.SetActive(false);
            _negativeView.gameObject.SetActive(true);
        }

        _progressFill.gameObject.SetActive(true);
        Data.OnRemainingChange += UpdateRemaining;
    }

    public void UpdateRemaining(float current, float max)
    {
        _progressFill.fillAmount = current / max;
    }

    public void ResetSlot()
    {
        IsOccupied = false;
        Data.OnRemainingChange -= UpdateRemaining;
        _progressFill.gameObject.SetActive(false);
        Data = null;
        gameObject.SetActive(false);
    }
}
