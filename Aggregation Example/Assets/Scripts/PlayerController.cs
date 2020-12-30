using Esc;
using Esc.api;
using Esc.connection;
using Esc.events;
using UnityEngine;

public class CE_ButtonClick : EscEvent
{
    public readonly string ButtonValue;

    public CE_ButtonClick(string buttonVal)
    {
        ButtonValue = buttonVal;
    }
}

public class PlayerController : Player
{
    private string _username;
    
    public PlayerController(Connection connection, string deviceId, string controllerUuid, string username, bool bot) :
        base(connection, deviceId, controllerUuid, username, bot)
    {
        _username = username;

        Connection.RegisterEventHandler(typeof(CE_ButtonClick), OnButtonClick);
    }

    public void OnConnected()
    {        if (Bot) return;
    }

    private void OnJoinGame(string eventName, int connectionId, EscEvent escEvent)
    {
        
    }

    public void OnReconnected()
    {
        if (Bot) return;
    }

    public void OnButtonClick(string eventName, int connectionId, EscEvent escEvent)
    {
        
        var buttonClick = (CE_ButtonClick) escEvent;
        
        switch (buttonClick.ButtonValue)
        {
            default:
                break;
        }
    }

    private void Initialize(GameObject prefab)
    {
        if (Bot) return;
    }
 
}