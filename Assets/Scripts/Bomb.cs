using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour 
{
    public const float MAX_LIFETIME_IN_SECONDS = 2.5f;

    private Transform ExplosionAnimation;
    private float Lifetime;

	void Start () 
	{
        GameManager.Instance.RegisterInstance(this.gameObject);
        ExplosionAnimation = transform.Find("Explosion");
        Lifetime = 0f;
	}
	
	void Update () 
	{
        Lifetime += Time.deltaTime;
        if (Lifetime % 60 > MAX_LIFETIME_IN_SECONDS)
        {
            var collider = gameObject.AddComponent<CircleCollider2D>();
            collider.radius = 1;
            collider.isTrigger = true;
            ExplosionAnimation.gameObject.SetActive(true);
            GameManager.Instance.UnregisterInstance(this.gameObject);
            Destroy(this.gameObject, 0.2f);
        }
	}
}
