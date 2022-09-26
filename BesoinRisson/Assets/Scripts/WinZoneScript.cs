using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZoneScript : MonoBehaviour
{
    [SerializeField] private GameObject _playerConcerned;
    [SerializeField] private GameObject _uiWinScreen;
   
    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("boop");
        if (other.Equals(_playerConcerned.GetComponent<Collider>()))
        {
            Debug.Log("sapass");
            _uiWinScreen.SetActive(true);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("yeet");
    }
}
