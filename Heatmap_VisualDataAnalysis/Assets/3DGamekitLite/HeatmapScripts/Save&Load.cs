using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveAndLoad : MonoBehaviour
{
    public KPIs_info all_data = new KPIs_info();

    public void Start()
    {        
        //all_data.kill_pos.Add(new Vector3(0, 13, 2));
        //all_data.kill_pos.Add(new Vector3(235345, 13, 2));

        //all_data.hit_pos.Add(new Vector3(1, 13, 2));
        //all_data.acid_pos.Add(new Vector3(2, 13, 2));
        //all_data.death_pos.Add(new Vector3(3, 13, 2));

        //all_data.hit_pos.Add(new Vector3(6, 13, 2));
        //all_data.acid_pos.Add(new Vector3(7, 13, 2));
        //all_data.death_pos.Add(new Vector3(8, 13, 2));
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(all_data, true);
        File.WriteAllText(Application.dataPath + "/KPIs_file.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/KPIs_file.json");
        KPIs_info data = JsonUtility.FromJson<KPIs_info>(json);

        //Debug.Log(data.kill_pos[1]);
        Debug.Log(data.hit_pos[1]);
        //Debug.Log(data.acid_pos[1]);
        Debug.Log(data.death_pos[0]);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SaveToJson();

        if (Input.GetKeyDown(KeyCode.L))
            LoadFromJson();
    }
}
