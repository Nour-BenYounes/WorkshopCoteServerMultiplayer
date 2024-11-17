using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textdeathspawn : MonoBehaviour
{
    public static textdeathspawn Instance;
    public TextMeshProUGUI announcementText;
   
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


    public void textchango(string text)
    {
        announcementText.text = text;
        announcementText.gameObject.SetActive(true);
    }
}
