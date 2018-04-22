using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    private const float MAX_LIFETIME = 2f;

    public Vector3 Direction;
    public float BulletSpeed;

    private float Lifetime;

	void Start () 
	{
        Lifetime = 0f;
        GameManager.Instance.RegisterInstance(this.gameObject);
    }
	
	void Update () 
	{
        Lifetime += Time.deltaTime;
        if (Lifetime >= MAX_LIFETIME)
        {
            GameManager.Instance.UnregisterInstance(this.gameObject);
            Destroy(this.gameObject);
        }

        transform.position += transform.up * BulletSpeed * Time.deltaTime;
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Contains("Player"))
        {
            GameManager.Instance.UnregisterInstance(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
