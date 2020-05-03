using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _G : MonoBehaviour
{
    public static _G Inst;
    
    public Camera MainCamera;

    #region Mono
    private void Awake()
    {
        Inst = this;
        // 初始化资源管理器
        _R._Attachment = this;

        // 初始化事件管理器
        _E.Init();

        // 初始化UI管理器
        if (_U.Inst == null)
            _U.Inst = GameObject.Find("_U").GetComponent<_U>();
            _U.Inst.Init();

        // 初始化音频管理器
        // TODO

        // 初始化游戏管理器
        GameObject[] provincesGo = new GameObject[Data.numOfProvince];
        provincesGo[0] = GameObject.Find("Capital");
        for (int i = 0; i < Data.numOfProvince - 1; i++)
            provincesGo[i + 1] = GameObject.Find("Province" + i.ToString());
        _D.Inst = new _D(provincesGo);
        _D.Inst._Attachment = this;

        // 该开菜单了兄
        UMainMenuContext context = new UMainMenuContext();
        context.invoker = null;
        context.isInGame = false;
        _U.ShowPanel("UMainMenu", 0, context);

        // 草，又不切场景，装你妈呢
        Object.DontDestroyOnLoad(gameObject);
        Object.DontDestroyOnLoad(MainCamera.gameObject);
    }


    private void OnDestroy()
    {
        Inst = null;
    }
    #endregion
}
