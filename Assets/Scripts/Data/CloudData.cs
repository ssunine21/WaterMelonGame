using System;
using System.Collections;
using System.Collections.Generic;

public class CloudData
{
    public List<CloudDataAttendance> cloudAttendanceDatas;
    public bool isNotice;
}

[Serializable]
public class CloudDataAttendance
{
    public int index;
    public int day;
    public int amount;
}