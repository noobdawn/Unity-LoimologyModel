using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _UC
{
    public _UP invoker;
}

/// <summary>
/// the base class of ui panel
/// </summary>
public class _UP : MonoBehaviour
{
    private _UP invokeUI;
    public _UC context;

    public virtual string Name { get; }
    public virtual void WhenFirstShow() { }
    public virtual void WhenShow() { }
    public virtual void WhenHide() { }
    public virtual void WhenDestroy() { }
    public virtual void WhenRefresh() { }
    public virtual void Exit()
    {
        _U.ClosePanel(this);
    }
}
