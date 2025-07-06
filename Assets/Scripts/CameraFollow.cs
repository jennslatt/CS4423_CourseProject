using TreeEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Vector3 offset;
    Vector3 newpos;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = player.transform.position - transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        newpos = transform.position;
        newpos.x = player.transform.position.x - offset.x;
        newpos.z = player.transform.position.z - offset.z;

        transform.position = newpos;

        //transform.position = player.transform.position - offset;
    }
}
