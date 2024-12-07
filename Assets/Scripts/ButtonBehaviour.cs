using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] float scaleSpeed;
    private void OnMouseEnter()
    {
        StartCoroutine(Scale(1.3f));
    }
    private void OnMouseExit()
    {
        StartCoroutine(Scale(1 / 1.3f)); Debug.Log("nfo5");
    }
    IEnumerator Scale(float scale)
    {
        Vector3 TargetScale = gameObject.transform.localScale * scale;
        while (transform.localScale.x < TargetScale.x || transform.localScale.y < TargetScale.y)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, TargetScale, scaleSpeed);
            yield return null;
        }
    }
}
