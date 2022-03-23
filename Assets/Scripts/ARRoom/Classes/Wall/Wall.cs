using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorType
{
    GroundFloor,
    FirstFloor
}
public class Wall : MonoBehaviour
{
    public FloorType onFloor;
}
