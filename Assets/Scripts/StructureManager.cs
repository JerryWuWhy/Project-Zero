// using SVS;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
//
// public class StructureManager : MonoBehaviour
// {
//     public static StructureManager Inst { get; private set; }
//     
//     public PlacementManager placementManager;
//
//     public int selectedHouseIndex;
//
//     private void Awake()
//     {
//         Inst = this;
//     }
//
//     // Method to place a house using a user-selected prefab index
//     public void PlaceHouse(Vector3Int position)
//     {
//         if (CheckPositionBeforePlacement(position))
//         {
//             // Ensure the selected index is within range
//             var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
//             if (selectedHouseIndex >= 0 && selectedHouseIndex < houseConfigs.Count)
//             {
//                 var houseConfig = houseConfigs[selectedHouseIndex];
//                 placementManager.PlaceObjectOnTheMap(position, houseConfig.prefab, CellType.Structure);
//                 DataManager.Inst.SetHouseData(position, houseConfig.id);
//                 AudioPlayer.instance.PlayPlacementSound();
//             }
//         }
//     }
//
//     private bool CheckPositionBeforePlacement(Vector3Int position)
//     {
//         if (!placementManager.CheckIfPositionInBound(position))
//         {
//             return false;
//         }
//
//         if (!placementManager.CheckIfPositionIsFree(position))
//         {
//             return false;
//         }
//
//         if (placementManager.GetNeighboursOfTypeFor(position, CellType.Road).Count <= 0)
//         {
//             return false;
//         }
//
//         return true;
//     }
// }
