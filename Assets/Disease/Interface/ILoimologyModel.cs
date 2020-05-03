/// <summary>
/// 某地的人群仓室
/// </summary>
public interface ILoimologyCompartment
{
    /// <summary>
    /// 每个患者每Tick接触到的人数
    /// </summary>
    float meetPerTick { get; }
    /// <summary>
    /// 无症状者
    /// </summary>
    int Health { get; }
    /// <summary>
    /// 症状者
    /// </summary>
    int Patient { get; }

    /// <summary>
    /// 潜在受众
    /// </summary>
    int Susceptible { get; set; }
    /// <summary>
    /// 潜伏期患者
    /// </summary>
    int Exposed { get; set; }
    /// <summary>
    /// 未得到收治的患者
    /// </summary>
    int Infected { get; set; }
    /// <summary>
    /// 已被收治的患者
    /// </summary>
    int Treated { get; set; }
    /// <summary>
    /// 康复者
    /// </summary>
    int Recovered { get; set; }
    /// <summary>
    /// 死亡人数
    /// </summary>
    int Death { get; set;}
    /// <summary>
    /// 总人数
    /// </summary>
    int Number { get; set;}

    int dS { get; set;}
    int dE { get; set;}
    int dI { get; set;}
    int dT { get; set;}
    int dR { get; set;}
    int dD { get; set;}
    int dC { get; set;}
}

public interface IMedic
{
    /// <summary>
    /// 总床位
    /// </summary>
    int Capacity { get; set;}
    /// <summary>
    /// 空床位
    /// </summary>
    int Empty { get; }
    /// <summary>
    /// 收治床位
    /// </summary>
    int Curing { get; set;}
    /// <summary>
    /// 收治率
    /// </summary>
    int CureRate { get; set;}
}

/// <summary>
/// 传染病的基本信息
/// </summary>
public interface ILoimology
{
    /// <summary>
    /// 每人次接触患病的几率
    /// </summary>
    float infectPerMeet { get; set; }
    /// <summary>
    /// 一个有症状的感染者经过治疗后痊愈的几率
    /// </summary>
    float cured { get; }
    /// <summary>
    /// 一个有症状的感染者未经过治疗痊愈的几率
    /// </summary>
    float selfCured { get; }
    /// <summary>
    /// 一个感染者在潜伏期中转换为有症状的几率
    /// </summary>
    float symptom { get; }
    /// <summary>
    /// 一个有症状的感染者在得到治疗的情况下死亡的几率
    /// </summary>
    float dead { get; }
    /// <summary>
    /// 一个有症状的感染者在未得到治疗的情况下死亡的几率
    /// </summary>
    float selfDead { get; }
    /// <summary>
    /// 治疗成功后未能产生抗体的几率
    /// </summary>
    float nonAnitbody { get; set; }
    /// <summary>
    /// 潜伏者是否允许感染他人
    /// </summary>
    bool canExposedInfectOthers { get; set; }

    float symptomDay { get; set; }
    float curedDay { get; set; }
    float selfCuredDay { get; set; }
    float deadDay { get; set; }
    float selfDeadDay { get; set; }
}