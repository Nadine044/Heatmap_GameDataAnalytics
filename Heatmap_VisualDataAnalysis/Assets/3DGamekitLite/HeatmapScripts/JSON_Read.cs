using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSON_Read : MonoBehaviour
{
    public TextAsset sessions_data_txt;

    [System.Serializable]
    public class Data
    {
        public float kill_pos;
        public float hit_pos;
        public float acid_pos;
        public float death_pos;
    }

    [System.Serializable]
    public class SessionsList
    {
        public Data sessions_data;
    }

    public SessionsList data_from_sessions = new SessionsList();

    // Start is called before the first frame update
    void Start()
    {
        data_from_sessions = JsonUtility.FromJson<SessionsList>(sessions_data_txt.text);
    }
}
