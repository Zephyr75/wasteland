using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [SerializeField]
    public List<List<Vector3>> savedPositions;
    public List<List<Quaternion>> savedRotations;
    public List<List<int>> savedTypes;
    public List<string> savedNames;
}
