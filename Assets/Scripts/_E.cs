using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件管理器
/// </summary>
public static class _E
{
    public static Dictionary<EventID, Delegate> eventDic;
    public static void Init()
    {
        eventDic = new Dictionary<EventID, Delegate>();
    }
    public static void ClearListener()
    {
        eventDic.Clear();
    }

    #region Trigger
    public static void TriggerEvent(EventID id)
    {
        if (!eventDic.ContainsKey(id))
        {
            Debug.Log("触发事件失败，该事件未被注册 - " + id.ToString());
            return;
        }
        Delegate[] ds = eventDic[id].GetInvocationList();
        foreach (var d in ds)
        {
            Action a = d as Action;
            a();
        }
    }
    public static void TriggerEvent<T>(EventID id, T arg1)
    {
        if (!eventDic.ContainsKey(id))
        {
            Debug.Log("触发事件失败，该事件未被注册 - " + id.ToString());
            return;
        }
        Delegate[] ds = eventDic[id].GetInvocationList();
        foreach (var d in ds)
        {
            Action<T> a = d as Action<T>;
            a(arg1);
        }
    }
    public static void TriggerEvent<T, U>(EventID id, T arg1, U arg2)
    {
        if (!eventDic.ContainsKey(id))
        {
            Debug.Log("触发事件失败，该事件未被注册 - " + id.ToString());
            return;
        }
        Delegate[] ds = eventDic[id].GetInvocationList();
        foreach (var d in ds)
        {
            Action<T, U> a = d as Action<T, U>;
            a(arg1, arg2);
        }
    }
    public static void TriggerEvent<T, U, V>(EventID id, T arg1, U arg2, V arg3)
    {
        if (!eventDic.ContainsKey(id))
        {
            Debug.Log("触发事件失败，该事件未被注册 - " + id.ToString());
            return;
        }
        Delegate[] ds = eventDic[id].GetInvocationList();
        foreach (var d in ds)
        {
            Action<T, U, V> a = d as Action<T, U, V>;
            a(arg1, arg2, arg3);
        }
    }
    public static void TriggerEvent<T, U, V, W>(EventID id, T arg1, U arg2, V arg3, W arg4)
    {
        if (!eventDic.ContainsKey(id))
        {
            Debug.Log("触发事件失败，该事件未被注册 - " + id.ToString());
            return;
        }
        Delegate[] ds = eventDic[id].GetInvocationList();
        foreach (var d in ds)
        {
            Action<T, U, V, W> a = d as Action<T, U, V, W>;
            a(arg1, arg2, arg3, arg4);
        }
    }
    #endregion

    #region Add & Remove
    private static void OnAddListener(EventID id, Delegate listener)
    {
        if (!eventDic.ContainsKey(id))
        {
            eventDic.Add(id, null);
        }
        Delegate d = eventDic[id];
        if (d != null && d.GetType() != listener.GetType())
        {
            Debug.LogError(string.Format(
                   "{0}事件添加失败，字典里的回调是{1}，试图添加的回调是{2}！",
                   id, d.GetType().Name, listener.GetType().Name));
        }
    }
    public static void AddListener(EventID id, Action d)
    {
        OnAddListener(id, d);
        eventDic[id] = (Action)eventDic[id] + d;
    }
    public static void AddListener<T>(EventID id, Action<T> d)
    {
        OnAddListener(id, d);
        eventDic[id] = (Action<T>)eventDic[id] + d;
    }
    public static void AddListener<T, U>(EventID id, Action<T, U> d)
    {
        OnAddListener(id, d);
        eventDic[id] = (Action<T, U>)eventDic[id] + d;
    }
    public static void AddListener<T, U, V>(EventID id, Action<T, U, V> d)
    {
        OnAddListener(id, d);
        eventDic[id] = (Action<T, U, V>)eventDic[id] + d;
    }
    public static void AddListener<T, U, V, W>(EventID id, Action<T, U, V, W> d)
    {
        OnAddListener(id, d);
        eventDic[id] = (Action<T, U, V, W>)eventDic[id] + d;
    }

    private static void OnRemoveListener(EventID id)
    {
        if (eventDic.ContainsKey(id) && eventDic[id] == null)
        {
            eventDic.Remove(id);
        }
    }
    public static void RemoveListener(EventID id, Action d)
    {
        if (eventDic.ContainsKey(id))
        {
            eventDic[id] = (Action)eventDic[id] - d;
            OnRemoveListener(id);
        }
    }
    public static void RemoveListener<T>(EventID id, Action<T> d)
    {
        if (eventDic.ContainsKey(id))
        {
            eventDic[id] = (Action<T>)eventDic[id] - d;
            OnRemoveListener(id);
        }
    }
    public static void RemoveListener<T, U>(EventID id, Action<T, U> d)
    {
        if (eventDic.ContainsKey(id))
        {
            eventDic[id] = (Action<T, U>)eventDic[id] - d;
            OnRemoveListener(id);
        }
    }
    public static void RemoveListener<T, U, V>(EventID id, Action<T, U, V> d)
    {
        if (eventDic.ContainsKey(id))
        {
            eventDic[id] = (Action<T, U, V>)eventDic[id] - d;
            OnRemoveListener(id);
        }
    }
    public static void RemoveListener<T, U, V, W>(EventID id, Action<T, U, V, W> d)
    {
        if (eventDic.ContainsKey(id))
        {
            eventDic[id] = (Action<T, U, V, W>)eventDic[id] - d;
            OnRemoveListener(id);
        }
    }
    #endregion
}
