using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ModelType
{
    Studio,
    Loft
}

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            modelType = ModelType.Loft;
        }
    }

    public Text debug1;
    public Text debug2;
    public ModelType modelType;

    public void Debug1 (string msg)
    {
        debug1.text = Time.fixedTime + ": " + msg;
    }

    public void Debug2(string msg)
    {
        debug2.text = Time.fixedTime + ": " + msg;
    }

}
