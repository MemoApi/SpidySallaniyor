using UnityEngine;

public class MovingWalls : MonoBehaviour
{

    [SerializeField] private Transform one, two;
    [SerializeField] private float speed;
    private Vector2 target;

    private void Start()
    {
        target = one.position;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        
        if(transform.position == one.position)
        {
            target = two.position;
        }
        if(transform.position == two.position)
        {
            target = one.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, target,step);

    }
}
