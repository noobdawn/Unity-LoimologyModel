using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 一局具体游戏的发生器
public class _D
{
    public static _D Inst = null;
    public MonoBehaviour _Attachment;

    public Province[] provinces;
    public Province capital { get { return provinces[0]; } }

    #region Time
    /// <summary>
    /// 倍速，最多支持30倍速
    /// </summary>
    public int Speed { get { return speeds[speedIdx]; } }

    private readonly int[] speeds = new int[]
    {
        0, 1, 2, 3, 5, 10, 15, 30, 60, 120, 240, 360, 720, 1440,
    };
    private int speedIdx = 0; 

    public void Unpause()
    {
        if (Playing) return;
        speedIdx = 1;
    }

    public void Faster()
    {
        speedIdx++;
        speedIdx = Mathf.Min(speedIdx, speeds.Length - 1);
    }

    public void Slower()
    {
        speedIdx--;
        speedIdx = Mathf.Max(1, speedIdx);
    }

    public void Pause()
    {
        if (!Playing) return;
        speedIdx = 0;
    }

    public int TotalMinutes, Minutes, Hours, Days, Weeks, Months, Years;
    public int TotalDays { get { return Mathf.CeilToInt(TotalMinutes / 1440f); } }
    public bool Playing { get { return Speed > 0; } }

    private void SyncTime()
    {
        Minutes = TotalMinutes % 60;
        Hours = Mathf.CeilToInt(TotalMinutes / 60f) % 24;
        Days = Mathf.CeilToInt(TotalMinutes / 1440f) % 30;
        Weeks = Mathf.CeilToInt(TotalMinutes / 10080f) % 7;
        Months = Mathf.CeilToInt(TotalMinutes / 43200f) % 12;
        Years = Mathf.CeilToInt(TotalMinutes / 525600f) % 365;
    }
    #endregion

    private Coroutine _tick;

    public _D(GameObject[] provinceInCanvas)
    {
        // 默认第一个是首都
        provinces = new Province[provinceInCanvas.Length];
        for (int i = 0; i < provinces.Length; i++)
        {
            provinces[i] = new Province();
            provinces[i].gameObject = provinceInCanvas[i];
            provinces[i].InitFromDataSource(i);
        }
        // 先暂停
        Pause();
    }

    #region Control
    /// <summary>
    /// 从存档中恢复数据，然后再开始
    /// </summary>
    public void Load()
    {
        // TODO
        StartGame();
    }

    /// <summary>
    /// 终止游戏，在旧游戏里直接开新游戏进程时候调用
    /// </summary>
    public void StopGame()
    {
        // TODO
        // 重置时间
        TotalMinutes = Minutes = Hours = Days = Weeks = Months = Years = 0;
    }

    /// <summary>
    /// 开始一局新游戏
    /// </summary>
    public void StartNewGame()
    {
        StopGame();
        // 随便往一个行省投放5~15名病人，nmb，投1个有可能突然暴毙就nm离谱
        var idx = UnityEngine.Random.Range(0, provinces.Length - 1);
        Loimology.Infect(UnityEngine.Random.Range(5, 15), provinces[idx]);

        StartGame();
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        _U.ShowPanel("UStateBar", 1, null);
        // 如果没有疾病数据，就随便找个
        if (baseDisease == null)
            baseDisease = Data.Disease[UnityEngine.Random.Range(0, Data.Disease.Length - 1)];
        Unpause();
        if (_tick != null)
        {
            Debug.LogError("cnm，你协程关了吗？");
            _Attachment.StopCoroutine(_tick);
        }
        _tick = _Attachment.StartCoroutine(TickCoroutine());
    }
    #endregion

    #region TotalInfo
    // 该部分是全国的疫情
    public int Health { get; private set; }
    public int Patient { get; private set; }
    public int Treated { get; private set; }
    public int Infected{ get; private set; }
bool isAlert { get { return Patient > 50; } }
    #endregion

    #region 


    /// <summary>
    /// 最最最关键的部分，定时计算的步骤
    /// 每0.2s走一次，没走一次多1min
    /// </summary>
    /// <returns></returns>
    private IEnumerator TickCoroutine()
    {
        while (true)
        {
            var curMin = TotalMinutes + Speed;
            // 如果是暂停就溜了溜了
            if (curMin != TotalMinutes)
            {
                // 先把时间同步一哈
                TotalMinutes = curMin;
                SyncTime();
                Health = Patient = Treated = Infected = 0;
                // 开始过各行省该tick内的流程
                for (int i = 0; i < Data.numOfProvince; i++)
                {
                    Province province = provinces[i];
                    // 拿到加成下的疾病
                    Disease disease = GetDisease(province);
                    // 拿到加成下的行省
                    province.BeforeTick();
                    if (isAlert)
                        Loimology.SEITRD_Tick(disease, province, province);
                    else
                        Loimology.Unaware_Tick(disease, province, province);
                    // 重新计算全国的数据
                    Health += province.Health;
                    Patient += province.Patient;
                    Treated += province.Treated;
                    Infected += province.Infected;
                }
                Debug.LogWarningFormat("Time:{0} H:{1} P:{2} T:{3} I:{4}", TotalMinutes, Health, Patient, Treated, Infected);
                if (Infected == 0)
                    Debug.Log("33");
                _E.TriggerEvent(EventID.TickEnd);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion

    #region Disease
    private DDisease baseDisease;
    private Disease GetDisease(Province province)
    {
        if (baseDisease == null)
        {
            Debug.LogErrorFormat("cnm, 爷的数据都没有nsnmn");
            baseDisease = Data.Disease[0];
        }
        Disease result = new Disease()
        {
            infectPerMeet = baseDisease.infectPerMeet,
            curedDay = baseDisease.curedDay,
            selfCuredDay = baseDisease.selfCuredDay,
            symptomDay = baseDisease.symptomDay,
            deadDay = baseDisease.deadDay,
            selfDeadDay = baseDisease.selfDeadDay,
            nonAnitbody = baseDisease.nonAnitbody,
            canExposedInfectOthers = baseDisease.canExposedInfectOthers,
        };
        // 根据各行省受到的调整值进行调整
        var finalMods = province.GetFinalMods();
        ModifyDisease(finalMods, ref result);
        return result;
    }

    private void ModifyDisease(Modifier[][] finalMods, ref Disease result)
    {
        if (finalMods == null) return;
        result.infectPerMeet = ModifyDisease(result.infectPerMeet, finalMods, EModifyType.InfectPerMeet);
        result.curedDay = ModifyDisease(result.curedDay, finalMods, EModifyType.Cured);
        result.selfCuredDay = ModifyDisease(result.selfCuredDay, finalMods, EModifyType.SelfCured);
        result.deadDay = ModifyDisease(result.deadDay, finalMods, EModifyType.Dead);
        result.selfDeadDay = ModifyDisease(result.selfDeadDay, finalMods, EModifyType.SelfDead);
    }

    private float ModifyDisease(float o, Modifier[][] finalMods, EModifyType type)
    {
        var inner = finalMods[0][(int)type - 1];
        var outter = finalMods[1][(int)type - 1];
        var multi = finalMods[2][(int)type - 1];
        return (o + inner.param) * (1 + multi.param) + outter.param;
    }
    #endregion

    #region Event

    #endregion
}
