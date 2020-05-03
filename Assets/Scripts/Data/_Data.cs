public struct Modifier
{
    public EModifyType type;
    public EModifyOperation op;
    public float param;
}

// 数据类，懒得写excel导出了，先拿这个凑活下
public static class Data
{
    public static float invTickToDay
    {
        get
        {
            if (_D.Inst != null)
            return _D.Inst.Speed * 0.0006944f;
            return 0.0006944f;
        }
    }
    public static int numOfProvince = 9;

    public static DProvince[] Province = DProvince.ToData();
    public static DPolicy[] Policy = DPolicy.ToData();
    public static DDisease[] Disease = DDisease.ToData();
}

/// <summary>
/// 行省数据
/// </summary>
public class DProvince
{
    public int id { get; private set; }
    public string name { get; private set; }
    public bool capital { get; private set; }

    public int populationInCity { get; private set; }
    public float meetPerDayInCity { get; private set; }
    public int populationInCountry { get; private set; }
    public float meetPerDayInCountry { get; private set; }


    public int berth { get; private set; }
    public int cureRate { get; private set; }

    public static DProvince[] ToData()
    {
        DProvince[] result = new DProvince[]
        {
            new DProvince(){
                id = 1,
                name = "test0",
                capital = true,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
            new DProvince(){
                id = 2,
                name = "test0",
                capital = false,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
            new DProvince(){
                id = 3,
                name = "test0",
                capital = false,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
            new DProvince(){
                id = 4,
                name = "test0",
                capital = false,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
            new DProvince(){
                id = 5,
                name = "test0",
                capital = false,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
            new DProvince(){
                id = 6,
                name = "test0",
                capital = false,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
            new DProvince(){
                id = 7,
                name = "test0",
                capital = false,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
            new DProvince(){
                id = 8,
                name = "test0",
                capital = false,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
            new DProvince(){
                id = 9,
                name = "capital",
                capital = false,
                populationInCity = 5000000,
                populationInCountry = 15000000,
                meetPerDayInCity = 5,
                meetPerDayInCountry = 3,
                cureRate = 4000,
                berth = 20000,
            },
        };
        return result;
    }
}

/// <summary>
/// 政策数据
/// </summary>
public class DPolicy
{
    public int id { get; private set; }
    public string name { get; private set; }
    public string tips { get; private set; }
    public int type { get; private set; }
    public int range { get; private set; }
    public bool isMulti { get; private set; }
    public bool isAdd { get; private set; }
    public int cost { get; private set; }
    public int[] premiss { get; private set; }

    public static DPolicy[] ToData()
    {
        DPolicy[] result = new DPolicy[]
        {
            new DPolicy()
            {
                id = 1,
                name = "公开新型传染病信息",
                tips = "您的卫生部门专家们发现了一种新的传染病，而您决定将其公之于众。公众们将怀抱着好奇端详着新型疫情的到来。",
                type = (int)EModifyType.None,
                range = (int)EModifyRange.AllProvince,
                isMulti = false,
                isAdd = false,
                cost = 10,
                premiss = null
            }
        };

        return result;
    }
}

/// <summary>
/// 疾病数据
/// </summary>
public class DDisease : ILoimology
{
    public int id { get; set; }
    public string name { get; set; }
    public float infectPerMeet { get; set; }
    public float cured { get { return curedDay * Data.invTickToDay; } }
    public float selfCured { get { return selfCuredDay * Data.invTickToDay; } }
    public float dead { get { return deadDay * Data.invTickToDay; } }
    public float selfDead { get { return selfDeadDay * Data.invTickToDay; } }
    public float symptom { get { return symptomDay * Data.invTickToDay; } }
    public float curedDay { get; set; }
    public float selfCuredDay { get; set; }
    public float symptomDay { get; set; }
    public float deadDay { get; set; }
    public float selfDeadDay { get; set; }
    public float nonAnitbody { get; set; }
    public bool canExposedInfectOthers { get; set; }

    public static DDisease[] ToData()
    {
        DDisease[] result = new DDisease[]
        {
            new DDisease()
            {
                id = 1,
                name = "New Virus",
                infectPerMeet = 0.2f,
                curedDay = 0.095f,
                selfCuredDay = 0.08f,
                symptomDay = 0.14f,
                deadDay = 0.005f,
                selfDeadDay = 0.02f,
                nonAnitbody = 0.01f,
                canExposedInfectOthers = true
            }
        };
        return result;
    }
}