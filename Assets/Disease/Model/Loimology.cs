using System;
using UnityEngine;

public static class Loimology
{
    public static int minMulti(float chance, int number)
    {
        float mulResult = chance * number;
        int floor = Mathf.FloorToInt(mulResult);
        float f = mulResult - floor;
        if (f == 0)
            return Mathf.Min(floor, number);
        if (UnityEngine.Random.Range(0f, 1f) <= f)
            floor++;
        return Mathf.Min(floor, number);
    }

    /// <summary>
    /// 无警觉模型支配的Tick
    /// </summary>
    public static void Unaware_Tick(ILoimology d, ILoimologyCompartment c, IMedic m)
    {
        int IcauseE = minMulti(c.meetPerTick * d.infectPerMeet * c.Susceptible / (float)c.Number, c.Infected);
        int IEcauseE = minMulti(c.meetPerTick * d.infectPerMeet * c.Susceptible / (float)c.Number, c.Exposed + c.Infected);
        int dS = d.canExposedInfectOthers ? IEcauseE : IcauseE;
        dS = Mathf.Min(dS, c.Susceptible);
        dS = -dS;

        int EcauseI = minMulti(d.symptom, c.Exposed);
        int dE = -dS - EcauseI;

        int IcauseD = minMulti(d.selfDead, c.Infected);
        int IcauseR = minMulti(d.selfCured, c.Infected);
        if (IcauseD + IcauseR > c.Infected)
            IcauseR -= (IcauseD + IcauseR - c.Infected);
        int dI = EcauseI - IcauseD - IcauseR;

        int dT = 0;

        int dR = IcauseR;

        int dD = IcauseD;

        if (dS + dE + dI + dT + dR + dD != 0)
            Debug.LogError("Your equation does not equal 0");

        c.dE = dE;
        c.dI = dI;
        c.dT = dT;
        c.dR = dR;
        c.dD = dD;
        c.Susceptible += dS;
        c.Exposed += dE;
        c.Infected += dI;
        c.Treated += dT;
        c.Recovered += dR;
        c.Death += dD;
    }

    /// <summary>
    /// SEITRD模型支配的Tick
    /// </summary>
    public static void SEITRD_Tick(ILoimology d, ILoimologyCompartment c, IMedic m)
    {
        int IcauseE = minMulti(c.meetPerTick * d.infectPerMeet * c.Susceptible / (float)c.Number, c.Infected);
        int IEcauseE = minMulti(c.meetPerTick * d.infectPerMeet * c.Susceptible / (float)c.Number, c.Exposed + c.Infected);
        int dS = d.canExposedInfectOthers ? IEcauseE : IcauseE;
        dS = Mathf.Min(dS, c.Susceptible);
        dS = -dS;

        int EcauseI = minMulti(d.symptom, c.Exposed);
        int dE = -dS - EcauseI;

        int IcauseD = minMulti(d.selfDead, c.Infected);
        int IcauseR = minMulti(d.selfCured, c.Infected);
        if (IcauseD + IcauseR > c.Infected)
            IcauseR -= (IcauseD + IcauseR - c.Infected);
        int IcauseT = 0;
        if (c.Infected - IcauseD - IcauseR > 0)
            IcauseT = Mathf.Min(c.Infected - IcauseD - IcauseR, m.Empty, m.CureRate);
        int dI = EcauseI - IcauseD - IcauseR - IcauseT;

        int TCauseR = minMulti(d.cured, c.Treated);
        int TCauseD = minMulti(d.dead, c.Treated);
        if (TCauseR + TCauseD > c.Treated)
            TCauseR -= TCauseR + TCauseD - c.Treated;
        int TcauseS = minMulti(d.nonAnitbody, IcauseT);
        int dT = IcauseT - TcauseS - TCauseD - TCauseR;
        dS += TcauseS;

        int dR = TCauseR + IcauseR;

        int dD = TCauseD + IcauseD;

        if (dS + dE + dI + dT + dR + dD != 0)
            Debug.LogError("Your equation does not equal 0");

        c.dE = dE;
        c.dI = dI;
        c.dT = dT;
        c.dR = dR;
        c.dD = dD;
        c.Susceptible += dS;
        c.Exposed += dE;
        c.Infected += dI;
        c.Treated += dT;
        c.Recovered += dR;
        c.Death += dD;

        m.Curing += dT;
    }

    /// <summary>
    /// 强制感染若干人
    /// </summary>
    /// <param name="v"></param>
    /// <param name="province"></param>
    public static void Infect(int v, ILoimologyCompartment c)
    {
        c.Susceptible -= v;
        c.Exposed += v;
    }

    /// <summary>
    /// 强制治好若干人
    /// </summary>
    /// <param name="v"></param>
    /// <param name="province"></param>
    public static void Recure(int v, ILoimologyCompartment c)
    {
        if (v >= c.Patient)
        {
            c.Recovered += (c.Infected + c.Treated);
            c.Infected = 0;
            c.Treated = 0;
            var r = (v - c.Patient);
            if (r >= c.Exposed)
            {
                c.Recovered += c.Exposed;
                c.Exposed = 0;
            }
            else
            {
                c.Recovered += r;
                c.Exposed -= r;
            }            
        }
        else
        {
            // 先从未收治的感染者中治疗
            if (v >= c.Infected)
            {
                c.Recovered += c.Infected;
                c.Infected = 0;
                var r = (v - c.Infected);
                c.Recovered += r;
                c.Treated -= r;
            }
            else
            {
                c.Recovered += v;
                c.Infected -= v;
            }
        }
    }
}
