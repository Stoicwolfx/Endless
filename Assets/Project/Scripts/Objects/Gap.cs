using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Globals.gameRunning) return;

        //this.transform.position = new Vector3(this.transform.position.x + Globals.scrollRate * Time.deltaTime,
        //                                      this.transform.position.y,
        //                                      this.transform.position.z);

        float maxX = this.transform.position.x + this.transform.localScale.x * 0.5f;
        if (maxX < Globals.destructionLimit)
        {
            Destroy(this.gameObject);
        }

    }

}
