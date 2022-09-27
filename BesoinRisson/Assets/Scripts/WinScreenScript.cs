using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{
    [SerializeField] private Scene _sceneToGo;

    public void MoveScene()
    {
        SceneManager.LoadScene(_sceneToGo.name);
    }
}
