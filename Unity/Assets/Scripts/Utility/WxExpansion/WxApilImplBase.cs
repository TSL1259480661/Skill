using System;
using UnityEngine.UI;

public interface WxApilImplBase
{
    public abstract void InitMina();
    public abstract void Login(int timeout, Action<string> success, Action<string> fail, Action complete);
    public abstract void CheckSession(Action success, Action fail);
    public abstract void ShowKeyBoard(InputField input);

    public abstract void HideKeyBoard();

    public abstract void ShowMinaLoading(string titleStr = "", bool needShowMask = true);

    public abstract void HideMinaLoading();
    
    public abstract void TriggerMinaGC();

    public abstract void RestartMiniProgram();

    public abstract void SetKeepScreenOn();

    public abstract void GetWXPlatform(Action<string, string, string, string> action);

    public abstract void GetWXUserInfo(Button authBtn, Action<string, string> action);

    public abstract void GetAvatar(string url, Action action);

    public abstract float GetSafeAreaHeight();

    public abstract void CopyText(string str, Action pCall = null);

    public abstract void ShowShareMenu();

    public abstract void HideShareMenu();

    /// <summary>
    /// 自动拉起隐私弹窗
    /// </summary>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    public abstract void RequirePrivacyAuthorize(Action success, Action fail);
    /// <summary>
    /// 手动打开
    /// </summary>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    public abstract void OpenPrivacyContract(Action success, Action fail);
}
