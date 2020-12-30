using System.Collections;
using System.Collections.Generic;
using Esc;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public GameObject playerPrefab;
    public List<Player> players;
    public Text aggText;
    private int _aggMessages;

    // Start is called before the first frame update
    private void Start()
    {
        players = new List<Player>();
        if (I == null) I = this;

        //When an aggregate message (for 10k+ player games), this defines which method to call to handle it
        Game.Instance.OnAggregationMessage = MessageReceived;

    }

    private void MessageReceived(Game.AggregationMessage a)
    {
        Debug.Log("Aggregate Message Received");
        _aggMessages++;
        throw new System.NotImplementedException();
    }
    
    // Update is called once per frame
    private void Update()
    {
        aggText.text = "Host Slug: " + Game.Instance.hostSlug + "\n" +
                       "Host URL: " + Game.Instance.hostUrl + "\n" +
                       "Connected Devices: " + Game.Instance.players.NumberOfPlayers + "\n" +
                       "Aggregate Messages Received: " + _aggMessages;
    }
}
