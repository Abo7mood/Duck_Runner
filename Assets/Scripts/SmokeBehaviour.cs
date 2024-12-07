using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBehaviour : MonoBehaviour
{
    public float MovementSpeed;
    private SpriteRenderer sprite;
        private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (transform.position.x < -sprite.size.x)
        {
            Destroy(gameObject);
            Instantiate(gameObject, new Vector3(sprite.size.x, 0, 0), Quaternion.identity);
        }
            transform.position -= new Vector3(MovementSpeed * Time.deltaTime, 0, 0);
    }



    
}
