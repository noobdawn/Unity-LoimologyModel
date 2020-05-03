using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传入折线图的数据
/// </summary>
public class LineChartData
{
    /// <summary>
    /// 折线ID，用于分类
    /// </summary>
    public int Idx;
    /// <summary>
    /// 单条折线的数据，需保证其是有序的
    /// </summary>
    private List<Vector2> Datas;
    /// <summary>
    /// 是否有序
    /// </summary>
    private bool sorted;

    public LineChartData(int idx)
    {
        Idx = idx;
        Datas = new List<Vector2>();
        sorted = true;
    }

    #region 获取折线的区间
    public Vector2 GetXRange()
    {
        if (Datas == null || Datas.Count == 0) return Vector2.zero;
        return new Vector2(Datas[0].x, Datas[Datas.Count - 1].x);
    }

    public Vector2 GetYRange()
    {
        if (Datas == null || Datas.Count == 0) return Vector2.zero;
        Vector2 result = new Vector2(Datas[0].y, Datas[0].y);
        for(int i = 0; i < Datas.Count; i++)
        {
            var value = Datas[i].y;
            if (value > result.y)
                result.y = value;
            if (value < result.x)
                result.x = value;
        }
        return result;
    }
    #endregion

    #region 对外开放的接口，保证有序
    public List<Vector2> GetDatas()
    {
        if (!sorted)
            Datas.Sort((x, y) => { return x.x.CompareTo(y.x); });
        sorted = true;
        return Datas;
    }

    public void AddValue(Vector2 value)
    {
        Datas.Add(value);
        sorted = false;
    }

    public void Clear()
    {
        Datas.Clear();
        sorted = true;
    }
    #endregion
}
