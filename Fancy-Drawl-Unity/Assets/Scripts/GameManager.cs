using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MessageType
{
    User,
    Bot
}

public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

}

public class GameManager : MonoBehaviour
{
    OscovaBot oscovaBot;

    
}




