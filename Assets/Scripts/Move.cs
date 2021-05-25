using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private float moveSpeed;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        moveSpeed = 1f / moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator SmoothMove(Vector2 end)
    {
        float distance = Vector2.Distance(rb2D.position, end);
        while(distance > float.Epsilon)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb2D.position, end, moveSpeed * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            distance = Vector2.Distance(rb2D.position, end);
            yield return null;
        }
    }

    protected bool Moving(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if(hit.transform == null)
        {
            //se mueve
            StartCoroutine(SmoothMove(end));

            return true;
        }
        return false;
    }

    protected abstract void OnCantMove(GameObject go);

    protected virtual void AttemptMove(int xDir, int yDir)
    {
        RaycastHit2D hit;
        bool canMove = Moving(xDir, yDir, out hit);
        if (canMove) return;

        OnCantMove(hit.transform.gameObject);
    }
}
