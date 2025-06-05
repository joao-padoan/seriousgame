using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
public class PlayerFly : MonoBehaviour
{
    Vector2 upForce;
    [SerializeField] private Rigidbody2D rb;
    public Camera MainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private DatabaseReference dbReference;
    public Image EMGBar;

    public float firebaseValue = 0f;
    void Start () {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Set up the listener for real-time database changes
            dbReference.Child("test/float").ValueChanged += HandleValueChanged;
        });
        upForce = new Vector2(0, 9.8f*1.6f);
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x-0.33f; //extents = size of width / 2
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y-0.33f; //extents = size of height / 2
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

    void Update () {
        if (firebaseValue!=1f && rb.position.y<=4.2)
        {
            GetComponent<Rigidbody2D>().AddForce(upForce);
        }
    }

    void LateUpdate(){
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }
}
