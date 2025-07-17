using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginWindow : SourceWindow
{
    [SerializeField] Button _btnContinue;
    [SerializeField] TMP_InputField _userName;
    private string _nickname;

    public override SourceWindow Init(SourcePanel panel)
    {
        _btnContinue.onClick.AddListener(onNext);
        _userName.onValueChanged.AddListener(onNickNameChange);
        return base.Init(panel);
    }

    void onNickNameChange(string name) => _nickname = name;
    void onNext() => ConfigModule.GetConfig<PlayfabConfig>().RegisterNewUser(_nickname);

    public override void Dispose()
    {
        _btnContinue.onClick.RemoveAllListeners();
    }
}
