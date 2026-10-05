using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject Pointer;
    NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Inputs();
    }

    private void Inputs()
    {
        //if (Input.GetMouseButtonDown(0)) // Left Mouse Button
        //{
        //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit rayHit;

        //    if (Physics.Raycast(ray, out rayHit))
        //    {
        //        Debug.Log("Mouse position: " + Input.mousePosition);
        //        navMeshAgent.SetDestination(rayHit.point);
        //        Pointer.transform.position = rayHit.point;
        //    }
        //}

        if (InputSystem.actions.FindAction("Click").IsPressed()) {
            Vector2 mousePos = InputSystem.actions.FindAction("Point").ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                //Debug.Log("Hit position: " + hit.point);
                navMeshAgent.SetDestination(hit.point);
                Pointer.transform.position = hit.point;
            }
        }
    }
}