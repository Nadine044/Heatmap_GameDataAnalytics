using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadVisualData : MonoBehaviour
{
    public GameObject skull;
    public GameObject hit;
    public GameObject killEnemy;
    public GameObject path;
    public List<Vector3> frontPathLines;
    public List<Vector3> backPathLines;

    public GameObject originalCubeForGrid;
    public GameObject CubeForGridToReplicate;
    public GameObject originalCubeForGridPos;

    List<GameObject> grid;


    [Range(1, 30)]
    public int gridRange = 1;
    public int gridRangeOld = 0;

    public int MaxDeathsCount = 0;
    public int MaxHitsCount = 0;
    public int MaxKillsCount = 0;
    public int MaxPathCount = 0;

    //[HideInInspector]
    public SaveAndLoad info;

    public bool deathEnabled = true;
    public bool hitEnable = false;
    public bool killEnemyEnabled = false;
    public bool pathEnabled = false;
    enum Mode {death = 0, hit, killEnemy, path }

    [SerializeField] Mode currentMode = Mode.death;
    Mode oldMode = Mode.death; // When buttons are implemented this will not be needed.

    Vector3 frontPoint = Vector3.zero;
    Vector3 backPoint = Vector3.zero;

    private void Awake()
    {
        grid = new List<GameObject>();
        info.LoadFromJson();
    }

    // Update is called once per frame
    public void Update()
    {
        //LoadVisualData_Assets();
        //frontPathLines and backPathLines lists already full after this previous function

        for (int i = 0; i < frontPathLines.Count; i++)
        {
            Vector3 dir = (frontPathLines[i] - backPathLines[i]).normalized;
            Vector3 pos = transform.TransformDirection(dir) * Vector3.Distance(frontPathLines[i], backPathLines[i]);
            Debug.DrawRay(frontPathLines[i] + new Vector3(0.0f, 2.0f, 0.0f), -pos, Color.green);
        }
        if (gridRange != gridRangeOld)
        {
            GridUpdate();
        }
        if(currentMode != oldMode)
        {
            ChangeMode(currentMode);
            oldMode = currentMode;
        }
    }
    private void ChangeMode(Mode newMode)
    {
        switch(newMode)
        {
            case Mode.death:
                deathEnabled = true;
                hitEnable = false;
                killEnemyEnabled = false;
                pathEnabled = false;
                break;

            case Mode.hit:
                deathEnabled = false;
                hitEnable = true;
                killEnemyEnabled = false;
                pathEnabled = false;
                break;

            case Mode.killEnemy:
                deathEnabled = false;
                hitEnable = false;
                killEnemyEnabled = true;
                pathEnabled = false;
                break;

            case Mode.path:
                deathEnabled = false;
                hitEnable = false;
                killEnemyEnabled = false;
                pathEnabled = true;
                break;
        }

        foreach(GameObject cube in grid)
            cube.GetComponent<GridLogic2>().SetColorFromMaxData();
        
    }
    private void GridUpdate()
    { 
        ResetGrid();
        CreateNewGrid();

        gridRangeOld = gridRange;
    }

    private void CreateNewGrid()
    {
        int totalCubes = gridRange * gridRange;
        float newCubeScaleX = originalCubeForGrid.transform.localScale.x / gridRange;
        float newCubeScaleZ = originalCubeForGrid.transform.localScale.z / gridRange;

        for (int i = 0; i < totalCubes; ++i)
        {
            GameObject newCube = GameObject.Instantiate(CubeForGridToReplicate, new Vector3(originalCubeForGridPos.transform.position.x + (newCubeScaleX / 2) + ((newCubeScaleX) * (i % gridRange)), originalCubeForGridPos.transform.position.y, originalCubeForGridPos.transform.position.z + (newCubeScaleZ / 2) + ((newCubeScaleZ) * (i / gridRange))), originalCubeForGrid.transform.rotation);
            newCube.transform.localScale = new Vector3(newCubeScaleX, newCube.transform.localScale.y, newCubeScaleZ);
            newCube.GetComponent<GridLogic2>().SetCountersFromData();
            grid.Add(newCube);

            if (newCube.GetComponent<GridLogic2>().deathsCount > MaxDeathsCount)
                MaxDeathsCount = newCube.GetComponent<GridLogic2>().deathsCount;

            if (newCube.GetComponent<GridLogic2>().hitsCount > MaxHitsCount)
                MaxHitsCount = newCube.GetComponent<GridLogic2>().hitsCount;

            if (newCube.GetComponent<GridLogic2>().killsCount > MaxKillsCount)
                MaxKillsCount = newCube.GetComponent<GridLogic2>().killsCount;

            if (newCube.GetComponent<GridLogic2>().pathCount > MaxPathCount)
                MaxPathCount = newCube.GetComponent<GridLogic2>().pathCount;
        }
    }

    private void ResetGrid()
    {
        if (grid.Count != 0)
        {
            foreach (GameObject obj in grid)
                Destroy(obj);
            grid.Clear();
        }

        MaxDeathsCount = 0;
        MaxHitsCount = 0;
        MaxKillsCount = 0;
        MaxPathCount = 0;
    }

    public void LoadVisualData_Assets()
    {
        if (info.all_data.kill_pos != null && killEnemyEnabled) 
        {
            for (int i = 0; i < info.all_data.kill_pos.Count; i++)
                Instantiate(killEnemy, new Vector3(info.all_data.kill_pos[i].x, info.all_data.kill_pos[i].y, info.all_data.kill_pos[i].z), transform.rotation);
        }

        if (info.all_data.hit_pos != null && hitEnable)
        {
            for (int i = 0; i < info.all_data.hit_pos.Count; i++)
                Instantiate(hit, new Vector3(info.all_data.hit_pos[i].x, info.all_data.hit_pos[i].y, info.all_data.hit_pos[i].z), transform.rotation);
        }


        if (info.all_data.death_pos != null && deathEnabled)
        {
            for (int i = 0; i < info.all_data.death_pos.Count; i++)
                Instantiate(skull, new Vector3(info.all_data.death_pos[i].x, info.all_data.death_pos[i].y, info.all_data.death_pos[i].z), transform.rotation);
        }

        if (info.all_data.path_pos != null && pathEnabled)
        {

            for (int i = 0; i < info.all_data.path_pos.Count; i++)
            {
                frontPoint = info.all_data.path_pos[i];
                frontPathLines.Add(frontPoint);

                if (i == 0)
                {
                    backPoint = frontPoint;
                    backPathLines.Add(backPoint);
                }
                else
                {
                    backPoint = info.all_data.path_pos[i - 1];
                    backPathLines.Add(backPoint);
                }

                Instantiate(path, new Vector3(info.all_data.path_pos[i].x, info.all_data.path_pos[i].y + 2.0f, info.all_data.path_pos[i].z), transform.rotation);
            }
        }

        killEnemyEnabled = false;
        deathEnabled = false;
        hitEnable = false;
        pathEnabled = false;
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position

        for(int i = 0; i < info.all_data.kill_pos.Count; ++i)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(info.all_data.kill_pos[i].x, info.all_data.kill_pos[i].y +1, info.all_data.kill_pos[i].z), 3);
        }
    }
}
