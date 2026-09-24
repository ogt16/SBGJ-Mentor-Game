using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SkillCheck : MonoBehaviour
{

    public Player player;
    public bool attacking;
    public bool inTarget;
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
        //if (transform.Find("Handle Slide Area").gameObject.transform.Find("Handle").gameObject.GetComponent<RectTransform>().rect.Overlaps(transform.Find("Target").gameObject.GetComponent<RectTransform>().rect))
        //{
        //    inTarget = true;
        //}
        //else
        //{
        //    inTarget = false;
        //}
        Debug.Log(inTarget);
        GetComponent<Slider>().value+=2;
        if (GetComponent<Slider>().value >= GetComponent<Slider>().maxValue)
        {
            player.SkillCheckMiss();
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

    public bool SetAttack()
    {
        attacking = true;
        Debug.Log("Attack sent!");
        if (inTarget)
        {
            Debug.Log("Hit");
            return true;
            //player.SkillCheckHit();
        }
        else
        {
            
            Debug.Log("Miss");
            return false;
            //player.SkillCheckMiss();
        }
    }
}
