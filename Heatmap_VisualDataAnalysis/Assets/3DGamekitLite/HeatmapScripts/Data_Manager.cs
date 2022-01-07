using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Manager : MonoBehaviour
{
    public JSON_Read json;
    Data_from_Manager data = new Data_from_Manager();
    public class Data_from_Manager
    {
        public float kill_pos;
        public float hit_pos;
        public float acid_pos;
        public float death_pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            LoadKPIs(data);

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("This is the position of kill_pos: " + data.kill_pos);
            Debug.Log("This is the position of hit_pos: " + data.hit_pos);
            Debug.Log("This is the position of acid_pos: " + data.acid_pos);
            Debug.Log("This is the position of death_pos: " + data.death_pos);
        }
    }

    Data_from_Manager LoadKPIs(Data_from_Manager info)
    {
        info.kill_pos = json.data_from_sessions.sessions_data.kill_pos;
        info.hit_pos = json.data_from_sessions.sessions_data.hit_pos;
        info.acid_pos = json.data_from_sessions.sessions_data.acid_pos;
        info.death_pos = json.data_from_sessions.sessions_data.death_pos;

        Debug.Log("This is the position of kill_pos: " + data.kill_pos);
        Debug.Log("This is the position of hit_pos: " + data.hit_pos);
        Debug.Log("This is the position of acid_pos: " + data.acid_pos);
        Debug.Log("This is the position of death_pos: " + data.death_pos);

        return info;
    }
}
