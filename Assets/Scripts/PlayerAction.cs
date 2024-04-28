using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float Speed;
    public GameManager manager;

    Animator anim;
    Rigidbody2D rigid;
    GameObject scanObject;
    float h;
    float v;
    bool isHorizonMove;
    Vector3 dirVec;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal");
        bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical");
        bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal");
        bool vUp = manager.isAction ? false : Input.GetButtonUp("Vertical");

        isHorizonMove = Mathf.Abs(h) > Mathf.Abs(v);

        if (anim.GetInteger("hAxisRaw") != h)
            anim.SetInteger("hAxisRaw", (int)h);
        else if (anim.GetInteger("vAxisRaw") != v)
            anim.SetInteger("vAxisRaw", (int)v);
        if (vDown && v == 1)
            dirVec = Vector3.up;
        if (vDown && v == -1)
            dirVec = Vector3.down;
        if (hDown && h == -1)
            dirVec = Vector3.left;
        if (hDown && h == 1)
            dirVec = Vector3.right;

        if (Input.GetButtonDown("Jump") && scanObject != null)
            manager.Action(scanObject);
    }
    void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * Speed;

        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
            scanObject = rayHit.collider.gameObject;
        else
            scanObject = null;
    }
}
