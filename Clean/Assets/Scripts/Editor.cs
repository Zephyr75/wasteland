using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class Editor : MonoBehaviour
{
    public GameObject player;
    public GameObject baseMachine, one, two, three, four, five, six, seven, eight, nine, zero;
    public GameObject editorUI, buttons, field, delete;
    public LayerMask layerBoth, layerSpawn;
    public float maxDistance;
    private RaycastHit hitMain, hitLink;
    public Camera camEditor;
    public Material transparent;
    private int moduleNbr = 1, selectedMachine;
    private GameObject machine;
    public TMP_Text name1, name2, name3, name4, name5;

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();
    private List<int> types = new List<int>();
    private string actualName;
    private List<List<Vector3>> savedPositions = new List<List<Vector3>>();
    private List<List<Quaternion>> savedRotations = new List<List<Quaternion>>();
    private List<List<int>> savedTypes = new List<List<int>>();
    private List<string> savedNames = new List<string>();


    private bool defRot, inEditor;
    private GameObject preview, module, toLink;
    private Quaternion preRot;
    private List<Vector3> around = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        LoadMachine();
        machine = GameObject.Instantiate(baseMachine, new Vector3(0, 10, 0), Quaternion.identity);
        machine.SetActive(true);
        module = one;
        preRot = Quaternion.identity;
        around.Add(Vector3.forward);
        around.Add(-Vector3.forward);
        around.Add(Vector3.right);
        around.Add(-Vector3.right);
        around.Add(Vector3.up);
        around.Add(-Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ClearSave();
        }

        if (savedPositions.Count > 4)
        {
            name5.text = savedNames[4];
        }
        if (savedPositions.Count > 3)
        {
            name4.text = savedNames[3];
        }
        if (savedPositions.Count > 2)
        {
            name3.text = savedNames[2];
        }
        if (savedPositions.Count > 1)
        {
            name2.text = savedNames[1];
        }
        if (savedPositions.Count > 0)
        {
            name1.text = savedNames[0];
        }

        if (savedPositions.Count < 5)
        {
            name5.text = "Empty";
        }
        if (savedPositions.Count < 4)
        {
            name4.text = "Empty";
        }
        if (savedPositions.Count < 3)
        {
            name3.text = "Empty";
        }
        if (savedPositions.Count < 2)
        {
            name2.text = "Empty";
        }
        if (savedPositions.Count < 1)
        {
            name1.text = "Empty";
        }
        if (!player.GetComponent<Character>().GetInVehicle() && Physics.Raycast(camEditor.ScreenPointToRay(Input.mousePosition), out hitMain, maxDistance, layerSpawn))
        {
            if (preview != hitMain.transform.gameObject)
            {
                Preview();
            }
            if (!defRot && Input.GetMouseButtonDown(0))
            {
                bool canBuild = false;
                canBuild = !Physics.Raycast(hitMain.collider.transform.position, hitMain.normal, 1);
                switch (moduleNbr)
                {
                    case 6:
                        if (Physics.Raycast(hitMain.collider.transform.position + hitMain.normal, preview.transform.right, 2) ||
                            Physics.Raycast(hitMain.collider.transform.position + hitMain.normal, - preview.transform.right, 2))
                        {
                            canBuild = false;
                        }
                        break;
                    case 7:
                        if (Physics.Raycast(hitMain.collider.transform.position + hitMain.normal, preview.transform.right, 2) ||
                            Physics.Raycast(hitMain.collider.transform.position + hitMain.normal, -preview.transform.right, 2))
                        {
                            canBuild = false;
                        }
                        break;
                    case 9:
                        if (Physics.Raycast(hitMain.collider.transform.position + hitMain.normal * 2, preview.transform.forward, 2) ||
                            Physics.Raycast(hitMain.collider.transform.position + hitMain.normal * 2, -preview.transform.forward, 2))
                        {
                            canBuild = false;
                        }
                        break;
                    case 0:
                        if (Physics.Raycast(hitMain.collider.transform.position, hitMain.normal, 3))
                        {
                            canBuild = false;
                        }
                        break;
                }
                if (canBuild)
                {
                    Build(module, ModulePosition(), preRot, false);
                }
            }
            EditRotation();
        }

        if (!player.GetComponent<Character>().GetInVehicle() && Physics.Raycast(camEditor.ScreenPointToRay(Input.mousePosition), out hitMain, maxDistance, layerBoth) && !defRot)
        {
            Delete();
        }
        if (!player.GetComponent<Character>().GetInVehicle()  && !defRot)
        {
            ChooseNext();
        }
        StartAction();
    }

    private void Preview()
    {
        Destroy(preview);
        preview = GameObject.Instantiate(module, ModulePosition(), preRot);
        preview.SetActive(true);
        foreach (Renderer child in preview.GetComponentsInChildren<Renderer>())
        {
            Material[] mats = new Material[child.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = transparent;
            }
            child.materials = mats;
        }
        foreach (Collider child in preview.GetComponentsInChildren<Collider>())
        {
            child.enabled = false;
        }
    }

    private void Build(GameObject mod, Vector3 pos, Quaternion rot, bool actual)
    {
        GameObject newMod = GameObject.Instantiate(mod, pos, rot);
        Destroy(preview);
        if (!actual)
        {
            positions.Add(pos);
            rotations.Add(rot);
            types.Add(moduleNbr);
        }
        newMod.SetActive(true);
        newMod.transform.parent = machine.transform;

        if (newMod.transform.GetComponent<Module>().isSpring)
        {
            foreach (Collider col in newMod.transform.parent.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(col, hitMain.collider, true);
            }
        }

        foreach (Vector3 direction in around)
        {
            if (Physics.Raycast(pos + direction * 9 / 20, direction, out hitLink, .15f, layerSpawn))
            {
                newMod.transform.GetComponent<Module>().Link(hitLink.transform.gameObject);
            }
        }
    }

    public void Rebuild()
    {
        Destroy(machine);
        machine = GameObject.Instantiate(baseMachine, new Vector3(0, 10, 0), Quaternion.identity);
        machine.SetActive(true);
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();
        types = new List<int>();
        actualName = "";
        for (int i = 0; i < savedPositions[selectedMachine].Count; i++)
        {
            Build(GetModule(savedTypes[selectedMachine][i]), savedPositions[selectedMachine][i], savedRotations[selectedMachine][i], false);
        }
        buttons.SetActive(false);
    }

    public void RebuildCurrent()
    {
        Destroy(machine);
        machine = GameObject.Instantiate(baseMachine, new Vector3(0, 10, 0), Quaternion.identity);
        machine.SetActive(true);

        for (int i = 0; i < positions.Count; i++)
        {
            Build(GetModule(types[i]), positions[i], rotations[i], true);
        }
    }

    public void RebuildNew()
    {
        Destroy(machine);
        machine = GameObject.Instantiate(baseMachine, new Vector3(0, 10, 0), Quaternion.identity);
        machine.SetActive(true);
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();
        types = new List<int>();
        actualName = "";
    }

    private void Delete()
    {
        if (Input.GetMouseButtonDown(1) && hitMain.collider.tag != "Main")
        {
            GameObject toDestroy;
            if (hitMain.transform.tag.Contains("Child"))
            {
                toDestroy = hitMain.transform.parent.gameObject;
            }
            else
            {
                toDestroy = hitMain.transform.gameObject;
            }
            int index = positions.Count;
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i] == toDestroy.transform.position)
                {
                    index = i;
                }
            }
            positions.RemoveAt(index);
            rotations.RemoveAt(index);
            types.RemoveAt(index);
            Destroy(toDestroy);
        }
    }

    private void EditRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            defRot = true;
        }
        if (defRot && !Input.GetKey(KeyCode.R))
        {
            bool up = Input.GetKey(KeyCode.UpArrow);
            bool down = Input.GetKey(KeyCode.DownArrow);
            bool right = Input.GetKey(KeyCode.RightArrow);
            bool left = Input.GetKey(KeyCode.LeftArrow);
            bool x = Input.GetKey(KeyCode.X);
            if (up)
            {
                preview.transform.Rotate(90, 0, 0);
            }
            if (down)
            {
                preview.transform.Rotate(-90, 0, 0);
            }
            if (right)
            {
                preview.transform.Rotate(0, 90, 0);
            }
            if (left)
            {
                preview.transform.Rotate(0, -90, 0);
            }
            if (up || down || left || right || x)
            {
                preRot = preview.transform.rotation;
                defRot = false;
            }
        }
    }

    private void StartAction()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inEditor)
            {
                player.SetActive(true);
                camEditor.gameObject.SetActive(false);
                editorUI.SetActive(false);
                Destroy(preview);
                foreach (Rigidbody rb in machine.GetComponentsInChildren<Rigidbody>())
                {
                    rb.useGravity = true;
                }
                foreach (Collider col in machine.GetComponentsInChildren<Collider>())
                {
                    col.enabled = true;
                }
            }
            else
            {
                player.SetActive(false);
                camEditor.gameObject.SetActive(true);
                editorUI.SetActive(true);
                RebuildCurrent();
            }
            inEditor = !inEditor;
        }
    }

    public void RegisterMachine()
    {
        if (savedPositions.Count < 5)
        {
            field.SetActive(true);
        }
        else
        {
            StartCoroutine(DisplayDelete());
        }
    }

    private void SaveMachine()
    {
        SaveData temp = new SaveData {savedPositions = savedPositions, savedRotations = savedRotations, savedTypes = savedTypes, savedNames = savedNames};
        SaveSystem.SaveData(temp);
        LoadMachine();
    }

    private void ClearSave()
    {
        SaveData temp = new SaveData {savedPositions = new List<List<Vector3>>(), savedRotations = new List<List<Quaternion>>(), savedTypes = new List<List<int>>(), savedNames = new List<string>()};
        SaveSystem.SaveData(temp);
        LoadMachine();
    }

    private void LoadMachine()
    {
        SaveData temp = SaveSystem.LoadData();
        if (temp != null)
        {
            savedPositions = temp.savedPositions;
            savedRotations = temp.savedRotations;
            savedTypes = temp.savedTypes;
            savedNames = temp.savedNames;
        }
    }

    private GameObject GetModule(int nbr)
    {
        switch (nbr)
        {
            case 1: return one;
            case 2: return two;
            case 3: return three;
            case 4: return four;
            case 5: return five;
            case 6: return six;
            case 7: return seven;
            case 8: return eight;
            case 9: return nine;
            default: return zero;
        }
    }

    private Vector3 ModulePosition()
    {
        float x = Mathf.Round(hitMain.collider.transform.position.x) + Mathf.Round(hitMain.normal.x);
        float y = Mathf.Round(hitMain.collider.transform.position.y) + Mathf.Round(hitMain.normal.y);
        float z = Mathf.Round(hitMain.collider.transform.position.z) + Mathf.Round(hitMain.normal.z);
        return new Vector3(x, y, z);
    }

    private void ChooseNext()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            module = one;
            moduleNbr = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            module = two;
            moduleNbr = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            module = three;
            moduleNbr = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            module = four;
            moduleNbr = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            module = five;
            moduleNbr = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            module = six;
            moduleNbr = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            module = seven;
            moduleNbr = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            module = eight;
            moduleNbr = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            module = nine;
            moduleNbr = 9;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            module = zero;
            moduleNbr = 0;
        }
    }


    //UI

    public void ActivateButtons(int index)
    {
        selectedMachine = index;
        buttons.SetActive(true);
    }

    public void ChooseName(TMP_Text newName)
    {
        actualName = newName.text;
        field.SetActive(false);
        savedPositions.Add(positions);
        savedRotations.Add(rotations);
        savedTypes.Add(types);
        savedNames.Add(actualName);
        SaveMachine();
    }
    

    public void Erase()
    {
        savedPositions.RemoveAt(selectedMachine);
        savedRotations.RemoveAt(selectedMachine);
        savedTypes.RemoveAt(selectedMachine);
        savedNames.RemoveAt(selectedMachine);
        buttons.SetActive(false);
        SaveMachine();
    }

    private IEnumerator DisplayDelete()
    {
        delete.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        delete.SetActive(false);
    }
}
