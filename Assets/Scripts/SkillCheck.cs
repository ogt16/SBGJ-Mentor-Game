using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SkillCheck : MonoBehaviour
{

    public Player player;
    public bool attacking;
    bool inTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Canvas obj in FindObjectsByType<Canvas>(sortMode:FindObjectsSortMode.None))
        { 
            if (obj.CompareTag("DayCanvas") == true)
            {
                transform.SetParent(obj.transform, false);
                break;
            }
        }
        transform.localPosition = Vector3.zero;
        attacking = false;
        inTarget = false;
        transform.Find("Target").gameObject.transform.localScale = new Vector3(GameData.Instance.skillCheckDifficulty, 1, 0);
        transform.Find("Target").gameObject.transform.localPosition =new Vector3(Random.Range(-20, 70), 0 ,0);
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        GetComponent<Slider>().value++;
        if (GetComponent<Slider>().value >= GetComponent<Slider>().maxValue)
        {
            player.SkillCheckMiss(this.gameObject);
            //gameObject.SetActive(false);
        }
        //attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inTarget = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inTarget = false;
    }

    public void SetAttack()
    {
        Debug.Log("Attack sent!");
        if (inTarget)
        {
            Debug.Log("Hit");
            player.SkillCheckHit(this.gameObject);
        }
        else
        {
            Debug.Log("Miss");
            player.SkillCheckMiss(this.gameObject);
        }
    }
}
