using System;
using App;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class WxApiImpl : WxApilImplBase
{
    private UDebugger debugger = new UDebugger("WxApiImpl");
    private InputField curInput = null;
    private bool isShowKeyboard = false;
    private static WXUserInfoButton userInfoBtn;
    public void InitMina()
    {
        // 微信生命周期监听
        WX.OnShow(OnMinaShow);
        WX.OnHide(OnMinaHide);
    }
    public void ShowKeyBoard(InputField input)
    {
        if (!isShowKeyboard) 
        {
            curInput = input;
            ShowKeyboardOption options = new ShowKeyboardOption();
            if (input.text != "")
            {
                options.defaultValue = input.text;
            }
            else 
            {
                options.defaultValue = "";
            }
            options.maxLength = 12;
            options.confirmType = "done";
            WX.ShowKeyboard(options);
            //绑定回调
            WX.OnKeyboardConfirm(OnKeyBoardConfirm);
            WX.OnKeyboardComplete(OnKeyBoardComplete);
            WX.OnKeyboardInput(OnKeyBoardInput);
            isShowKeyboard = true;
        }
    }

    public void HideKeyBoard()
    {
        if (isShowKeyboard)
        {
            WX.HideKeyboard(new HideKeyboardOption());
            WX.OffKeyboardConfirm(OnKeyBoardConfirm);
            WX.OffKeyboardComplete(OnKeyBoardComplete);
            WX.OffKeyboardInput(OnKeyBoardInput);
            curInput = null;
            isShowKeyboard = false;
        }
    }
    public void ShowMinaLoading(string titleStr = "", bool needShowMask = true)
    {
        ShowLoadingOption loadingOption = new ShowLoadingOption();
        loadingOption.title = titleStr;
        loadingOption.mask = needShowMask;
        WX.ShowLoading(loadingOption);
    }

    public void HideMinaLoading()
    {
        HideLoadingOption options = new HideLoadingOption();
        WX.HideLoading(options);
    }

    public void TriggerMinaGC()
    {
        WX.TriggerGC();
    }


    /// <summary>
    /// 小游戏回到前台事件
    /// </summary>
    /// <param name="result"></param>
    private void OnMinaShow(OnShowListenerResult result)
    {
        // TODO 抛事件
        Debug.Log("微信切换后台");
    }

    /// <summary>
    /// 小游戏切到后台（杀进程不能识别）
    /// </summary>
    /// <param name="result"></param>
    private void OnMinaHide(GeneralCallbackResult result)
    {
        // TODO 抛事件
        Debug.Log("微信切换前台");
    }

    private void OnKeyBoardConfirm(OnKeyboardInputListenerResult v) { HideKeyBoard(); }
    private void OnKeyBoardComplete(OnKeyboardInputListenerResult v) { HideKeyBoard(); }
    private void OnKeyBoardInput(OnKeyboardInputListenerResult v) 
    {
        if (curInput != null && curInput.isFocused)
        {
            curInput.text = v.value;
        }
    }

    public void RestartMiniProgram()
    {
        WX.RestartMiniProgram(null);
    }

	public void SetKeepScreenOn()
	{
		WX.SetKeepScreenOn(new SetKeepScreenOnOption() { keepScreenOn = true });
	}


    private void DestroyUserInfoBtn()
    {
        if (userInfoBtn != null)
        {
            userInfoBtn.Destroy();
            userInfoBtn = null;
        }
    }

    private void ShowUserInfoBtn(bool isShow)
    {
        if (userInfoBtn == null) return;
        if (isShow) userInfoBtn.Show();
        else userInfoBtn.Hide();
    }

	public void GetWXPlatform(Action<string, string, string, string> action)
	{
		DeviceInfo dev = WX.GetDeviceInfo();
        if (dev != null) action?.Invoke(dev.platform, dev.system, dev.brand, dev.model);
	}

	public void GetWXUserInfo(Button authBtn, Action<string, string> action)
	{
		GetSettingOption setTingOp = new GetSettingOption();
        setTingOp.success = (e) =>
        {
            if (!e.authSetting.ContainsKey("scope.userInfo") || !e.authSetting["scope.userInfo"])
            {
                Camera uiCamera = GameObject.Find("Global/UICamera").GetComponent<Camera>();
                Vector3 vec = uiCamera.WorldToScreenPoint(authBtn.transform.position);
                RectTransform rect = authBtn.transform as RectTransform;
                debugger.Log("harry   " + vec + "   " + rect.rect.width + "   " + rect.rect.height);
                int x = (int)(vec.x - (rect.rect.width / 2f));
                int y = (int)((Screen.height - vec.y) - (rect.rect.height / 2f));
                //调用请求获取用户信息
                userInfoBtn = WX.CreateUserInfoButton(x, y, (int)rect.rect.width, (int)rect.rect.height, "zh_CN", false);
                ShowUserInfoBtn(true);
                userInfoBtn.OnTap((data) =>
                {
                    if (data.errCode == 0)
                    {
                        //用户已允许获取个人信息，返回的data即为用户信息
                        action?.Invoke(data.userInfo.nickName, data.userInfo.avatarUrl);
                        DestroyUserInfoBtn();
                    }
                    else
                    {
                        action?.Invoke(null, null);
                        //用户未允许获取个人信息
                    }
                    debugger.Log($"harry   {data.errCode}");
                });
            }
            else
            {
                debugger.Log("已经同意权限");
                //直接获取用户信息
                GetUserInfoOption userInfo = new GetUserInfoOption()
                {
                    withCredentials = false,
                    lang = "zh_CN",
                    success = (data) =>
                    {
                        debugger.Log("harry成功   " + data.errMsg);
                        action?.Invoke(data.userInfo.nickName, data.userInfo.avatarUrl);
                    },
                    complete = (data) =>
                    {
                        debugger.Log("harry完成   "+data.errMsg);
                    },
                    fail = (data) =>
                    {
                        debugger.Log("harry失败   " + data.errMsg);
                    }
                };
                WX.GetUserInfo(userInfo);
            }
        };
        WX.GetSetting(setTingOp);
	}

    public void GetAvatar(string url, Action action)
    {
        // Sprite sprite = null;
        // UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
        // await req.SendWebRequest();
        // if (req.result == UnityWebRequest.Result.Success)
        // {
        //     Texture2D texture = DownloadHandlerTexture.GetContent(req);
        //     sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        // }
        // else
        // {
        //     Log.Error("DownLoadAvatar   " + req.error);
        // }
        // req.Dispose();
        // return sprite;
    }

	public float GetSafeAreaHeight()
	{
		float safearea = 0;
        WeChatWASM.SystemInfo systemInfo = WX.GetSystemInfoSync();
        if (systemInfo.statusBarHeight > 20)
        {
            safearea = (float)systemInfo.statusBarHeight + 20;
        }
        Debug.Log($"safearea={safearea}");
        return safearea;
	}

	public void CopyText(string str, Action pCall = null)
	{
		WX.SetClipboardData(new SetClipboardDataOption()
        {
            data = str,
            success = (result) => { pCall?.Invoke(); },
            complete = (result) => { debugger.Log("复制完成" + result.errMsg); },
            fail = (result) => { debugger.Log("复制失败" + result.errMsg); },
        });
	}

	public void ShowShareMenu()
	{
		WX.ShowShareMenu(new ShowShareMenuOption() { menus = new string[] { "shareAppMessage", "shareTimeline" } });
	}

	public void HideShareMenu()
	{
		WX.HideShareMenu(new HideShareMenuOption() { menus = new string[] { "shareAppMessage", "shareTimeline" } });
	}

	public void Login(int timeout, Action<string> success, Action<string> fail, Action complete)
	{
        LoginOption option = new LoginOption();
        option.timeout = timeout;

        Action<LoginSuccessCallbackResult> successCb = (rsp) => {
            success?.Invoke(rsp.code);
        };
        option.success = successCb;

        Action<RequestFailCallbackErr> failCb = (rsp) => {
            fail?.Invoke(rsp.errMsg);
        };
        option.fail = failCb;

        Action<GeneralCallbackResult> completeCb = (rsp) => {
            complete?.Invoke();
        };
        option.complete = completeCb;

		WX.Login(option);
	}

	public void CheckSession(Action success, Action fail)
	{
        CheckSessionOption option = new CheckSessionOption();
        Action<GeneralCallbackResult> successCb = (rsp) => {
            success?.Invoke();
        };
        Action<GeneralCallbackResult> failCb = (rsp) => {
            fail?.Invoke();
        };
        option.success =successCb;
        option.fail = failCb;
		WX.CheckSession(option);
	}

	public void RequirePrivacyAuthorize(Action success, Action fail)
	{
        RequirePrivacyAuthorizeOption option = new RequirePrivacyAuthorizeOption();
        Action<GeneralCallbackResult> successCb = (rsp) => {
            success?.Invoke();
        };
         Action<GeneralCallbackResult> failCb = (rsp) => {
            fail?.Invoke();
        };
        option.success = successCb;
        option.fail = failCb;
		WX.RequirePrivacyAuthorize(option);
	}

	public void OpenPrivacyContract(Action success, Action fail)
	{
        OpenPrivacyContractOption option = new OpenPrivacyContractOption();
         Action<GeneralCallbackResult> successCb = (rsp) => {
            success?.Invoke();
        };
         Action<GeneralCallbackResult> failCb = (rsp) => {
            fail?.Invoke();
        };
        option.success = successCb;
        option.fail = failCb;
		WX.OpenPrivacyContract(option);
	}
}
