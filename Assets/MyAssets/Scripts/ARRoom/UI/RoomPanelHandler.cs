using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPanelHandler : MonoBehaviour
{
    bool isShow = false;

    public void Trigger()
    {
        if (isShow)
            LeanTween.moveLocalY(gameObject, 630, 0.2f).setEaseOutQuart();
        else
            LeanTween.moveLocalY(gameObject, 325, 0.2f).setEaseOutQuart();

        isShow = !isShow;
    }

}
