using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public TextAsset txt;

    public KPIs data_to_save;
    public KPIs data_loaded;
    public struct KPIs
    {
        public List<Vector3> kills_pos; //number of killed monsters
        public List<Vector3> hits_pos;
        public List<Vector3> lost_acid_pos;
        public List<Vector3> deaths_pos; //number of times that Ellen dies
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        {//Create data hardcoded
            data_to_save = new KPIs()
            {
                kills_pos = new List<Vector3>()
            {
                new Vector3(8,9,7.6f),
                new Vector3(12,2,7.6f)
            },
                hits_pos = new List<Vector3>()
            {
                new Vector3(1,9,7.6f)
            }
            };
        }


        LoadData();
        yield return new WaitForSeconds(1);
        
      //  data_loaded = JsonUtility.FromJson<KPIs>(txt.text);
    }

    static void WriteDisk(string file)
    {
        string path = "Assets/Resources/data_file.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(file);
        writer.Close();
    }
        // Update is called once per frame
        void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(data_loaded.kills_pos[1]);
            Debug.Log(data_loaded.hits_pos[0]);
            Debug.Log(data_loaded.lost_acid_pos[0]);
            Debug.Log(data_loaded.deaths_pos[0]);
        }
    }


    public void SaveData()
    {
        WriteDisk(JsonUtility.ToJson(data_to_save));
    }

    public void LoadData()
    {
        //data_loaded = JsonUtility.FromJson<KPIs>(txt.text);
    }
    public /*static*/ void ReadString()
    {
        string path = Application.persistentDataPath + "/data_file.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string aa = reader.ReadToEnd();
        Debug.Log(aa);
        reader.Close();
        data_loaded = JsonUtility.FromJson<KPIs>(aa);
    }
}
