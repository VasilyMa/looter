public class CreateCharacterLayout : SourceLayout
{
    public CharacterContainerData Data;

    public void UpdateCharacterSlots()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            var slotData = _slots[i].GetSlot<CreateCharacterSlot>(); 
            _slots[i].UpdateView();
        }
    }
}
