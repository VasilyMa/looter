using System;

using PlayFab;
using PlayFab.ClientModels;

public class SelectedCharacterPanel : SourcePanel
{
    public override void OnOpen(params Action[] onComplete)
    {
        var combinedCallbacks = AddCallback(onComplete, LoadCharacters);

        base.OnOpen(combinedCallbacks);
    }

    void LoadCharacters()
    {
        StartCoroutine(SaveModule.LoadFromPlayFab<CharacterContainerData>(onComplete, onSuccsess, onFailure));
    }

    void onComplete(CharacterContainerData data)
    {
        var layout = GetLayout<SelectedCharacterLayout>();

        layout.Data = data;
    }

    void onSuccsess(GetUserDataResult result)
    {
        UpdateSlots();
    }

    void onFailure(PlayFabError error)
    {
        //To do send message to player
    }

    void UpdateSlots()
    {
        OpenLayout<SelectedCharacterLayout>();
    } 
}
