using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatrolBehavior
{
    None,
    UpDown,
    LeftRight
}

public class Enemy : MonoBehaviour 
{
    public float EnemySpeed;

    public GameObject DoorToToggleOnDeath;
    public int DoorToToggleId = -1;
    public bool OnDeathToggleAll = false;

    public PatrolBehavior Behavior;

    private Rigidbody2D Rigidbody;
    private Vector3 PatrolDirection;
    private bool Destroyed = false;

	void Start () 
	{
        Rigidbody = GetComponent<Rigidbody2D>();

        if (Behavior == PatrolBehavior.UpDown)
        {
            PatrolDirection = new Vector3(0f, 1f, 0f);
        }

        if (Behavior == PatrolBehavior.LeftRight)
        {
            PatrolDirection = new Vector3(1f, 0f, 0f);
        }
	}
	
	void Update () 
	{
		if (Behavior != PatrolBehavior.None)
        {
            Rigidbody.velocity = PatrolDirection * EnemySpeed;
        }
	}
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Destroyed)
        {
            return;
        }

        if (collision.GetComponent<Bullet>() != null || collision.GetComponent<Bomb>() != null)
        {
            if (DoorToToggleOnDeath != null)
            {
                DoorToToggleOnDeath.SendMessage("Toggle");
            }

            if (OnDeathToggleAll)
            {
                GameManager.Instance.ToggleAllLaserDoors();
            }

            GameManager.Instance.OnEntityDestroyed(this.gameObject);
            Destroyed = true;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("Player"))
        {
            return;
        }

        if (Behavior != PatrolBehavior.None)
        {
            PatrolDirection = -PatrolDirection;
        }
    }
}
