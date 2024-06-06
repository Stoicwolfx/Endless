using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;
    public float rotation;

    [SerializeField] private Gap gapPrefab;

    private readonly float initialXPos = -7.5f;
    private readonly float initialYPos = -3.5f;
    private readonly float initialXScale = 3.0f;
    private readonly float initialYScale = 3.0f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Globals.gameRunning) return;

        //this.transform.position = new Vector3(this.transform.position.x + Globals.scrollRate * Time.deltaTime,
        //                                      this.transform.position.y,
        //                                      this.transform.position.z);

        //maxX = this.transform.position.x + this.transform.localScale.x * 0.5f;
        if (maxX < Globals.destructionLimit)
        {
            Destroy(this.gameObject);
        }

        Globals.destructionLimit -= Globals.scrollRate * Time.deltaTime;
        Globals.creationLimit -= Globals.scrollRate * Time.deltaTime;

    }

    //Will create a surface in the starting position everytime
    public void Create()
    {
        this.transform.position = new Vector3(initialXPos, initialYPos, 0.0f);
        this.transform.localScale = new Vector3(initialXScale, initialYScale, 0.0f);

        this.maxX = this.transform.position.x + this.transform.localScale.x * 0.5f;
        this.maxY = this.transform.position.y + this.transform.localScale.y * 0.5f;

        this.minX = this.transform.position.x - this.transform.localScale.x * 0.5f;
        this.minY = this.transform.position.y - this.transform.localScale.y * 0.5f;

        this.rotation = 0.0f;
    }

    public void Create(float lastX, float lastY, float maxJump, float maxGap)
    {
        float minHeight, maxHeight;
        float xPos, yPos, xScale, yScale;

        float xGap = 0.0f;
        float shiftX = 0.0f;
        float shiftY = 0.0f;

        bool rotated = false;

        //Decide if up or down
        if (((Random.value < 0.5) &&
            ((lastY - Globals.screenBottom) > (Globals.minSurfaceHeight + Globals.minHeightDelta))) ||
            ((lastY - Globals.screenBottom) > (Globals.maxSurfaceHeight - Globals.minHeightDelta)))
        {
            //Go Down
            minHeight = Globals.minSurfaceHeight;
            maxHeight = (lastY - Globals.screenBottom) - Globals.minHeightDelta;
        }
        else
        {
            //Go up
            minHeight = (lastY - Globals.screenBottom) + Globals.minHeightDelta;
            maxHeight = (((lastY - Globals.screenBottom) + maxJump) < Globals.maxSurfaceHeight) ? ((lastY - Globals.screenBottom) + maxJump) : Globals.maxSurfaceHeight;
        }

        //check if Gap used
        if (Random.value < 0.3)
        {
            xGap = Random.Range(Globals.minGap, maxGap);
            Gap gap = Instantiate(gapPrefab);
            gap.transform.SetParent(this.transform.parent, false);
            gap.transform.position = new Vector3(lastX + xGap / 2.0f, 0f, 0f);
            gap.transform.localScale = new Vector3(xGap, 10.0f, 1.0f);
        }

        xScale = Random.Range(Globals.minSurfaceWidth, Globals.maxSurfaceWidth);
        yScale = Random.Range(minHeight, maxHeight);

        this.transform.localScale = new Vector3(xScale, yScale, 0.0f);
        xPos = lastX + this.transform.localScale.x * 0.5f + xGap;
        yPos = Globals.screenBottom + this.transform.localScale.y * 0.5f;

        //Determine if rotated
        if (Random.value < 0.2)
        {
            rotated = true;
            this.rotation = Random.Range(Globals.minRotationDeg, Globals.maxRotationDeg) * ((Random.value < 0.5) ? -1 : 1);
            this.transform.Rotate(0.0f, 0.0f, this.rotation, Space.Self);

            Vector2[] corners = new Vector2[4];
            float[] angles = new float[4];
            float radius = Mathf.Sqrt(xScale * xScale + yScale * yScale) / 2.0f;

            angles[0] = Mathf.Atan2(yScale, xScale) + rotation * Mathf.Deg2Rad;
            angles[1] = Mathf.Atan2(yScale, -xScale) + rotation * Mathf.Deg2Rad;
            angles[2] = Mathf.Atan2(-yScale, -xScale) + rotation * Mathf.Deg2Rad;
            angles[3] = Mathf.Atan2(-yScale, xScale) + rotation * Mathf.Deg2Rad;

            corners[0] = new Vector2(radius * Mathf.Cos(angles[0]), radius * Mathf.Sin(angles[0]));
            corners[1] = new Vector2(radius * Mathf.Cos(angles[1]), radius * Mathf.Sin(angles[1]));
            corners[2] = new Vector2(radius * Mathf.Cos(angles[2]), radius * Mathf.Sin(angles[2]));
            corners[3] = new Vector2(radius * Mathf.Cos(angles[3]), radius * Mathf.Sin(angles[3]));

            //sort corners
            for (int i = 0; i < corners.Length; i++)
            {
                for (int j = 0; j < corners.Length; j++)
                {
                    if (i == j) continue;
                    if (corners[i].y < corners[j].y)
                    {
                        (corners[j], corners[i]) = (corners[i], corners[j]);
                    }
                }
            }

            //get min and max X/Y
            for (int i = 0; i < corners.Length; i++)
            {
                if (corners[i].x < this.minX) this.minX = corners[i].x;
                if (corners[i].x > this.maxX) this.maxX = corners[i].x;
            }
            this.minY = corners[0].y;
            this.maxY = corners[3].y;

            //calculate shifts
            shiftX = 0;

            shiftY = ((yPos + corners[3].y - Globals.maxSurfaceHeight) > (yPos + corners[1].y - Globals.screenBottom)) ?
                (yPos + corners[3].y - Globals.maxSurfaceHeight) : (yPos + corners[1].y - Globals.screenBottom);

            xPos += shiftX;
            yPos -= shiftY;
        }

        this.transform.position = new Vector3(xPos, yPos, 0.0f);

        //Get minX/maxX and minY/maxY
        if (rotated)
        {
            this.maxX += xPos;
            this.maxY += yPos;
            this.minX += xPos;
            this.minY += yPos;
        }
        else
        {
            this.maxX = xPos + xScale * 0.5f;
            this.minX = xPos - xScale * 0.5f;
            this.maxY = yPos + yScale * 0.5f;
            this.minY = yPos - yScale * 0.5f;
        }

    }
}
