using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptBig : MonoBehaviour
{
    public Animator _animator;
    AudioSource _audio;
    Rigidbody2D _rigidbody;
    Transform _transform;

    public LayerMask groundLayer;

    // player script
    public int direction = 1;
    public float moveSpeed;
    public int jumpHeight;
    public float slideTime;
    public float shootTime;

    // horizontal movement
    public Transform groundCheck;
    public Transform groundCheck2;
    private float moveTime = 0;

    // player slide
    bool cantStopSliding;
    public Transform slideCheck;
    public Transform slideCheck2;
    private float slideTimeLeft;
    private float slideSpeed;

    // player shoot
    public Lemon lemonScript;
    public Lemon lemonScript2;
    public Lemon lemonScript3;
    public GameObject lemon;
    public GameObject lemon2;
    public GameObject lemon3;
    private float shootTimeLeft;
    private float shootButtonHeldDown;
    public float lvl2HoldDownTime;
    public float lvl3HoldDownTime;
    int bulletLevel = 1;

    // player climb
    public Transform climbCheck;
    public Transform climbCheck2;
    public Transform climbCheck3;
    private RaycastHit2D hitInfo;
    private RaycastHit2D hitInfo2;
    private RaycastHit2D hitInfo3;
    public LayerMask ladderLayer;
    public GameObject ladder;
    private float currentGravity;
    public AudioClip shootSound;
    public AudioClip landSound;

    // player hurt
    private int playerLayer;
    private int itemLayer;

    public bool canMove;
    public bool isGrounded;
    public bool isMoving;
    public bool isRunning;
    public bool isSliding;
    public bool isIdleShooting;
    public bool isShooting;
    public bool canClimbUp;
    public bool isOnLadder;
    public bool isClimbing;
    public bool isAtTopOfLadder;
    public bool isAtFullHealth;

    private GameObject gameManager;
    private GameManager gameManagerScript;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        // _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        canMove = true;

        // player slide
        slideTimeLeft = slideTime;
        slideSpeed = moveSpeed * 2;

        // player shoot
        shootTimeLeft = shootTime;

        // player climb
        currentGravity = _rigidbody.gravityScale;

        // player hurt
        playerLayer = LayerMask.NameToLayer("Player");
        itemLayer = LayerMask.NameToLayer("Item");

        // player teleport in
        // _animator.SetTrigger("PlayerBeamIn");

        gameManager = GameObject.FindWithTag("MainCamera");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    void Update()
    {
        // horizontal movement and player jump
        GroundCheck();
        if (canMove && !isIdleShooting)
        {
            DirectionCheck();
            Moving();
            Jump();
        }

        // player slide
        PressSlideButton();
        SlideCheck();
        ContinuousSlide();

        // player shoot
        ShootCheck();
        Shooting();
        ChargeShot();

        // player climb
        CanClimb();
        StartClimbing();
        ClimbingOnLadder();
        JumpOffLadder();
        AtTopOfLadder();
        ShootingOnLadder();
        LadderGoingDown();

        // player hurt
        IgnoreItems();
        if (gameManagerScript.playerHealth <= 0)
        {
            canMove = false;
            _rigidbody.velocity = new Vector2(0, 0);
            _rigidbody.isKinematic = true;
        }

        // player beam in
        // BeamIn();

        // Debug.Log();
    }

    //////////////////////////////////////////////////////////////////////////////// horizontal movement
    void GroundCheck()
    {

        bool check1 = Physics2D.Linecast(_transform.position, groundCheck.position, groundLayer);
        bool check2 = Physics2D.Linecast(_transform.position, groundCheck2.position, groundLayer);

        if (!isGrounded && (check1 || check2))
        {
            _audio.PlayOneShot(landSound);
        }

        isGrounded = check1 || check2;

        _animator.SetBool("isGrounded", isGrounded);

    }

    void DirectionCheck()
    {
        Vector3 localScale = _transform.localScale;
        if ((Input.GetAxisRaw("Horizontal") == 1 && direction == -1) || (Input.GetAxisRaw("Horizontal") == -1 && direction == 1))
        {
            direction *= -1;
        }
        localScale.x = direction;
        _transform.localScale = localScale;
    }

    void Moving()
    {
        MoveCheck();
        RunCheck();
        Running();
    }

    void MoveCheck()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            isMoving = true;
            moveTime += Time.deltaTime;
        }
        else
        {
            isMoving = false;
            moveTime = 0;
        }

        _animator.SetBool("isMoving", isMoving);
    }

    void RunCheck()
    {
        if (moveTime > 0.085f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        _animator.SetBool("isRunning", isRunning);
    }

    void Running()
    {
        if (!isSliding)
        {
            if (isRunning || !isGrounded)
            {
                _rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, _rigidbody.velocity.y);
            }
            else
            {
                _rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * (moveSpeed * 0.2f), _rigidbody.velocity.y);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////// jump
    void Jump()
    {
        float vy = _rigidbody.velocity.y;
        if (Input.GetButtonDown("Jump") && isGrounded && Input.GetAxisRaw("Vertical") > -1 && !isSliding)
        {
            vy = 0f;
            _rigidbody.AddForce(new Vector2(0, jumpHeight));
        }

        if (Input.GetButtonUp("Jump") && vy > 0)
        {
            _rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, -1f);
        }
    }

    //////////////////////////////////////////////////////////////////////////////// slide
    void PressSlideButton()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && Input.GetAxisRaw("Vertical") == -1)
        {
            isSliding = true;
        }
        _animator.SetBool("isSliding", isSliding);
    }

    void SlideCheck()
    {
        if (Physics2D.Linecast(_transform.position, slideCheck.position, groundLayer) || Physics2D.Linecast(_transform.position, slideCheck2.position, groundLayer))
        {
            cantStopSliding = true;
        }
        else
        {
            cantStopSliding = false;
        }
    }

    void ContinuousSlide()
    {
        if (isSliding && slideTimeLeft > 0 || isSliding && cantStopSliding)
        {
            _rigidbody.velocity = new Vector2(direction * slideSpeed, _rigidbody.velocity.y);
            slideTimeLeft -= Time.deltaTime;
        }
        else if ((isSliding && slideTimeLeft < 0 && !cantStopSliding) || !isGrounded || isOnLadder)
        {
            isSliding = false;
            slideTimeLeft = slideTime;
        }
    }

    //////////////////////////////////////////////////////////////////////////////// shoot
    void ShootCheck()
    {
        if (Input.GetButtonDown("Fire1") && GameObject.FindGameObjectsWithTag("Bullet").Length < 2)
        {
            shootTimeLeft = shootTime;
            isShooting = true;
            Bullet();
        }
        else if (Input.GetButtonDown("Fire1") && GameObject.FindGameObjectsWithTag("Bullet").Length < 3)
        {
            isShooting = true;
            Bullet();
        }
        else if (Input.GetButton("Fire2") && GameObject.FindGameObjectsWithTag("Bullet").Length < 3)
        {
            isShooting = true;
            Bullet();
        }

        lemonScript.Flip(direction);
        lemonScript2.Flip(direction);
        lemonScript3.Flip(direction);

        if ((direction < 0 && lemon3.transform.localScale.x > 0) || (direction > 0 && lemon3.transform.localScale.x < 0))
        {
            Vector2 theScale = new Vector2();
            theScale = lemon3.transform.localScale;
            theScale.x *= -1;
            lemon.transform.localScale = theScale;
            lemon2.transform.localScale = theScale;
            lemon3.transform.localScale = theScale;
        }

        _animator.SetBool("isShooting", isShooting);
    }

    void Bullet()
    {
        _audio.PlayOneShot(shootSound);
        float bx = _transform.position.x + 1.5f;
        float by = _transform.position.y - 0.3f;

        if (isSliding)
        {
            bx += (1f * direction);
            by += 0.3f;
        }

        if (direction < 0)
        {
            bx -= 3f;
        }

        if (bulletLevel == 1)
        {
            GameObject b = Instantiate(lemon, new Vector3(bx, by, -1), Quaternion.identity) as GameObject;
        }
        else if (bulletLevel == 2)
        {
            GameObject b = Instantiate(lemon2, new Vector3(bx, by, -1), Quaternion.identity) as GameObject;
        }
        else
        {
            GameObject b = Instantiate(lemon3, new Vector3(bx, by, -1), Quaternion.identity) as GameObject;
        }


    }

    void Shooting()
    {
        if (isShooting && isGrounded && shootTimeLeft <= shootTime && Input.GetAxisRaw("Horizontal") == 0 && !isSliding && shootTimeLeft > 0)
        {
            isIdleShooting = true;
            shootTimeLeft -= Time.deltaTime;

            if (isShooting && isGrounded && Input.GetAxisRaw("Horizontal") == 0 && shootTimeLeft < 0)
            {
                isShooting = false;
                isIdleShooting = false;
                shootTimeLeft = shootTime;
            }
        }
        else if (isShooting && shootTimeLeft <= shootTime && shootTimeLeft > 0)
        {
            shootTimeLeft -= Time.deltaTime;

            if (isShooting && shootTimeLeft < 0)
            {
                isShooting = false;
                isIdleShooting = false;
                shootTimeLeft = shootTime;
            }
        }
    }

    void ChargeShot()
    {
        if (Input.GetButton("Fire1"))
        {
            shootButtonHeldDown += Time.deltaTime;
        }
        else if (Input.GetButtonUp("Fire1") && shootButtonHeldDown > lvl2HoldDownTime)
        {
            Bullet();
            isShooting = true;
        }
        else
        {
            shootButtonHeldDown = 0f;
        }

        if (shootButtonHeldDown > lvl2HoldDownTime && shootButtonHeldDown < lvl3HoldDownTime)
        {
            bulletLevel = 2;
        }
        else if (shootButtonHeldDown > lvl3HoldDownTime)
        {
            bulletLevel = 3;
        }
        else
        {
            bulletLevel = 1;
        }
    }

    //////////////////////////////////////////////////////////////////////////////// climb
    void CanClimb()
    {
        hitInfo = Physics2D.Raycast(climbCheck.position, Vector2.up, 1, ladderLayer);
        hitInfo2 = Physics2D.Raycast(climbCheck2.position, Vector2.up, 1, ladderLayer);
        hitInfo3 = Physics2D.Raycast(climbCheck3.position, Vector2.up, 1, ladderLayer);

        if (hitInfo)
        {
            canClimbUp = true;
            ladder = hitInfo.transform.gameObject;
        }
        else
        {
            canClimbUp = false;
            isOnLadder = false;
            isClimbing = false;
        }
    }

    void StartClimbing()
    {
        if (canClimbUp && Input.GetAxisRaw("Vertical") == 1)
        {
            isOnLadder = true;
            isGrounded = false;
        }
        else if (isOnLadder && isGrounded)
        {
            isOnLadder = false;
            isClimbing = false;
            _rigidbody.gravityScale = currentGravity;
        }
        _animator.SetBool("isOnLadder", isOnLadder);
    }

    void ClimbingOnLadder()
    {
        if (isOnLadder)
        {
            _rigidbody.transform.position = new Vector3(ladder.transform.position.x, _transform.position.y, _transform.position.z);
            _rigidbody.velocity = new Vector2(0, Input.GetAxisRaw("Vertical") * (moveSpeed - 1));
            _rigidbody.gravityScale = 0;

            if (Input.GetAxisRaw("Vertical") != 0 && !isGrounded)
            {
                isClimbing = true;
            }
            else
            {
                isClimbing = false;
            }

            if (!canClimbUp)
            {
                isOnLadder = false;
            }
        }
        else
        {
            _rigidbody.gravityScale = currentGravity;
        }

        _animator.SetBool("isClimbing", isClimbing);
    }

    void JumpOffLadder()
    {
        if (isOnLadder && Input.GetAxisRaw("Horizontal") != 0 && Input.GetButtonDown("Jump"))
        {
            isOnLadder = false;
            isClimbing = false;
        }
    }

    void AtTopOfLadder()
    {
        if (hitInfo && !hitInfo2)
        {
            isAtTopOfLadder = true;
        }
        else
        {
            isAtTopOfLadder = false;
        }
        _animator.SetBool("isAtTopOfLadder", isAtTopOfLadder);
    }

    void ShootingOnLadder()
    {
        if (isOnLadder && isShooting)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.constraints = ~RigidbodyConstraints2D.FreezePositionX | ~RigidbodyConstraints2D.FreezePositionY;
        }
        else if (_rigidbody.isKinematic == true && isOnLadder && !isShooting)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void LadderGoingDown()
    {
        if (hitInfo3 && !hitInfo && !hitInfo2 && isGrounded && Input.GetAxisRaw("Vertical") == -1)
        {
            _transform.position = new Vector3(_transform.position.x, _transform.position.y - 1, _transform.position.z);
            ladder = hitInfo3.transform.gameObject;
            isOnLadder = true;
            isGrounded = false;
            isAtTopOfLadder = true;
            _rigidbody.gravityScale = currentGravity;
        }
    }

    //////////////////////////////////////////////////////////////////////////////// hurt
    void IgnoreItems()
    {
        if (gameManagerScript.playerHealth == 28)
        {
            isAtFullHealth = true;
        }
        else
        {
            isAtFullHealth = false;
        }
        Physics2D.IgnoreLayerCollision(playerLayer, itemLayer, isAtFullHealth);
    }

    //////////////////////////////////////////////////////////////////////////////// beam in
    void BeamIn()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerTeleportBeam") || _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerTeleportSpawn"))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }
}
