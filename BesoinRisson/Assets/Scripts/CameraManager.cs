using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _rival;
    
    [SerializeField] private GameObject _playerfinish;
    [SerializeField] private GameObject _rivalfinish;

    [SerializeField] private float _cameraMovespeed;
    private bool ready;

    private void Start()
    {
        StartCoroutine(TimeCountdown());
    }

    private float CalcDistance(GameObject player, GameObject arrival)
    {
        return Math.Abs((player.transform.position - arrival.transform.position).x);
    }

    private GameObject CompareBothPlayersDistances()
    {
        if (CalcDistance(_player, _playerfinish) < CalcDistance(_rival, _rivalfinish)) return _player;
        return _rival;
    }

    private void Update()
    {
        if (!ready) return;
        if (CompareBothPlayersDistances().Equals(_player))
        {
            //en fct de la direction qui est faces
            transform.position = Vector3.Lerp(transform.position,new Vector3(_player.transform.position.x + 5, 0, -10), Time.deltaTime * _cameraMovespeed);
        }
        else transform.position = Vector3.Lerp(transform.position,new Vector3(_rival.transform.position.x - 5, 0, -10), Time.deltaTime * _cameraMovespeed);
    }

    IEnumerator TimeCountdown()
    {
        // ui pour mettre un countdown
        yield return new WaitForSeconds(3);
        ready = true;
    }
}
