using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public new BoxCollider2D collider2D;
    public Rigidbody2D rb;
    
    public float scrollSpeed = -2f;
    public float offset;
    public float startPos;
    private float height;

    private void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        height = collider2D.size.y;
        collider2D.enabled = false;

        rb.velocity = new Vector2(0, scrollSpeed); ;
    }

    private void Update()
    {
        if(transform.position.y < (-height - offset))
        {
            Vector2 resetPosition = new Vector2(0, (height  * 3f) + startPos + offset);
            transform.position = (Vector2)transform.position + resetPosition;
        }
    }

    /*private float length, startpos;
    public float horizontalSpeed = 0;
    public float verticalSpeed = .2f;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.y * (1 - parallaxEffect));
        float distance = (cam.transform.position.y * parallaxEffect);

        //transform.position = new Vector3(transform.position.x, startpos + distance, transform.position.z);
        Vector2 offset = new Vector2(Time.fixedTime * horizontalSpeed, Time.fixedTime * verticalSpeed);

        transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }*/
}
