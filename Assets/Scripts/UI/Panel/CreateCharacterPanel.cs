using System;

using Statement;

using UnityEngine;
using UnityEngine.UI;

public class CreateCharacterPanel : SourcePanel
{
    [SerializeField] protected Button _btnNext;
    [SerializeField] protected Button _btnBack;

    public override void Init(SourceCanvas canvasParent)
    {
        _btnNext.onClick.AddListener(Next);
        _btnBack.onClick.AddListener(Back);

        base.Init(canvasParent);
    }

    public override void OnOpen(params Action[] onComplete)
    {
        OpenLayout<CreateCharacterLayout>().OnOpen();

        base.OnOpen(onComplete);
    } 
    void Next()
    {
        PlayerEntity.Instance.CreateNewCharacter();

        State.Instance.RunCoroutine(SaveModule.SaveToPlayFab<CharacterContainerData>(onComplete, onFailure));
    }

    void onComplete(PlayFab.ClientModels.UpdateUserDataResult data)
    {
        Statement.State.Instance.RequestLoadingScene(3);
    }

    void onFailure(PlayFab.PlayFabError error)
    {

    }

    void Back()
    {
        Statement.State.Instance.RequestLoadingScene(1);
    }

    public override void OnDipose()
    {
        _btnNext.onClick.RemoveAllListeners();
        _btnBack.onClick.RemoveAllListeners();
        base.OnDipose();
    }
}
