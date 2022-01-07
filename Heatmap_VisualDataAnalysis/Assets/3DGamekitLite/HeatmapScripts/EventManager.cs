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
        ellen.OnDeath.AddListener(SaveDeathData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SaveDeathData()
    {
        data.all_data.death_pos.Add(ellen.transform.position);
    }
}
