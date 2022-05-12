using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Transform[] patrolPath;
    private int currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = Random.Range(0, patrolPath.Length);
        transform.position = patrolPath[currentPosition].position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(patrolPath[currentPosition]);
        transform.position = Vector3.MoveTowards(transform.position, patrolPath[currentPosition].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, patrolPath[currentPosition].position) < 0.2f)
        {
            currentPosition = Random.Range(0, patrolPath.Length);
        }
    }
}
