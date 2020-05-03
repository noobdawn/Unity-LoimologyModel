using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单个疾病，由_D产生并分配
/// </summary>
public struct Disease : ILoimology
{
    public float symptom { get { return symptomDay * Data.invTickToDay; } }
    public float cured { get { return curedDay * Data.invTickToDay; } }
    public float selfCured { get { return selfCuredDay * Data.invTickToDay; } }
    public float dead { get { return deadDay * Data.invTickToDay; } }
    public float selfDead { get { return selfDeadDay * Data.invTickToDay; } }
    public float infectPerMeet { get; set; }
    public float curedDay { get; set; }
    public float selfCuredDay { get; set; }
    public float symptomDay { get; set; }
    public float deadDay { get; set; }
    public float selfDeadDay { get; set; }
    public float nonAnitbody { get; set; }
    public bool canExposedInfectOthers { get; set; }
}
