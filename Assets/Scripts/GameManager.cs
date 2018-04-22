using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityAction AllEnemiesDestroyed;
    public GameObject RestartHint;

    public int CurrentLevelId = 0;

    private int numEnemies;

    private GameObject CurrentLevel;

    private List<GameObject> InstantiatedObjects;
    private List<Door> LaserDoors;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple resource managers! Wat!");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InstantiatedObjects = new List<GameObject>();
        LaserDoors = new List<Door>();

        LoadMap(CurrentLevelId);

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            Debug.Log("Scene loaded");
            SceneManager.SetActiveScene(scene);
        };
    }

    private void Update()
    {
        if (Input.GetButtonDown("Reload"))
        {
            ReloadLevel();
        }
    }

    public void OnEntityDestroyed(GameObject enemy)
    {
        Debug.Log(enemy.name + " destroyed");
        if (InstantiatedObjects.Contains(enemy))
        {
            InstantiatedObjects.Remove(enemy);
            if (enemy.GetComponent<Enemy>() != null)
            {
                numEnemies--;
            }
        }

        if (enemy.name.Contains("Player"))
        {
            RestartHint.SetActive(true);
        }

        if (numEnemies <= 0 && AllEnemiesDestroyed != null)
        {
            AllEnemiesDestroyed.Invoke();
        }
    }

    public void RegisterInstance(GameObject go)
    {
        if (!InstantiatedObjects.Contains(go))
        {
            InstantiatedObjects.Add(go);
        }
    }

    public void UnregisterInstance(GameObject go)
    {
        if (InstantiatedObjects.Contains(go))
        {
            InstantiatedObjects.Remove(go);
        }
    }

    public void LoadNextLevel()
    {
        CleanUpLevel();
        LoadMap(++CurrentLevelId);
    }

    public void ReloadLevel()
    {
        CleanUpLevel();
        LoadMap(CurrentLevelId);
    }

    public void LoadMap(int mapId)
    {
        if (ResourceManager.Instance.Maps.Count <= mapId)
        {
            Debug.LogWarning("Tried to load a non-existant map! Assuming the game is over");
            SceneManager.LoadScene("end");
            return;
        }

        // Instantiate the map
        CurrentLevel = Instantiate(ResourceManager.Instance.Maps[mapId]);

        // Move it to Z=10
        CurrentLevel.transform.position = new Vector3(0, 0, 10);

        // Center the map up with the camera
        Transform collision = CurrentLevel.transform.Find("Walls/Collision");
        if (collision != null)
        {
            var collider = collision.gameObject.GetComponent<PolygonCollider2D>();
            Camera.main.transform.position = new Vector3
            (
                collider.bounds.center.x,
                collider.bounds.center.y,
                -10f
            );
        }

        // Iterate over all the objects in the level and replace them with our game objects
        Transform objects = CurrentLevel.transform.Find("Objects");
        Dictionary<int, GameObject> lasers = new Dictionary<int, GameObject>();

        for (int i = 0; i < objects.childCount; i++)
        {
            Transform child = objects.GetChild(i);
            CustomProperties props = child.GetComponent<CustomProperties>();
            if (child.name == "Player Start")
            {
                GameObject player = Instantiate(ResourceManager.Instance.Player);
                player.transform.position = new Vector3(child.position.x, child.position.y, 0);
                InstantiatedObjects.Add(player);

                Destroy(child.gameObject);
            }

            if (child.name == "Enemy")
            {
                GameObject enemy = Instantiate(ResourceManager.Instance.Enemy);
                enemy.transform.position = new Vector3(child.position.x, child.position.y, 0);
                InstantiatedObjects.Add(enemy);

                var enemyComponent = enemy.GetComponent<Enemy>();
                if (props != null && props.OnDeathToggle >= 0)
                {
                    enemyComponent.DoorToToggleId = props.OnDeathToggle;
                }

                if (props != null && props.OnDeathToggleAll)
                {
                    enemyComponent.OnDeathToggleAll = props.OnDeathToggleAll;
                }

                if (props != null && props.PatrolBehavior != null)
                {
                    if (props.PatrolBehavior == "patrol_up_down")
                    {
                        enemyComponent.Behavior = PatrolBehavior.UpDown;
                    }

                    if (props.PatrolBehavior == "patrol_left_right")
                    {
                        enemyComponent.Behavior = PatrolBehavior.LeftRight;
                    }
                }

                Destroy(child.gameObject);
                numEnemies++;
            }

            if (child.name == "RDL")
            {
                GameObject levelDoor = Instantiate(ResourceManager.Instance.LevelDoors);
                // TODO: Figure out how to square this up without magic numbers
                levelDoor.transform.position = new Vector3(child.position.x + 0.32f, child.position.y, 0);
                InstantiatedObjects.Add(levelDoor);

                Destroy(child.gameObject);
            }

            if (child.name == "RDR")
            {
                Destroy(child.gameObject);
            }

            if (child.name == "Controls")
            {
                GameObject controlsHelp = Instantiate(ResourceManager.Instance.Controls);
                controlsHelp.transform.position = new Vector3(child.position.x, child.position.y, -5);
                InstantiatedObjects.Add(controlsHelp);

                Destroy(child.gameObject);
            }

            if (child.name == "Controls2")
            {
                GameObject controlsHelp = Instantiate(ResourceManager.Instance.Controls2);
                controlsHelp.transform.position = new Vector3(child.position.x, child.position.y, -5);
                InstantiatedObjects.Add(controlsHelp);

                Destroy(child.gameObject);
            }

            if (props != null && props.IsLaser)
            {
                int id = int.Parse(child.name);

                GameObject laser = Instantiate(ResourceManager.Instance.LaserDoor);
                laser.transform.position = new Vector3(child.position.x + 0.32f, child.position.y + 0.32f, -5);
                Debug.Log(props.IsOpen);
                laser.GetComponent<Door>().IsOpen = props.IsOpen;
                InstantiatedObjects.Add(laser);
                lasers.Add(id, laser);

                LaserDoors.Add(laser.GetComponent<Door>());

                RaycastHit2D hit = Physics2D.Raycast(new Vector2(laser.transform.position.x, laser.transform.position.y - 2.1f), -Vector2.up);
                if (hit.collider != null)
                {
                    float distance = Mathf.Abs(hit.point.y - transform.position.y);
                    laser.transform.Find("ClosedState").transform.localScale = new Vector3(1, 40, 1);
                }
                else
                {
                    Debug.LogWarning("Hit nothing");
                }

                Destroy(child.gameObject);
            }
        }

        // Do another pass over the enemies to hook up their toggle doors
        foreach (var instance in InstantiatedObjects)
        {
            Enemy enemy = instance.GetComponent<Enemy>();
            if (enemy == null || enemy.DoorToToggleId == -1)
            {
                continue;
            }

            if (!lasers.ContainsKey(enemy.DoorToToggleId))
            {
                Debug.LogError("Unmatched door/enemy pair!");
                continue;
            }

            enemy.DoorToToggleOnDeath = lasers[enemy.DoorToToggleId];
        }
    }

    public void CleanUpLevel()
    {
        Destroy(CurrentLevel);
        foreach(var obj in InstantiatedObjects)
        {
            // Clean up door action listeners
            if (obj.GetComponent<Door>() != null)
            {
                obj.GetComponent<Door>().CleanUp();
            }
            Destroy(obj);
        }

        RestartHint.SetActive(false);
        InstantiatedObjects.Clear();
        LaserDoors.Clear();
        numEnemies = 0;
    }

    public void ToggleAllLaserDoors()
    {
        foreach(var door in LaserDoors)
        {
            door.Toggle();
        }
    }
}
