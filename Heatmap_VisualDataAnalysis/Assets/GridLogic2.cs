using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLogic2 : MonoBehaviour
{
    public int deathsCount = 0;
    public int hitsCount = 0;
    public int killsCount = 0;
    public int pathCount = 0;

    public LoadVisualData visualData;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3 pos in visualData.info.all_data.kill_pos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0f);
            foreach(Collider col in hitColliders)
            {
                if (col.gameObject == this.gameObject)
                    ++killsCount;
            }
        }

        foreach (Vector3 pos in visualData.info.all_data.death_pos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0f);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == this.gameObject)
                    ++deathsCount;
            }
        }

        foreach (Vector3 pos in visualData.info.all_data.hit_pos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0f);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == this.gameObject)
                    ++hitsCount;
            }
        }

        foreach (Vector3 pos in visualData.info.all_data.path_pos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0f);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == this.gameObject)
                    ++pathCount;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
