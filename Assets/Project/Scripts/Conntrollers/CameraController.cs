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
        if (Globals.scrollDelay > 0) return;

        this.bounds.center += new Vector2(Globals.scrollRate * Time.deltaTime, 0.0f);
        this.mainCamera.transform.position = new Vector3 (this.bounds.center.x, this.bounds.center.y, this.mainCamera.transform.position.z);
    }

    public void ResetCamera()
    {
        this.bounds.center = new Vector2(0.0f, 0.0f);
        this.mainCamera.transform.position = new Vector3(0.0f, 0.0f, this.mainCamera.transform.position.z);
    }

    public Rect GetBounds()
    {
        return this.bounds;
    }
}
