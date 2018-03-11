using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeControll : MonoBehaviour
{
    private GameObject clawGameObject;

    public float moveSpeed = 5f;
    public float clawDistance = 2.5f;
    public float clawAttackTime = 0.2f;
    private bool clawAttack = false;
    private float attackTimeHolde = 0;

    private Rigidbody2D companionRigidbody;
    private CompanionController companionController;


    // Use this for initialization
    void Start()
    {
        companionController = GetComponent<CompanionController>();
        companionRigidbody = GetComponent<Rigidbody2D>();
        clawGameObject = transform.GetChild(0).gameObject;
    }

    //With a lack of better name. Run() sounds like a thread.
    public void RunExecute()
    {
        MoveByAxis();
        Combat();
    }

    public void UndoTakeover()
    {
        clawGameObject.SetActive(false);
        attackTimeHolde = 0;
        clawAttack = false;
    }

    //Move function for the companion
    public void MoveByAxis()
    {
        float vmov = Input.GetAxis("Vertical");
        float hmov = Input.GetAxis("Horizontal");

        Vector2 heading = new Vector2(hmov, vmov);

        Vector2 nextPosition = (Vector2)transform.position + (heading * moveSpeed * Time.deltaTime);
        //Do we need a check?
        companionRigidbody.MovePosition(nextPosition);

        companionController.UpdateAnimation(heading.normalized);
    }


    /*** Attacking code for the companion
     * Gets a hitbox child of the companion
     * Sets it to active for 2 seconds then sets it inactive again
    */
    public void Combat()
    {
        //Set claw angle child heading to mouse cursor.
        Vector2 cursorScreenPos = Input.mousePosition;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);

        //Debug only
        Vector2 posNormalCursor = (cursorPos.normalized * 3) + (Vector2)transform.position;
        Debug.DrawLine(transform.position, posNormalCursor, Color.red);

        Vector2 heading = (cursorPos - (Vector2)transform.position).normalized;
        //Debug.Log(heading);

        //Comanion attack = Space atm.
        if (Input.GetAxisRaw("CompanionAttack") == 1 && clawAttack == false)
        {
            clawGameObject.transform.localPosition = (heading * clawDistance);
            clawAttack = true;
            clawGameObject.SetActive(true);
        }
        if (attackTimeHolde < clawAttackTime && clawAttack)
        {
            //Play animation?

            if (attackTimeHolde < clawAttackTime)
            {
                attackTimeHolde += Time.deltaTime;
            }
        }
        else if (attackTimeHolde > clawAttackTime)
        {
            clawGameObject.SetActive(false);
            attackTimeHolde = 0;
            clawAttack = false;
        }

        
    }
}
