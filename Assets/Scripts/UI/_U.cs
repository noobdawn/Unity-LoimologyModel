using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _U : MonoBehaviour
{
    public static _U Inst = null;

    private static _U _inst;

    private Dictionary<int, Canvas> _canvas;
    private Dictionary<string, _UP> _panels;

    private Transform _canvasRoot;

    public void Init()
    {
        if (Inst == null)
            Inst = this;
        // build canvas group
        GameObject cto = GameObject.Find("_Canvas");
        _canvasRoot = cto.transform;
        GetCanvas(0);
    }

    private Canvas GetCanvas(int index)
    {
        if (_canvas == null)
            _canvas = new Dictionary<int, Canvas>();
        if (_canvas.ContainsKey(index))
            return _canvas[index];
        GameObject co = new GameObject(index.ToString());
        co.transform.SetParent(_canvasRoot);
        var result = co.AddComponent<Canvas>();
        _canvas.Add(index, result);
        var rt = result.transform as RectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = Vector2.one * 0.5f;
        rt.anchoredPosition = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.offsetMin = Vector2.zero;
        result.sortingOrder = index;
        co.AddComponent<GraphicRaycaster>();
        return result;
    }

    private void _ClosePanel(_UP panel)
    {
        string name = panel.Name;
        if (_panels == null)
            _panels = new Dictionary<string, _UP>();
        if (!_panels.ContainsKey(name))
            return;
        var panelInDic = _panels[name];
        _panels.Remove(name);
        panelInDic.WhenDestroy();
        Destroy(panelInDic.gameObject);
    }

    public static void ClosePanel(_UP panel)
    {
        Inst._ClosePanel(panel);
    }

    public void _ShowPanel(string panelName, int canvasIdx, _UC context)
    {
        if (_panels == null)
            _panels = new Dictionary<string, _UP>();
        if (_panels.ContainsKey(panelName))
            return;
        string path = "UI/" + panelName;
        var obj = _R.Load(path) as GameObject;
        var panelObj = Instantiate<GameObject>(obj);
        var rt = panelObj.transform as RectTransform;
        var position = rt.anchoredPosition;
        var offsetMax = rt.offsetMax;
        var offsetMin = rt.offsetMin;
        panelObj.transform.SetParent(GetCanvas(canvasIdx).transform);
        rt.anchoredPosition = position;
        rt.offsetMax = offsetMax;
        rt.offsetMin = offsetMin;
        var panel = panelObj.GetComponent<_UP>();
        _panels.Add(panelName, panel);
        panel.context = context;
        panel.WhenFirstShow();
        // TODO 这里给个动画
        panel.WhenShow();
    }

    public static void ShowPanel(string panelName, int canvasIdx, _UC context)
    {
        Inst._ShowPanel(panelName, canvasIdx, context);
    }
}
