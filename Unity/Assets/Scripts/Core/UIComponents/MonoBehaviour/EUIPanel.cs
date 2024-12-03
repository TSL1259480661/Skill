using UnityEngine;

namespace ET
{
    public enum UIWindowType
    {
        Normal,    // 普通主界面
        Fixed,     // 固定窗口
        PopUp,     // 弹出窗口
        Other,      //其他窗口
    }

    public class EUIPanel : MonoBehaviour
    {
        public UIWindowType WindowType = UIWindowType.Normal;

        public string Des = string.Empty;
    }
}
