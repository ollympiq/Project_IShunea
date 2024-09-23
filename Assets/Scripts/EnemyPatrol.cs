using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;
    [SerializeField] private float idleDuration;
    private float idleTimer;
    [SerializeField] private Animator anim;
    private void Awake()
    {
        initScale = enemy.localScale;
        movingLeft = true; 
        
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1); 
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1); 
            else
                DirectionChange();
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving",true);
        
        if (_direction == -1)
        {
            
            enemy.localScale = new Vector3(4, initScale.y, initScale.z);
        }
        else
        {
            
            enemy.localScale = new Vector3(-4, initScale.y, initScale.z);
        }

        
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
                                     enemy.position.y, enemy.position.z);
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;
     
        if (idleTimer > idleDuration)
            movingLeft = !movingLeft; 
    }

    private void OnDisable()
    {
        anim.SetBool("moving",false);
    }
}
