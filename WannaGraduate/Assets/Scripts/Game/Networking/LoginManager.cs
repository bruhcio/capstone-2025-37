using GooglePlayGames;
using GooglePlayGames.BasicApi;
using I2.Loc;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Assaz
{
    public class LoginManager : MonoBehaviour
	{
        //----------------- Inspector Properties -----------------
        #region Inspector Properties
        #endregion

        //----------------- Methods & Properties -----------------
        #region Methods & Properties
        public static LoginManager Instance { get; private set; }

        static string displayName = "";

        [Button]
        public async void SignIn()
        {
            //StartSceneUI.SetStateText("로그인 중...");

            if (Application.platform == RuntimePlatform.Android)
            {
                PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
            }
            else
            {
                if (!AuthenticationService.Instance.SessionTokenExists)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    Debug.Log("SignIn is successful.");
                }

                NetworkingService.uid = "TESTER";
                //LoadFromCloud();
            }
        }

        private void CheckNickname()
        {
            name = PlayerSaveDataModel.data.playerName;

            if (name == null || name == string.Empty || name == "")
            {
                //StartSceneUI.SetStateText("닉네임 설정 필요!");
                //PopupManager.instance.Popup(StartScenePopupIndex.AskDisplayName_Popup);
            }
            else
            {
                LatestVersionChecker.Instance.CheckLatestVersion();
            }
        }


        public string GetUserDisplayName()
        {
            return PlayerSaveDataModel.data.playerName;
        }

        //GPGS Authenticate()에 전달할 콜백 메서드
        void ProcessAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                //PlayGamesPlatform.Instance.RequestServerSideAccess(false, async authCode => await ProcessServerAuthCode(authCode));

                PlayGamesPlatform.Instance.RequestServerSideAccess(false, async idToken =>
                {
                    string name;
                    if (!AuthenticationService.Instance.SessionTokenExists)
                    {
                        await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(idToken);
                        Debug.Log("SignIn is successful.");
                    }
                    NetworkingService.uid = AuthenticationService.Instance.PlayerId;

                    //LoadFromCloud();
                });
            }
        }

        /*
        async Task ProcessServerAuthCode(string serverAuthCode)
        {
            Debug.Log("Server Auth Code: " + serverAuthCode);

            var request = new LoginWithGooglePlayGamesServicesRequest
            {
                ServerAuthCode = serverAuthCode,
                CreateAccount = true,
                TitleId = PlayFabSettings.TitleId
            };

            PlayFabClientAPI.LoginWithGooglePlayGamesServices(request, OnLoginWithGooglePlayGamesServicesSuccess, OnLoginWithGooglePlayGamesServicesFailure);
        }
        */

        /*
        private static void OnLoginWithGooglePlayGamesServicesSuccess(LoginResult result)
        {
            Debug.Log("PF Login Success LoginWithGooglePlayGamesServices");

            StartSceneUI.SetStateText(LocalizationManager.GetTermTranslation("SignInSuccess"));

            NetworkingService.uid = result.PlayFabId;
            //MixpanelManager.IdetifyUser();
            //GlobalEventManager<EGlobalUIChangedEventTypes>.InvokeEvent(EGlobalUIChangedEventTypes.UUID_Changed);
            //StartSceneManager.instance.OnLoginCompleted();

            PlayFabClientAPI.GetPlayerProfile(new()
            {
                PlayFabId = result.PlayFabId,
            }, (result) =>
            {
                displayName = result.PlayerProfile.DisplayName;

                if (displayName == null || displayName == string.Empty)
                {
                    StartSceneUI.SetStateText("닉네임 설정 필요!");
                    PopupManager.instance.Popup(StartScenePopupIndex.AskDisplayName_Popup);
                }
                else
                {
                    LatestVersionChecker.Instance.CheckLatestVersion();
                }

            }, (error) =>
            {
                StartSceneUI.SetStateText("Error: 닉네임 불러오기 실패");
            });

            //PlayFabClientAPI.UpdateUserTitleDisplayName
        }

        
        private static void OnLoginWithGooglePlayGamesServicesFailure(PlayFabError error)
        {
            Debug.Log("PF Login Failure LoginWithGooglePlayGamesServices: " + error.GenerateErrorReport());

            StartSceneUI.SetStateText("PF Login Failure LoginWithGooglePlayGamesServices: " + error.GenerateErrorReport());
        }
        */
        

        /*
        public void UpdateDisplayName(string toUpdate, Action<UpdateUserTitleDisplayNameResult> result, Action<PlayFabError> error = null)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(new()
            {
                DisplayName = toUpdate,
            },
            (_result) =>
            {
                displayName = _result.DisplayName;
                result(_result);

            }, error);
        }
        */

        /*//GPGS RequestServerSideAccess() 에 전달할 콜백 메서드

        //Unity Auth Service
        async Task ProcessServerAuthCode(string serverAuthCode)
        {
            Debug.Log("Server Auth Code: " + serverAuthCode);

            //For Unity Service Sign In ======
            try
            {
                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(serverAuthCode);

                var name = await AuthenticationService.Instance.GetPlayerNameAsync(false);
                NetworkingService.uid = AuthenticationService.Instance.PlayerId;

                if (name == null || name == string.Empty)
                {
                    StartSceneUI.SetStateText("닉네임 설정 필요!");
                    PopupManager.instance.Popup(StartScenePopupIndex.AskDisplayName_Popup);
                }
                else
                {
                    displayName = name;
                    LatestVersionChecker.Instance.CheckLatestVersion();
                }
            }
            catch (Exception ex)
            {
                StartSceneUI.SetStateText(ex.Message);
            }
        }*/

        public void UpdateDisplayName(string toUpdate, Action onSuccess, Action<string> onError)
        {
            try
            {
                PlayerSaveDataModel.data.playerName = toUpdate;
                PlayerSaveDataModel.Save();
               
                NetworkingService.CreateUserData((data) =>
                {
                    if (data["result"].boolValue)
                    {
                        onSuccess();
                    }
                    else
                    {
                        onError(data["due"].stringValue);
                    }
                });
            }
            catch
            {
                onError("");
            }
        }
        #endregion

        //----------------- Unity -----------------
        #region Unity Methods
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
                return;
            }
        }

        private async void Start()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            SignIn();
        }



        /*
        [Button]
        private void LoadFromCloud()
        {
            StartSceneUI.SetStateText("데이터 로드 중...");

            SaveManager.targetChannelIndex = 0;
            SaveManager.Load();

            // 세이브 파일이 없다면, 클라우드에서 로드
            if (SaveManager.data.nickname == "")
            {
                StartSceneUI.SetStateText("클라우드 로드 중...");
                Debug.Log("Called");
                NetworkingService.LoadUserData(data =>
                {
                    SaveManager.Load();
                    CheckNickname();
                });
            }
            else
            {
                CheckNickname();
            }
        }
        */
        #endregion
    }
}
