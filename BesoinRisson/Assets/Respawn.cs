using System.Collections;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public void StartRespawn(GameObject you, GameObject adversary, float pos)
    {
        StartCoroutine(RespawnPlayer(you, adversary, pos));
    }
    
    public IEnumerator RespawnPlayer(GameObject you, GameObject adversary, float pos)
    {
        yield return new WaitForSeconds(2f);
        you.transform.position = new Vector2(adversary.transform.position.x+pos, 0.75f);
        you.SetActive(true);
    }
}
