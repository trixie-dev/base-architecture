// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
//
// public class SessionManager : SessionManagerBase
// {
//     #region Singelton
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
//     public override async void Initialize()
//     {
//         Game.SetSessionState(true);
//         base.Initialize();
//     }
//     
//     public override void QuitMatch()
//     {
//         Game.SetSessionState(false);
//         base.QuitMatch();
//         
//     }
// }