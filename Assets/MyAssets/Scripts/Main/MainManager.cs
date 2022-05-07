using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            DontDestroyOnLoad(this);
        }
    }

    public bool firstTime = true;

    [SerializeField] public GameObject[] aRModelPrefabs;
    [HideInInspector] public GameObject selectedARModelPrefab;
    public string roomName;
    public string roomKey = "";

}
