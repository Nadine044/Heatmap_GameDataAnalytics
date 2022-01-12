using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadVisualData : MonoBehaviour
{
    enum Mode { death = 0, hit, killEnemy, path, acid, fall }
    //enum Visibility { death, hits, kills, paths, acidDeaths, fallDeaths }

    //------ Prefabs to instantiate ------
    public GameObject skull;
    public GameObject hit;
    public GameObject killEnemy;
    public GameObject ball;
    public GameObject arrow;
    public GameObject fall;
    public GameObject acid;

    List<GameObject> arrows = new List<GameObject>();
    List<GameObject> deathsInstantiates = new List<GameObject>();
    List<GameObject> hitsInstantiates = new List<GameObject>();
    List<GameObject> killsInstantiates = new List<GameObject>();
    List<GameObject> acidInstantiates = new List<GameObject>();
    List<GameObject> fallInstantiates = new List<GameObject>();

    public GameObject CubeForGridToReplicate;

    //------ Data ------
    public KPIs_info currentData;
    public SaveAndLoad saveFile;

    //Arrows
    public List<GameObject> allPathBalls;

    //For the grid
    [Range(1, 30)]
    public int subdivisions = 1;

    List<GameObject> grid;

    public GameObject originalCubeForGrid;
    public GameObject originalCubeForGridPos;

    //Counters
    public int MaxDeathsCount = 0;
    public int MaxHitsCount = 0;
    public int MaxKillsCount = 0;
    public int MaxPathCount = 0;
    public int MaxAcidCount = 0;
    public int MaxFallCount = 0;


    //bools mode - Maybe will not be needed with the dropdown, could be eliminated.
    public bool deathEnabled = true;
    public bool hitEnable = false;
    public bool killEnemyEnabled = false;
    public bool pathEnabled = false;
    public bool arrowsEnabled = false;
    public bool acidEnabled = false;
    public bool fallEnabled = false;


    [SerializeField] Mode currentMode = Mode.death;


    //------ UI elements ------
    public Dropdown currentGameDropdown;
    public Dropdown currentModeDropdown;
    List<string> DropGames = new List<string>();
    public Toggle showAllGames;
    public Slider subdivisionsSlider;
    public Text subdivisionsText;

    //------ FUNCTIONS -------
    private void Start()
    {
        grid = new List<GameObject>();

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
        currentGameDropdown.onValueChanged.AddListener(delegate { CurrentGameChanged();});
        showAllGames.onValueChanged.AddListener(delegate { ShowAllGamesChanged(); });
        currentModeDropdown.onValueChanged.AddListener(delegate{ ChangeMode(); });
        subdivisionsSlider.onValueChanged.AddListener(delegate { GridUpdate(); });

        CreateNewGrid();
        LoadVisualData_Assets();

    }

    // Update is called once per frame
    public void Update()
    {
        //frontPathLines and backPathLines lists already full after this previous function

        
    }
    void ResetInstantiates()
    {
        if (allPathBalls.Count != 0)
        {
            foreach (GameObject go in allPathBalls)
                Destroy(go);
        }
        if (arrows.Count != 0)
        {
            foreach (GameObject go in arrows)
                Destroy(go);
        }
        if (deathsInstantiates.Count != 0)
        {
            foreach (GameObject go in deathsInstantiates)
                Destroy(go);
        }
        if (hitsInstantiates.Count != 0)
        {
            foreach (GameObject go in hitsInstantiates)
                Destroy(go);
        }
        if (killsInstantiates.Count != 0)
        {
            foreach (GameObject go in killsInstantiates)
                Destroy(go);
        }
        if (acidInstantiates.Count != 0)
        {
            foreach (GameObject go in acidInstantiates)
                Destroy(go);
        }
        if (fallInstantiates.Count != 0)
        {
            foreach (GameObject go in fallInstantiates)
                Destroy(go);
        }

        allPathBalls.Clear();
        arrows.Clear();
        deathsInstantiates.Clear();
        hitsInstantiates.Clear();
        killsInstantiates.Clear();
        acidInstantiates.Clear();
        fallInstantiates.Clear();
        

    }
    private void ChangeMode()
    {
        currentMode = (Mode)currentModeDropdown.value;
        ResetInstantiates();
        switch (currentMode)
        {
            case Mode.death:
                deathEnabled = true;
                hitEnable = false;
                killEnemyEnabled = false;
                pathEnabled = false;
                arrowsEnabled = false;
                acidEnabled = false;
                fallEnabled = false;
                break;

            case Mode.hit:
                deathEnabled = false;
                hitEnable = true;
                killEnemyEnabled = false;
                pathEnabled = false;
                arrowsEnabled = false;
                acidEnabled = false;
                fallEnabled = false;
                break;

            case Mode.killEnemy:
                deathEnabled = false;
                hitEnable = false;
                killEnemyEnabled = true;
                pathEnabled = false;
                arrowsEnabled = false;
                acidEnabled = false;
                fallEnabled = false;
                break;

            case Mode.path:
                deathEnabled = false;
                hitEnable = false;
                killEnemyEnabled = false;
                pathEnabled = true;
                arrowsEnabled = true;
                acidEnabled = false;
                fallEnabled = false;
                break;

            case Mode.acid:
                deathEnabled = false;
                hitEnable = false;
                killEnemyEnabled = false;
                pathEnabled = false;
                arrowsEnabled = false;
                acidEnabled = true;
                fallEnabled = false;
                break;

            case Mode.fall:
                deathEnabled = false;
                hitEnable = false;
                killEnemyEnabled = false;
                pathEnabled = false;
                arrowsEnabled = false;
                acidEnabled = false;
                fallEnabled = true;
                break;
        }

        foreach (GameObject cube in grid)
            cube.GetComponent<GridLogic2>().SetColorFromMaxData();

        LoadVisualData_Assets();
    }
    private void CreateNewGrid()
    {
        int totalCubes = subdivisions * subdivisions;
        float newCubeScaleX = originalCubeForGrid.transform.localScale.x / subdivisions;
        float newCubeScaleZ = originalCubeForGrid.transform.localScale.z / subdivisions;

        for (int i = 0; i < totalCubes; ++i)
        {
            GameObject newCube = GameObject.Instantiate(CubeForGridToReplicate, new Vector3(originalCubeForGridPos.transform.position.x + (newCubeScaleX / 2) + ((newCubeScaleX) * (i % subdivisions)), originalCubeForGridPos.transform.position.y, originalCubeForGridPos.transform.position.z + (newCubeScaleZ / 2) + ((newCubeScaleZ) * (i / subdivisions))), originalCubeForGrid.transform.rotation);
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

        if (cube.GetComponent<GridLogic2>().acidCount > MaxAcidCount)
            MaxAcidCount = cube.GetComponent<GridLogic2>().acidCount;

        if (cube.GetComponent<GridLogic2>().fallCount > MaxFallCount)
            MaxFallCount = cube.GetComponent<GridLogic2>().fallCount;
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

    private void GridUpdate()
    {
        subdivisions = (int)subdivisionsSlider.value;
        subdivisionsText.text = ("Subdivisions: " + subdivisions.ToString());
        ResetGrid();
        CreateNewGrid();
    }

    private void ResetMaxCounters()
    {
        MaxDeathsCount = 0;
        MaxHitsCount = 0;
        MaxKillsCount = 0;
        MaxPathCount = 0;
        MaxAcidCount = 0;
        MaxFallCount = 0;
    }

    public void LoadVisualData_Assets()
    {
        if (currentData.kill_pos != null && killEnemyEnabled)
        {
            for (int i = 0; i < currentData.kill_pos.Count; i++)
               killsInstantiates.Add(Instantiate(killEnemy, new Vector3(currentData.kill_pos[i].x, currentData.kill_pos[i].y, currentData.kill_pos[i].z), transform.rotation));
 
        }

        if (currentData.hit_pos != null && hitEnable)
        {
            for (int i = 0; i < currentData.hit_pos.Count; i++)
                hitsInstantiates.Add(Instantiate(hit, new Vector3(currentData.hit_pos[i].x, currentData.hit_pos[i].y, currentData.hit_pos[i].z), transform.rotation));
        }


        if (currentData.death_pos != null && deathEnabled)
        {
            for (int i = 0; i < currentData.death_pos.Count; i++)
                deathsInstantiates.Add(Instantiate(skull, new Vector3(currentData.death_pos[i].x, currentData.death_pos[i].y, currentData.death_pos[i].z), transform.rotation));
        }

        if (currentData.fall_pos != null && fallEnabled)
        {
            for (int i = 0; i < currentData.fall_pos.Count; i++)
                fallInstantiates.Add(Instantiate(fall, new Vector3(currentData.fall_pos[i].x, currentData.fall_pos[i].y + 5.0f, currentData.fall_pos[i].z), transform.rotation));
        }

        if (currentData.acid_pos != null && acidEnabled)
        {
            for (int i = 0; i < currentData.acid_pos.Count; i++)
                acidInstantiates.Add(Instantiate(acid, new Vector3(currentData.acid_pos[i].x, currentData.acid_pos[i].y, currentData.acid_pos[i].z), transform.rotation));
        }

        if (currentData.paths != null && pathEnabled)
        {
            InstantiateBalls();
        }

        if (arrowsEnabled)
        {
            for (int i = 0; i < allPathBalls.Count; i++)
            {
                GameObject go = Instantiate(arrow, new Vector3(allPathBalls[i].transform.position.x, allPathBalls[i].transform.position.y, allPathBalls[i].transform.position.z), new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));

                if (i + 1 >= allPathBalls.Count)
                {
                    Debug.Log("No more arrows to draw");
                }
                else
                {
                    go.transform.LookAt(new Vector3(allPathBalls[i + 1].transform.position.x, allPathBalls[i + 1].transform.position.y, allPathBalls[i + 1].transform.position.z));
                }

                arrows.Add(go);
            }
        }
    }

    private void InstantiateBalls()
    {
        for (int x = 0; x < currentData.paths.Count; ++x)
        {
            for (int i = 0; i < currentData.paths[x].pathPositions.Count; i++)
            {
                GameObject balls_go = Instantiate(ball, new Vector3(currentData.paths[x].pathPositions[i].x, currentData.paths[x].pathPositions[i].y + 2.0f, currentData.paths[x].pathPositions[i].z), transform.rotation);
                balls_go.SetActive(false);
                allPathBalls.Add(balls_go);
            }
        }
    }

    private void UpdateAllData()
    {
        ResetInstantiates();
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

        LoadVisualData_Assets();
    }
    void CurrentGameChanged()
    {
        currentData = saveFile.all_games.games[currentGameDropdown.value];
        UpdateAllData();
    }
    void ShowAllGamesChanged()
    {
        if(showAllGames.isOn)
        {
            currentGameDropdown.interactable = false;

            currentData = new KPIs_info();
            foreach (KPIs_info data in saveFile.all_games.games)
            {
                foreach (Vector3 deathPos in data.death_pos)
                    currentData.death_pos.Add(deathPos);

                foreach (Vector3 hitPos in data.hit_pos)
                    currentData.hit_pos.Add(hitPos);

                foreach (Vector3 killPos in data.kill_pos)
                    currentData.kill_pos.Add(killPos);

                foreach (PlayerPath pathPos in data.paths)
                    currentData.paths.Add(pathPos);

                foreach (Vector3 acidPos in data.acid_pos)
                    currentData.acid_pos.Add(acidPos);

                foreach (Vector3 fallPos in data.fall_pos)
                    currentData.fall_pos.Add(fallPos);

                UpdateAllData();

            }
        }
        else
        {
            currentGameDropdown.interactable = true;

            CurrentGameChanged();

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
