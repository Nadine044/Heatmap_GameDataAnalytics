using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KPIs_info
{
    public KPIs_info()
    {
        kill_pos = new List<Vector3>();
        hit_pos = new List<Vector3>();
        acid_pos = new List<Vector3>();
        fall_pos = new List<Vector3>();
        death_pos = new List<Vector3>();
        paths = new List<PlayerPath>();
        game_number = -1;
    }
    public List<Vector3> kill_pos;
    public List<Vector3> hit_pos;
    public List<Vector3> acid_pos;
    public List<Vector3> fall_pos;
    public List<Vector3> death_pos;
    public List<PlayerPath> paths;
    public int game_number = -1;
    //falls
}

[System.Serializable]
public class KPIs_Games
{
    public List<KPIs_info> games;
}

[System.Serializable]
public class PlayerPath
{
    public List<Vector3> path = new List<Vector3>();
}
