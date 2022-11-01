using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColliderRule : RuleTile<ColliderRule>
{
    public List<TileBase> Siblings = new List<TileBase>();

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    return other != null;
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return other == null;
                }
        }

        return base.RuleMatch(neighbor, other);
    }
}