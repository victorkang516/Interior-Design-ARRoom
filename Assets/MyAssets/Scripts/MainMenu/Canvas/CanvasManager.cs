using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CanvasType {
    Login,
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

        CanvasController mainMenuCanvasController = canvasControllerList.Find(canvasController => canvasController.canvasType == CanvasType.Login);
        if (mainMenuCanvasController != null)
        {
            mainMenuCanvasController.gameObject.SetActive(true);
            lastActiveCanvas = mainMenuCanvasController;
        } 
        else { Debug.LogWarning("CanvasManager: MainMenu Not Found"); }
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
