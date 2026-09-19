using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float speed = 10;
    Vector2 velocity;
    BoxCollider2D boxCollider;
    Rigidbody2D _rigidbody;
    List<Collider2D> insideTriggerBoxCollider = new List<Collider2D>();
    float influencePower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        boxCollider.size = new Vector2(GameData.Instance.influenceRadius, GameData.Instance.influenceRadius);
        influencePower = GameData.Instance.influencePower;
    }

    // Update is called once per frame
    void Update()
    {
        // _rigidbody.AddForce( new Vector3(velocity.x, velocity.y, 0));
        transform.position += new Vector3(velocity.x, velocity.y, 0);
    }

    void OnAttack(InputValue value)
    {
        foreach (Collider2D other in insideTriggerBoxCollider)
        {
            DayPerson otherDayPerson = other.GetComponent<DayPerson>();
            if (otherDayPerson != null)
            {
                otherDayPerson.Influence(influencePower);
            }
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 moveValue = value.Get<Vector2>();
        moveValue.Normalize();
        velocity = moveValue * speed * Time.deltaTime;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!insideTriggerBoxCollider.Contains(collision))
        {
            insideTriggerBoxCollider.Add(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (insideTriggerBoxCollider.Contains(collision))
        {
            insideTriggerBoxCollider.Remove(collision);
        }
    }
}
