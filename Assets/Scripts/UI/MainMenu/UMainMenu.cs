using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UMainMenuContext : _UC
{
    /// <summary>
    /// 是不是在游戏过程中呼出的，涉及到保存是否可用
    /// </summary>
    public bool isInGame;
}

public class UMainMenu : _UP
{
    public override string Name => "UMainMenu";
    public Button ButtonStart, ButtonExit;

    public override void WhenFirstShow()
    {
        if (context == null || !(context is UMainMenuContext))
        {
            Debug.LogError("Context Error!");
            Exit();
            Application.Quit();
            return;
        }
        var tmpContext = context as UMainMenuContext;
        if (tmpContext.isInGame)
        {
            // TODO
        }
        ButtonStart.onClick.AddListener(OnClickStart);
        ButtonExit.onClick.AddListener(OnClickExit);
        base.WhenFirstShow();
    }

    private void OnClickStart()
    {
        if (_D.Inst == null)
        {
            Exit();
            Application.Quit();
            return;
        }
        _D.Inst.StartNewGame();
        Exit();
    }

    private void OnClickExit()
    {
        UDialogContext context = new UDialogContext();
        context.callback = (b) =>
        {
            if (b) Application.Quit();
        };
        context.titleText = "提示";
        context.noText = "否";
        context.yesText = "是";
        context.contentText = "若在游戏中，将丢失未保存数据！";
        context.invoker = this;
        context.isSingle = false;
        _U.ShowPanel("UDialog", 9, context);
    }
}
