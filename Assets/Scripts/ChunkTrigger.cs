using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    MapController mc;
    public GameObject TargetMap;

    void Start()
    {
        mc = FindObjectOfType<MapController>(); 
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mc.CurrentChunk = TargetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (mc.CurrentChunk == TargetMap) 
            { 
                mc.CurrentChunk = null; 
            }
        }
    }
}
