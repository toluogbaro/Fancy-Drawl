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

public class BotDialog : Dialog
{
    [Expression("Hello Bot")]
    public void Hello(Context context, Result result)
    {
        result.SendResponse("Hello User!");
    }
}

public class GameManager : MonoBehaviour
{
    OscovaBot oscovaBot;

    List<Message> Messages = new List<Message>();

    public GameObject chatPanel, textObject;
    public InputField chatBox;
    public Color UserColor, BotColor;
    public GameObject pf;

    void Start()
    {
        try
        {
            oscovaBot = new OscovaBot();
            OscovaBot.Logger.LogReceived += (s, o) =>
            {
                Debug.Log($"OscovaBot: {o.Log}");
            };

            oscovaBot.Dialogs.Add(new BotDialog());
            //oscovaBot.ImportWorkspace("Assets/bot-kb.west");
            oscovaBot.Trainer.StartTraining();

            oscovaBot.MainUser.ResponseReceived += (sender, evt) =>
            {
                AddMessage($"Bot: {evt.Response.Text}", MessageType.Bot);
            };
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

       
    }

    private void Update()
    {
        MessageCheck();
        OnUserEnter();
    }


    private void MessageCheck()
    {
        if (Messages.Count >= 25)
        {
            Destroy(Messages[0].textObject.gameObject);
            Messages.Remove(Messages[0]);
        }
    }

    private void AddMessage(string messageText, MessageType messageType)
    {

        var newMessage = new Message { text = messageText };

        
        GameObject newText = Instantiate(pf as GameObject);
        newText.transform.SetParent(GameObject.Find("Content").transform, false);


        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = messageText;
        newMessage.textObject.color = messageType == MessageType.User ? UserColor : BotColor;

        Messages.Add(newMessage);
    }

    private void SendMessageToBot()
    {
        var userMessage = chatBox.text;

        if (!string.IsNullOrEmpty(userMessage))
        {
            Debug.Log($"OscovaBot:[USER] {userMessage}");
            AddMessage($"User: {userMessage}", MessageType.User);
            var request = oscovaBot.MainUser.CreateRequest(userMessage);
            var evaluationResult = oscovaBot.Evaluate(request);
            evaluationResult.Invoke();

            chatBox.Select();
            chatBox.text = "";
        }
    }

   private void OnUserEnter()
   {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToBot();
        }

   }

    


}




