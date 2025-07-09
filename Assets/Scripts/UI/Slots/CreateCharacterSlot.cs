public class CreateCharacterSlot : SourceSlot
{
    public CharacterBase Character;

    public override void OnClick()
    {
        PlayerEntity.Instance.CurrentBaseCharacter = Character;
    }

    public override T GetSlot<T>()
    {
        return this as T;
    }

    public override void UpdateView()
    {
        _icon.sprite = Character.Character.CharacterIcon;
    }
}
