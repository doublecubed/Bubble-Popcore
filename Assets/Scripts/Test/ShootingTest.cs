using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingTest : MonoBehaviour
{
    public Vector2 pos;
    public Vector2 directionVector;
    public Vector2 hitPoint;
    public Vector2 bounceVector;
    public Vector2 secondHitPoint;
    public bool mouseDown;
    public LineRenderer renderer;
    public bool hitsBoundary;
    public float angle;
    
    public LayerMask mask;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }

        if (Input.GetMouseButton(0) && mouseDown)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 position = transform.position;
            directionVector = (pos - position).normalized;

            renderer.startColor = Color.white;
            renderer.endColor = Color.white;
            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            
            RaycastHit2D hitInfo = Physics2D.Raycast(position, directionVector);
           
            hitPoint = hitInfo.point;
            
            angle = Vector2.Angle(directionVector, Vector2.right);
            
            renderer.SetPosition(0, position);
            renderer.SetPosition(1, hitInfo.point);
            renderer.SetPosition(2, position);

            if (hitInfo.transform.CompareTag("Boundary"))
            {
                int hitLayer = hitInfo.transform.gameObject.layer;
                if (hitLayer == 6) Debug.Log($"it hits {hitLayer}");
                if (hitLayer == 7) Debug.Log($"it hits {hitLayer}");
                
                
                
                int nextLayer = hitLayer == 6 ? 7 : 6;

                mask = 1 << nextLayer | 1 << 8 | 1 << 9;
                Debug.Log($"nextLayerMask = {mask.value}");

                bounceVector = new Vector2(-directionVector.x, directionVector.y);

                RaycastHit2D secondHit = Physics2D.Raycast(hitInfo.point, bounceVector, Mathf.Infinity, mask);

                secondHitPoint = secondHit.point;
                
                renderer.SetPosition(2, secondHitPoint);

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            renderer.SetPosition(0, transform.position);
            renderer.SetPosition(1, transform.position);
            renderer.SetPosition(2, transform.position);

            directionVector = Vector2.zero;
            bounceVector = Vector2.zero;
        }
    }
}
