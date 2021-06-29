using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private LayerMask platform;
    private Rigidbody2D rigid;
    private Animator anim;
    private CapsuleCollider2D capCollider;
    private bool MarioIsAlive = true;
    //float curMoveSpeed = 0;
    //float curJumpSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        MarioIsAlive = true;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capCollider = GetComponent<CapsuleCollider2D>();
    }

    private bool isGrounded() {
        RaycastHit2D ray = Physics2D.CapsuleCast(capCollider.bounds.center, 
                                            capCollider.bounds.size,
                                            capCollider.direction,
                                            0f, Vector2.down, .1f,
                                            platform);
        return ray.collider != null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (MarioIsAlive && isGrounded() && Input.GetKeyDown(KeyCode.Space)) {
            //rigid.AddForce(new Vector2(0, jumpSpeed));
            //rigid.velocity = new Vector2(0, jumpSpeed);
            //curJumpSpeed = rigid.velocity.y + jumpSpeed;
            float jumpSpeed = 22f;
            rigid.velocity = Vector2.up * jumpSpeed;
            
        }
        movement();
        if (!MarioIsAlive) anim.Play("Mario_Dead");
        else if (isGrounded()) {
            if (Input.GetKey(KeyCode.DownArrow)) {
                rigid.velocity = new Vector2(0, 0);
                anim.Play("Mario_Duck");
            } else if (rigid.velocity.x == 0) {
                anim.Play("Mario_Standing");
            }
            else if (rigid.velocity.x != 0) {
                anim.Play("Mario_Walking");
            }
        } else anim.Play("Mario_Jumping");
    }

    private void movement() {
        if (MarioIsAlive && Input.GetKey(KeyCode.LeftArrow)) {
            float moveSpeed = 10f;
            //rigid.AddForce(new Vector2(-speed, 0));
            //rigid.velocity = new Vector2(-moveSpeed, -jumpSpeed);
            //curMoveSpeed = -moveSpeed;
            //anim.SetTrigger("Mario_Walking"); //catches by FPS so a lot of trigger
            rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);
            GetComponent<SpriteRenderer>().flipX = false;
        } else {
            float moveSpeed = 10f;
            if ( MarioIsAlive && Input.GetKey(KeyCode.RightArrow)) {
                //rigid.AddForce(new Vector2(speed, 0));
                //rigid.velocity = new Vector2(moveSpeed, -jumpSpeed);
                //curMoveSpeed = moveSpeed;
                //anim.SetTrigger("Mario_Walking");
                rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    private void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.tag.Equals("Enemy")) {
            MarioIsAlive = false;
            capCollider.enabled = !capCollider.enabled;
            rigid.velocity = new Vector2(0, 20f);
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("SampleScene");
    }

    void FixedUpdate() {
        //rigid.velocity = new Vector2(curMoveSpeed, curJumpSpeed);
    }
}
