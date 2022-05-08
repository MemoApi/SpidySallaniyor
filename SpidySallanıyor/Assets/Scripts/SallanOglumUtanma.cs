using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SallanOglumUtanma : MonoBehaviour
{
    // Components
    private SpringJoint2D sj;
    private LineRenderer lineRenderer;
    private Rigidbody2D rb;
    [SerializeField] private GameMenager gameMenager;
    [SerializeField] private GameObject deathEffect;
    

    [Header("Grapple Variables")]   
    [SerializeField] private LayerMask layerMask;
    private Vector3 mousePos = Vector3.zero;
    [Range(1, 50)]
    [SerializeField] private float grapleDistance = 10f;
    public Transform grapplePoint;
    private RaycastHit2D hit;
    private Vector2 rayDirect;

    [Header("Grapple Animation")]
    [Range(0, 2)]
    [SerializeField] private float animationDuration = 2f;

    private Vector3 checkPoint;

    
    


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        sj = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        checkPoint = new Vector3(0, 0, 0);
    }
    

    IEnumerator AnimateLine(bool isDead)
    {
        
        float startTime = Time.time;
        Vector3 startPos = lineRenderer.GetPosition(0);
        Vector3 endPos = lineRenderer.GetPosition(1);

        Vector3 pos = startPos;

        

        while(pos != endPos)
        {
            float t = (Time.time - startTime) / animationDuration;
            pos = Vector3.Lerp(startPos, endPos, t);
            lineRenderer.SetPosition(1, pos);
            yield return null;
        }
        if(isDead)
        {
            CharDied();           
        }
        
    }

    

    private void Update()
    {
        
        
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            float AngleRad = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x);
            float angle = (180 / Mathf.PI) * AngleRad - 90;

            rb.rotation = angle;


             
        if (Input.GetMouseButton(0))
        {

            if (Input.GetMouseButtonDown(0))
            {
                
                Vector3 mouseWzPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseWzPos);

                mousePos.z = 2;               

                rayDirect = (Vector2)mousePos - (Vector2)grapplePoint.position;
                hit = Physics2D.Raycast((Vector2)grapplePoint.position, rayDirect.normalized, grapleDistance, layerMask);

                if (hit) 
                    if (!IsPointerOverUIObject()) 
                        if(Time.timeScale!=0) FindObjectOfType<SoundManagerScript>().play("Grapple");

                
            }

            if (hit)
            {

                if (!IsPointerOverUIObject())
                {
                    float hookDistance = Vector2.Distance((Vector2)grapplePoint.position, (Vector2)hit.point);

                    sj.distance = Mathf.Abs(hookDistance - 1.3f);
                    sj.enabled = true;
                    sj.connectedAnchor = hit.point;

                    lineRenderer.enabled = true;

                    lineRenderer.SetPosition(0, grapplePoint.position);                  
                    lineRenderer.SetPosition(1, hit.point);

                    if (hit.transform.CompareTag("E_Wall"))
                    {/// Destroy character or damage
                        StartCoroutine(AnimateLine(true));
                        return;
                    }

                    StartCoroutine(AnimateLine(false));
                }
                
            }
            
        }
        else
        {
            sj.enabled = false;
            lineRenderer.enabled = false;         
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("E_Wall"))
        {
            CharDied();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("CheckPoint"))
        {
            checkPoint = collision.transform.position;
            Destroy(collision.gameObject,.1f);
            if(checkPoint==new Vector3(190,0,0))
            {
                gameMenager.OpenMenu();
            }
        }
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void CharDied()
    {
        GameObject d_Effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(d_Effect, .5f);
        sj.enabled = false;
        lineRenderer.enabled = false;
        hit = Physics2D.Raycast((Vector2)grapplePoint.position, Vector2.zero, 0, layerMask);        
        gameMenager.chardied(checkPoint);
    }


}
