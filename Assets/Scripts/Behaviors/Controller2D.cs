using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{

    public float gravity = -.5f;
    public float skinWidth = .03f;
    public float speed = 3f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public float jumpVelocity = .2f;
    public float maxClimbAngle = 50f;
    public float xDrag = 10f;

    bool canJump = false;
    bool xCollided = false;
    float horizontalRaySpacing;
    float horizontalSpeed;
    float verticalRaySpacing;


    BoxCollider2D myCollider;
    Animator myAnimator;
    SpriteRenderer myRenderer;
    Item heldItem;

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
    }

    public void movement(float inputX, bool jumping)
    {
        MoveSprite();
        UpdateRaycastOrigins();
        UpdateVelocity(inputX, jumping);
        checkVerticalCollisions();
        checkHorizontalCollisions();
    }

    void checkVerticalCollisions() {
        Vector2 rayDirectionVert, rayOriginVert;

        float rayDistanceVert;
        RaycastHit2D hit;

        //check vertical collisions
        rayDirectionVert = (velocity.y <= 0 ? Vector2.down :  Vector2.up);
        rayOriginVert = (velocity.y <= 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft);
        rayDistanceVert = (Mathf.Abs(velocity.y) + skinWidth) * rayDirectionVert.y;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(rayOriginVert + Vector2.right * verticalRaySpacing * i, rayDirectionVert * Mathf.Abs(rayDistanceVert), Color.red);
            hit = Physics2D.Raycast(rayOriginVert + Vector2.right * verticalRaySpacing * i, rayDirectionVert, Mathf.Abs(rayDistanceVert));
            if (hit)
            {
                Debug.Log("Got a hit!");
                if (hit.collider.gameObject.tag == "platform" || 
                    ((hit.collider.gameObject.tag == "passthrough_platform") && (rayDirectionVert == Vector2.down)))
                {
                    velocity.y = (hit.distance - skinWidth) * rayDirectionVert.y;
                    rayDistanceVert = hit.distance;

                    //set canJump if we're moving down and there are vertical hits
                    if (rayDirectionVert == Vector2.down)
                    {
                        canJump = true;
                        myAnimator.SetBool("isJumping", false);
                    }
                }
            }
        }
    }

    void checkHorizontalCollisions()
    {
        Vector2 rayDirectionHoriz, rayOriginHoriz;
        float rayDistanceHoriz;
        RaycastHit2D hit;

        //set needed values
        rayDirectionHoriz = (velocity.x <= 0 ? Vector2.left : Vector2.right);
        rayOriginHoriz = (velocity.x <= 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
        rayDistanceHoriz = (Mathf.Abs(velocity.x) + skinWidth) * rayDirectionHoriz.x;

        //iterate through rays looking for a collision
        for (int i = 0; i < horizontalRayCount; i++)
            {
            Debug.DrawRay(rayOriginHoriz + Vector2.up * horizontalRaySpacing * i, rayDirectionHoriz * Mathf.Abs(rayDistanceHoriz), Color.red);
            hit = Physics2D.Raycast(rayOriginHoriz + Vector2.up * horizontalRaySpacing * i, rayDirectionHoriz, Mathf.Abs(rayDistanceHoriz)) ;

            if (hit)
            {
                xCollided = true;
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
            } else {
                xCollided = false;
            }
        }
    }



    void slopeAdjustment(Vector2 slopeNormal) {
        float slopeAngle = Vector2.Angle(slopeNormal, Vector2.up);
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * velocity.x * Mathf.Sign(velocity.x);
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * velocity.x;

    }


    void UpdateVelocity(float inputX, bool jumping) {
        velocity.y += gravity * Time.deltaTime;
        if (jumping) {
            Debug.Log("in jumping, canjump is " + canJump);
        }
        if (jumping && canJump) {
            Debug.Log("Trying to jump. Velocity is " + velocity.ToString("F4"));
            velocity.y += jumpVelocity;
            myAnimator.SetBool("isJumping", true);
            canJump = false;
            Debug.Log("After jump. Velocity is " + velocity.ToString("F4"));

        }

        //Below is code for drag after left/right input are stopped
        if ((inputX < -.3f) || (inputX > .3f) || xCollided)
        {
            horizontalSpeed = speed;
            velocity.x = inputX * horizontalSpeed * Time.deltaTime;
        } else {
            // if we've gotten pretty slow, just stop. Otherwise, reduce with drag
            if (horizontalSpeed < .5f) {
                velocity.x = 0.0f;
            } else {
                //horizontalSpeed -= (Time.deltaTime * xDrag);
                horizontalSpeed = 0;
                velocity.x = horizontalSpeed * Time.deltaTime * Mathf.Sign(velocity.x);
            }
        }
        myAnimator.SetFloat("runVelocity", Mathf.Abs(velocity.x));

        //Figure out whether the sprite should face left or right.
        //No velocity should just continue facing whatever direction we were.
        if (velocity.x < 0.0) {
            myRenderer.flipX = true;
        } else if (velocity.x > 0.0) {
            myRenderer.flipX = false;
        }
    }

    void MoveSprite() {
        //Debug.Log("MOVING velocity is " + velocity.ToString("F4"));
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


    public bool grabItem(){
        Bounds bounds = myCollider.bounds;
        float length = bounds.size.x * 2.0f;

        int grabRayCount = 8;
        float raySpacing = bounds.size.y / grabRayCount;

        //project the rays and see if we get a hit
        Vector2 rayOriginHoriz, rayDirectionHoriz;
        RaycastHit2D hit;
        rayOriginHoriz = (velocity.x <= 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
        rayDirectionHoriz = (velocity.x <= 0 ? Vector2.left : Vector2.right);

        for (int i = 0; i < grabRayCount; i++) {
            hit = Physics2D.Raycast(rayOriginHoriz + Vector2.up * raySpacing * i, rayDirectionHoriz, length);

            if (hit) {
                if (hit.collider.gameObject.tag == "item") {
                    Debug.Log("got an item!");
                    heldItem = hit.collider.gameObject.GetComponent<Item>();
                    heldItem.grabItem(transform);
                    return true;
                }
            }
        }
        return false;
    }


    public void throwItem() {
        Vector2 dir;
        if (myRenderer.flipX) {
            dir = Vector2.left;
        } else {
            dir = Vector2.right;
        }
        heldItem.throwItem(dir);
    }
}
