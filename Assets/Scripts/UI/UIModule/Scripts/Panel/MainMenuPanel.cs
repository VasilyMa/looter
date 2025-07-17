using UnityEngine;
using UnityEngine.UI;
using Statement;

public class MainMenuPanel : SourcePanel
{
    [SerializeField] private Button _btnStart;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnExit;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

        _btnStart.onClick.AddListener(onNext);
    }
    void onNext() => MainMenuState.Instance.StartMatchmaking();

    void onSettingsOpen()
    {

    }
}
