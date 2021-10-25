using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletCollisionHandler))]
public class CleitonBehaviour : MonoBehaviour {

   void SpawnBullet(){
	Quaternion rotation;
	if(facingY != FacingStateY.None){
		if(facingY == FacingStateY.Down){
			rotation = Quaternion.Euler(0,0,270);
		}
		else{
			rotation = Quaternion.Euler(0,0,90);
		}

	}
	else{
		if(facingX == FacingStateX.Right){
			rotation = Quaternion.Euler(0,0,0);
		}
		else{
			rotation = Quaternion.Euler(0,0,180);
		}
	}
	GameObject.Instantiate(BulletPrefab, transform.position, rotation);

   }

   void BulletCollision(Collider2D col, Bullet b){
	health -= b.Damage;
	if(health <= 0) Destroy(gameObject);
	Destroy(b.gameObject);
	Debug.Log("Collision");
   }

   bool IsGrounded() {
    	Vector2 posM = transform.position + new Vector3 (0, -0.5f, 0);
	Vector2 posR = transform.position + new Vector3 (0.5f, -0.5f, 0);
	Vector2 posL = transform.position + new Vector3 (-0.5f, -0.5f, 0);

    	Vector2 direction = Vector2.down;
    	float distance = 0.1f;
    	RaycastHit2D hitM = Physics2D.Raycast(posM, direction, distance, GroundLayer);
    	if (hitM.collider != null) {
        	if((hitM.collider.tag == "SolidPlatform" || hitM.collider.tag == "DropPlatform" && facingY != FacingStateY.Down) && SpeedY <= 0) return true;
    	}
	RaycastHit2D hitR = Physics2D.Raycast(posR, direction, distance, GroundLayer);
        if (hitR.collider != null) {
                if((hitR.collider.tag == "SolidPlatform" || hitR.collider.tag == "DropPlatform" && facingY != FacingStateY.Down) && SpeedY <= 0) return true;
        }
	RaycastHit2D hitL = Physics2D.Raycast(posL, direction, distance, GroundLayer);
        if (hitL.collider != null) {
                if((hitL.collider.tag == "SolidPlatform" || hitL.collider.tag == "DropPlatform"&& facingY != FacingStateY.Down) && SpeedY <= 0) return true;
        }
    
    	return false;
    }
   bool IsCeiled(){
	Vector2 posM = transform.position + new Vector3 (0, 0.5f, 0);
        Vector2 posR = transform.position + new Vector3 (0.5f, 0.5f, 0);
        Vector2 posL = transform.position + new Vector3 (-0.5f, 0.5f, 0);

        Vector2 direction = Vector2.up;
        float distance = 0.1f;

	RaycastHit2D hitM = Physics2D.Raycast(posM, direction, distance, GroundLayer);
        if (hitM.collider != null) {
                if(hitM.collider.tag == "SolidPlatform") return true;
        }
        RaycastHit2D hitR = Physics2D.Raycast(posR, direction, distance, GroundLayer);
        if (hitR.collider != null) {
                if(hitR.collider.tag == "SolidPlatform") return true;
        }
        RaycastHit2D hitL = Physics2D.Raycast(posL, direction, distance, GroundLayer);
        if (hitL.collider != null) {
                if(hitL.collider.tag == "SolidPlatform") return true;
        }

        return false;

   }

    bool IsWalled(Vector3 pos){
        Vector2 posR = pos + new Vector3 (0.5f, 0, 0);
	Vector2 posL = pos + new Vector3 (-0.5f, 0, 0);

        float distance = 0.1f;
        RaycastHit2D hitR = Physics2D.Raycast(posR, Vector2.right, distance, GroundLayer);
	RaycastHit2D hitL = Physics2D.Raycast(posL, Vector2.left, distance, GroundLayer);
        if (hitR.collider != null) {
                if(hitR.collider.tag == "SolidPlatform") return true;
        }
	if (hitL.collider != null) {
                if(hitL.collider.tag == "SolidPlatform") return true;
        }

        return false;

   }

   public LayerMask GroundLayer;
   public GameObject BulletPrefab;

   public float JumpSpeed;
   public float DoubleJumpSpeed;

   public float SpeedX;
   public float SpeedY;

   public float AccelXGround;
   public float AccelXAir;
   public float AccelY;

   public float AirFriction;
   public float GroundFriction;

   public int MaxHealth;

   public float MaxGroundSpeed;
   public float MaxAirSpeed;

   public float MinYSpeed;
   public float MaxYSpeed;

   public int CoyoteTimeFrames;
   public int JumpBufferFrames;
   public int DJumpBufferFrames;
   
   public bool DJumpEnabled;

   Rigidbody2D phys;
   SpriteRenderer Renderer;
   int jumpBuffer;
   int doubleJumpBuffer;
   int coyoteFrames;
   bool grounded;
   bool ceiled;
   bool walled;
   bool hasDoubleJump;	
   bool jumpPressed;
   public int health;

