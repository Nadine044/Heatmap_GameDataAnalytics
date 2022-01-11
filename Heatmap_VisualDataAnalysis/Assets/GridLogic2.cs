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
    private void Start()
    {
        SetColorFromMaxData();
    }



    private void ResetCounters()
    {
        deathsCount = 0;
        hitsCount = 0;
        killsCount = 0;
        pathCount = 0;
    }

    public void SetCountersFromData()
    {
        ResetCounters();

        foreach (Vector3 pos in visualData.currentData.kill_pos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0f);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == this.gameObject)
                    ++killsCount;
            }
        }

        foreach (Vector3 pos in visualData.currentData.death_pos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0f);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == this.gameObject)
                    ++deathsCount;
            }
        }

        foreach (Vector3 pos in visualData.currentData.hit_pos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0f);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == this.gameObject)
                    ++hitsCount;
            }
        }

        foreach (Vector3 pos in visualData.currentData.path_pos)
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0f);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == this.gameObject)
                    ++pathCount;
            }
        }
    }

    public void SetColorFromMaxData()
    {
        if (visualData.deathEnabled)
        {
            float r;
            if (visualData.MaxDeathsCount != 0)
                r = (float)deathsCount / visualData.MaxDeathsCount; // between 0 and 1 (min and max)
            else
                r = 0;
            float red = (r * 2);
            if (red > 1)
                red = 1;
            float blue = 1;
            if (r - 0.5 > 0)
            {
                blue -= ((r - 0.5f) * 2);
            }
            GetComponent<Renderer>().material.color = new Color(red, 0.0f, blue, 0.75f);
        }
        else if (visualData.hitEnable)
        {
            float r;
            if (visualData.MaxHitsCount != 0)
                r = (float)hitsCount / visualData.MaxHitsCount; // between 0 and 1 (min and max)
            else
                r = 0; float red = (r * 2);
            if (red > 1)
                red = 1;
            float blue = 1;
            if (r - 0.5 > 0)
            {
                blue -= ((r - 0.5f) * 2);
            }
            GetComponent<Renderer>().material.color = new Color(red, 0.0f, blue, 0.75f);
        }
        else if (visualData.killEnemyEnabled)
        {
            float r;
            if (visualData.MaxKillsCount != 0)
                r = (float)killsCount / visualData.MaxKillsCount; // between 0 and 1 (min and max)
            else
                r = 0; float red = (r * 2);
            if (red > 1)
                red = 1;
            float blue = 1;
            if (r - 0.5 > 0)
            {
                blue -= ((r - 0.5f) * 2);
            }
            GetComponent<Renderer>().material.color = new Color(red, 0.0f, blue, 0.75f);
        }
        else if (visualData.pathEnabled)
        {
            float r;
            if (visualData.MaxPathCount != 0)
                r = (float)pathCount / visualData.MaxPathCount; // between 0 and 1 (min and max)
            else
                r = 0; float red = (r * 2);

            if (red > 1)
                red = 1;

            float blue = 1;
            if (r - 0.5 > 0)
            {
                blue -= ((r - 0.5f) * 2);
            }
            GetComponent<Renderer>().material.color = new Color(red, 0.0f, blue, 0.75f);
        }
    }
}
