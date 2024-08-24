using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    public static PrefabsManager Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    
    
    public GameObject PlayerPrefab;
    public GameObject MobPrefab;
    public GameObject OrePrefab;
    public GameObject ResourcesSender;
    public GameObject LootPrefab;

    [Header("UI")] 
    public GameObject AnswerOption;
    
    [Header("DEBUG")]
    public GameObject ColliderPrefab;

    public GameObject HitOrePrefabWithLoot;
    public GameObject HitOrePrefabWithoutLoot;


    public GameObject GetPlayerPrefab()
    {
        return PlayerPrefab;
    }
}