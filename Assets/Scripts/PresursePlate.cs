using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresursePlate : MonoBehaviour
{
    [SerializeField] private Transform gate;          
    [SerializeField] private Transform plate;         
    [SerializeField] private float raisedHeight = 3f; 
    [SerializeField] private float platePressDepth = 0.2f;
    [SerializeField] private float moveSpeed = 2f;    

    private Vector3 initialGatePosition;              
    private Vector3 raisedGatePosition;               
    private Vector3 initialPlatePosition;             
    private Vector3 pressedPlatePosition;             
    private int objectsOnPlate = 0;                   
    private bool gateMovingUp = false;                

    private void Start()
    {
        
        initialGatePosition = gate.position;

        
        raisedGatePosition = new Vector3(initialGatePosition.x, initialGatePosition.y + raisedHeight, initialGatePosition.z);

        
        initialPlatePosition = plate.position;
        pressedPlatePosition = new Vector3(initialPlatePosition.x, initialPlatePosition.y - platePressDepth, initialPlatePosition.z);
    }

    private void Update()
    {
       
        if (gateMovingUp && gate.position.y != raisedGatePosition.y)
        {
            gate.position = Vector3.MoveTowards(gate.position, raisedGatePosition, moveSpeed * Time.deltaTime);
        }
        else if (!gateMovingUp && gate.position.y != initialGatePosition.y)
        {
            gate.position = Vector3.MoveTowards(gate.position, initialGatePosition, moveSpeed * Time.deltaTime);
        }

        
        if (gateMovingUp && plate.position != pressedPlatePosition)
        {
            plate.position = Vector3.MoveTowards(plate.position, pressedPlatePosition, moveSpeed * Time.deltaTime);
        }
        else if (!gateMovingUp && plate.position != initialPlatePosition)
        {
            plate.position = Vector3.MoveTowards(plate.position, initialPlatePosition, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") || collision.CompareTag("Player"))
        {
            objectsOnPlate++;
            gateMovingUp = true;  
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") || collision.CompareTag("Player"))
        {
            objectsOnPlate--;
            if (objectsOnPlate == 0)
            {
                gateMovingUp = false;  
            }
        }
    }
}
