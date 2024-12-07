using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    Vector2 minScale;//the object scale
    [SerializeField] Vector2 maxScale;//the object scale target
    [SerializeField] bool repetable;//is it loop?
    [SerializeField] float speed;//the speed for the scaler
    [SerializeField] float duration;//the time to make the scaler

  
  
    private void OnEnable()
    {
        StartCoroutine(Repeat());
    }
    private IEnumerator Repeat()
    {
        minScale = transform.localScale;//set the minscale target
        while (repetable)//while the repetable is true then :
        {
            yield return RepeatLerp(minScale, maxScale, duration);
            yield return RepeatLerp(maxScale, minScale, duration);

        }
    }
    public IEnumerator RepeatLerp(Vector2 a, Vector2 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector2.Lerp(a, b, i);
            yield return null;
        }
    }
}
