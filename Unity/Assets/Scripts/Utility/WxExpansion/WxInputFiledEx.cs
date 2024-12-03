using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WeChatWASM;

/// <summary>
/// 使用方法：将脚本挂载到对应的InputField下即可
/// </summary>
public class WxInputFiledEx : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public InputField input;
    public void OnPointerClick(PointerEventData eventData)
    {
        ShowKeyboard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log($"OnPointerExit：isFocused ? {input.isFocused}, gameObject:{eventData.pointerCurrentRaycast.gameObject.name}");
        // 双击不会弹出
        if (eventData.pointerCurrentRaycast.gameObject != input.gameObject && !input.isFocused)
        {
            HideKeyboard();
        }
    }

    public void OnInput(OnKeyboardInputListenerResult v)
    {
        if (input.isFocused)
        {
            input.text = v.value;
        }
    }

    public void OnConfirm(OnKeyboardInputListenerResult v)
    {
        // 输入法confirm回调
        HideKeyboard();
    }

    public void OnComplete(OnKeyboardInputListenerResult v)
    {
        // 输入法complete回调
        HideKeyboard();
    }

    private void ShowKeyboard()
    {
        WxApiHelper.ShowKeyBoard(input);
    }

    private void HideKeyboard()
    {
        WxApiHelper.HideKeyBoard();
    }
}


