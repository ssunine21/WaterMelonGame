using System;
using System.Collections;
using System.Collections.Generic;

public class CloudData
{
    public List<CloudDataAttendance> cloudAttendanceDatas;
}

[Serializable]
public class CloudDataAttendance
{
    public int index;
    public int day;
    public int amount;
}