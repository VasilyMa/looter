using System;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : SourceBar
{
    [SerializeField] private Slider _healthValueSlider;
    private StatusEffectSlotView[] _slots;

    private readonly List<EffectViewData> _activeEffects = new List<EffectViewData>();
    private readonly Dictionary<EffectViewData, StatusEffectSlotView> _effectSlotMap = new Dictionary<EffectViewData, StatusEffectSlotView>();

    public override SourceBar Init(SourcePanel panel)
    {
        ObserverEntity.Instance.PlayerHealthValueChanged += OnHealthChanged;
        ObserverEntity.Instance.PlayerEffectAdded += OnEffectAdded;
        ObserverEntity.Instance.PlayerEffectRemoved += OnEffectRemoved;

        _slots = GetComponentsInChildren<StatusEffectSlotView>(true);

        foreach (var slot in _slots)
        {
            slot.ResetSlot();
        }

        return base.Init(panel);
    }

    private void OnEffectAdded(EffectViewData effect)
    {
        // Находим свободный слот для этого типа эффекта
        var targetSlots = _slots.Where(s =>
            s.IsPositive == effect.IsPositive &&
            !s.IsOccupied).ToArray();

        if (targetSlots.Length == 0)
        {
            // Если нет свободных слотов, ищем самый низкоприоритетный эффект того же типа
            var replaceable = _activeEffects
                .Where(e => e.IsPositive == effect.IsPositive)
                .OrderBy(e => e.Weight)
                .FirstOrDefault();

            if (replaceable != null && effect.Weight > replaceable.Weight)
            {
                RemoveEffect(replaceable);
                AddEffectToSlot(effect);
            }
        }
        else
        {
            AddEffectToSlot(effect);
        }
    }

    private void AddEffectToSlot(EffectViewData effect)
    {
        var freeSlot = _slots.FirstOrDefault(s =>
            s.IsPositive == effect.IsPositive &&
            !s.IsOccupied);

        if (freeSlot != null)
        {
            freeSlot.Data = effect;
            freeSlot.UpdateView();
            _activeEffects.Add(effect);
            _effectSlotMap[effect] = freeSlot; 
        }
    }

    private void OnEffectRemoved(EffectViewData effect)
    {
        RemoveEffect(effect);
    }

    private void RemoveEffect(EffectViewData effect)
    {
        if (_effectSlotMap.TryGetValue(effect, out var slot))
        {
            slot.ResetSlot();
            _activeEffects.Remove(effect);
            _effectSlotMap.Remove(effect);
        }
    }

    private void OnHealthChanged(float current, float max)
    {
        float value = current / max;
        _healthValueSlider.value = value;
    }

    public override void Dispose()
    {
        foreach (var effect in _activeEffects.ToArray())
        {
            RemoveEffect(effect);
        }

        ObserverEntity.Instance.PlayerHealthValueChanged -= OnHealthChanged;
        ObserverEntity.Instance.PlayerEffectAdded -= OnEffectAdded;
        ObserverEntity.Instance.PlayerEffectRemoved -= OnEffectRemoved;
    }
}

    public class EffectViewData
{
    public event Action<float, float> OnRemainingChange;
    public Sprite Icon; 
    public bool IsPositive;
    public int Weight;

    public void UpdateRemaining(float current, float max)
    { 
        OnRemainingChange?.Invoke(current, max);
    }
}