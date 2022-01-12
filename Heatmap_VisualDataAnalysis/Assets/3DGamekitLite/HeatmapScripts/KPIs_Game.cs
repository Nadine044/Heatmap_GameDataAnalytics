using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KPIs_Game
{
    public KPIs_Game()
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
public class SavedGames
{
    public List<KPIs_Game> games;
}

[System.Serializable]
public class PlayerPath
{
    public List<Vector3> pathPositions = new List<Vector3>();
}
