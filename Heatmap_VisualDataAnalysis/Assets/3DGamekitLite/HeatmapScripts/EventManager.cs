using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gamekit3D;

public class EventManager : MonoBehaviour
{
    public Damageable ellen;
    public SaveAndLoad data;
    // Start is called before the first frame update
    void Start()
    {
        //subscribe to Ellen "death" and "hit" events.
        ellen.OnDeath.AddListener(SaveDeathData);
        ellen.OnReceiveDamage.AddListener(SaveHitData);

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
    void SaveDeathData()
    {
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
}
