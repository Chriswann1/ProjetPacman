using System.Linq;
using Random = UnityEngine.Random;

public class Orange : Enemy
{
    public override void Update() //Overriding the Update of Enemy class to change the target function
    {
        
        target = _pathfinder.maptilemap.layoutGrid.GetCellCenterWorld(Pathfinder.TileNode.Keys.ToArray()[Random.Range(0, Pathfinder.TileNode.Keys.Count-1)]);
        base.Update();
    }
}
