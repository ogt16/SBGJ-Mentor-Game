using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D cl;
    List<GameObject> collidingWithTrigger;
    bool influencing;
    public GameObject skillCheck;
    bool skillCheckActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collidingWithTrigger = new List<GameObject>();
        cl = GetComponent<CircleCollider2D>();

        cl.radius = GameData.Instance.influenceRadius;
        transform.Find("InfluenceRadiusVisual").gameObject.transform.localScale = Vector3.one * 2 * cl.radius;
        bool influencing = false;
        skillCheckActive = false;
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
        rb.linearVelocity = value * GameData.Instance.walkSpeed;
    }

    public void OnJump(InputValue input)
    {
        //List<GameObject> toRemove = new List<GameObject>();
        if (input.Get<float>() == 1.0f)
        {
            influencing = true;
        }
        else
        {
            influencing = false;
        }
        //foreach (GameObject obj in collidingWithTrigger)
        //{
        //    if (obj != null)
        //    {
        //        if (obj.CompareTag("Character"))
        //        {
        //            //Increase influence
        //            obj.GetComponent<CharacterData>().influence += GameData.Instance.influenceSpeed * input.Get<int>();
        //            obj.GetComponent<CharacterData>().transform.Find("InformationPanel").gameObject.transform.Find("Canvas").gameObject.transform.Find("InfluenceMeter").gameObject.GetComponent<Slider>().value = obj.GetComponent<CharacterData>().influence;
        //            if (obj.GetComponent<CharacterData>().influence >= 100)
        //            {
        //                // Character is removed from drone list in day scene on next fixed update call
        //                GameData.Instance.AddFollower(obj.GetComponent<CharacterData>());
        //                toRemove.Add(obj);
        //            }
        //        }
        //    }
        //}

        //foreach (GameObject obj in toRemove)
        //{
        //    collidingWithTrigger.Remove(obj);
        //    obj.SetActive(false);
        //}
    }

    //Influence while Space is held
    public void FixedUpdate()
    {
        List<GameObject> toRemove = new List<GameObject>();
        if (influencing)
        {
            {
                foreach (GameObject obj in collidingWithTrigger)
                {
                    if (obj != null)
                    {
                        if (obj.CompareTag("Character"))
                        {
                            //Increase influence
                            obj.GetComponent<CharacterData>().influence += GameData.Instance.influenceSpeed;
                            obj.GetComponent<CharacterData>().transform.Find("InformationPanel").gameObject.transform.Find("Canvas").gameObject.transform.Find("InfluenceMeter").gameObject.GetComponent<Slider>().value = obj.GetComponent<CharacterData>().influence;
                            if (obj.GetComponent<CharacterData>().influence >= 100)
                            {
                                // Character is removed from drone list in day scene on next fixed update call
                                GameData.Instance.AddFollower(obj.GetComponent<CharacterData>());
                                toRemove.Add(obj);
                                if (skillCheck != null)
                                {
                                    Destroy(skillCheck);
                                }
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

            if (UnityEngine.Random.Range(0, 10000) >= GameData.Instance.skillCheckFrequency && !skillCheckActive && collidingWithTrigger.Count != 1)
            {
                Debug.Log(collidingWithTrigger.Count);
                Instantiate(skillCheck);
                skillCheck.GetComponent<SkillCheck>().player = this;
                skillCheckActive = true;
            }
        }
    }

    public void SkillCheckHit(GameObject skillCheck)
    {
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject obj in collidingWithTrigger)
        {
            if (obj != null)
            {
                if (obj.CompareTag("Character"))
                {
                    //Increase influence
                    obj.GetComponent<CharacterData>().influence += GameData.Instance.skillCheckPerfectReward;
                    obj.GetComponent<CharacterData>().transform.Find("InformationPanel").gameObject.transform.Find("Canvas").gameObject.transform.Find("InfluenceMeter").gameObject.GetComponent<Slider>().value = obj.GetComponent<CharacterData>().influence;
                    if (obj.GetComponent<CharacterData>().influence >= 100)
                    {
                        // Character is removed from drone list in day scene on next fixed update call
                        GameData.Instance.AddFollower(obj.GetComponent<CharacterData>());
                        toRemove.Add(obj);
                        if (skillCheck != null)
                        {
                            Destroy(skillCheck);
                        }
                    }
                }
            }
        }

        foreach (GameObject obj in toRemove)
        {
            collidingWithTrigger.Remove(obj);
            obj.SetActive(false);
        }

        Destroy(skillCheck);
        skillCheckActive = false;
    }

    public void SkillCheckMiss(GameObject skillCheck)
    {
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject obj in collidingWithTrigger)
        {
            if (obj != null)
            {
                if (obj.CompareTag("Character"))
                {
                    //Increase influence
                    obj.GetComponent<CharacterData>().influence -= GameData.Instance.skillCheckRecovery;
                    obj.GetComponent<CharacterData>().transform.Find("InformationPanel").gameObject.transform.Find("Canvas").gameObject.transform.Find("InfluenceMeter").gameObject.GetComponent<Slider>().value = obj.GetComponent<CharacterData>().influence;
                    if (obj.GetComponent<CharacterData>().influence >= 100)
                    {
                        // Character is removed from drone list in day scene on next fixed update call
                        GameData.Instance.AddFollower(obj.GetComponent<CharacterData>());
                        toRemove.Add(obj);
                        if (skillCheck != null)
                        {
                            Destroy(skillCheck);
                        }
                    }
                }
            }
        }

        foreach (GameObject obj in toRemove)
        {
            collidingWithTrigger.Remove(obj);
            obj.SetActive(false);
        }

        Destroy(skillCheck);
        skillCheckActive = false;
    }

    public void OnAttack()
    {
        Debug.Log("Attacked");
        if (skillCheck != null)
        {
            skillCheck.GetComponent<SkillCheck>().attacking = true;
        }
    }
}
