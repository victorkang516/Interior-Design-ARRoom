using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class ObjectListHandler : MonoBehaviour
{
    #region Furniture Prefabs
    [SerializeField]
    public GameObject[] bedPrefabs;
    [SerializeField]
    public GameObject[] sofaPrefabs;
    #endregion

    #region UI Prefab
    [SerializeField]
    GameObject objectItemPrefab;
    #endregion

    private GameObject selectedObject;


    public void CreateObjectList (GameObject selectedObject)
    {
        MainManager.Instance.Debug2("Selection Generator - Display selections ");

        this.selectedObject = selectedObject;

        switch (this.selectedObject.tag)
        {
            case "Bed":
                BindData(bedPrefabs);
                break;
            case "Sofa":
                BindData(sofaPrefabs);
                break;
        }
        
    }

    private void BindData (GameObject[] prefabs)
    {
        MainManager.Instance.Debug2("Selection Generator - Prefab: " + prefabs.Length);

        for (int i = 0; i < prefabs.Length; i++)
        {
            int x = i;
            GameObject selectionItem = Instantiate(objectItemPrefab, transform);
            selectionItem.GetComponent<Button>().onClick.AddListener(() => { OnItemClicked(prefabs[x]); });
            selectionItem.transform.GetChild(0).GetComponent<Image>().sprite = prefabs[i].GetComponent<ARObject>().Thumbnail;
            selectionItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = prefabs[i].GetComponent<ARObject>().ObjectName;
        }
    }

    private void OnItemClicked (GameObject prefab)
    {
        ReplaceObjectWith(prefab);
    }

    private void ReplaceObjectWith (GameObject prefab)
    {
        Instantiate(prefab, selectedObject.transform.position, selectedObject.transform.rotation, selectedObject.transform.parent);
        Destroy(selectedObject);
    }

    public void EmptyObjectList()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
