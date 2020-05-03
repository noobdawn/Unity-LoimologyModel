using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UDialogContext : _UC
{
    public System.Action<bool> callback;
    public string titleText, contentText, yesText, noText;
    public bool isSingle;
}


public class UDialog : _UP
{
    public override string Name => "UDialog";
    public Button ButtonY, ButtonN;
    public Text Content, TextY, TextN, Title;

    public override void WhenFirstShow()
    {
        if (context == null || !(context is UDialogContext))
        {
            Debug.LogError("Context Error!");
            Exit();
            return;
        }
        var tmpContext = context as UDialogContext;
        TextY.text = tmpContext.yesText;
        TextN.text = tmpContext.noText;
        Title.text = tmpContext.titleText;
        Content.text = tmpContext.contentText;
        ButtonN.gameObject.SetActive(!tmpContext.isSingle);
        ButtonY.onClick.AddListener(OnClickY);
        ButtonN.onClick.AddListener(OnClickN);
        base.WhenFirstShow();
    }

    private void OnClickN()
    {
        var tmpContext = context as UDialogContext;
        if (tmpContext.callback != null)
            tmpContext.callback.Invoke(false);
        Exit();
    }

    private void OnClickY()
    {
        var tmpContext = context as UDialogContext;
        if (tmpContext.callback != null)
            tmpContext.callback.Invoke(true);
        Exit();
    }

}
