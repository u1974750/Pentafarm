public enum ItemType{
    Seed,
    CommoditySeed,
    Plants,      
    Watering_tool,  //watering can
    Hoeing_tool,    //hoe
    Chopping_tool,  //axe
    Breaking_tool,  //pickaxe
    Reaping_tool,   //scythe (hoz)
    Collecting_tool,//cesta
    Reapable_scenery,
    Commodity,       //muebles
    none,           
    count              //number of items on the list
}
public enum InventoryLocation
{
    player,
    chest,
    recoverShop,
    count 
}
public enum PlayerAction
{
    idle,
    walk,
    watering,
    hoeing,
    reaping,
    collecting,
    building,
    placingSeeds,
    run,
    count,
    demolishing,
    sleep
}

public enum SnapCell
{ 
    center, 
    edgV, 
    edgH,
    vertex 
};
