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
    [SerializeField]
    public GameObject[] rackPrefabs;
    [SerializeField]
    public GameObject[] cabinetPrefabs;
    [SerializeField]
    public GameObject[] tvTablePrefabs;
    [SerializeField]
    public GameObject[] coffeeTablePrefabs;
    [SerializeField]
    public GameObject[] lampPrefabs;
    [SerializeField]
    public GameObject[] officeTablePrefabs;
    [SerializeField]
    public GameObject[] chairPrefabs;
    [SerializeField]
    public GameObject[] kitchenChairPrefabs;
    [SerializeField]
    public GameObject[] kitchenTablePrefabs;
    [SerializeField]
    public GameObject[] kitchenShelfPrefabs;
    [SerializeField]
    public GameObject[] modularKitchenTablePrefabs;
    [SerializeField]
    public GameObject[] washbasinPrefabs;

    #endregion


    #region Paint Materials

    public GameObject[] paintMaterials;

    #endregion


    #region Floor Materials

    public GameObject[] floorMaterials;

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
            case "Rack":
                BindData(rackPrefabs);
                break;
            case "Cabinet":
                BindData(cabinetPrefabs);
                break;
            case "TV Table":
                BindData(tvTablePrefabs);
                break;
            case "Coffee Table":
                BindData(coffeeTablePrefabs);
                break;
            case "Lamp":
                BindData(lampPrefabs);
                break;
            case "Office Table":
                BindData(officeTablePrefabs);
                break;
            case "Chair":
                BindData(chairPrefabs);
                break;
            case "Kitchen Chair":
                BindData(kitchenChairPrefabs);
                break;
            case "Kitchen Table":
                BindData(kitchenTablePrefabs);
                break;
            case "Kitchen Shelf":
                BindData(kitchenShelfPrefabs);
                break;
            case "Modular Kitchen Table":
                BindData(modularKitchenTablePrefabs);
                break;
            case "Washbasin":
                BindData(washbasinPrefabs);
                break;
            case "Paint":
                BindData(paintMaterials);
                break;
            case "Floor":
                BindData(floorMaterials);
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
        if (selectedObject.CompareTag("Paint"))
        {
            ReplacePaintWith(prefab.GetComponent<Renderer>().material);
        }
        else if (selectedObject.CompareTag("Floor"))
        {
            ReplaceFloorWith(prefab.GetComponent<Renderer>().material);
        }
        else
        {
            ReplaceObjectWith(prefab);
        }
    }

    private void ReplaceObjectWith(GameObject prefab)
    {
        Instantiate(prefab, selectedObject.transform.position, selectedObject.transform.rotation, selectedObject.transform.parent);
        Destroy(selectedObject);
    }

    private void ReplacePaintWith (Material material)
    {
        GameObject room = selectedObject.transform.parent.gameObject;
        foreach (Transform wall in room.transform)
        {
            WallPaint[] wallPaints = wall.GetComponentsInChildren<WallPaint>();
            foreach (WallPaint wallPaint in wallPaints)
            {
                wallPaint.GetComponent<Renderer>().material = material;
            }
        }
    }

    private void ReplaceFloorWith(Material material)
    {
        GameObject room = selectedObject.transform.parent.gameObject;
        foreach (Transform floor in room.transform)
        {
            floor.GetComponent<Renderer>().material = material;
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
