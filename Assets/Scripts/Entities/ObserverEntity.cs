using System;

public class ObserverEntity : SourceEntity
{
    public static ObserverEntity Instance;
    public event Action<float, float> PlayerHealthValueChanged;
    public event Action<EffectViewData> PlayerEffectAdded;
    public event Action<EffectViewData> PlayerEffectRemoved;

    public ObserverEntity()
    {
        Instance = this;
    }


    public override void Init()
    {

    }

    public void UpdatePlayerHealthValue(float current, float max)
    {
        PlayerHealthValueChanged?.Invoke(current, max); 
    }

    public void AddPlayerEffectValue(EffectViewData effectData) 
    {
        PlayerEffectAdded?.Invoke(effectData);
    }
    public void RemovePlayerEffectValue(EffectViewData effectData) 
    {
        PlayerEffectAdded?.Invoke(effectData);
    }
}
