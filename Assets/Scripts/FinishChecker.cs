using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishChecker : MonoBehaviour
{
    [SerializeField]
    private GameObject finishLine;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.position.y < transform.position.y)
        {
            finishLine.GetComponent<Animator>().SetBool("CloseTheFinish", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.position.y < transform.position.y)
        {
            finishLine.GetComponent<Animator>().SetBool("CloseTheFinish", false);
        }
    }

}
