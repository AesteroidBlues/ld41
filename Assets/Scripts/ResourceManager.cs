using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour 
{
    [Header("Ships")]
    public GameObject Player;
    public GameObject Enemy;

    [Header("Ammo")]
    public GameObject BasicBullet;
    public GameObject BasicBomb;

    [Header("Level Elements")]
    public GameObject LevelDoors;
    public GameObject LaserDoor;
    public GameObject Controls;
    public GameObject Controls2;

    [Header("Maps")]
    public List<GameObject> Maps;

    public static ResourceManager Instance;

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
}
