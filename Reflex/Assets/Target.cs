using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private int score;
    private int targetHit;
    void Start()
    {
        StartCoroutine(SelfDestruct());



    }

    private void OnMouseDown()
    {
        Destroy(gameObject);

        GameControl.currentScore += 1;

    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2f);

        if(this == null)
        {
            Debug.Log("click");

        }
        else
        {
            Destroy(gameObject);
            Handheld.Vibrate();
            Debug.Log("Destroy");
            GameControl.score += 1;

        }

    }

}