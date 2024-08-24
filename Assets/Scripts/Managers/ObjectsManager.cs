// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class ObjectsManager : MonoBehaviour
// {
//     #region Singelton
//     public static ObjectsManager Instance;
//
//     public void Awake()
//     {
//         if (Instance != null)
//         {
//             Destroy(gameObject);
//         }
//         else
//         {
//             Instance = this;
//         }
//     }
//     #endregion
//     public Dictionary<int, Ore> ActiveOres { get; private set; } = new ();
//     public Dictionary<int, GameObject> ResourcesSenders { get; private set; } = new ();
//     
//     
//     private List<Ore> _oresPull = new ();
//     [SerializeField]
//     private List<Loot> _lootPull = new();
//     
//     public bool IsOreExist(int entityId)
//     {
//         return ActiveOres.ContainsKey(entityId) || _oresPull.Exists(ore => ore.EntityId == entityId);
//     }
//     
//     public void SyncOre(ServerOreStruct ore)
//     {
//         if(ActiveOres.ContainsKey(ore.EntityId))
//         {
//             ActiveOres[ore.EntityId].SyncData(ore);
//         }
//         else
//         {
//             SpawnOre(ore);
//         }
//     }
//     
//     public void SyncResourcesSender(ServerResourcesSenderStruct sender)
//     {
//         if(ResourcesSenders.ContainsKey(sender.EntityId))
//         {
//             ChangeResourcesSenderState(sender.EntityId, sender.IsActive);
//         }
//         else
//         {
//             SpawnResourcesSender(sender);
//         }
//     }
//     
//     public void ChangeResourcesSenderState(int entityId, bool state)
//     {
//         if(ResourcesSenders.TryGetValue(entityId, out var sender))
//         {
//             SpriteRenderer spriteRenderer = sender.transform.Find("head").GetComponent<SpriteRenderer>();
//             spriteRenderer.color = state ? Color.green : Color.red;
//         }
//     }
//     
//     public Ore SpawnOre(ServerOreStruct data)
//     {
//         Ore ore;
//         if (_oresPull.Count > 0)
//         {
//             ore = _oresPull[0];
//             _oresPull.RemoveAt(0);
//             ore.gameObject.SetActive(true);
//         }
//         else
//         {
//             ore = Instantiate(PrefabsManager.Instance.OrePrefab).GetComponent<Ore>();
//             ore.Initialize();
//         }
//         ore.SyncData(data);
//         ore.gameObject.name = $"Ore_{data.EntityId}";
//         ActiveOres.Add(data.EntityId, ore);
//         return ore;
//     }
//     
//     public void DespawnOre(int id)
//     {
//         if (ActiveOres.TryGetValue(id, out var ore))
//         {
//             ore.gameObject.SetActive(false);
//             _oresPull.Add(ore);
//             ActiveOres.Remove(id);
//         }
//     }
//     
//     public void DespawnOre(Ore ore)
//     {
//         ore.gameObject.SetActive(false);
//         _oresPull.Add(ore);
//         ActiveOres.Remove(ore.EntityId);
//     }
//
//     public void SpawnResourcesSender(ServerResourcesSenderStruct data)
//     {
//         GameObject resourcesSender = Instantiate(PrefabsManager.Instance.ResourcesSender);
//         resourcesSender.name = "ResourcesSender";
//         SpriteRenderer spriteRenderer = resourcesSender.transform.Find("head").GetComponent<SpriteRenderer>();
//         spriteRenderer.color = data.IsActive ? Color.green : Color.red;
//         ResourcesSenders.Add(data.EntityId, resourcesSender);
//     }
//     
//     public Loot SpawnOreLoot(OreType type, Vector3 position)
//     {
//         Loot loot;
//         if (_lootPull.Count > 0)
//         {
//             loot = _lootPull[0];
//             _lootPull.RemoveAt(0);
//             loot.gameObject.SetActive(true);
//         }
//         else
//         {
//             GameObject lootGO = Instantiate(PrefabsManager.Instance.LootPrefab);
//             loot = lootGO.GetComponent<Loot>();
//             loot.Initialize();
//         }
//         loot.transform.localScale = Vector3.one;
//         loot.SyncData(type, position);
//         return loot;
//     }
//     
//     public void DespawnLoot(Loot loot)
//     {
//         loot.gameObject.SetActive(false);
//         _lootPull.Add(loot);
//     }
//     
//     public void TransferLootToPortal(Dictionary<int, int> loot, Transform playerPos)
//     {
//         StartCoroutine(TransferToPortalCoroutine(loot, playerPos, 10f));
//     }
//     
//     private IEnumerator TransferToPortalCoroutine(Dictionary<int, int> loot, Transform playerPos, float duration)
//     {
//         // loot - key id, value count. Convert dictionary to List<int> where int is id
//         Queue<int> lootQueue = new Queue<int>();
//         
//         foreach (var pair in loot)
//         {
//             for (int i = 0; i < pair.Value; i++)
//             {
//                 lootQueue.Enqueue(pair.Key);
//             }
//         }
//         Vector3 portalPos = new Vector3(7, 7, 0);
//         float interval = duration / lootQueue.Count;
//         while (lootQueue.Count > 0)
//         {
//             int id = lootQueue.Dequeue();
//             OreType type = id == 9000 ? OreType.Stone : OreType.Copper;
//             Loot lootGO = SpawnOreLoot(type, playerPos.position);
//             lootGO.TransferToPortal(portalPos);
//             yield return new WaitForSeconds(interval);
//         }
//         
//         
//     }
//     
//     public void HitOre(int entityId, GameObject player, int lootId, OreSize size)
//     {
//         if (ActiveOres.TryGetValue(entityId, out var ore))
//         {
//             ore.TakeDamageWithDelay(player, lootId, 0.6f, size);
//         }
//     }
//
//     public Ore GetOre(int oreId)
//     {
//         return ActiveOres.GetValueOrDefault(oreId);
//     }
// }