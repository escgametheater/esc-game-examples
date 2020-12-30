using System;
using System.Collections.Generic;
using Esc.api;
using Esc.events;

public class DataConnectionManager
{
    
}

//This is the shape of an input event from a controller
//"CE" is being used to represent "Controller Event"
public class CE_ButtonClick : EscEvent
{
    public readonly string ButtonValue;

    public CE_ButtonClick(string buttonValue)
    {
        ButtonValue = buttonValue;
    }
}

//This is the shape of an input event from Unity
//"GE" is being used to represent "Game Event"
public class GE_PlayerData : EscEvent
{
    public readonly string PlayerName;

    public GE_PlayerData(string playerName)
    {
        PlayerName = playerName;
    }
}

[Serializable]
[ESCGameDefinitions]

//This class refers to a spreadsheet of data on the CMS
public class GameData
{

    //Modtype "replace" means that others can override the data by posting a different sheet in a mod
    [ESCDataSheet(ModType.Replace)]
    //Each "list" here refers to an individual sheet in a spreadsheet
    public List<MyData> myData;
    
    public List<PlayerNames> playerNames;

}

//This refers to an individual sheet in spreadsheet
public class MyData
{
    //each of these variables refers to a column header
    //Unique ID is required for tracking the data
    [ESCUniqueIDColumn()] public int ID;
    
    //Custom image is a type of data - you'll be pulling in a string called a "slug"
    //this "slug" can be used to easily load a Texture2D that can be applied to a GameObject
    [ESCCustomImageAsset(1, 1, 1200, modGroup = "MyFirstEscGame")] public string ImageSlug;
    
    //Default is purely string data (and can be used for numbers, etc, and converted)
    [ESCDefaultColumnType("GameName", modGroup = "MyFirstEscGame")]
    public string GameName;
    
    //Custom audio is a type of data - you'll be pulling in a string called a "slug"
    //this "slug" can be used to easily load a sound without any of the Unity audio management!
    [ESCCustomAudioAsset(modGroup = "MyFirstEscGame")]
    public string AudioSlug;
}
public class PlayerNames
{
    //each of these variables refers to a column header
    //Unique ID is required for tracking the data
    [ESCUniqueIDColumn()] public int ID;

    [ESCDefaultColumnType("FirstName", modGroup = "MyFirstEscGame")]
    public string FirstName;
    
    [ESCDefaultColumnType("LastName", modGroup = "MyFirstEscGame")]
    public string LastName;
    
}