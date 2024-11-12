using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : NetworkBehaviour
{
    //[SyncVar(hook = nameof(OnColorChange))]
    //public Color playerColor = Color.white;
    //private Material playerMaterialClone;
    public CharacterController _controller;
    protected Vector3 moveVector;
    float v = 0;
    float h = 0;
    public float moveSpeed = 2;

    public GameObject[] playerModels;
    [SyncVar(hook = nameof(OnModelIndexChanged))]
    private int modelIndex = -1;
    private static int modelIndexCounter = 0;

    private void OnModelIndexChanged(int oldIndex, int newIndex)
    {
       
        foreach (var model in playerModels)
        {
            model.SetActive(false);
        }

        Debug.Log("rererre"+newIndex);
        if (newIndex >= 0 && newIndex < playerModels.Length)
        {
            playerModels[newIndex].SetActive(true);
        }
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CmdSetPlayerModelIndex();
       Camera.main.GetComponent<CameraFollow>().target = transform;
        Camera.main.GetComponent<CameraFollow>().decalagestart();

    }

    [Command]
    private void CmdSetPlayerModelIndex()
    {
            modelIndex = modelIndexCounter;
            modelIndexCounter = (modelIndexCounter + 1) % playerModels.Length;
    }


    //void OnColorChange(Color _old, Color _new)
    //{
    //    playerMaterialClone = new Material(GetComponent<Renderer>().material);
    //    playerMaterialClone.color = _new;

    //    GetComponent<Renderer>().material = playerMaterialClone;
    //}

    //public override void OnStartLocalPlayer()
    //{
    //    base.OnStartLocalPlayer();

    //    Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    //    CmdSetupColor(color);
    //}
    //[Command]
    //void CmdSetupColor(Color _color)
    //{
    //    playerColor = _color;
    //}

    // Update is called once per frame

    public virtual void move()
    {
        if (PlayerHealth.isDead) return;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        moveVector = new Vector3(h, 0, v);
        if (moveVector.magnitude > 0 && !PlayerHealth.isDead)
        {
            if (moveVector.magnitude > 1) moveVector = moveVector.normalized;
            _controller.Move(moveVector * moveSpeed * Time.deltaTime);
            transform.forward = moveVector;
        }
    }
    void Update()
    {
        //float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
        //float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        //transform.Rotate(0, moveX, 0);
        //transform.Translate(0, 0, moveZ);
        if (isLocalPlayer)
        {
            move();
        }
      
    }
}
