using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    float walkSpeed = 10;
    Rigidbody2D rb;

    List<GameObject> collidingWithTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collidingWithTrigger = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collidingWithTrigger.Contains(collision.gameObject))
        {
            collidingWithTrigger.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collidingWithTrigger.Contains(collision.gameObject))
        {
            collidingWithTrigger.Remove(collision.gameObject);
        }
    }

    public void OnMove(InputValue input)
    {
        if (rb == null) { return; }

        Vector2 value = input.Get<Vector2>();
        rb.linearVelocity = value * walkSpeed;
    }

    public void OnAttack()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject obj in collidingWithTrigger)
        {
            if (obj != null)
            {
                if (obj.CompareTag("Character"))
                {
                    // Character is removed from drone list in day scene on next fixed update call
                    GameData.Instance.AddFollower(obj);
                    toRemove.Add(obj);
                }
            }
        }

        foreach (GameObject obj in toRemove)
        {
            collidingWithTrigger.Remove(obj);
            obj.SetActive(false);
        }
    }
}
