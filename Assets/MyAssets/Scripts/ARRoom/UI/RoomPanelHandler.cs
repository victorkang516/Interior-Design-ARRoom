using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPanelHandler : MonoBehaviour
{
    bool isShow = false;

    public void Trigger()
    {
        isShow = !isShow;

        if (isShow)
            LeanTween.moveLocalY(gameObject, 250, 0.2f).setEaseOutQuart();
        else
            LeanTween.moveLocalY(gameObject, 640, 0.2f).setEaseOutQuart();
    }

}
