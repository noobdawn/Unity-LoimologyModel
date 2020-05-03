
public enum EModifyType
{
    None,
    MeetPerDayInCity,
    MeetPerDayInCountry,
    Berth,
    InfectPerMeet,
    Cured,
    SelfCured,
    Dead,
    SelfDead,
}

public enum EModifyRange
{
    None,
    AllProvince,
    SingleProvince,
    Loimology,
}

public enum EModifyOperation
{
    /// <summary>
    /// 先做该加法再做乘法
    /// </summary>
    InnerPlus,
    /// <summary>
    /// 做完乘法再做该加法
    /// </summary>
    OutterPlus,
    Multipy,
}

public enum EventID
{
    /// <summary>
    /// 每Tick计算完成，响应该事件一般是显示和表现方面的逻辑
    /// </summary>
    TickEnd,
}