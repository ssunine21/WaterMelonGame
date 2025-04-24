using System;
using System.Collections.Generic;

public class CloudData
{
    public List<CloudDataAttendance> cloudAttendanceDatas;
    public List<CloudDataBookReward> cloudBookRewardDatas;
    public List<CloudDataBookAdReward> cloudBookAdRewardDatas;
    public bool isNotice;
}

[Serializable]
public class CloudDataAttendance
{
    public int index;
    public int day;
    public int amount;
}

[Serializable]
public class CloudDataBookReward
{
    public int exp;
    public int gold;
}

[Serializable]
public class CloudDataBookAdReward
{
    public int exp;
    public int gold;
}