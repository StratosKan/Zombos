using UnityEngine;

public class HeadbobScript : MonoBehaviour
{
    //THIS SCRIPT IS TEMPORARY, IT IS COPY/PASTE FROM PREVIOUS PROJECT
    [Header("TEMPORARY SCRIPT")]
    public float bobbingSpeedWalk = 0.3f;
    public float bobbingSpeedRun = 0.6f;
    public float bobbingAmountDefault = 0.035f;
    public float bobbingAmountAimed = 0.055f;

    private float midpointY = 0f;
    private float midpointX = 0f;
    private float timer = 0.0f;
    private Vector3 headpos;
    private float targetBobAmount;

    private InputManager input;
    private Stats_Manager stats;
    private PlayerMovementScript playerMovement;

    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
        stats = GameObject.FindGameObjectWithTag("Manager").GetComponent<Stats_Manager>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
        headpos = transform.localPosition;
        midpointY = transform.localPosition.y;
        midpointX = transform.localPosition.x;
    }

    void FixedUpdate()
    {
        if (playerMovement.isPlayerGrounded())
        {
            if (input.MouseAimHold)
            {
                targetBobAmount = bobbingAmountAimed;
            }
            else
            {
                targetBobAmount = bobbingAmountDefault;
            }

            float waveslice = 0.0f;
            float horizontal = input.Horizontal;
            float vertical = input.Vertical;

            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(timer);
                if (input.IsRunning && stats.GetPlayerStamina() > 0f)
                {
                    timer = timer + bobbingSpeedRun;
                }
                else
                {
                    timer = timer + bobbingSpeedWalk;
                }
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }
            }
            if (waveslice != 0)
            {
                float translateChange = waveslice * targetBobAmount;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, -1.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                headpos.y = midpointY + translateChange;
                headpos.x = midpointX + translateChange;
            }
            else
            {
                Mathf.Lerp(headpos.y, midpointY, 1f);
                Mathf.Lerp(headpos.x, -midpointX, 1f);
            }
            transform.localPosition = headpos;
        }   
    }
}