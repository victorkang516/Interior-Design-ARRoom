using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPrefabStorage : MonoBehaviour
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


    #region

    public Material[] roomPaintMaterials;

    public Material[] roomFloorMaterials;

    #endregion
}
