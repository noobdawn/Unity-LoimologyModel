using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province : IMedic, ILoimologyCompartment
{
    public GameObject gameObject;

    #region Data
    public DProvince dataSource;


    public void InitFromDataSource(int idx)
    {
        dataSource = Data.Province[idx];

        mods = new List<Modifier>();
        ClearFinalMods();


        dS = dE = dI = dT = dR = dD = dC = Curing = Exposed = Infected = Treated = Recovered = Death = 0;
        Number = dataSource.populationInCity + dataSource.populationInCountry;
        Susceptible = Number;

        meetPerDay = (dataSource.populationInCountry * dataSource.meetPerDayInCountry +
            dataSource.populationInCity * dataSource.meetPerDayInCity) / Number;

        Capacity = dataSource.berth;
        CureRate = dataSource.cureRate;
    }

    /// <summary>
    /// 更新一哈，主要针对病床数和每日接触人数
    /// </summary>
    internal void BeforeTick()
    {
        var finalMods = GetFinalMods();
        ModifyProvince(finalMods);
    }

    private void ModifyProvince(Modifier[][] finalMods)
    {
        if (finalMods == null) return;
        float meetPerDayInCity = ModifyProvince(dataSource.meetPerDayInCity, finalMods, EModifyType.MeetPerDayInCity);
        float meetPerDayInCountry = ModifyProvince(dataSource.meetPerDayInCountry, finalMods, EModifyType.MeetPerDayInCountry);
        meetPerDay = (meetPerDayInCity * dataSource.populationInCity + meetPerDayInCountry * dataSource.populationInCountry) / (dataSource.populationInCountry + dataSource.populationInCity);

        int newCapacity = ModifyProvince(Capacity, finalMods, EModifyType.Berth);
        // 如果降了床位，多出来的病人释放到Infected里！
        if (newCapacity < Curing)
        {
            // 弗里曼大草
            var freeman = Curing - newCapacity;
            Capacity = newCapacity;
            Curing = newCapacity;
            Treated -= freeman;
            Infected += freeman;
        }
    }

    private int ModifyProvince(int o, Modifier[][] finalMods, EModifyType type)
    {
        var inner = finalMods[0][(int)type - 1];
        var outter = finalMods[1][(int)type - 1];
        var multi = finalMods[2][(int)type - 1];
        return Mathf.CeilToInt((o + inner.param) * (1 + multi.param) + outter.param);
    }

    private float ModifyProvince(float o, Modifier[][] finalMods, EModifyType type)
    {
        var inner = finalMods[0][(int)type - 1];
        var outter = finalMods[1][(int)type - 1];
        var multi = finalMods[2][(int)type - 1];
        return (o + inner.param) * (1 + multi.param) + outter.param;
    }
    #endregion

    #region Interface
    public float meetPerDay { get; set; }
    public float meetPerTick { get { return meetPerDay * Data.invTickToDay; } }

    public int Health { get { return Susceptible + Exposed + Recovered; } }

    public int Patient { get { return Infected + Treated; } }

    public int Susceptible { get; set; }
    public int Exposed { get; set; }
    public int Infected { get; set; }
    public int Treated { get; set; }
    public int Recovered { get; set; }
    public int Death { get; set; }
    public int Number { get; set; }
    public int dS { get; set; }
    public int dE { get; set; }
    public int dI { get; set; }
    public int dT { get; set; }
    public int dR { get; set; }
    public int dD { get; set; }
    public int dC { get; set; }
    public int Capacity { get; set; }

    public int Empty { get { return Capacity - Curing; } }

    public int Curing { get; set; }
    public int CureRate { get; set; }
    #endregion

    #region Modifier
    /// 封装List接口，保证对外只提供一个FinalMods的效果
    private List<Modifier> mods;
    private List<Modifier> innerAdds, multipies, outterAdds;
    private Modifier[][] finalMods;
    private bool isModified;

    /// <summary>
    /// 绝了，爹给你直接alloc一批
    /// </summary>
    private void ClearFinalMods()
    {
        var enums = System.Enum.GetValues(typeof(EModifyType));
        Modifier[][] fimomods = new Modifier[3][]{
            new Modifier[enums.Length - 1],
            new Modifier[enums.Length - 1],
            new Modifier[enums.Length - 1]
        };
    }
    
    private void CheckMods()
    {
        if (mods == null) mods = new List<Modifier>();
        if (finalMods == null) ClearFinalMods();
        if (innerAdds == null) innerAdds = new List<Modifier>();
        if (multipies == null) multipies = new List<Modifier>();
        if (outterAdds == null) outterAdds = new List<Modifier>();
    }

    public Modifier[][] GetFinalMods()
    {
        CheckMods();
        if (isModified)
            CaculateMods();
        return finalMods;
    }

    public void Clear()
    {
        CheckMods();
        mods.Clear();
        isModified = false;
    }

    public void AddMod(Modifier mod)
    {
        if (mod.type == EModifyType.None) return;
        CheckMods();
        mods.Add(mod);
        isModified = true;
    }

    private void CaculateMods()
    {
        CheckMods();
        innerAdds.Clear();
        multipies.Clear();
        outterAdds.Clear();
        ClearFinalMods();
        // 分离三种计算类型
        foreach (var mod in mods)
        {
            if (mod.op == EModifyOperation.InnerPlus)
                innerAdds.Add(mod);
            else if (mod.op == EModifyOperation.Multipy)
                multipies.Add(mod);
            else
                outterAdds.Add(mod);
        }
        // 合并计算成一个屌东西
        // 校验是啥，懒得做的，嘻嘻
        foreach (var mod in mods)
        {
            if (mod.type == EModifyType.None) continue;
            finalMods[(int)mod.op][(int)mod.type - 1].param += mod.param;
        }
        isModified = false;
    }
    #endregion
}
