using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlatformerController2D : MonoBehaviour
{
    public GameObject genericAttack;

    public float activeXVel = 0.0f;
    public float activeYVel = 0.0f;

    public float gravity = -.05f;
    public float skinWidth = .03f;
    public float speed = 50f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public float jumpVelocity = .2f;
    public float maxClimbAngle = 50f;
    public float xDrag = 10f;
    public float xBounceFactor = 0.0f;

    private bool isEnabled;
    private int framesHorizontalCol = 0;
    private int framesVerticalCol = 0;

    bool canJump = false;
    float horizontalRaySpacing;
    float horizontalSpeed;
    float verticalRaySpacing;

    BoxCollider2D myCollider;
    Animator myAnimator;
    SpriteRenderer myRenderer;

    Vector2 velocity;

    RaycastOrigins raycastOrigins;

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }


    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();

        CalculateRaySpacing();
        velocity = Vector2.zero;
        isEnabled = true;
    }


    void Update()
    {
        if (isEnabled)
        {
            MoveSprite();
            UpdateRaycastOrigins();
            UpdateVelocity();
            UpdateAnimator();
            checkVerticalCollisions();
            checkHorizontalCollisions();
        }
    }


    public void enable()
    {
        isEnabled = true;
    }


    public void disable()
    {
        isEnabled = false;
    }


    public void addForceXVel(float addvel)
    {
        activeXVel += addvel;
    }


    public void setActiveXVel(float newvel)
    {
        activeXVel = newvel;
    }


    public void addForceYVel(float addvel)
    {
        activeYVel += addvel;
    }


    public void setActiveYVel(float newvel)
    {
        activeYVel = newvel;
    }


    public void Jump(){
        if (canJump)
        {
            velocity.y += jumpVelocity;
            myAnimator.SetBool("isJumping", true);
            canJump = false;
        }
    }


    public void Attack() {
        if (genericAttack != null) {
            GameObject attackObject = Instantiate(genericAttack, CalculateItemSpawnLocation(), Quaternion.identity);
            attackObject.GetComponent<SpriteRenderer>().flipX = myRenderer.flipX;
        } else {
            Debug.Log("Can't attack. Generic attack hasn't been set in PlatfomerController2d.");
        }
    }


    void checkVerticalCollisions()
    {
        Vector2 rayDirectionVert, rayOriginVert;

        float rayDistanceVert;
        RaycastHit2D hit ;
        bool hadVertHit = false;

        //check vertical collisions
        rayDirectionVert = (velocity.y <= 0 ? Vector2.down : Vector2.up);
        rayOriginVert = (velocity.y <= 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft);
        rayDistanceVert = (Mathf.Abs(velocity.y) + skinWidth) * rayDirectionVert.y;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(rayOriginVert + Vector2.right * verticalRaySpacing * i, rayDirectionVert * Mathf.Abs(rayDistanceVert), Color.red);
            hit = Physics2D.Raycast(rayOriginVert + Vector2.right * verticalRaySpacing * i, rayDirectionVert, Mathf.Abs(rayDistanceVert));
            if (hit)
            {
                hadVertHit = true;
                if (hit.collider.gameObject.tag == "platform")
                {
                    velocity.y = (hit.distance - skinWidth) * rayDirectionVert.y;
                    rayDistanceVert = hit.distance;

                    //set canJump if we're moving down and there are vertical hits
                    if (rayDirectionVert == Vector2.down)
                    {
                        canJump = true;
                        if (myAnimator)
                        {
                            myAnimator.SetBool("isJumping", false);
                        }
                    }
                }
            }
        }

        if (hadVertHit) {
            framesVerticalCol++;
            framesVerticalCol = Mathf.Min(framesVerticalCol, int.MaxValue - 1);
        } else {
            framesVerticalCol = 0;
        }
    }


    void checkHorizontalCollisions()
    {
        Vector2 rayDirectionHoriz, rayOriginHoriz;
        float rayDistanceHoriz;
        RaycastHit2D hit;
        bool hadHorizHit = false;

        //check horizontal collisions 
        rayDirectionHoriz = (velocity.x <= 0 ? Vector2.left : Vector2.right);
        rayOriginHoriz = (velocity.x <= 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
        rayDistanceHoriz = (Mathf.Abs(velocity.x) + skinWidth) * rayDirectionHoriz.x;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Debug.DrawRay(rayOriginHoriz + Vector2.up * horizontalRaySpacing * i, rayDirectionHoriz * Mathf.Abs(rayDistanceHoriz), Color.red);
            hit = Physics2D.Raycast(rayOriginHoriz + Vector2.up * horizontalRaySpacing * i, rayDirectionHoriz, Mathf.Abs(rayDistanceHoriz));

            if (hit)
            {
                hadHorizHit = true;
                if (hit.collider.gameObject.tag == "platform")
                {
                    {
                        if ((i == 0) && (Vector2.Angle(hit.normal, Vector2.up) < maxClimbAngle) && canJump)
                        {
                            //adjust for slope
                            slopeAdjustment(hit.normal);
                        }
                        else
                        {
                            velocity.x = (hit.distance - skinWidth) * rayDirectionHoriz.x;
                        }
                    }
                }
            }
        }

        if (hadHorizHit)
        {
            framesHorizontalCol++;
            framesHorizontalCol = Mathf.Min(framesHorizontalCol, int.MaxValue - 1);
        }
        else
        {
            framesHorizontalCol = 0;
        }
    }


    void slopeAdjustment(Vector2 slopeNormal)
    {
        float slopeAngle = Vector2.Angle(slopeNormal, Vector2.up);
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * velocity.x * Mathf.Sign(velocity.x);
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * velocity.x;

    }


    void UpdateVelocity()
    {

        //handle gravity
        velocity.y += gravity * Time.deltaTime;
        velocity.y *= Time.timeScale;

        if ((activeXVel < -.3f) || (activeXVel > .3f))
        {
            horizontalSpeed = speed;
            velocity.x = activeXVel * horizontalSpeed * Time.deltaTime;
        }
        else
        {
            // if we've gotten pretty slow, just stop. Otherwise, reduce with drag
            if (horizontalSpeed < .5f || (framesHorizontalCol > 1))
            {
                velocity.x = 0.0f;
            }
            else
            {
                horizontalSpeed -= (Time.deltaTime * xDrag);
                velocity.x = horizontalSpeed * Time.deltaTime * Mathf.Sign(velocity.x);
                if (framesHorizontalCol == 1) {
                    velocity.x *= -1;
                }
            }
        }
        activeXVel = 0.0f;
    }


    void UpdateAnimator() {
        if (myAnimator)
        {
            myAnimator.SetFloat("runVelocity", Mathf.Abs(velocity.x));
        }
        //Figure out whether the sprite should face left or right.
        //No velocity should just continue facing whatever direction we were.
        if (velocity.x < 0.0)
        {
            myRenderer.flipX = true;
        }
        else if (velocity.x > 0.0)
        {
            myRenderer.flipX = false;
        }
    }


    void MoveSprite()
    {
        transform.Translate(velocity);
    }


    void UpdateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }


    void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }


    Vector3 CalculateItemSpawnLocation(float xPadding = 8f) {
        float itemLocationX = ((myRenderer.size.x / 2) + xPadding) * Mathf.Sign(velocity.x) + transform.position.x;
        return new Vector3(itemLocationX, transform.position.y, -1);

    }
}
