using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public int RowCount, ColumnCount;

    [SerializeField] ItemHolder itemHolderObject;

    [SerializeField] Transform gridStartPoint;

    [SerializeField] bool isVerticalGrid;


    public float xOffset;

    [ShowIf("isVerticalGrid")] public float yOffset;

    [HideIf("isVerticalGrid")] public float zOffset;

    public ItemHolder[,] GridsArray;
    public List<ItemHolder> Grids;

    public GridManager Initialize()
    {
        int index = 0;

        GridsArray = new ItemHolder[ColumnCount, RowCount];

        for (int j = 0; j < RowCount; j++)
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                GridsArray[i, j] = Grids[index];
                index++;
            }
        }

        return this;
    }

    public ItemHolder FindHolderByGridPos(Vector2Int targetGridPos)
    {
        var targetHolder = Grids.FirstOrDefault(x => x.GridPosition == targetGridPos);
        if (targetHolder)
        {
            return targetHolder;
        }
        else
        {
            return null;
        }
    }

    public bool CheckPlaceableByPosition(Vector2Int targetPos)
    {
        var targetlist = Grids.Where(x =>
            x.GridPosition == targetPos && x.CurrentItem is Rail && !(x.CurrentItem as Rail).IsSetted()).ToList();
        if (targetlist.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


#if UNITY_EDITOR
    [Button(SdfIconType.GenderTrans, Style = ButtonStyle.Box, IconAlignment = IconAlignment.LeftEdge),
     GUIColor(0, 1f, 1f)]
    public void CreateGrids()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        Grids.Clear();

        GridsArray = new ItemHolder[ColumnCount, RowCount];

        int index = 0;

        if (isVerticalGrid)
        {
            for (int j = 0; j < RowCount; j++)
            {
                for (int i = 0; i < ColumnCount; i++)
                {
                    var position = new Vector3(i * xOffset + gridStartPoint.position.x,
                        -j * yOffset + gridStartPoint.position.y, transform.position.z);

                    GridsArray[i, j] = (PrefabUtility.InstantiatePrefab(itemHolderObject.gameObject) as GameObject)
                        ?.GetComponent<ItemHolder>();
                    GridsArray[i, j].transform.position = position;
                    GridsArray[i, j].transform.rotation = Quaternion.identity;
                    GridsArray[i, j].transform.SetParent(transform);

                    Grids.Add(GridsArray[i, j]);

                    GridsArray[i, j].ID = index;
                    GridsArray[i, j].GridPosition = new Vector2Int(i, j);

                    index++;
                }
            }
        }

        else
        {
            for (int j = 0; j < RowCount; j++)
            {
                for (int i = 0; i < ColumnCount; i++)
                {
                    var position = new Vector3(i * xOffset + gridStartPoint.position.x, transform.position.y,
                        -j * zOffset + gridStartPoint.position.z);

                    GridsArray[i, j] = (PrefabUtility.InstantiatePrefab(itemHolderObject.gameObject) as GameObject)
                        ?.GetComponent<ItemHolder>();
                    GridsArray[i, j].transform.position = position;
                    GridsArray[i, j].transform.rotation = Quaternion.identity;
                    GridsArray[i, j].transform.SetParent(transform);

                    Grids.Add(GridsArray[i, j]);

                    GridsArray[i, j].ID = index;
                    GridsArray[i, j].GridPosition = new Vector2Int(i, j);

                    index++;
                }
            }
        }
    }
#endif

    [Button(SdfIconType.Trash, Style = ButtonStyle.Box, IconAlignment = IconAlignment.LeftEdge), GUIColor(1f, 0, 0)]
    public void ClearGrids()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        Grids.Clear();
    }

    public List<ItemHolder> GetNeighbours(Vector2Int targetPos)
    {
        var neighbours = new List<ItemHolder>();

        if (targetPos.y + 1 < ColumnCount)
        {
            neighbours.Add(GridsArray[targetPos.x, targetPos.y + 1]);
        }

        if (targetPos.y - 1 > -1)
        {
            neighbours.Add(GridsArray[targetPos.x, targetPos.y - 1]);
        }

        if (targetPos.x + 1 < RowCount)
        {
            neighbours.Add(GridsArray[targetPos.x + 1, targetPos.y]);
        }

        if (targetPos.x - 1 > -1)
        {
            neighbours.Add(GridsArray[targetPos.x - 1, targetPos.y]);
        }

        return neighbours;
    }
}