# Game Data
## Defining Game Data
Define the shape of your data and how it can be modded by others.
```text
[Serializable]
[ESCGameDefinitions]	
public class GameData
{        
    [ESCDataSheet(ModType.Replace, Cardinality.One)]
    public List<Branding> brandDefinitions;
}
```

Step by step:

```c#
[Serializable]
[ESCGameDefinitions]	
public class GameData
```
This defines a class that will correspond with the `gameDefinitions`
Custom Data Key for your game on https://develop.esc.games.  

To update this game data, you will create and upload a .xlsx file.


```c#
    [ESCDataSheet(ModType.ReplaceCascade, Cardinality.One)]
    public List<Branding> brandDefinitions;
```

This defines a sheet in the .xslx file named `brandDefinitions`.

 `ModType.None` would mean that this data cannot be modded.
 `ModType.Append` would mean that mod data will be appended to this data.
 `ModType.Replace` would means that the data must be overridden by a mod.
 `ModType.ReplaceCascade` means that the data will be optionally overridden by a mod.

`Cardinality.One` means that there can only be one row of data in the spreadsheet
`Cardinality.Many` would mean that there can any number of rows of data in the sheet

## Moddable Game Data Types
Let's take a look at the `Branding` class we are getting a list of ONE of in our GameData

```c#
	[Serializable]
	public class Branding
	{
		[ESCUniqueIDColumn]
		public int brandingId;
		
		[ESCCustomImageAsset(1.0f, 1.0f, 2048)]
		public string gameLogo;

		[ESCCustomImageAsset(2.0f,
			1.0f,
			1024,
			modGroup = "branding",
			label = "Powered By Logo",
			description = "Upload a 2:1 ratio PNG image with min-width 1024px")]
		public string poweredByLogo;
		
		[ESCColorString(modGroup = "branding",
			label = "Primary Color",
			description = "Used for splash screens and transitions")]
		public string primaryColor;
		
		[ESCColorString(modGroup = "branding",
			label = "Background Color",
			description = "Used for splash screens and transitions")]
		public string backgroundColor;		
	}
```

There are only five properties: `brandingId`, `gameLogo`, `poweredByLogo`, `primaryColor`, and `backgroundColor`

Each property is adorned with an Attribute that describes its type.  Here are all the types in this example:

* `[ESCUniqueIDColumn]` - this column will be used by ESC to query for specific rows
* `[ESCCustomImageAsset]` - this column will contain the slug of a custom asset that is in fact an image
    * aspect ratio x, y, and minWidth can be specified 
* `[ESCColorString]` - a hex value representing an RGB color eg #FF0000 for red.  

Here is the full list of types

* `[ESCUniqueIDColumn]` - this column will be used to query for specific rows
* `[ESCColorString]` - a hex value representing an RGB color eg #FF0000 for red 
* `[ESCUrl]` - a url 
* `[ESCCustomAudioAsset]` - a slug referencing a custom asset that is in fact an audio file
* `[ESCCustomImageAsset]` - a slug referencing a custom asset that is in fact an image
* `[ESCCustomVideoAsset]` - a slug referencing a custom asset that is in fact a video
* `[ESCCustomVttAsset]` - a slug referencing custom asset that is a video text track file used to synchronize cues to a clock
* `[ESCTextColumnType]` - this is a text column 
* `[ESCEnumColumnType]` - this lets you set a picklist of options available for this column

Each type can include
* `label` - shown on develop.esc.games during mod process
* `description` - shown on develop.esc.games during mod process
* `modGroup` - used to group moddable fields on develop.esc.games 



## Fetching Game Data

```c#
public class GameController : MonoBehaviour {
  public GameData _gameData;
  
  public void Start() {
  
    new GameDataFetcher<GameData>().getGameData(gameData =>
    {
        Debug.Log("GameData - LoadGameData received results. :)");
        
        if (this.gameData.questions != null)
        {
        Debug.Log("GameData - Preserving questions");
        gameData.questions = this.gameData.questions;
        }
        
        _gameData = gameData;
        
        LoadTexturesAndColors();
    
    }, err =>
    {
        Debug.Log("Error getting game data");
    });
  }
}
```

# Custom Assets

After you load your game data load textures and colors:

```c#
class EscMaterial : MonoBehaviour { 
    public Material primaryColorMaterial; // set inside unity editor
    public Material gameTitleMaterial; // set inside unity editor

    public void LoadTexturesAndColors() {
        // Load the image 
        Texture2D logoTexture = Game.Instance.LoadImage(_gameData.brandDefinitions[0].gameLogo, 2048, 2048);
        gameTitleMaterial.mainTexture = logoTexture;
        gameTitleMaterial.color = Color.white;
        
        // Load the color
        ColorUtility.TryParseHtmlString(branding.primaryColor, out primaryColor);
        primaryColorMaterial.color = primaryColor;
    }
}
```

When the time comes, play audio:
```c#
Game.Instance.PlayAudio(wait: 0, audioslug: audioAssetSlug, vttslug: vttAssetSlug, id: currentId);
```

Or video:
```c#
Game.Instance.PlayVideo(wait: 0, videoslug: videoAssetSlug, vttslug: vttAssetSlug, id: currentId);
```

Or just run VTT cues:
```c#
Game.Instance.PlayAudio(wait: 0, audioslug: "none", vttslug: vttAssetSlug, id: currentId);
```

