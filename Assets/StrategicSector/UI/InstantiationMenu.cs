using UnityEngine;
using System.Collections;

public class InstantiationMenu : MonoBehaviour {

    public GameObject connectorS;
    public GameObject connectorM;
    public GameObject connectorL;
    public GameObject plantModule_Oxygen;
    public GameObject plantModule_Grain;
    public GameObject plantModule_Barnyard;

    Stackables.ProcessingStackables stackablesProcessing;
    Rect getCellRect(int row, int col = 0, int x = 25, int y = 25, int dx = 200, int dy = 25) {
        return new Rect(x + dx * col, y + dy * row, dx, dy);
    }
    void OnGUI() {
            //int row = 0;
            //GUI.Label(getCellRect(row++), "Build Station Version 0.0.1");
            
            //if (GUI.Button(getCellRect(row++), "SmallConnector (key S)")) {
            //    stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorS));
            //}
            //if (GUI.Button(getCellRect(row++), "MediumConnector (key M)")) {
            //    stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorM));
            //}
            //if (GUI.Button(getCellRect(row++), "LargeConnector (key L)")) {
            //    stackablesProcessing.OnInstantiateByGUIButton(Instantiate(connectorL));
            //}
            //if (GUI.Button(getCellRect(row++), "PlantModule Grain (key G)")) {
            //    stackablesProcessing.OnInstantiateByGUIButton(Instantiate(plantModule_Grain));
            //}
            //if (GUI.Button(getCellRect(row++), "PlantModule Oxygen (key O)")) {
            //    stackablesProcessing.OnInstantiateByGUIButton(Instantiate(plantModule_Oxygen));
            //}
            //if (GUI.Button(getCellRect(row++), "PlantModule Barnyard (key B)")) {
            //    stackablesProcessing.OnInstantiateByGUIButton(Instantiate(plantModule_Barnyard));
            //}
        
        }
    void Awake() {

        string folder = "StrategicSector/";
        if (!connectorS)
            connectorS = (GameObject)Resources.Load(folder + "ConnectorS", typeof(GameObject));
        if (!connectorM)
            connectorM = (GameObject)Resources.Load(folder + "ConnectorM", typeof(GameObject));
        if (!connectorL)
            connectorL = (GameObject)Resources.Load(folder + "ConnectorL", typeof(GameObject));
        if (!plantModule_Oxygen)
            plantModule_Oxygen = (GameObject)Resources.Load(folder + "PlantModule_oxygen", typeof(GameObject));
        if (!plantModule_Grain)
            plantModule_Grain = (GameObject)Resources.Load(folder + "PlantModule_grain", typeof(GameObject));
        if (!plantModule_Barnyard)
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
            GenConstructionObj(connectorS);
        }else
        if (Input.GetKeyDown(KeyCode.M)) {
            GenConstructionObj(connectorM);
        }else
        if (Input.GetKeyDown(KeyCode.L)) {
            GenConstructionObj(connectorL);             
        }else
        if (Input.GetKeyDown(KeyCode.O)) {
            GenConstructionObj(plantModule_Oxygen);            
        }else
        if (Input.GetKeyDown(KeyCode.G)) {
            GenConstructionObj(plantModule_Grain);            
        }else
        if (Input.GetKeyDown(KeyCode.B)) {
            GenConstructionObj(plantModule_Barnyard);            
        } 

    }
    public void GenConstructionObject(GameObject gmObj) {
        stackablesProcessing.OnInstantiateByGUIButton(Instantiate(gmObj));        
    }
    void GenConstructionObj(GameObject gmObj) {
        stackablesProcessing.OnTargetCapture(Instantiate(gmObj).GetComponentInChildren<MeshFilter>().gameObject.transform);        
    }
}
