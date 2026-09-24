using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SkillCheck : MonoBehaviour
{

    public Player player;
    public bool attacking;
    public bool inTarget;

    float targetWidth = GameData.Instance.skillCheckDifficulty;
    float targetMiddleX;

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
        targetMiddleX = Random.Range(-20, 70);

        transform.Find("Target").gameObject.transform.localScale = new Vector3(targetWidth, 1, 0);
        transform.Find("Target").gameObject.transform.localPosition =new Vector3(targetMiddleX, 0 ,0);
        transform.localScale = Vector3.one;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (targetMiddleX - (targetWidth / 2) < transform.Find("Handle Slide Area").gameObject.transform.Find("Handle").gameObject.GetComponent<RectTransform>().localPosition.x && targetMiddleX + (targetWidth / 2) > transform.Find("Handle Slide Area").gameObject.transform.Find("Handle").gameObject.transform.position.x)
        {
            inTarget = true;
        }
        else
        {
            inTarget = false;
        }
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
