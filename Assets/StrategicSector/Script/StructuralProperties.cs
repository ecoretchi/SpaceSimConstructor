using UnityEngine;
using System.Collections;

[System.Serializable]
public class NamedProperty {
    public string nameID;
    public bool enabled = true;
    public double value;
}
/// <summary>
/// Plant add productions per cycle
/// </summary>
[System.Serializable]
public class PlantProperty : NamedProperty {    
}
[System.Serializable]
public class StorageProperty : NamedProperty {
    public double capacity;
}
/// <summary>
/// Influence add specifics in absolute
/// </summary>
[System.Serializable]
public class InfluenceProperty : NamedProperty {
    /// <summary>
    /// property active only if avail product consumption
    /// </summary>
    public bool productionRequire = true;
    public string[] productionsID;
}
[System.Serializable]
public class Facility{
    public string nameID;
    public bool enabled = true;
    /// <summary>
    /// use negative value to setup consumation and positive value to setup production results
    /// </summary>
    public PlantProperty[]  productionInOut;
    /// <summary>
    /// facility could add positive or negative properties,
    /// </summary>
    public InfluenceProperty[] influences;
}

[System.Serializable]
public class JobProperty {
    /// <summary>
    /// see the card`s for each jobs in text description file or in documentation
    /// </summary>
    public string jobName;
    public double factor;
}
[System.Serializable]
public class JobVacancy {
    /// <summary>
    /// a responsibility from jobs
    /// </summary>
    public JobProperty[] duties;
}
[System.Serializable]
public class Apartments {
    public enum Type {
        Pure,
        Normal,
        Lux
    };
    /// <summary>
    /// 
    /// </summary>
    public Type type;
    /// <summary>
    /// living space for civil
    /// </summary>
    public int lodging;
    /// <summary>
    /// Population by default living from scratch 
    /// </summary>
    public int defPopulation;
    /// <summary>
    /// 
    /// </summary>
    public int curPopulation { get; set; }
}

[System.Serializable]
public class TradeProperty {
    /// <summary>
    /// buy items
    /// </summary>
    public int buy;
    /// <summary>
    /// sell items
    /// </summary>
    public int sell;
    /// <summary>
    /// item price condition to make transaction
    /// </summary>
    public float price;
}