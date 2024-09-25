using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGeneration : MonoBehaviour
{
    public GameObject one, two, three, four, five, six, seven, eight, nine, ten, eleven, twelve;
    private List<GameObject> building = new List<GameObject>();
    private GameObject current;

    // Start is called before the first frame update
    void Start()
    {
        building.Add(one);
        building.Add(two);
        building.Add(three);
        building.Add(four);
        building.Add(five);
        building.Add(six);
        building.Add(seven);
        building.Add(eight);
        building.Add(nine);
        building.Add(ten);
        building.Add(eleven);
        building.Add(twelve);
        GenerateBuilding();
    }

    private void GenerateBuilding()
    {
        for (int i = 0; i < 128; i++)
        {
            current = GameObject.Instantiate(building[Random.Range(0, building.Count - 1)], new Vector3(i * -15.95f, 0, 0), Quaternion.identity);
            current.transform.parent = transform;
        }
    }
}