## Image Assets
We support PNG assets to load into textures, you need the right kinda PNG
## Audio Assets
We support WAV
## Video Assets
We support mp4
## VTT Assets
We support trigging cues on a clock with VTT.  Example format: 
```text
00:00:00.001 --> 00:00:00.501
{
  "type": "call",
  "method": "LoadSequence",
  "parameters": {
    "sequence": "Admin",
	"time": 0
  }
}
```

Which will invoke this function because it was adorned with our `VTTAttribute` attribute.  You cannot call your functions from VTT if they do not have the VTT Attribute. 
```c#
	[VTTAttribute]
	public void LoadSequence(string sequence, string animation = "", float time = 0, float endPreviousSequenceTime = 0,
		bool videoMode = false, float transitionTime = 0)
	{

	}
```

You can see the VTT available functions in your build details on develop.esc.games 

# MultiPlayer
## Creating a Player Controller Object for each connected player

Attach the ESC's delegate for creating player objects
```c#
void Awake 
{
    Game.Instance.CreatePlayer += CreatePlayer; // creates a new player object
}
```

Create a new player controller when a player connects
```c#
public CreatePlayer(Connection connection, string deviceId, string controllerUuid, string username, bool bot)
{
    MyPlayerController newPlayer = new MyPlayerController(connection, deviceId, controllerUuid, username, bot);
    return newPlayer;
}

```

Use your own player class with your own instance variables
```c#
public class MyPlayerController : Player {
    int score;
    public MyPlayerController(Connection connection, string deviceId, string controllerUuid, string username, bool bot) : base(connection, deviceId, controllerUuid, username, bot) 
    {
        if (Game.Instance.VerboseDebugging) Debug.Log($"Creating a Player Controller deviceID {deviceId} controllerUuid{controllerUuid} username{username} bot{bot}\n");
        Connection?.RegisterEventHandler(typeof(MyControllerEvent), HandleMyControllerEvent);
    }
}
```
	
Listen for events from the controller 
```c#
void HandleMyControllerEvent(string eventName, int connectionId, EscEvent escEvent) {
    var myControllerEvent = (MyControllerEvent) escEvent;
    // do the right thing!
    Game.Instance.players.PlayerByConnectionId.TryGetValue(connectionId, out var player);
    var myPlayer = (MyPlayerController)player;
    myPlayer.score++;
}  
```

Access players from another MonoBehaviour
* `Game.Instance.players.PlayerByConnectionId` - `Dictionary<int, Player>`
* `Game.Instance.players.PlayerByDeviceId` - `Dictionary<string, Player>`
* `Game.Instance.players.PlayerByUsername` - `Dictionary<string, Player>`
* `Game.Instance.players.PlayerIdByGameObjectId` - `Dictionary<int, string>`

## Controlling a per-player Game Object
```c#
void Awake 
{
    Game.Instance.CreatePlayerGameObject += CreatePlayerGameObject; // creates a new game object associated with a new player
}
```

## Hooking into Player Presence Events

```c#
void Awake()
{
    Game.Instance.OnPlayerReconnected += ReconnectPlayer; // called when a player reconnects
    Game.Instance.OnPlayerDisconnect += PlayerDisconnected; // called when a player's connection disconnects.  your choice whether to call OnPlayerLeft
    Game.Instance.OnPlayerTimeout += PlayerTimedOut; // called when a player's connection timed out.  your choice whether to call OnPlayerLeft
    Game.Instance.OnPlayerLeft += PlayerLeft; // called by timed out and/or disconnected to indicate to the rest of the game the player has left

    Game.Instance.EventManager.RegisterEventHandler(typeof(HostAndGameInfo), typeof(HostAndGameInfo).Name,
        HostAndGameInfoHandler);
}
```

## Hooking into Aggregate/indirect messages

```c#
void Awake()
{
    Game.Instance.OnAggregationMessage += OnAggregationMessage; // called when the game receives an aggregate message
}
```

* Game.Instance.CreatePlayer
* Game.Instance.OnPlayerJoined;
* Game.Instance.OnPlayerReconnected;
* Game.Instance.OnPlayerLeft;
* Game.Instance.OnPlayerDisconnected;
* Game.Instance.OnPlayerTimedOut;
* Game.Instance.CreatePlayerGameObject;
* Game.Instance.OnAggregationMessage;

# VTT
## Cues & Timing 
## Calling Functions 
## Syncing with Audio/Video

# Analytics

### HostTrackEvent
A couple of different ways to track events via the Host from Unity:

Dictionary method:
```
using Esc.api.Requests;
...
    Dictionary<string, object> trackDict = new Dictionary<string, object>();
    trackDict.Add("KillerTeam", car.TeamIndex);
    trackDict.Add("Killer", car.player.Username);
    trackDict.Add("Victim", collCar.player.Username);
    trackDict.Add("VictimTeam", collCar.TeamIndex);
    HostTrackEvent trackEvent = new HostTrackEvent("Car.Killed", car.player.Username, trackDict);
    trackEvent.Send();
```
Flat object method:
```
            HostTrackEvent trackEvent = new HostTrackEvent("CarGameLogic.Start", "", this);
            trackEvent.Send();
```
If the object isn't flat, or you want to include only some fields, place this attribute before the class definition: `[JsonObject(MemberSerialization.OptIn)]` and this before the fields you want to keep: `[JsonProperty]`
