using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadVisualData : MonoBehaviour
{
    public GameObject skull;
    public GameObject hit;
    public GameObject killEnemy;
    public GameObject path;

    [HideInInspector]
    public SaveAndLoad info;

    bool deathEnabled = true;
    bool hitEnable = true;
    bool killEnemyEnabled = true;
    bool pathEnabled = true;


    // Start is called before the first frame update
    public void Start()
    {
        info = new SaveAndLoad();

        string json = File.ReadAllText(Application.dataPath + "/KPIs_file.json");
        info.all_data = JsonUtility.FromJson<KPIs_info>(json);
    }

    // Update is called once per frame
    public void Update()
    {
        LoadVisualData_Assets();
    }

    public void LoadVisualData_Assets()
    {
        if (info.all_data.kill_pos != null && killEnemyEnabled)
        {
            for (int i = 0; i < info.all_data.kill_pos.Count; i++)
                Instantiate(killEnemy, new Vector3(info.all_data.kill_pos[i].x, info.all_data.kill_pos[i].y, info.all_data.kill_pos[i].z), transform.rotation);
        }

        if (info.all_data.hit_pos != null && hitEnable)
        {
            for (int i = 0; i < info.all_data.hit_pos.Count; i++)
                Instantiate(hit, new Vector3(info.all_data.hit_pos[i].x, info.all_data.hit_pos[i].y, info.all_data.hit_pos[i].z), transform.rotation);
        }


        if (info.all_data.death_pos != null && deathEnabled)
        {
            for (int i = 0; i < info.all_data.death_pos.Count; i++)
                Instantiate(skull, new Vector3(info.all_data.death_pos[i].x, info.all_data.death_pos[i].y, info.all_data.death_pos[i].z), transform.rotation);
        }

        if (info.all_data.path_pos != null && pathEnabled)
        {
            for (int i = 0; i < info.all_data.path_pos.Count; i++)
                Instantiate(path, new Vector3(info.all_data.path_pos[i].x, info.all_data.path_pos[i].y + 2.0f, info.all_data.path_pos[i].z), transform.rotation);
        }

        killEnemyEnabled = false;
        deathEnabled = false;
        hitEnable = false;
        pathEnabled = false;
    }
}
