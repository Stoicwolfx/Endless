using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Rect bounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bounds.center -= new Vector2(Globals.scrollRate * Time.deltaTime, 0.0f);
        this.mainCamera.transform.position = new Vector3 (bounds.center.x, bounds.center.y, this.mainCamera.transform.position.z);
    }

    public Rect GetBounds()
    {
        return bounds;
    }
}
