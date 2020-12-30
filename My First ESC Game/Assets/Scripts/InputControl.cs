using System.Collections.Generic;
using Esc;
using Esc.connection;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public List<string> playerNames;

    public static string playerPhase;
    // Use this for initialization
    private void Start()
    {
        //ESC specific controller code
        Game.Instance.CreatePlayer += CreatePlayer;
        Game.Instance.CreatePlayerGameObject += CreatePlayerGameObject;
        Game.Instance.OnPlayerReconnected = PlayerReconnect;
        Game.Instance.OnPlayerJoined = PlayerJoin;
        
        //Calling "PlayerLeft" on both, but you can change that depending on desired outcomes
        Game.Instance.OnPlayerDisconnected = player => PlayerLeft("disconnected[" + player.Username + "]", player);
        Game.Instance.OnPlayerTimedOut = player => PlayerLeft("timeout[" + player.Username + "]", player);

    }

    private void OnApplicationQuit()
    {
        Game.HostStopGame();
    }
    
    private void PlayerJoin(Esc.Player player)
    {
        (player as PlayerController)?.OnConnected();
        player.Connection.SendMessage(Game.Instance.BroadcastState);
    }

    private void PlayerReconnect(Esc.Player player)
    {
        (player as PlayerController)?.OnReconnected();
        player.Connection.SendMessage(Game.Instance.BroadcastState);
    }

    private void PlayerLeft(string why, Esc.Player player)
    {   
    }

    private Esc.Player CreatePlayer(Connection connection, string deviceId, string controllerUuid,
        string username,
        bool bot)
    {
        return new PlayerController(connection, deviceId, controllerUuid, username, bot);
    }


    private GameObject CreatePlayerGameObject(Esc.Player player)
    {
        playerNames.Add(player.Username);
        GameManager.I.players.Add(player);
        var playerObj = Instantiate(GameManager.I.playerPrefab);
        playerObj.name = "Player #"+GameManager.I.players.Count;
        player.GameObject = playerObj;
        player.GameObject.SetActive(true);
        return player.GameObject;
        return null;
    }
    
}