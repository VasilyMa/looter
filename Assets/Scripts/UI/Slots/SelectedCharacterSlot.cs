using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectedCharacterSlot : SourceSlot
{
    protected RawImage _characterImage;
    public CharacterData CharacterData;
    public int Index;

    public override SourceSlot Init(SourceLayout layout)
    {
        _characterImage = GetComponentInChildren<RawImage>();

        return base.Init(layout);
    }

    public override void OnClick()
    {
        Debug.Log($"Click slot {CharacterData.CHARACTER_KEY_ID}");

        PlayerEntity.Instance.CurrentCharacterSlotIndex = Index;

        if (CharacterData == null || string.IsNullOrEmpty(CharacterData.CHARACTER_KEY_ID))
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            var charactersConfig = ConfigModule.GetConfig<CharacterConfig>();

            if (charactersConfig.TryGetCharacter(CharacterData.CHARACTER_KEY_ID, out var characterBase))
            {
                PlayerEntity.Instance.CurrentPlayerCharacter = characterBase.LoadCharacter(CharacterData);
                SceneManager.LoadScene(3);
            }
        }
    }

    public override T GetSlot<T>()
    {
        return this as T;
    }

    public override void UpdateView()
    {
        var interfaceConfig = ConfigModule.GetConfig<InterfaceConfig>();

        if (CharacterData == null || string.IsNullOrEmpty(CharacterData.CHARACTER_KEY_ID))
        {
            _icon.sprite = interfaceConfig.AddCharacterIcon;
            _icon.enabled = true;
        }
        else
        {
            //todo view character
            //_characterImage.enabled = true;
        }

        _loading.Stop();
        _loading.gameObject.SetActive(false);
    }
}