   enum FacingStateX : byte{
	Left = 0,
	Right = 1
   }

   enum FacingStateY : byte{
	None = 0,
	Up = 1,
	Down = 2
   }
   
   public enum State{
	NeutralGround = 0,
	NeutralAir = 1,
	CoyoteAir = 2,
	WallStick = 3,
	Dead = 4
   }

   FacingStateX facingX; 
   FacingStateY facingY;
   State state;

   void ApplyGravity(){
	SpeedY += AccelY;
	if(SpeedY < MinYSpeed){
        	SpeedY = MinYSpeed;
        }
   }

   void ApplyGroundAccel(float haxis){

	SpeedX *= GroundFriction;

	SpeedX += haxis * AccelXGround;

        if(SpeedX > MaxGroundSpeed){
                SpeedX = MaxGroundSpeed;
        }
        else if(SpeedX < -MaxGroundSpeed){
                SpeedX = -MaxGroundSpeed;
        }
   }

   void ApplyAirAccel(float haxis){
	
	SpeedX *= AirFriction;


	SpeedX += haxis * AccelXAir;

        if(SpeedX > MaxAirSpeed){
                SpeedX = MaxAirSpeed;
        }
        else if(SpeedX < -MaxAirSpeed){
                SpeedX = -MaxAirSpeed;
        }
   }

   void DoJump(){
	   if(jumpBuffer > 0){
                if(!ceiled) SpeedY = JumpSpeed;
                jumpBuffer = 0;
                coyoteFrames = 0;
        }
   }
   

   void DoDoubleJump(){
	hasDoubleJump = false;
        SpeedY = DoubleJumpSpeed;
   }

   void Start() {
	phys = GetComponent<Rigidbody2D>();
	Renderer = GetComponent<SpriteRenderer>();
	GetComponent<BulletCollisionHandler>().HandlerFunction = new BulletCollisionHandler.HandlerFunctionDelegate(BulletCollision);
	jumpBuffer = 0;
	doubleJumpBuffer = 0;
	hasDoubleJump = DJumpEnabled;
	health = MaxHealth;
   }
   void Update(){
	grounded = IsGrounded();
        ceiled = IsCeiled();
	if(Input.GetButtonDown("Fire1")){
		SpawnBullet();
	}
	if(jumpBuffer > 0){
		jumpBuffer--;
	}
	if(doubleJumpBuffer > 0){
		doubleJumpBuffer--;
	}
	if(!grounded && coyoteFrames > 0){
		state = State.CoyoteAir;
		coyoteFrames--;
	}
	else if(grounded){
		coyoteFrames = CoyoteTimeFrames;
		state = State.NeutralGround;
	}
	else{
		state = State.NeutralAir;
	}
	var h_axis_input = Input.GetAxisRaw("Horizontal");
	var v_axis_input = Input.GetAxisRaw("Vertical");

	if(v_axis_input != 0){
		if(v_axis_input > 0){
			facingY = FacingStateY.Up; 
		}
		else if(v_axis_input < 0){
			facingY = FacingStateY.Down;
		}
	}
	else{
		facingY = FacingStateY.None;
	}
	
	if(h_axis_input != 0){
		if(h_axis_input > 0){
			facingX = FacingStateX.Right;
		}
		else{
			facingX = FacingStateX.Left;
		}
	}
	
	jumpPressed = Input.GetButtonDown("Jump");
	jumpBuffer = jumpPressed?JumpBufferFrames:jumpBuffer;

	if(state == State.CoyoteAir){
		ApplyGravity();
		ApplyAirAccel(h_axis_input);
		if(jumpBuffer > 0){
			DoJump();
		}
		if(DJumpEnabled) hasDoubleJump = true;
	}

	else if(state == State.NeutralAir){
		ApplyGravity();
		ApplyAirAccel(h_axis_input);
		if(jumpPressed){
			if(hasDoubleJump){
				DoDoubleJump();
				hasDoubleJump = false;
			}
			else{
				jumpBuffer = JumpBufferFrames;
			}
		}
		if(ceiled && SpeedY > 0){
			SpeedY = 0;
		}
	}

	else if(state == State.NeutralGround){
		SpeedY = 0;
		if(jumpBuffer > 0){
			DoJump();
		}
		ApplyGroundAccel(h_axis_input);
		if(DJumpEnabled) hasDoubleJump = true;
	}

	Vector2 nextPos = new Vector2(transform.position.x + SpeedX*Time.deltaTime, transform.position.y + SpeedY*Time.deltaTime);

	if(!IsWalled(nextPos)) gameObject.transform.position = nextPos;
	else{ 
		SpeedX = 0;
		gameObject.transform.position = new Vector2(transform.position.x , transform.position.y + SpeedY*Time.deltaTime);
	}
   }
}
