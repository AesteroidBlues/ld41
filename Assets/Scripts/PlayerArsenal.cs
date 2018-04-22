using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{

}

public enum BombType
{

}

public class PlayerArsenal : MonoBehaviour 
{
    private const float MAX_SHOT_TIMER = 300f;
    private const float MAX_SHOT_COOLDOWN = 0.2f;
    private const float MAX_BOMB_COOLDOWN = 2.5f;

    public BombType Bomb;
    public ShotType Shot;
    public int NumBombs;

    public float ShotTimer;

    private float ShotCooldown;
    private float BombCooldown;

	void Start () 
	{
        ShotTimer = MAX_SHOT_TIMER;
	}
	
	void Update () 
	{
        if (ShotCooldown > 0)
            ShotCooldown -= Time.deltaTime;

        if (BombCooldown > 0)
            BombCooldown -= Time.deltaTime;

        if (Input.GetButtonDown("FireBullet") && ShotCooldown <= 0)
        {
            GameObject bullet = GameObject.Instantiate(ResourceManager.Instance.BasicBullet);
            bullet.transform.position = this.transform.position;
            bullet.transform.rotation = this.transform.rotation;

            ShotCooldown = MAX_SHOT_COOLDOWN;
        }

        if (Input.GetButtonDown("FireBomb") && BombCooldown <= 0)
        {
            GameObject bomb = GameObject.Instantiate(ResourceManager.Instance.BasicBomb);
            bomb.transform.position = this.transform.position;
            bomb.transform.rotation = Quaternion.identity;

            BombCooldown = MAX_BOMB_COOLDOWN;
        }
    }
}
