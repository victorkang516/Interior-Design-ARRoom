using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMessageBoxHandler : MonoBehaviour
{
    Text messageText;

    private void Start()
    {
        messageText = gameObject.transform.GetChild(0).GetComponent<Text>();
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    public void onPlayerJoined(string playerNickname)
    {
        messageText.text = playerNickname + " joined!";
        ShowMessage();
    }

    public void onPlayerLeft(string playerNickname)
    {
        messageText.text = playerNickname + " left";
        ShowMessage();
    }

    private void ShowMessage ()
    {
        StopAllCoroutines();
        gameObject.transform.localPosition = new Vector2(0, 300);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.25f).setEaseOutBack();

        yield return new WaitForSeconds(2f);

        LeanTween.moveLocalY(gameObject, 600, 0.2f).setEaseOutQuart();

        yield break;
    }
}
