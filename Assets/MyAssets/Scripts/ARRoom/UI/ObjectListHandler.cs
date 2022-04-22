using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class ObjectListHandler : MonoBehaviour
{

    #region UI Prefab
    [SerializeField]
    GameObject objectItemPrefab;
    #endregion

    public PlayerManager playerManager;
    ObjectsPrefabStorage objectsPrefabStorage;

    private void Start()
    {
        objectsPrefabStorage = GameObject.Find("ObjectsPrefabStorage").GetComponent<ObjectsPrefabStorage>();
    }


    public void CreateObjectList (GameObject selectedObject)
    {

        switch (selectedObject.tag)
        {
            case "Bed":
                BindData(objectsPrefabStorage.bedPrefabs);
                break;
            case "Sofa":
                BindData(objectsPrefabStorage.sofaPrefabs);
                break;
            case "Rack":
                BindData(objectsPrefabStorage.rackPrefabs);
                break;
            case "Cabinet":
                BindData(objectsPrefabStorage.cabinetPrefabs);
                break;
            case "TV Table":
                BindData(objectsPrefabStorage.tvTablePrefabs);
                break;
            case "Coffee Table":
                BindData(objectsPrefabStorage.coffeeTablePrefabs);
                break;
            case "Lamp":
                BindData(objectsPrefabStorage.lampPrefabs);
                break;
            case "Office Table":
                BindData(objectsPrefabStorage.officeTablePrefabs);
                break;
            case "Chair":
                BindData(objectsPrefabStorage.chairPrefabs);
                break;
            case "Kitchen Chair":
                BindData(objectsPrefabStorage.kitchenChairPrefabs);
                break;
            case "Kitchen Table":
                BindData(objectsPrefabStorage.kitchenTablePrefabs);
                break;
            case "Kitchen Shelf":
                BindData(objectsPrefabStorage.kitchenShelfPrefabs);
                break;
            case "Modular Kitchen Table":
                BindData(objectsPrefabStorage.modularKitchenTablePrefabs);
                break;
            case "Washbasin":
                BindData(objectsPrefabStorage.washbasinPrefabs);
                break;
            case "Paint":
                BindData(objectsPrefabStorage.paintMaterials);
                break;
            case "Floor":
                BindData(objectsPrefabStorage.floorMaterials);
                break;
        }
        
    }

    private void BindData (GameObject[] prefabs)
    {

        for (int i = 0; i < prefabs.Length; i++)
        {
            int x = i;
            GameObject selectionItem = Instantiate(objectItemPrefab, transform);
            selectionItem.GetComponent<Button>().onClick.AddListener(() => { OnItemClicked(prefabs[x], x); });
            selectionItem.transform.GetChild(0).GetComponent<Image>().sprite = prefabs[i].GetComponent<ARObject>().Thumbnail;
            selectionItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = prefabs[i].GetComponent<ARObject>().ObjectName;
        }
    }

    private void OnItemClicked (GameObject prefab, int index)
    {
        if (playerManager != null)
        {
            playerManager.EmitChangeTheObjectModelTo(index, prefab.tag);
        }
    }

    public void EmptyObjectList()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
