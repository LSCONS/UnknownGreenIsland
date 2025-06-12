using UnityEngine;

public static class ReadonlyDataLayer
{
    //추가한 레이어의 이름을 기반으로 LayerMask를 생성할 경우 아래와 같이 선언해서 저장.
    public static readonly LayerMask PlayerLayerMask            = 1 << LayerMask.NameToLayer("Player");
    public static readonly LayerMask GroundLayerMask            = 1 << LayerMask.NameToLayer("Ground");
    public static readonly LayerMask ResourceObjectLayerMask    = 1 << LayerMask.NameToLayer("ResourceObject");
    public static readonly LayerMask EnemyLayerMask             = 1 << LayerMask.NameToLayer("Enemy");
    public static readonly LayerMask BuildingLayerMask          = 1 << LayerMask.NameToLayer("BuildingPre");
    public static readonly LayerMask InteractionLayerMask       = 1 << LayerMask.NameToLayer("Interaction");
    public static readonly LayerMask WeaponLayerMask            = 1 << LayerMask.NameToLayer("Weapon");
    public static readonly LayerMask InteractionCookLayerMask   = 1 << LayerMask.NameToLayer("InteractionCook");
    public static readonly LayerMask InteractionWorkLayerMask   = 1 << LayerMask.NameToLayer("InteractionWork");
}

public static class ReadonlyDataItem
{
    //자원 아이템 정리 (3000 ~ 3999)
    public static readonly int Wood         = 3000;
    public static readonly int Stone        = 3001;
    public static readonly int Obsidian     = 3002;
    public static readonly int LogWood      = 3003;
    public static readonly int Leather      = 3004;
    public static readonly int IronStone    = 3005;
    public static readonly int Fiber        = 3006;
    public static readonly int BlueFree     = 3007;


    //제작 아이템 정리 (2000 ~ 2999)
    public static readonly int Axe              = 2000;
    public static readonly int Knife            = 2001;
    public static readonly int SWORD            = 2002;
    public static readonly int IronPickaxe      = 2003;
    public static readonly int ObsidianPickaxe  = 2004;
    public static readonly int BluePickaxe      = 2005;
    public static readonly int BattleAX         = 2006;
    public static readonly int IronKatana       = 2007;
    public static readonly int ObsidianKatana   = 2008;
    public static readonly int StoneSpear       = 2009;
    public static readonly int BlueSpear        = 2010;
    public static readonly int HoneyOintment    = 2011;
    public static readonly int HwangRyeongo     = 2012;
    public static readonly int Jaungo           = 2013;


    //음식 아이템 정리 (1000 ~ 1999)
    public static readonly int Tofu             = 1000;
    public static readonly int Steak            = 1001;
    public static readonly int Skewer           = 1002;
    public static readonly int MeatStew         = 1003;
    public static readonly int MeatSoup         = 1004;
    public static readonly int CookMeat         = 1005;
    public static readonly int CookGreenMeat    = 1006;
    public static readonly int Budaejjigae      = 1007;
}


public static class ReadonlyAnimator
{
    public static readonly string Attack = "IsAttack";
}
