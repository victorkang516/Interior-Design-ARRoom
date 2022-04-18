using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CanvasType {
    Login,
    Register,
    Home,
    CreateARRoom,
    NewARRoom,
    LoadARRoom,
    JoinARRoom,
    Loading
}

public class CanvasManager : MonoBehaviour
{
    List<CanvasController> canvasControllerList;
    CanvasController lastActiveCanvas;

    private void Awake()
    {
        canvasControllerList = gameObject.GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach( canvasController => canvasController.gameObject.SetActive(false));

        CanvasController initialCanvasController = canvasControllerList.Find(canvasController => canvasController.canvasType == CanvasType.Login);
        if (initialCanvasController != null)
        {
            initialCanvasController.gameObject.SetActive(true);
            lastActiveCanvas = initialCanvasController;
        } 
        else
        {
            initialCanvasController = canvasControllerList.Find(canvasController => canvasController.canvasType == CanvasType.Home);
            initialCanvasController.gameObject.SetActive(true);
            lastActiveCanvas = initialCanvasController;

        }
    }

    public void SwitchCanvas(CanvasType desiredType)
    {
        CanvasController desiredCanvas = canvasControllerList.Find(canvas => canvas.canvasType == desiredType);
        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas.gameObject.SetActive(false);
            lastActiveCanvas = desiredCanvas;
        }
        else { Debug.LogWarning("CanvasManager: " + desiredType + " Not Found"); }
    }

}
