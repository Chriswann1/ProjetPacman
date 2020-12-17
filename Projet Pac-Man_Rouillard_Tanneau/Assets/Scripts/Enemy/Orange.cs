using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Orange : Enemy
{
    public override void Update()
    {
        
        target = _pathfinder.maptilemap.layoutGrid.GetCellCenterWorld(Pathfinder.TileNode.Keys.ToArray()[Random.Range(0, Pathfinder.TileNode.Keys.Count-1)]);
        base.Update();
    }
}
