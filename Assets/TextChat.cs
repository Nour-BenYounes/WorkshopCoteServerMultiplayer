using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;


public class TextChat : MonoBehaviour
{
    public static TextChat Instance;
    public TMP_InputField inputField;
    public TMP_Text chatDisplay;
    public Button sendbutton;
    public Controller controller;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void writeMessage(string message)
    {
        chatDisplay.text += "\n" + message;

    }

    [Client]
    public void SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            controller.CmdSend(inputField.text);
            inputField.text = "";
        }
    }

    public void Playersend(Controller controller)
    {
        this.controller = controller;
        sendbutton.onClick.AddListener(SendMessage);
    }




}
