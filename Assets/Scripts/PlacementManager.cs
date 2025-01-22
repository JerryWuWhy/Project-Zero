using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    
    public static PlacementManager Inst { get; private set; }
    public int width, height;
    Grid placementGrid;
    private Dictionary<Vector3Int, StructureModel> temporaryRoadobjects = new Dictionary<Vector3Int, StructureModel>();
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();

    private void Awake()
    {
        Inst = this;
        placementGrid = new Grid(width, height);
    }

    private void Start()
    {
        if (DataManager.Inst.houses.Count == 0)
        {
            foreach (var config in ConfigManager.Inst.initialHouseConfigs)
            {
                DataManager.Inst.SetHouseData(config.blockId, config.houseId);
            }
        }

        foreach (var house in DataManager.Inst.houses)
        {
            PlaceHouse(house.blockId, house.houseId, false);
        }
    }
    public Transform GetHouseTransformByBlockId(int blockId)
    {
        // 找到 blockId 对应的 BlockConfig
        var blockConfig = ConfigManager.Inst.GetBlockConfig(blockId);
        if (blockConfig != null)
        {
            // 找到 blockConfig.housePos 对应的 StructureModel
            if (structureDictionary.ContainsKey(blockConfig.housePos))
            {
                return structureDictionary[blockConfig.housePos].transform;
            }
        }

        return null; // 如果找不到，返回 null
    }

    public void PlaceHouse(int blockId, int houseId, bool save = true)
    {
        var blockConfig = ConfigManager.Inst.GetBlockConfig(blockId);
        var houseConfig = ConfigManager.Inst.GetHouseConfig(houseId);
        PlaceObjectOnTheMap(blockConfig.housePos, houseConfig.prefab, CellType.Structure);
        if (save)
        {
            DataManager.Inst.SetHouseData(blockId, houseId);
        }
    }

    internal CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
    }

    internal bool CheckIfPositionInBound(Vector3Int position)
    {
        if (position.x >= 0 && position.x < width && position.z >= 0 && position.z < height)
        {
            return true;
        }

        return false;
    }

    internal void PlaceObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        structureDictionary.Add(position, structure);
        DestroyNatureAt(position);
    }

    public void RemoveObject(Vector3Int position)
    {
        if (structureDictionary.ContainsKey(position))
        {
            Destroy(structureDictionary[position].gameObject);
            structureDictionary.Remove(position);
            placementGrid[position.x, position.z] = CellType.Empty; //change back the cell type
        }
    }
    
    private void DestroyNatureAt(Vector3Int position)
    {
        RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f),
            transform.up, Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature"));
        foreach (var item in hits)
        {
            Destroy(item.collider.gameObject);
        }
    }

    internal bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionIsOfType(position, CellType.Empty);
    }

    private bool CheckIfPositionIsOfType(Vector3Int position, CellType type)
    {
        return placementGrid[position.x, position.z] == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadobjects.Add(position, structure);
    }

    internal List<Vector3Int> GetNeighboursOfTypeFor(Vector3Int position, CellType type)
    {
        var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }

        return neighbours;
    }

    private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

    internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)
    {
        var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z),
            new Point(endPosition.x, endPosition.z));
        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Point point in resultPath)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }

        return path;
    }

    internal void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadobjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }

        temporaryRoadobjects.Clear();
    }

    internal void AddtemporaryStructuresToStructureDictionary()
    {
        foreach (var structure in temporaryRoadobjects)
        {
            structureDictionary.Add(structure.Key, structure.Value);
            DestroyNatureAt(structure.Key);
        }

        temporaryRoadobjects.Clear();
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadobjects.ContainsKey(position))
            temporaryRoadobjects[position].SwapModel(newModel, rotation);
        else if (structureDictionary.ContainsKey(position))
            structureDictionary[position].SwapModel(newModel, rotation);
    }

    public StructureModel GetStructModel(Vector3Int pos)
    {
        return structureDictionary.ContainsKey(pos) ? structureDictionary[pos] : null;
    }
}