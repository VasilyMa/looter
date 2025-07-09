using System;
using System.Collections;
using System.Xml;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;

[CreateAssetMenu(fileName = "Playfab", menuName = "Config/Playfab")]
public class PlayfabConfig : Config
{
    public event Action OnRegistrationComplete;
    public event Action OnLoginSuccess;
    public event Action<string> OnLoginError;

    public const string playerPrefsKey = "PlayfabUniqueID";
    public const string passKey = "passKey";
    public const string emailKey = "emailKey";
    public const string rememberKey = "remember";
    [HideInInspector] public bool isConnectAndReady;

    public readonly string UniqueID;

    public override IEnumerator Init()
    {
        isConnectAndReady = false;

        if (PlayerPrefs.HasKey(rememberKey))
        {
            if (PlayerPrefs.HasKey(playerPrefsKey))
            {
                string uniqueID = PlayerPrefs.GetString(playerPrefsKey);

                var request = new LoginWithCustomIDRequest
                {
                    CustomId = uniqueID,
                    CreateAccount = false // ������� ��� ����������
                };

                PlayFabClientAPI.LoginWithCustomID(request, result =>
                {
                    isConnectAndReady = true;

                    PlayerEntity.Instance.PlayfabCustomID = uniqueID;

                }, error =>
                {
                    Debug.LogError("������ �����: " + error.GenerateErrorReport());
                    OnLoginError?.Invoke(error.GenerateErrorReport());
                });
            }
            else
            {
                Statement.State.Instance.GetCanvas<LoginCanvas>().OpenPanel<LoginPanel>().OpenWindow<LoginWindow>();
            }
        }
        else
        {
            Statement.State.Instance.GetCanvas<LoginCanvas>().OpenPanel<LoginPanel>().OpenWindow<LoginWindow>();
        }

        yield return new WaitUntil(() => isConnectAndReady);

        Statement.State.Instance.GetCanvas<LoginCanvas>().ClosePanel<LoginPanel>();
    }
    public void RegisterNewUser(string nickname, string email, string password, bool rememberMe = false)
    {
        if (rememberMe)
        {
            PlayerPrefs.SetInt(rememberKey, 1);
        }
        else
        {
            PlayerPrefs.SetInt(rememberKey, 0);
        }

        // ���������� ���������� ID ��� �������� �����
        string customId = Guid.NewGuid().ToString();

        // 1. ������� ������������ ����� email � ������
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            Username = nickname,
            RequireBothUsernameAndEmail = true
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, registerResult =>
        {
            Debug.Log("Email ����������� �������");

            // 2. ����� �������� ����������� ����������� CustomID
            var linkRequest = new LinkCustomIDRequest
            {
                CustomId = customId,
                ForceLink = false
            };

            PlayFabClientAPI.LinkCustomID(linkRequest, linkResult =>
            {
                Debug.Log("CustomID ������� ��������");

                UpdateDisplayName(nickname);

                PlayerPrefs.SetString(playerPrefsKey, customId);
                PlayerPrefs.Save();
                PlayerEntity.Instance.PlayfabCustomID = customId;
                PlayerEntity.Instance.PlayerNickName = nickname;

                OnRegistrationComplete?.Invoke();
                isConnectAndReady = true;

            }, linkError =>
            {
                Debug.LogError("������ �������� CustomID: " + linkError.GenerateErrorReport());
            });

        }, registerError =>
        {
            Debug.LogError("������ �����������: " + registerError.GenerateErrorReport());

            // ���� ������������ ��� ����������, ������� ����� � ��������� CustomID
            if (registerError.Error == PlayFabErrorCode.EmailAddressNotAvailable)
            {
                TryLoginAndLinkCustomId(email, password, customId, rememberMe);
            }
        });
    }
    public void LoginWithEmail(string email, string password, bool rememberMe = false)
    {
        if (rememberMe)
        {
            PlayerPrefs.SetString(passKey, password);
            PlayerPrefs.SetString(emailKey, email);
            PlayerPrefs.SetInt(rememberKey, 1);
        }
        else
        {
            PlayerPrefs.SetInt(rememberKey, 0);
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true,
                GetPlayerProfile = true,
                ProfileConstraints = new PlayerProfileViewConstraints
                {
                    ShowDisplayName = true
                }
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, result =>
        {
            PlayerEntity.Instance.PlayfabCustomID = result.InfoResultPayload.AccountInfo?.CustomIdInfo?.CustomId;

            if (!PlayerPrefs.HasKey(playerPrefsKey))
            {
                PlayerPrefs.SetString(playerPrefsKey, PlayerEntity.Instance.PlayfabCustomID);
            }

            Debug.Log("Email login successful");
            isConnectAndReady = true;
            OnLoginSuccess?.Invoke();
        }, 
        error =>
        {
            string errorMessage = ProcessLoginError(error);
            Debug.LogError("Email login failed: " + error.GenerateErrorReport());
            OnLoginError?.Invoke(errorMessage);
        });
    }
    private void TryLoginAndLinkCustomId(string email, string password, string customId, bool rememberMe)
    {
        var loginRequest = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, loginResult =>
        {
            // ����������� CustomID ����� �����
            var linkRequest = new LinkCustomIDRequest
            {
                CustomId = customId,
                ForceLink = false
            };

            PlayFabClientAPI.LinkCustomID(linkRequest, linkResult =>
            {
                Debug.Log("CustomID ������� �������� � ������������� ��������");

                if (rememberMe)
                {
                    PlayerPrefs.SetString(playerPrefsKey, customId);
                    PlayerPrefs.Save();
                }

                OnRegistrationComplete?.Invoke();

                isConnectAndReady = true;
            }, linkError =>
            {
                Debug.LogError("������ �������� CustomID: " + linkError.GenerateErrorReport());
            });

        }, loginError =>
        {
            Debug.LogError("������ �����: " + loginError.GenerateErrorReport());
        });
    }
    public void QuickLogin(Action onSuccess, Action<string> onError)
    {
        if (!PlayerPrefs.HasKey(playerPrefsKey))
        {
            onError?.Invoke("No saved login data");
            return;
        }

        string customId = PlayerPrefs.GetString(playerPrefsKey);

        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            onSuccess?.Invoke();
        }, error =>
        {
            onError?.Invoke(error.GenerateErrorReport());
        });
    }
    private string ProcessLoginError(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
                return "Invalid email";
            case PlayFabErrorCode.InvalidPassword:
                return "Invalid password";
            case PlayFabErrorCode.AccountNotFound:
                return "Account not avaible";
            default:
                return $"Server error: {error.GenerateErrorReport()}";
        }
    }
    void UpdateDisplayName(string nickname)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nickname
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
        {
            Debug.Log("������� �������: " + nickname);
        }, error =>
        {
            Debug.LogError("������ ���������� ��������: " + error.GenerateErrorReport());
        });
    }
}

