
/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private float jumpForce = 40f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private float crouchHeight = 0.8f;

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;

    private DatabaseReference dbReference;
    private float firebaseValue = 0f;
    private Animator anim;
    private bool run = false;
    private void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Set up the listener for real-time database changes
            dbReference.Child("test/float").ValueChanged += HandleValueChanged;
        });
        anim = GetComponentInChildren<Animator> ();
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null && args.Snapshot.Value != null)
        {
            firebaseValue = float.Parse(args.Snapshot.Value.ToString());
            print(firebaseValue);
        }
    }

    private void Update()
    {
        if(GameManager.Instance.isPlaying == true && run==false){
            anim.SetBool("isRunning", true);
            run = true;
        }

        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        #region JUMPING
        if (isGrounded && firebaseValue != 1f)
        {
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;

            if (jumpTimer < jumpTime)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
            isJumping = false;
            jumpTimer = 0;
        }   

        #endregion

        #region CROUCHING
        if (isGrounded && firebaseValue < 0f)
        {
            GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z);
            if (isJumping)
            {
                GFX.localScale = new Vector3(GFX.localScale.x, 0.1139818f, GFX.localScale.z);
            }
        }

        if (firebaseValue < 0f)
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 0.1139818f, GFX.localScale.z);
        }
        #endregion
    }
} */
//////////////////////////////////////////////////
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;

    private DatabaseReference dbReference;
    public float firebaseValue = 0f;

    private Animator anim;
    [SerializeField] public AudioSource jumpSound;
    public Image EMGBar;
    public static PlayerMovement instance;

    void Awake()
    {
      instance = this;
    }
    private void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Set up the listener for real-time database changes
            dbReference.Child("test/float").ValueChanged += HandleValueChanged;
        });
        anim = GetComponentInChildren<Animator>();
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null && args.Snapshot.Value != null)
        {
            firebaseValue = float.Parse(args.Snapshot.Value.ToString());
            EMGBar.fillAmount = firebaseValue/3;
            print(firebaseValue);
        }
    }
    private void Update()
    {
        if (GameManager.Instance.isPlaying == true)
        {
            anim.SetBool("isRunning", true);
        }

        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        #region JUMPING
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump(jumpForce);
        }

        if (isJumping && Input.GetButton("Jump"))
        {
            if (jumpTimer < jumpTime)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;

            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimer = 0;
        }

        #endregion


    }

    public void Jump(float jumpForce){
        isJumping = true;
        anim.SetTrigger("Jump");
        jumpSound.Play();
        rb.velocity = Vector2.up * jumpForce;
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;

    private DatabaseReference dbReference;
    public float firebaseValue = 0f;

    private Animator anim;
    [SerializeField] public AudioSource jumpSound;
    public Image EMGBar;
    public static PlayerMovement instance;

    void Awake()
    {
      instance = this;
    }
    private void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Set up the listener for real-time database changes
            dbReference.Child("test/float").ValueChanged += HandleValueChanged;
        });
        anim = GetComponentInChildren<Animator>();
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null && args.Snapshot.Value != null)
        {
            firebaseValue = float.Parse(args.Snapshot.Value.ToString());
            EMGBar.fillAmount = firebaseValue/3;
            print(firebaseValue);
        }
    }
    private void Update()
    {
        if (GameManager.Instance.isPlaying == true)
        {
            anim.SetBool("isRunning", true);
        }

        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        #region JUMPING
        if (isGrounded && firebaseValue != 1f)
        {
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;

            if (jumpTimer < jumpTime)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
            isJumping = false;
            jumpTimer = 0;
        }  
        #endregion


    }

    public void Jump(float jumpForce){
        isJumping = true;
        anim.SetTrigger("Jump");
        jumpSound.Play();
        rb.velocity = Vector2.up * jumpForce;
    }
}