using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    bool daytime = true;

    //Upgradable Stats + starting values
    public float influenceRadius;
    public float droneSpawnRate;
    public float influenceSpeed;
    public float dayLength;
    public float quality;
    public float passiveFollowerGain;
    public float startingInfluence;
    public float walkSpeed;
    public float skillCheckDifficulty;
    public float skillCheckPerfectReward;
    public float skillCheckRecovery;
    public float skillCheckFrequency;

    List<GameObject> collidingWithTrigger;

    public float passiveFollowerCount;
    public float passiveFollowerTimer = 60;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collidingWithTrigger = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (daytime)
        { 
            passiveFollowerTimer -= Time.deltaTime;
            if (passiveFollowerTimer < 0)
            {
                passiveFollowerTimer = 10;
                passiveFollowerCount += passiveFollowerGain;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collidingWithTrigger.Contains(collision.gameObject))
        {
            collidingWithTrigger.Add(collision.gameObject);
            if (collision.gameObject.CompareTag("Character"))
            {
                collision.gameObject.transform.Find("InformationPanel").gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collidingWithTrigger.Contains(collision.gameObject))
        {
            if (collision.gameObject.CompareTag("Character"))
            {
                collision.gameObject.transform.Find("InformationPanel").gameObject.SetActive(false);
            }
            collidingWithTrigger.Remove(collision.gameObject);
        }
    }

    public void OnMove(InputValue input)
    {
        if (rb == null) { return; }

        Vector2 value = input.Get<Vector2>();
        rb.linearVelocity = value * walkSpeed;
    }

    public void OnJump(InputValue input)
    {
        float value = input.Get<float>();
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject obj in collidingWithTrigger)
        {
            if (obj != null)
            {
                if (obj.CompareTag("Character"))
                {
                    //Increase influence
                    obj.GetComponent<CharacterData>().influence += influenceSpeed * value;
                    if (obj.GetComponent<CharacterData>().influence >= 100)
                    {
                        // Character is removed from drone list in day scene on next fixed update call
                        GameData.Instance.AddFollower(obj);
                        toRemove.Add(obj);
                    }
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
