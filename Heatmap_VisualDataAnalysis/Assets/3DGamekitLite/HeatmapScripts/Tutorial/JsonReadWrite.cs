using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonReadWrite : MonoBehaviour
{
    public float kill_pos_DATA;
    public float hit_pos_DATA;
    public float acid_pos_DATA;
    public float death_pos_DATA;

    public void Start()
    {
        kill_pos_DATA = 2.3f;
        hit_pos_DATA = 3.3f;
        acid_pos_DATA = 4.3f;
        death_pos_DATA = 5.3f;
    }

    public void SaveToJson()
    {
        KPIs_info data = new KPIs_info();

        data.kill_pos = kill_pos_DATA;
        data.hit_pos = hit_pos_DATA;
        data.acid_pos = acid_pos_DATA;
        data.death_pos = death_pos_DATA;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/KPIs_file.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/KPIs_file.json");
        KPIs_info data = JsonUtility.FromJson<KPIs_info>(json);

        kill_pos_DATA = data.kill_pos;
        hit_pos_DATA = data.hit_pos;
        acid_pos_DATA = data.acid_pos;
        death_pos_DATA = data.death_pos;

        Debug.Log(data.kill_pos);
        Debug.Log(data.hit_pos);
        Debug.Log(data.acid_pos);
        Debug.Log(data.death_pos);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SaveToJson();

        if (Input.GetKeyDown(KeyCode.L))
            LoadFromJson();

        if (Input.GetKeyDown(KeyCode.C))
            ChangeNumbers();
    }

    void ChangeNumbers()
    {
        kill_pos_DATA = 1;
        hit_pos_DATA = 1;
        acid_pos_DATA = 1;
        death_pos_DATA = 1;
    }
}
