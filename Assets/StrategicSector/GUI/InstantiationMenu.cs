using UnityEngine;
using System.Collections;

public class InstantiationMenu : MonoBehaviour {

    GameObject connectorS;
    GameObject connectorM;
    GameObject connectorL;
    GameObject plantModule_Oxygen;
    GameObject plantModule_Grain;
    GameObject plantModule_Barnyard;
    
    Stackables.ProcessingStackables stackablesProcessing;
    Rect getCellRect(int row, int col = 0, int x = 25, int y = 25, int dx = 200, int dy = 25) {
        return new Rect(x + dx * col, y + dy * row, dx, dy);
    }
    void OnGUI() {
            int row = 0;
            GUI.Label(getCellRect(row++), "Build Station Version 0.0.1");
            
            if (GUI.Button(getCellRect(row++), "SmallConnector (key S)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorS));
            }
            if (GUI.Button(getCellRect(row++), "MediumConnector (key M)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorM));
            }
            if (GUI.Button(getCellRect(row++), "LargeConnector (key L)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorL));
            }
            if (GUI.Button(getCellRect(row++), "PlantModule Grain (key G)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(plantModule_Grain));
            }
            if (GUI.Button(getCellRect(row++), "PlantModule Oxygen (key O)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(plantModule_Oxygen));
            }
            if (GUI.Button(getCellRect(row++), "PlantModule Barnyard (key B)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(plantModule_Barnyard));
            }
        
        }
    void Awake() {

        string folder = "StrategicSector/";

        connectorS = (GameObject)Resources.Load(folder + "ConnectorS", typeof(GameObject));
        connectorM = (GameObject)Resources.Load(folder + "ConnectorM", typeof(GameObject));
        connectorL = (GameObject)Resources.Load(folder + "ConnectorL", typeof(GameObject));
        plantModule_Oxygen = (GameObject)Resources.Load(folder + "PlantModule_oxygen", typeof(GameObject));
        plantModule_Grain = (GameObject)Resources.Load(folder + "PlantModule_grain", typeof(GameObject));
        plantModule_Barnyard = (GameObject)Resources.Load(folder + "PlantModule_barnyard", typeof(GameObject));

        stackablesProcessing = FindObjectOfType<Stackables.ProcessingStackables>();

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update() {

        if (stackablesProcessing.IsTarget())
            return;

        if (Input.GetKeyDown(KeyCode.S)) {
            stackablesProcessing.OnTargetCapture(Instantiate(connectorS).GetComponentInChildren<MeshFilter>().gameObject.transform);
        }else
        if (Input.GetKeyDown(KeyCode.M)) {
            stackablesProcessing.OnTargetCapture(Instantiate(connectorM).GetComponentInChildren<MeshFilter>().gameObject.transform);
        }else
        if (Input.GetKeyDown(KeyCode.L)) {
            stackablesProcessing.OnTargetCapture(Instantiate(connectorL).GetComponentInChildren<MeshFilter>().gameObject.transform);
        }else
        if (Input.GetKeyDown(KeyCode.O)) {
            stackablesProcessing.OnTargetCapture(Instantiate(plantModule_Oxygen).GetComponentInChildren<MeshFilter>().gameObject.transform); 
        }else
        if (Input.GetKeyDown(KeyCode.G)) {
            stackablesProcessing.OnTargetCapture(Instantiate(plantModule_Grain).GetComponentInChildren<MeshFilter>().gameObject.transform); 
        }else
        if (Input.GetKeyDown(KeyCode.B)) {
            stackablesProcessing.OnTargetCapture(Instantiate(plantModule_Barnyard).GetComponentInChildren<MeshFilter>().gameObject.transform); 
        } 
    }
}
