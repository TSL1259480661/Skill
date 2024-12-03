using System;
using App;
using UnityEngine;
using UnityEngine.UI;

public class WxApiEditorImpl : WxApilImplBase
{
    private UDebugger debugger = new UDebugger("WxApiEditorImpl");
    public void InitMina()
    {

    }
    public void ShowKeyBoard(InputField input)
    {
        debugger.Log("拉起键盘");
    }

    public void HideKeyBoard()
    {
        debugger.Log("关掉键盘");
    }

    public void HideMinaLoading()
    {
        
    }

    public void ShowMinaLoading(string titleStr = "", bool needShowMask = true)
    {
        
    }

    public void TriggerMinaGC()
    {
        debugger.Log("手动GC");
    }

    public void RestartMiniProgram()
    {
        debugger.Log("重启游戏");
    }

	public void SetKeepScreenOn()
	{
		debugger.Log("屏幕常亮");
	}

	public void GetWXPlatform(Action<string, string, string, string> action)
	{
		debugger.Log("获取当前平台");
	}

	public void GetWXUserInfo(Button authBtn, Action<string, string> action)
	{
		debugger.Log("拉取微信账号信息");
        action?.Invoke("", "");
	}

    public void GetAvatar(string url, Action action)
	{
		debugger.Log("拉取微信头像");
        action?.Invoke();
	}

	public float GetSafeAreaHeight()
	{
		debugger.Log("获取安全高度");
        return 50;
	}

	public void CopyText(string str, Action pCall = null)
	{
		GUIUtility.systemCopyBuffer = str;
        pCall?.Invoke();
	}

	public void ShowShareMenu()
	{
		debugger.Log("拉起wx分享");
	}

	public void HideShareMenu()
	{
		debugger.Log("关闭wx分享");
	}

	public void Login(int timeout, Action<string> success, Action<string> fail, Action complete)
	{
		debugger.Log("wx 登录");
        success?.Invoke("");
	}

	public void CheckSession(Action success, Action fail)
	{
        debugger.Log("检查toke是否过期");
		success?.Invoke();
	}

	public void RequirePrivacyAuthorize(Action success, Action fail)
	{
		debugger.Log("自动弹隐私弹窗");
		success?.Invoke();
	}

	public void OpenPrivacyContract(Action success, Action fail)
	{
		debugger.Log("手动弹隐私弹窗");
		success?.Invoke();
	}
}
