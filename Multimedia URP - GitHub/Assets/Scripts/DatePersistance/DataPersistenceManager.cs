using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance {get; private set;}

    private void Awake(){
        if(instance != null){
            Debug.LogError("Only one Data Persistance Manager in the scene.");
        }
        instance = this;
    }

    private void Start(){
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame(){
        this.gameData = new GameData();
    }

    public void LoadGame(){
        this.gameData = dataHandler.Load();

        if(this.gameData == null){
            Debug.Log("No data was found. Intializing to default values");
        NewGame();
        }

        foreach (IDataPersistence dataPersistanceObj in dataPersistenceObjects){
            dataPersistanceObj.LoadData(gameData);
        }

    }

    public void SaveGame(){
        foreach (IDataPersistence dataPersistanceObj in dataPersistenceObjects){
            dataPersistanceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit(){
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
