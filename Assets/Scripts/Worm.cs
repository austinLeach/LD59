using UnityEngine;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Worm : MonoBehaviour
{
    Rigidbody2D rigidBody2D;

    Vector2 direction;
    float horizontal;
    float vertical;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidBody2D.position;
        position.x = position.x + 10f * horizontal * Time.deltaTime;
        position.y = position.y + 10f * vertical * Time.deltaTime;
        rigidBody2D.MovePosition(position);
    }
}
