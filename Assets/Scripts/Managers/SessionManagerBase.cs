// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
//
// public class SessionManagerBase : MonoBehaviour
// {
//     #region Singelton
//     public static SessionManagerBase Instance;
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
//
//     public bool IsInitialized { get; private set; } = false;
//     public bool IsOfflineMode = false;
//     
//     public Dictionary<int, IEntity> Players { get; private set; } = new Dictionary<int, IEntity>();
//     public Dictionary<int, IEntity> Mobs { get; private set; } = new Dictionary<int, IEntity>();
//     public List<GameObject> NPCs;
//     public IEntity LocalPlayer;
//
//     public void Start()
//     {
//         Initialize();
//     }
//     public virtual async void Initialize()
//     {
//         Debug.Log("SessionManagerBase Initialize");
//         InitializeNPCs();
//         if (IsOfflineMode)
//         {
//             ServerPlayerStruct player = new ServerPlayerStruct
//             {
//                 EntityId = 0,
//                 PersonaTag = "OfflinePlayer",
//                 Name = "OfflinePlayer",
//                 Position = new Vector2(10, -10),
//                 Radius = 1
//             };
//             SpawnPlayer(true, player, false);
//             return;
//         }
//         Debug.Log("Connect to cardinal");
//         await CardinalManager.CreateSessionPlayer();
//         Debug.Log("Session Player created");
//         _worldState = await CardinalManager.GetWorldState();
//         Debug.Log("World state received. Length: " + _worldState.Players.Count + " players, " + _worldState.Ores.Count + " ores, " + _worldState.Mobs.Count + " mobs, " + _worldState.ResourcesSenders.Count + " resources senders.");
//         SyncWorldState(_worldState);
//         
//         // _debugState = await CardinalManager.GetDebugState();
//         // SyncDebugState(_debugState);
//         IsInitialized = true;
//     }
//     
//     protected virtual void SyncWorldState(ServerWorldStateStruct worldState)
//     {
//         Debug.Log("SyncWorldState");
//         foreach (var ore in worldState.Ores)
//         {
//             ObjectsManager.Instance.SyncOre(ore);
//         }
//         Debug.Log("Ores synced");
//         foreach (var player in worldState.Players)
//         {
//             bool isLocal = player.EntityId == worldState.LocalPlayerId;
//             if (Players.TryGetValue(player.EntityId, out var comp))
//             {
//                 comp.GameObject.transform.position = player.Position;
//                 comp.SyncData(player);
//             }
//             else
//             {
//                 SpawnPlayer(isLocal, player);
//             }
//         }
//         
//         foreach (var mob in worldState.Mobs)
//         {
//             if (Mobs.TryGetValue(mob.EntityId, out var comp))
//             {
//                 comp.GameObject.transform.position = mob.Position;
//                 comp.SyncData(mob);
//             }
//             else
//             {
//                 SpawnMob(mob);
//             }
//         }
//         
//         // foreach (var resourcesSender in worldState.ResourcesSenders)
//         // {
//         //     ObjectsManager.Instance.SyncResourcesSender(resourcesSender);
//         // }
//     }
//     private void SyncDebugState(ServerDebugStateStruct worldState)
//     {
//         foreach (var colliderStruct in worldState.Colliders)
//         {
//             ObjectsManager.Instance.ActiveOres.TryGetValue(colliderStruct.EntityId, out var ore);
//             Players.TryGetValue(colliderStruct.EntityId, out var player);
//             GameObject obj = ore != null ? ore.gameObject : player.GameObject;
//             if (obj == null)
//             {
//                 continue;
//             }
//             GameObject col = Instantiate(PrefabsManager.Instance.ColliderPrefab, colliderStruct.Position, Quaternion.identity);
//             col.transform.localScale = new Vector3(colliderStruct.Size.x, colliderStruct.Size.y, 1);
//             col.transform.SetParent(obj.transform);
//         }
//     }
//     
//     public void InitializeNPCs()
//     {
//         foreach (var npc in NPCs)
//         {
//             npc.GetComponent<IEntity>().Initialize(false);
//         }
//         Debug.Log("NPCs initialized");
//     }
//
//     public IEntity TryGetEntity(int id, out IEntity entity)
//     {
//         if (Players.TryGetValue(id, out entity))
//         {
//             return entity;
//         }
//         if (Mobs.TryGetValue(id, out entity))
//         {
//             return entity;
//         }
//         return null;
//     }
//     
//     public bool IsLocalPlayer(int id)
//     {
//         return LocalPlayer.EntityId == id;
//     }
//     
//     
//     public void SpawnPlayer(bool isLocal, ServerPlayerStruct data, bool IsOnlineMode = true, string msg = default)
//     {
//         if (isLocal && LocalPlayer != null)
//         {
//             return;
//         }
//         GameObject playerPrefab = PrefabsManager.Instance.GetPlayerPrefab();
//
//         GameObject player = Instantiate(playerPrefab, data.Position, Quaternion.identity);
//         Player playerComp = player.GetComponent<Player>();
//         playerComp.Initialize(isLocal, IsOnlineMode);
//         playerComp.SyncData(data);
//         Players.Add(data.EntityId, playerComp);
//         if (isLocal)
//         {
//             LocalPlayer = playerComp;
//             player.name = "LocalPlayer " + data.EntityId + Time.time;
//         }
//         else
//         {
//             player.name = "RemotePlayer" + data.EntityId;
//         }
//         
//     }
//     
//     public void SpawnMob(ServerMobStruct data)
//     {
//         GameObject mobPrefab = PrefabsManager.Instance.MobPrefab;
//         GameObject mob = Instantiate(mobPrefab, data.Position, Quaternion.identity);
//         Mob mobComp = mob.GetComponent<Mob>();
//         mobComp.Initialize(false, true);
//         mobComp.SyncData(data);
//         Mobs.Add(data.EntityId, mobComp);
//         mob.name = "Mob" + data.EntityId;
//     }
//     
//     public void DestroyEntity(int id, ServerObjects type)
//     {
//         var list = type == ServerObjects.Player ? Players : Mobs;
//         Debug.Log("Destroying entity " + id);
//         if (!list.ContainsKey(id))
//         {
//             return;
//         }
//         bool isLocal = id == LocalPlayer.EntityId;
//         // Destroy the player.
//         list[id].SafeDestroy();
//     
//         // Remove the player from the players array.
//         list.Remove(id);
//         
//         if (isLocal)
//         {
//             Invoke(nameof(QuitMatch), 2f);
//         }
//         //OnNumberOfPlayersChange?.Invoke();
//     }
//     
//     
//     public virtual void QuitMatch()
//     {
//         _ = QuitMatchAsync();
//         
//     }
//     private async Task QuitMatchAsync()
//     {
//         DestroyEntity(LocalPlayer.EntityId, ServerObjects.Player);
//         Players.Clear();
//         await CardinalManager.DestroySessionPlayer();
//         CustomSceneManager.Instance.LoadLobby();
//         //await SaveSystem.AddToBalance(LocalPlayer.PlayerScore.ConsumedParticles);
//         //LoadingScreen.Instance.LoadScene("Menu");
//     }
//
//     public async void OnApplicationQuit()
//     {
//         Game.SetSessionState(false);
//         DestroyEntity(LocalPlayer.EntityId, ServerObjects.Player);
//         await CardinalManager.DestroySessionPlayer();
//         await CardinalManager.Socket.CloseAsync();
//     }
// }