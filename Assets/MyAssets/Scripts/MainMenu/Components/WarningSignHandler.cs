using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSignHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(0, 1);
    }

    public void Show ()
    {
        transform.localScale = new Vector2(0, 1);
        LeanTween.scale(gameObject, new Vector2(1, 1), 0.1f).setEaseInSine();
    }

    public void Hide ()
    {
        transform.localScale = new Vector2(0, 1);
    }
}
