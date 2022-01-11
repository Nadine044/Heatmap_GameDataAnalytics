using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KPIs_info
{
    public List<Vector3> kill_pos;
    public List<Vector3> hit_pos;
    public List<Vector3> acid_pos;
    public List<Vector3> death_pos;
    public List<Vector3> path_pos;
    public int game_number = -1;
    //falls
}

public class KPIs_Games
{
    public List<KPIs_info> games;
}
