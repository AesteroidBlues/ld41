using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float SpeedMultiplier;

    private Rigidbody2D Rigidbody;

	void Start ()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle Input
        float xComponent = Input.GetAxis("Horizontal");
        float yComponent = Input.GetAxis("Vertical");
        Vector3 velocity = new Vector3(xComponent, yComponent);
        Rigidbody.velocity = velocity.normalized * SpeedMultiplier;


        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (worldPos - (Vector2)transform.position).normalized;
        this.transform.up = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Next Room")
        {
            GameManager.Instance.LoadNextLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("Enemy"))
        {
            GameManager.Instance.OnEntityDestroyed(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
