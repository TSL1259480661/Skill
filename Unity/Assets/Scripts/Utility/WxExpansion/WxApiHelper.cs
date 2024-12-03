using System;
using App;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// WX相关接口都由该Helper转发，Code不直接调用 WX.
/// WX相关接口，webgl直接预览会报错 转换小游戏再预览
/// </summary>
/// 
public static class WxApiHelper 
{
    private static UDebugger debugger = new UDebugger("WxApiHelper");
    private static WxApilImplBase curWxApiImpl;
    public static void InitMina()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        curWxApiImpl = new WxApiImpl();
#else
        curWxApiImpl = new WxApiEditorImpl();
#endif
        curWxApiImpl.InitMina();
    }

    /// <summary>
    /// 拉起小游戏的Loading
    /// </summary>
    /// <param name="titleStr"></param>
    /// <param name="needShowMask"></param>
    public static void ShowMinaLoading(string titleStr = "", bool needShowMask = true) { curWxApiImpl.ShowMinaLoading(titleStr, needShowMask); }

    /// <summary>
    /// 关闭小游戏的Loading
    /// </summary>
    public static void HideMinaLoading() { curWxApiImpl.HideMinaLoading(); }

    /// <summary>
    /// 拉起键盘
    /// </summary>
    /// <param name="input">unity输入框</param>
    public static void ShowKeyBoard(InputField input) { curWxApiImpl.ShowKeyBoard(input); }

    /// <summary>
    /// 关闭键盘
    /// </summary>
    public static void HideKeyBoard() { curWxApiImpl.HideKeyBoard(); }


    /// <summary>
    /// 手动触发GC
    /// </summary>
    public static void TriggerMinaGC() { curWxApiImpl.TriggerMinaGC(); }

    public static void SetKeepScreenOn() { curWxApiImpl.SetKeepScreenOn(); }

    public static void GetWXPlatform(Action<string, string, string, string> action) { curWxApiImpl.GetWXPlatform(action); }

    public static void GetWXUserInfo(Button authBtn, Action<string, string> action) { curWxApiImpl.GetWXUserInfo(authBtn, action); }

    public static void GetAvatar(string url, Action action) { curWxApiImpl.GetAvatar(url, action); }

    public static float GetSafeAreaHeight() { return curWxApiImpl.GetSafeAreaHeight(); }

    public static void CopyText(string str, Action pCall = null) { curWxApiImpl.CopyText(str, pCall); }

    public static void ShowShareMenu() { curWxApiImpl.ShowShareMenu(); }

    public static void HideShareMenu() { curWxApiImpl.HideShareMenu(); }

    public static void RestartMiniProgram() { curWxApiImpl.RestartMiniProgram(); }
    public static void SetTopSafeArea(Transform target) 
    {
#if !UNITY_EDITOR
        RectTransform rectTrans = target.GetComponent<RectTransform>();
        rectTrans.offsetMax = new Vector2(rectTrans.offsetMax.x, rectTrans.offsetMax.y - WxApiHelper.GetSafeAreaHeight());
#endif
    }

    /// <summary>
    /// 微信登录
    /// </summary>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    public static void WxLogin(Action<string> success, Action<string> fail)
    {
        curWxApiImpl.Login(30, success, fail, null);
    }

    /// <summary>
    /// 检查token过期
    /// </summary>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    public static void CheckSession(Action success, Action fail)
    {
        curWxApiImpl.CheckSession(success, fail);
    }

    public static void WxPrivacyAuthorize(Action success, Action fail)
    {
        curWxApiImpl.RequirePrivacyAuthorize(success, fail);
    }

    public static void WxOpenPrivacy(Action success, Action fail)
    {
        curWxApiImpl.OpenPrivacyContract(success, fail);
    }
}

