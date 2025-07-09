using UnityEngine.UI;
using UnityEngine;

public class SignupWindow : SourceWindow
{
    [SerializeField] private InputField _email;
    [SerializeField] private InputField _login;
    [SerializeField] private InputField _password;
    [SerializeField] private Text _rememberMe;
    [SerializeField] private Button _rememberMeBtn;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _loginBtn;

    bool isRemember;

    public override SourceWindow Init(SourcePanel panel)
    {
        _rememberMeBtn.onClick.AddListener(Remember);
        _nextBtn.onClick.AddListener(Next);
        _loginBtn.onClick.AddListener(Login);

        return base.Init(panel);
    }
    void Next()
    {
        ConfigModule.GetConfig<PlayfabConfig>().RegisterNewUser(_login.text, _email.text, _password.text, isRemember);
    }

    void Login()
    {
        _panel.OpenWindow<LoginWindow>();
    }

    void Remember()
    {
        isRemember = !isRemember;

        if (isRemember)
        {
            _rememberMe.fontStyle = FontStyle.BoldAndItalic;
        }
        else
        {
            _rememberMe.fontStyle = FontStyle.Bold;
        }
    }

    public override void Dispose()
    {
        _loginBtn.onClick.RemoveAllListeners();
        _rememberMeBtn.onClick.RemoveAllListeners();
        _nextBtn.onClick.RemoveAllListeners();
    }
}

