using Esc;
using Esc.connection;
using Esc.events;
using UnityEngine;

public class PlayerController : Player
{
    private string _username;

    public PlayerController(Connection connection, string deviceId, string controllerUuid, string username, bool bot) :
        base(connection, deviceId, controllerUuid, username, bot)
    {
        _username = GameManager.I.firstName[Random.Range(0,GameManager.I.firstName.Count)]+
            " " +
            GameManager.I.lastName[Random.Range(0,GameManager.I.lastName.Count)];

        //We put an event handler on the "CE_ButtonClick" which will run this function if a player taps a button
        Connection.RegisterEventHandler(typeof(CE_ButtonClick), OnButtonClick);
    }

    public void OnConnected()
    {
        if (Bot) return;
        var playerMessage = new GE_PlayerData(_username);
        Connection.SendMessage(playerMessage);

    }

    private void OnJoinGame(string eventName, int connectionId, EscEvent escEvent)
    {
    }

    public void OnReconnected()
    {
        if (Bot) return;
        var playerMessage = new GE_PlayerData(_username);
        Connection.SendMessage(playerMessage);

    }

    //This function runs when a "CE_ButtonClick" event happens on an individual controller
    public void OnButtonClick(string eventName, int connectionId, EscEvent escEvent)
    {
        var buttonClick = (CE_ButtonClick) escEvent;

        GameManager.I.playerData.text = _username + " has pressed a button with this value: " + buttonClick.ButtonValue;
        
        GameManager.I.PlayAudio();
        
        switch (buttonClick.ButtonValue)
        {
        }
    }

    private void Initialize(GameObject prefab)
    {
        if (Bot) return;
    }
}