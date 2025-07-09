using UnityEngine;
using UnityEngine.UI;

public class LoginWindow : SourceWindow
{
    [SerializeField] private InputField _email;
    [SerializeField] private InputField _password;
    [SerializeField] private Text _rememberMe;
    [SerializeField] private Button _rememberMeBtn;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _signupBtn;

    bool isRemember;

    public override SourceWindow Init(SourcePanel panel)
    {
        _rememberMeBtn.onClick.AddListener(Remember);
        _nextBtn.onClick.AddListener(Next);
        _signupBtn.onClick.AddListener(Signup);
        return base.Init(panel);
    }

    public override void OnOpen()
    {
        if (PlayerPrefs.HasKey(PlayfabConfig.passKey))
        {
            _password.text = PlayerPrefs.GetString(PlayfabConfig.passKey);
        }

        if (PlayerPrefs.HasKey(PlayfabConfig.emailKey))
        {
            _email.text = PlayerPrefs.GetString(PlayfabConfig.emailKey);
        }

        if (PlayerPrefs.GetInt(PlayfabConfig.rememberKey, 0) == 0)
        {
            _rememberMe.fontStyle = FontStyle.Bold;
        }
        else
        {
            _rememberMe.fontStyle = FontStyle.BoldAndItalic;
        }

        base.OnOpen();
    }

    void Next()
    {
        ConfigModule.GetConfig<PlayfabConfig>().LoginWithEmail(_email.text, _password.text, isRemember);
    }
    
    void Signup()
    {
        _panel.OpenWindow<SignupWindow>();
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
        _signupBtn.onClick.RemoveAllListeners();
        _rememberMeBtn.onClick.RemoveAllListeners();
        _nextBtn.onClick.RemoveAllListeners();
    }
}
