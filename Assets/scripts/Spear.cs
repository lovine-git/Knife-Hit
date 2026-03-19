using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField]
    private Vector2 throwForce;

    private bool canThrow = true;

    private Rigidbody2D rb;
    private BoxCollider2D spearCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spearCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
      
        if (GameController.Instance != null && !GameController.Instance.IsGameActive)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && canThrow)
        {
            canThrow = false;
            rb.AddForce(throwForce, ForceMode2D.Impulse);
            rb.gravityScale = 1;

            GameController.Instance.GameUI.DecrementDisplaySpearCount();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Target"))
        {

            GetComponent<AudioSource>().Play();

            GetComponent<ParticleSystem>().Play();

            
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            this.transform.SetParent(collision.collider.transform);

            spearCollider.offset = new Vector2(spearCollider.offset.x, -0.4f);
            spearCollider.size = new Vector2(spearCollider.size.x, 1.2f);

            GameController.Instance.OnSuccessfulSpearHit();
        }
        else if (collision.collider.CompareTag("Spear"))
        {
           
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2);
            rb.angularVelocity = -200f;


            GameObject[] existingSpears = GameObject.FindGameObjectsWithTag("Spear");
            foreach (GameObject s in existingSpears)
            {
                Rigidbody2D otherRb = s.GetComponent<Rigidbody2D>();
                if (otherRb != null)
                {
                    otherRb.bodyType = RigidbodyType2D.Static; 
                }
            }

            GameController.Instance.StartGameOverSequence(false);
        }
    }
}