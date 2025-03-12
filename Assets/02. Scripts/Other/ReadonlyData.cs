using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReadonlyData
{
    public static readonly string PlayerLayer = "Player";
    public static readonly string GroundLayer = "Ground";
    public static readonly string ResourceObjectLayer = "ResourceObject";
    public static readonly string EnemyLayer = "Enemy";
    public static readonly string BuildingLayer = "Building";

    public static readonly LayerMask PlayerLayerMask = 1 << LayerMask.NameToLayer(PlayerLayer);
    public static readonly LayerMask GroundLayerMask = 1 << LayerMask.NameToLayer(GroundLayer);
    public static readonly LayerMask ResourceObjectLayerMask = 1 << LayerMask.NameToLayer(ResourceObjectLayer);
    public static readonly LayerMask EnemyLayerMask = 1 << LayerMask.NameToLayer(EnemyLayer);
    public static readonly LayerMask BuildingLayerMask = 1 << LayerMask.NameToLayer(BuildingLayer);
}
