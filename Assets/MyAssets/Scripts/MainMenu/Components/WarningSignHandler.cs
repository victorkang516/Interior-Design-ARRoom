using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void Show(string message, Vector2 position)
    {
        transform.localScale = new Vector2(0, 1);
        LeanTween.scale(gameObject, new Vector2(1, 1), 0.1f).setEaseInSine();

        transform.GetChild(0).GetComponent<Text>().text = message;

        gameObject.transform.localPosition = position;
    }

    public void Hide ()
    {
        transform.localScale = new Vector2(0, 1);
    }
}
