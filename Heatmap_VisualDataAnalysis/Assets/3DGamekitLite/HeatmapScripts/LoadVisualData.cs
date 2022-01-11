using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadVisualData : MonoBehaviour
{
    public GameObject skull;
    public GameObject hit;
    public GameObject killEnemy;
    public GameObject path;
    public GameObject arrow;

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
    public SaveAndLoad saveFile;

    public bool deathEnabled = true;
    public bool hitEnable = false;
    public bool killEnemyEnabled = false;
    public bool pathEnabled = false;
    public bool arrowsEnabled = false;
    enum Mode {death = 0, hit, killEnemy, path }

    [SerializeField] Mode currentMode = Mode.death;
    Mode oldMode = Mode.death; // When buttons wll be implemented this will not be needed.

    public KPIs_info currentData;

    public Dropdown currentGameDropdown;
    List<string> DropGames = new List<string>();

    Vector3 frontPoint = Vector3.zero;
    Vector3 backPoint = Vector3.zero;

    private void Start()
    {
        grid = new List<GameObject>();

        saveFile.LoadFromJson();
        currentGameDropdown.ClearOptions();
        if (saveFile.all_games.games.Count == 0)
        {
            Debug.Log("No games are saved!!");

            DropGames = new List<string> { "No game loaded" };
        }
        else
        {
            foreach (KPIs_info gameIterator in saveFile.all_games.games)
            {
                DropGames.Add("Game Number " + gameIterator.game_number.ToString());
            }
            currentData = saveFile.all_games.games[0]; //Load by default the first one.
        }

        currentGameDropdown.AddOptions(DropGames);
        currentGameDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged();});
    }

    // Update is called once per frame
    public void Update()
    {
        //LoadVisualData_Assets();
        //frontPathLines and backPathLines lists already full after this previous function

        if (gridRange != gridRangeOld)
        {
            GridUpdate();
        }
        if(currentMode != oldMode)
        {
            ChangeMode(currentMode);
            oldMode = currentMode;
        }

        if (arrowsEnabled)
        {
            for (int i = 0; i < frontPathLines.Count; i++)
            {
                Vector3 dir = (frontPathLines[i] - backPathLines[i]).normalized;
                Vector3 pos = transform.TransformDirection(dir) * Vector3.Distance(frontPathLines[i], backPathLines[i]);
                //Debug.DrawRay(frontPathLines[i] + new Vector3(0.0f, 2.0f, 0.0f), -pos, Color.green);

                if (i <= frontPathLines.Count)
                {
                    Instantiate(arrow, new Vector3(frontPathLines[i].x, frontPathLines[i].y + 1.5f, frontPathLines[i].z), transform.rotation);
                    arrowsEnabled = false;
                }
            }
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
                arrowsEnabled = false;
                break;

            case Mode.hit:
                deathEnabled = false;
                hitEnable = true;
                killEnemyEnabled = false;
                pathEnabled = false;
                arrowsEnabled = false;
                break;

            case Mode.killEnemy:
                deathEnabled = false;
                hitEnable = false;
                killEnemyEnabled = true;
                pathEnabled = false;
                arrowsEnabled = false;
                break;

            case Mode.path:
                deathEnabled = false;
                hitEnable = false;
                killEnemyEnabled = false;
                pathEnabled = true;
                arrowsEnabled = true;
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
            SetMaxCounters(newCube);
        }
    }

    private void SetMaxCounters(GameObject cube)
    {
        if (cube.GetComponent<GridLogic2>().deathsCount > MaxDeathsCount)
            MaxDeathsCount = cube.GetComponent<GridLogic2>().deathsCount;

        if (cube.GetComponent<GridLogic2>().hitsCount > MaxHitsCount)
            MaxHitsCount = cube.GetComponent<GridLogic2>().hitsCount;

        if (cube.GetComponent<GridLogic2>().killsCount > MaxKillsCount)
            MaxKillsCount = cube.GetComponent<GridLogic2>().killsCount;

        if (cube.GetComponent<GridLogic2>().pathCount > MaxPathCount)
            MaxPathCount = cube.GetComponent<GridLogic2>().pathCount;
    }

    private void ResetGrid()
    {
        if (grid.Count != 0)
        {
            foreach (GameObject obj in grid)
                Destroy(obj);
            grid.Clear();
        }

        ResetMaxCounters();
    }

    private void ResetMaxCounters()
    {
        MaxDeathsCount = 0;
        MaxHitsCount = 0;
        MaxKillsCount = 0;
        MaxPathCount = 0;
    }

    public void LoadVisualData_Assets()
    {
        if (currentData.kill_pos != null && killEnemyEnabled) 
        {
            for (int i = 0; i < currentData.kill_pos.Count; i++)
                Instantiate(killEnemy, new Vector3(currentData.kill_pos[i].x, currentData.kill_pos[i].y, currentData.kill_pos[i].z), transform.rotation);
        }

        if (currentData.hit_pos != null && hitEnable)
        {
            for (int i = 0; i < currentData.hit_pos.Count; i++)
                Instantiate(hit, new Vector3(currentData.hit_pos[i].x, currentData.hit_pos[i].y, currentData.hit_pos[i].z), transform.rotation);
        }


        if (currentData.death_pos != null && deathEnabled)
        {
            for (int i = 0; i < currentData.death_pos.Count; i++)
                Instantiate(skull, new Vector3(currentData.death_pos[i].x, currentData.death_pos[i].y, currentData.death_pos[i].z), transform.rotation);
        }

        if (currentData.path_pos != null && pathEnabled)
        {

            for (int i = 0; i < currentData.path_pos.Count; i++)
            {
                frontPoint = currentData.path_pos[i];
                frontPathLines.Add(frontPoint);

                if (i == 0)
                {
                    backPoint = frontPoint;
                    backPathLines.Add(backPoint);
                }
                else
                {
                    backPoint = currentData.path_pos[i - 1];
                    backPathLines.Add(backPoint);
                }

                Instantiate(path, new Vector3(currentData.path_pos[i].x, currentData.path_pos[i].y + 2.0f, currentData.path_pos[i].z), transform.rotation);
            }
        }

        killEnemyEnabled = false;
        deathEnabled = false;
        hitEnable = false;
        pathEnabled = false;
    }

    void DropdownValueChanged()
    {
        currentData = saveFile.all_games.games[currentGameDropdown.value];
        ResetMaxCounters();
        foreach (GameObject current in grid)
        {
            current.GetComponent<GridLogic2>().SetCountersFromData();
            SetMaxCounters(current);
        }
        foreach (GameObject current in grid) //Needed before set all the data and the max from all and then we can put the correct color for each one.
        {
            current.GetComponent<GridLogic2>().SetColorFromMaxData();
        }
    }
    //void OnDrawGizmos()
    //{
    //    // Draw a yellow sphere at the transform's position

    //    for(int i = 0; i < currentData.kill_pos.Count; ++i)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawSphere(new Vector3(currentData.kill_pos[i].x, currentData.kill_pos[i].y +1, currentData.kill_pos[i].z), 3);
    //    }
    //}
}
