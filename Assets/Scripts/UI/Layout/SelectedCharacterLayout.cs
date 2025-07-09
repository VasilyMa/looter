public class SelectedCharacterLayout : SourceLayout
{
    public CharacterContainerData Data;

    public override void OnOpen()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            var slot = _slots[i].GetSlot<SelectedCharacterSlot>();
            slot.CharacterData = Data.Characters[i];
            slot.Index = i;
            slot.UpdateView();
        }

        base.OnOpen();
    } 
}
