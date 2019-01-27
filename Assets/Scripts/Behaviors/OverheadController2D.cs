﻿/*
 * PlatformerController2D is intended to be a generic physics controller for
 * use in platformers without using the built in unity physics engine (instead
 * rays are used to check for collisions).
 * 
 * Other scripts (for example, and input handling script in the case of players)
 * should utilize this controller primarily through the setActiveX/YVel
 * functions, where are for use with continuous input, or the addForceX/YVel
 * functions, which are for one-time additions of velocity. Additionally, the 
 * Jump and Attack functions can be used as expected. Attack will simply spawn
 * the prefab specified in the Generic Attack field in the inspector (for an
 * example of this, check Assets/Prefabs/Items/GenericAttack).
 * 
 * There are also a series of modifiers for the physics of the gameobject, such
 * as gravity, jumpVelocity, xDrag (friction) and xBounceFactor (amount the
 * the asset bounces off walls).
 * 
 * To fully utilize the animation transitions, an Animator should be set up with
 * at least three states:
 *   * Idle
 *   * Moving
 *   * Jumping
 * 
 * Animator parameters isJumping and runVelocity must be set up to handle
 * transitions between these.
 * 
 * Current physics default values are set for mid-size (think 16-bit era) pixel
 * art sprites at 1 PPU. At some point I'd like to dynamically set this value 
 * based on asset sizes, but for the time being, if you add this to a gameobject
 * and it's not responding as expected, the physics settings likely need to be
 * tweaked to better values for your sprites/PPU settings.
 * 
 * In order for collisions to work correctly, either the gameobject will need
 * to be set to the IgnoreRaycase layer, or probably the better option is to set
 * per-project settings of Edit -> Project Settings -> Queries start in 
 * colliders to "off".
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class OverheadController2D : MonoBehaviour
{
    public GameObject genericAttack;

    public float activeXVel = 0.0f;
    public float activeYVel = 0.0f;

    public float skinWidth = .03f;
    public float speed = 50f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public float maxClimbAngle = 50f;
    public float xDrag = 10f;
    public float xBounceFactor = 0.0f;
    public float attackTimeout = 1f;
    public bool makeAttackChild = false;
    public bool blockEnemies = false;

    private bool isEnabled;
    private int framesHorizontalCol = 0;
    private int framesVerticalCol = 0;

    bool canAttack = true;
    float timeSinceAttack = 0;
    float horizontalRaySpacing;
    float horizontalSpeed;
    float verticalSpeed;
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
            CheckAttackTimeout();
            MoveSprite();
            UpdateRaycastOrigins();
            UpdateAnimator();
            UpdateVelocity();
            checkVerticalCollisions();
            checkHorizontalCollisions();
        }
    }

    /*
    * Enables all functions called in the Update method. 
    */
    public void enable()
    {
        isEnabled = true;
    }


    /*
    * Disables all functions in the Update method. Note, this may
    * not fully freeze all functionality (for example, if velocity is not
    * zeroed out).
    */
    public void disable()
    {
        isEnabled = false;
    }



    /*
     * One-time add of force to x-axis velocity. Example of use would be a jump.
     */
    public void addForceXVel(float addvel)
    {
        activeXVel += addvel;
    }


    /*
    * Sets force on x-axis intended to be used as the basis of the velocity 
    * each update (likely a range from -1 to 1). This is multiplied by speed
    * and adjusted by other factors, and can be directly fed the x-axis from
    * controller/button input
    */
    public void setActiveXVel(float newvel)
    {
        activeXVel = newvel;
    }


    /*
    * One-time add of force to y-axis velocity. Example of use would be a jump.
    */
    public void addForceYVel(float addvel)
    {
        activeYVel += addvel;
    }


    /*
    * Sets force on y-axis intended to be used as the basis of the velocity 
    * each update (likely a range from -1 to 1). This is multiplied by speed
    * and adjusted by other factors, and can be directly fed the y-axis from
    * controller/button input
    */
    public void setActiveYVel(float newvel)
    {
        activeYVel = newvel;
    }


    /*
     * Spawns prefab to attack in front of the sprite. Although "attacks" could
     * take a number for forms, the intended approach for this generic attack
     * is for the prefab to have a collider that checks for collisions with
     * objects of a certain tag (for example, "Enemy"), then removes health
     * from that object.
     */
    public void Attack(string attackDir = "up") {
        float xPadding = 0f;
        float yPadding = 0f;
        float xShotSpeed = 0f;
        float yShotSpeed = 0f;

        if (attackDir == "up")
        {
            yPadding = 8f;
            yShotSpeed = 2f;
        }
        else if (attackDir == "down") {
            yPadding = -8f;
            yShotSpeed = -2f;
        } else if (attackDir == "left") {
            xPadding = -8f;
            xShotSpeed = -2f;
        } else if (attackDir == "right") {
            xPadding = 8f;
            xShotSpeed = 2f;
        }

        if (canAttack)
        {
            if (genericAttack != null)
            {
                canAttack = false;
                GameObject attackObject = Instantiate(genericAttack, CalculateItemSpawnLocation(xPadding, yPadding), Quaternion.identity);
                attackObject.GetComponent<ParentAudioShot>().xSpeed = xShotSpeed;
                attackObject.GetComponent<ParentAudioShot>().ySpeed = yShotSpeed;

                if (makeAttackChild)
                {
                    attackObject.transform.SetParent(transform, true);
                }
            }
            else
            {
                Debug.Log("Can't attack. Generic attack hasn't been set in PlatfomerController2d.");
            }
        }
    }


    void CheckAttackTimeout() {
        if (canAttack == false)
        {
            timeSinceAttack += Time.deltaTime;
            if (timeSinceAttack > attackTimeout) {
                canAttack = true;
                timeSinceAttack = 0f;
            }
        }
    }

    void checkVerticalCollisions()
    {
        Vector2 rayDirectionVert, rayOriginVert;

        float rayDistanceVert;
        RaycastHit2D hit ;
        bool hadVertHit = false;


        rayDirectionVert = (velocity.y <= 0 ? Vector2.down : Vector2.up);
        rayOriginVert = (velocity.y <= 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft);
        rayDistanceVert = (Mathf.Abs(velocity.y) + skinWidth) * rayDirectionVert.y;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(rayOriginVert + Vector2.right * verticalRaySpacing * i, rayDirectionVert * Mathf.Abs(rayDistanceVert), Color.red);
            hit = Physics2D.Raycast(rayOriginVert + Vector2.right * verticalRaySpacing * i, rayDirectionVert, Mathf.Abs(rayDistanceVert));
            if (hit)
            {
                //tracks how many continuous cycles we've had vertical collisions for.
                //This isn't actually used for anything, but could hypothecally be used
                //for vertical bounces as we currently have for horizontal bounces.
                hadVertHit = true;
                if ((hit.collider.gameObject.tag == "platform") ||
                    ((hit.collider.gameObject.tag == "passthrough_platform") && (rayDirectionVert == Vector2.down)) ||
                    ((hit.collider.gameObject.tag == "Enemy") && (blockEnemies == true)))
                {
                    velocity.y = (hit.distance - skinWidth) * rayDirectionVert.y;
                    rayDistanceVert = hit.distance;
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
                //Tracks how many contiguous cycles we've had horizontal collisions. 
                //This is used to handle horizontal bounces (the only happen if there's
                //no active x velocity on the first time a wall is hit).
                hadHorizHit = true;
                if ((hit.collider.gameObject.tag == "platform") ||
                   ((hit.collider.gameObject.tag == "Enemy") && (blockEnemies == true)))
                {
                    {
                        if ((i == 0) && (Vector2.Angle(hit.normal, Vector2.up) < maxClimbAngle))
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


    /*
     * Adjusts the angle of the velocity according to the normal of the slope
     * we're colliding with, so the object "climbs" the slope.
     */
    void slopeAdjustment(Vector2 slopeNormal)
    {
        float slopeAngle = Vector2.Angle(slopeNormal, Vector2.up);
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * velocity.x * Mathf.Sign(velocity.x);
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * velocity.x;

    }


    /*
     * UpdateVelocity takes the active x and y velocity (likely from input), and
     * uses the various modifiers like gravity, drag and bounces to calculate a
     * new velocity.
     */
    void UpdateVelocity()
    {

        velocity.y *= Time.timeScale;
        velocity.x *= Time.timeScale;

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
                if (horizontalSpeed < 0.0f) {
                    horizontalSpeed = 0.0f;
                }
                velocity.x = horizontalSpeed * Time.deltaTime * Mathf.Sign(velocity.x);
                if (framesHorizontalCol == 1) {
                    velocity.x *= -1;
                }
            }
        }

        if ((activeYVel < -.3f) || (activeYVel > .3f))
        {
            verticalSpeed = speed;
            velocity.y = activeYVel * verticalSpeed * Time.deltaTime;
        }
        else
        {
            // if we've gotten pretty slow, just stop. Otherwise, reduce with drag
            if (verticalSpeed < .5f || (framesVerticalCol > 1))
            {
                velocity.y = 0.0f;
            }
            else
            {
                verticalSpeed -= (Time.deltaTime * xDrag);
                if (verticalSpeed < 0.0f)
                {
                    verticalSpeed = 0.0f;
                }
                velocity.y = verticalSpeed * Time.deltaTime * Mathf.Sign(velocity.y);
                if (framesVerticalCol == 1)
                {
                    velocity.y *= -1;
                }
            }
        }

        //reset activeYVel to 0f. This needs to be re-asserted each cycle.
        activeXVel = 0.0f;
        activeYVel = 0.0f;

    }


    void UpdateAnimator() {
        if (myAnimator)
        {
            myAnimator.SetFloat("ActiveYVel", activeYVel);
            myAnimator.SetFloat("ActiveXVel", Mathf.Abs(activeXVel));
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


    /*
     * Takes in to account sprite direction and size to pick a good place to 
     * spawn used "items" in front of the object.
     */
    Vector3 CalculateItemSpawnLocation(float xPadding = 0f, float yPadding = 8f, bool padColliderX = false, bool padColliderY = false) {
        float collPadX = 0f;
        float collPadY = 0f;

        if (padColliderX) {
            collPadX = (myRenderer.size.x / 2);
        }

        if (padColliderX)
        {
            collPadY = (myRenderer.size.y / 2);
        }

        float itemLocationX = (collPadX + xPadding) + myCollider.bounds.center.x;
        float itemLocationY = (collPadY + yPadding) + myCollider.bounds.center.y;
        return new Vector3(itemLocationX, itemLocationY, -1);

    }


    void resizeColliderY(float newScale=1f) {
        Debug.Log("old offset" + myCollider.offset);
        Debug.Log("old size " + myCollider.size);

        float newSizeY = myCollider.size.y * newScale;
        float newOffsetY = (myCollider.size.y - newSizeY) / 2;

        myCollider.size = new Vector2(myCollider.size.x, newSizeY);
        myCollider.offset = new Vector2(myCollider.offset.x, myCollider.offset.y - newOffsetY);

        Debug.Log("new offset" + myCollider.offset);
        Debug.Log("new size " + myCollider.size);
    }
}
