using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gamekit3D;

public class EventManager : MonoBehaviour
{
    public Damageable ellen;
    public SaveAndLoad data;

    private float time = 0.0f;
    float period = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //subscribe to Ellen "death" and "hit" events.
        ellen.OnDeath.AddListener(SaveDeathData);
        ellen.OnReceiveDamage.AddListener(SaveHitData);
        ellen.OnDeathFall.AddListener(SaveDeathFallData);
        ellen.OnDeathAcid.AddListener(SaveDeathAcidData);

        //subscribe to enemies "death" event wich will be the kill position of ellen.
        Damageable[] AllObjects = FindObjectsOfType<Damageable>();
        foreach (Damageable obj in AllObjects)
        {
            if (obj.gameObject.layer == 23/*Enemies*/ && obj.gameObject.name != "Cube")
            {
                obj.OnDeath.AddListener(SaveKillData);
                Debug.Log(obj.gameObject.name);
                Debug.Log(obj.gameObject.transform.position);
            }
        }
    }

    public void Update()
    {
        time += Time.deltaTime;

        if (time >= period)
        {
            time = time - period;
            SavePath();
            Debug.Log("HEY, IT HAS BEEN 3 SECONDS FROM NOW ON :)");
        }
    }

    void SaveDeathData()
    {
        data.all_data.hit_pos.Add(ellen.transform.position); //Save hit that kills Ellen (when she dies the program not calls the damage recive event by itself)
        data.all_data.death_pos.Add(ellen.transform.position);
    }

    void SaveHitData()
    {
        data.all_data.hit_pos.Add(ellen.transform.position);
    }
    void SaveKillData()
    {
        data.all_data.kill_pos.Add(ellen.transform.position); //Pos of Ellen killing enemies
    }

    void SavePath()
    {
        data.all_data.path_pos.Add(ellen.transform.position);
    }

    void SaveDeathAcidData()
    {
        data.all_data.acid_pos.Add(ellen.transform.position);
    }

    void SaveDeathFallData()
    {
        data.all_data.fall_pos.Add(ellen.transform.position);
    }
}
