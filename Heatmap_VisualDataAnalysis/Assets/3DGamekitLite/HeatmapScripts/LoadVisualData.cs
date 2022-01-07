using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadVisualData : MonoBehaviour
{
    public GameObject redCross;

    [HideInInspector]
    public SaveAndLoad info;

    bool deathCrossEnable = true;

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
        DeathCross();
    }

    public void DeathCross()
    {
        if (info.all_data.death_pos != null && deathCrossEnable)
        {
            for (int i = 0; i < info.all_data.death_pos.Count; i++)
            {
                Instantiate(redCross, new Vector3(info.all_data.death_pos[i].x, info.all_data.death_pos[i].y, info.all_data.death_pos[i].z), transform.rotation);
            }
        }
        deathCrossEnable = false;
    }
}
