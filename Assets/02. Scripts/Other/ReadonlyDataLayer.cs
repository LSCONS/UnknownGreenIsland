using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReadonlyDataLayer
{
    //추가한 레이어의 이름을 public static readonly string의 형태로 고정해서 저장.
    public static readonly string PlayerLayer = "Player";
    public static readonly string GroundLayer = "Ground";
    public static readonly string ResourceObjectLayer = "ResourceObject";
    public static readonly string EnemyLayer = "Enemy";
    public static readonly string BuildingLayer = "BuildingPre";
    public static readonly string InteractionLayer = "Interaction";
    public static readonly string WeaponLayer = "Weapon";
    public static readonly string InteractionCookLayer = "InteractionCook";
    public static readonly string InteractionWorkLayer = "InteractionWork";

    //추가한 레이어의 이름을 기반으로 LayerMask를 생성할 경우 아래와 같이 선언해서 저장.
    public static readonly LayerMask PlayerLayerMask = 1 << LayerMask.NameToLayer(PlayerLayer);
    public static readonly LayerMask GroundLayerMask = 1 << LayerMask.NameToLayer(GroundLayer);
    public static readonly LayerMask ResourceObjectLayerMask = 1 << LayerMask.NameToLayer(ResourceObjectLayer);
    public static readonly LayerMask EnemyLayerMask = 1 << LayerMask.NameToLayer(EnemyLayer);
    public static readonly LayerMask BuildingLayerMask = 1 << LayerMask.NameToLayer(BuildingLayer);
    public static readonly LayerMask InteractionLayerMask = 1 << LayerMask.NameToLayer(InteractionLayer);
    public static readonly LayerMask WeaponLayerMask = 1 << LayerMask.NameToLayer(WeaponLayer);
    public static readonly LayerMask InteractionCookLayerMask = 1 << LayerMask.NameToLayer(InteractionCookLayer);
    public static readonly LayerMask InteractionWorkLayerMask = 1 << LayerMask.NameToLayer(InteractionWorkLayer);
}

public static class ReadonlyDataItem
{
    //자원 아이템 정리 (3000 ~ 3999)
    public static readonly int Wood = 3000;
    public static readonly int Stone = 3001;


    //제작 아이템 정리 (2000 ~ 2999)
    public static readonly int Axe = 2000;

    //음식 아이템 정리 (1000 ~ 1999)
    public static readonly int Tofu = 1000;

}
