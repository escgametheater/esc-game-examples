using System;
using System.Collections;
using System.Collections.Generic;
using Esc;
using Esc.api;
using Esc.events;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    //Game Data
    private GameData _gameData;
    
    //Game Image Holder
    public GameObject gameImageHolder;

    //a bool that ensures your data is loaded before you try to use it!
    private bool _gameDataInitialized;

    //we will bring in some "moddable" custom data elements
    private string _gameTitle;
    private string _gameImageSlug;
    private string _gameAudioSlug;

    //iterate a unique number whenever a sound is played
    private int _soundId;
    
    //store the first and last names in a list
    public List<string> firstName;
    public List<string> lastName;

    //on screen text with the join information
    public Text joinText;
    
    //on screen text for player data
    public Text playerData;

    //a game object to hold players as they join
    public GameObject playerPrefab;

    //a list of all of the players
    public List<Player> players;

    // Start is called before the first frame update
    private void Start()
    {
        //always initialize lists!
        players = new List<Player>();
        firstName = new List<string>();
        lastName = new List<string>();

        //when referencing GameManager from other scripts, use "GameManager.I"
        if (I == null) I = this;

        //When an aggregate message (for 10k+ player games), this defines which method to call to handle it
        Game.Instance.OnAggregationMessage = MessageReceived;

        //Here is an example of gathering custom / moddable data
        GrabCustomData();

        //This event handler will trigger once the game "slug" is loaded to ensure the correct join information is on the screen        
        Game.Instance.EventManager.RegisterEventHandler(typeof(HostAndGameInfo), SlugLoaded);
    }

    private void GrabCustomData()
    {
        //Use Game Data Fetcher - notice that there is a new class type, defined over in DataConnectionManager
        new GameDataFetcher<GameData>().getGameData(gameData =>
        {
            Debug.Log("GameData - LoadGameData received results. :)");

            _gameData = gameData;

            foreach (var T in _gameData.myData)
            {
                _gameTitle = T.GameName;
                _gameImageSlug = T.ImageSlug;
                _gameAudioSlug = T.AudioSlug;
            }

            foreach (var T in _gameData.playerNames)
            {
                firstName.Add(T.FirstName);
                lastName.Add(T.LastName);
            }
            
            _gameDataInitialized = true;
            
            MakeImage();
            
        }, err => { Debug.Log("Error getting game data"); });
    }

    private void SlugLoaded(string eventtype, int connectionid, EscEvent escevent)
    {
        joinText.text = _gameTitle + " • Join Now • esc.io/" + Game.Instance.hostSlug;
    }

    public void PlayAudio()
    {
        Game.Instance.PlayAudio(_gameAudioSlug, id: "soundNumber_" + _soundId);
    }
    
    public void MakeImage()
    {
        //This is the main ESC script, which bring the image in as a Texture2D
        var tempTexture = Game.Instance.LoadImage(_gameImageSlug, 1200, 1200);
        
        //The next bunch of code is all Unity-based.
        
        //You can process it however you want - we are using Sprite.Create here
        gameImageHolder.AddComponent<SpriteRenderer>().sprite = Sprite.Create(
            tempTexture,new Rect(0, 0, 1200, 1200),
            new Vector2()
        );
        
        //This is just moving the image so it's on screen
        gameImageHolder.GetComponent<Transform>().position = 
            new Vector2(-Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x/2, -Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        
        //Here we scale it down by 50%
        gameImageHolder.GetComponent<Transform>().localScale = 
            new Vector2(.50f,.50f);
        
        //Here we change the color so it doesn't conflict with the text
        gameImageHolder.GetComponent<SpriteRenderer>().color = Color.red;

    }

    private void MessageReceived(Game.AggregationMessage a)
    {
        throw new NotImplementedException();
    }
}