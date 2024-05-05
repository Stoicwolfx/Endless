using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create(float angle, Projectile projectile)
    {
        this.transform.RotateAround(this.transform.position, this.transform.forward, angle);
    }
}
