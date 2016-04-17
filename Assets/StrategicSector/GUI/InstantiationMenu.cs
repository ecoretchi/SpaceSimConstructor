using UnityEngine;
using System.Collections;

public class InstantiationMenu : MonoBehaviour {

    GameObject connectorS;
    GameObject connectorM;
    GameObject connectorL;
    GameObject plantModule;
    
    Stackables.ProcessingStackables stackablesProcessing;
    Rect getCellRect(int row, int col = 0, int x = 0, int y = 0, int dx = 200, int dy = 25) {
        return new Rect(x + dx * col, y + dy * row, dx, dy);
    }
    void OnGUI() {
            int row = 0;
            GUI.Label(getCellRect(row++), "Build Station Version 0.0.1");
            
            if (GUI.Button(getCellRect(row++), "Connector S (key S)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorS));
            }
            if (GUI.Button(getCellRect(row++), "Connector M (key M)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorM));
            }
            if (GUI.Button(getCellRect(row++), "Connector L (key L)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorL));
            }
            if (GUI.Button(getCellRect(row++), "PlantModule (key P)")) {
                stackablesProcessing.OnInstantiateByGUIButton(Instantiate(plantModule)); 
            }
        
        }
    void Awake() {

        string folder = "StrategicSector/";
        connectorS = (GameObject)Resources.Load(folder + "ConnectorS", typeof(GameObject));
        connectorM = (GameObject)Resources.Load(folder + "ConnectorM", typeof(GameObject));
        connectorL = (GameObject)Resources.Load(folder + "ConnectorL", typeof(GameObject));
        plantModule = (GameObject)Resources.Load(folder + "PlantModule", typeof(GameObject)); 

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
            stackablesProcessing.captureTarget(Instantiate(connectorS).GetComponentInChildren<MeshFilter>().gameObject.transform);
        }else
        if (Input.GetKeyDown(KeyCode.M)) {
            stackablesProcessing.captureTarget(Instantiate(connectorM).GetComponentInChildren<MeshFilter>().gameObject.transform);
        }else
        if (Input.GetKeyDown(KeyCode.L)) {
            stackablesProcessing.captureTarget(Instantiate(connectorL).GetComponentInChildren<MeshFilter>().gameObject.transform);
        }else
        if (Input.GetKeyDown(KeyCode.P)) {
            stackablesProcessing.captureTarget(Instantiate(plantModule).GetComponentInChildren<MeshFilter>().gameObject.transform); 
        } 
    }
}
