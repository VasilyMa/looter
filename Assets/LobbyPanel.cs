using Statement;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : SourcePanel
{
    [SerializeField] Button _btnNext;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

        _btnNext.onClick.AddListener(onNext);
    }

    void onNext() => LobbyState.Instance.StartMatchmaking();

    public override void OnDipose()
    {
        base.OnDipose();

        _btnNext.onClick.RemoveAllListeners();
    }
}
