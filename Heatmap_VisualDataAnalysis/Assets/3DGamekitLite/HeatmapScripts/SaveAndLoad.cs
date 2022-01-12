using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveAndLoad : MonoBehaviour
{
    public KPIs_Game all_data = new KPIs_Game();
    public SavedGames all_games = new SavedGames();
    public void Awake()
    {
        LoadFromJson();
        all_data.game_number = 1 + all_games.games.Count;
        PlayerPath firstPath = new PlayerPath();
        all_data.paths.Add(firstPath);
        //    all_data.kill_pos.Add(new Vector3(0, 13, 2));
        //    all_data.kill_pos.Add(new Vector3(235345, 13, 2));

        //    all_data.hit_pos.Add(new Vector3(1, 13, 2));
        //    all_data.acid_pos.Add(new Vector3(2, 13, 2));
        //    all_data.death_pos.Add(new Vector3(3, 13, 2));

        //    all_data.hit_pos.Add(new Vector3(6, 13, 2));
        //    all_data.acid_pos.Add(new Vector3(7, 13, 2));
        //    all_data.death_pos.Add(new Vector3(8, 13, 2));
    }

    public void SaveToJson()
    {
        LoadFromJson();
        if (all_games.games.Count != 0)
        {
            if (all_games.games[all_games.games.Count - 1].game_number == all_data.game_number) //this means this game is alredy save it. Erase that and save it again.
            {
                all_games.games.RemoveAt(all_games.games.Count - 1);
                Debug.Log("This game is already save it. replacing that game...");
            }
            else
                Debug.Log("This game is NOT already save it. creating a new slot...");
        }
        else
            Debug.Log("This game is NOT already save it. creating a new slot...");

        all_games.games.Add(all_data);
        string json = JsonUtility.ToJson(all_games, true);
        File.WriteAllText(Application.dataPath + "/KPIs_file.json", json);

        Debug.Log("FILE SAVED!");
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/KPIs_file.json");
        all_games = JsonUtility.FromJson<SavedGames>(json);
        Debug.Log("FILE LOADED!");

        //Debug.Log(data.kill_pos[1]);
        //Debug.Log(data.hit_pos[1]);
        //Debug.Log(data.acid_pos[1]);
        //Debug.Log(data.death_pos[1]);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SaveToJson();

        if (Input.GetKeyDown(KeyCode.L))
            LoadFromJson();
    }
}
