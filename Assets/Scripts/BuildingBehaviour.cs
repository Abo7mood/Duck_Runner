using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour
{
    const float DestoryDistance=-25f;
    public float MovementSpeed;
    

    private void Update()
    {
        if (transform.position.x < DestoryDistance) Destroy(gameObject);
        transform.position -= new Vector3(MovementSpeed * Time.deltaTime * GameManager.Instance.SpeedMultiplier, 0,0);
        
    }

}
