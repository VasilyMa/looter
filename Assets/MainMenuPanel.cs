using Statement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : SourcePanel
{
    [SerializeField] Button _btnNext;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

        _btnNext.onClick.AddListener(onNext);
    }

    void onNext() => MainMenuState.Instance.StartMatchmaking();

    public override void OnDipose()
    {
        base.OnDipose();

        _btnNext.onClick.RemoveAllListeners();
    }
}
