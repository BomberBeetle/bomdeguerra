using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	GameObject.Instantiate<TestBullet>(BulletPrefab, transform.position, rotation);

   }

   bool IsGrounded() {
    	Vector2 posM = transform.position + new Vector3 (0, -0.5f, 0);
	Vector2 posR = transform.position + new Vector3 (0.5f, -0.5f, 0);
	Vector2 posL = transform.position + new Vector3 (-0.5f, -0.5f, 0);

    	Vector2 direction = Vector2.down;
    	float distance = 0.1f;
    	RaycastHit2D hitM = Physics2D.Raycast(posM, direction, distance, GroundLayer);
    	if (hitM.collider != null) {
        	if((hitM.collider.tag == "SolidPlatform" || hitM.collider.tag == "DropPlatform") && SpeedY <= 0 ) return true;
    	}
	RaycastHit2D hitR = Physics2D.Raycast(posR, direction, distance, GroundLayer);
        if (hitR.collider != null) {
                if((hitR.collider.tag == "SolidPlatform" || hitR.collider.tag == "DropPlatform") && SpeedY <= 0 ) return true;
        }
	RaycastHit2D hitL = Physics2D.Raycast(posL, direction, distance, GroundLayer);
        if (hitL.collider != null) {
                if((hitL.collider.tag == "SolidPlatform" || hitL.collider.tag == "DropPlatform") && SpeedY <= 0 ) return true;
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
   public TestBullet BulletPrefab;

   public float JumpSpeed;

   public float SpeedX;
   public float SpeedY;

   public float AccelXGround;
   public float AccelXAir;
   public float AccelY;

   public float AirFriction;
   public float GroundFriction;


   public float MaxGroundSpeed;
   public float MaxAirSpeed;

   public float MinYSpeed;
   public float MaxYSpeed;

   public int CoyoteTimeFrames;
   public int JumpBufferFrames;

   Rigidbody2D phys;
   SpriteRenderer Renderer;
   int jumpBuffer;
   int coyoteFrames;
   bool grounded;
   bool ceiled;
   bool walled;

   enum FacingStateX : byte{
	Left = 0,
	Right = 1
   }

   enum FacingStateY : byte{
	None = 0,
	Up = 1,
	Down = 2
   }

   FacingStateX facingX; 
   FacingStateY facingY;


   void Start() {
	phys = GetComponent<Rigidbody2D>();
	Renderer = GetComponent<SpriteRenderer>();
	jumpBuffer = 0;
   }
   void Update(){
	
	if(Input.GetButtonDown("Fire1")){
		SpawnBullet();
	}
	if(jumpBuffer > 0){
		jumpBuffer--;
	}
	if(!grounded && coyoteFrames > 0){
		coyoteFrames--;
	}
	else if(grounded){
		coyoteFrames = CoyoteTimeFrames;
	}
	grounded = IsGrounded();
	ceiled = IsCeiled();
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
	

	jumpBuffer = Input.GetButtonDown("Jump")?JumpBufferFrames:jumpBuffer;
	if(grounded || coyoteFrames > 0){
		if(grounded){ 
			Renderer.color = Color.red;
	//		SpeedY = 0;
		}
		else{
			Renderer.color = Color.cyan;
			SpeedY += AccelY;

                	if(SpeedY < MinYSpeed){
                        	SpeedY = MinYSpeed;
                	}	
		}

		SpeedX += h_axis_input * AccelXGround;
		
		SpeedX *= GroundFriction;

		if(SpeedX > MaxGroundSpeed){
			SpeedX = MaxGroundSpeed;
		}
		else if(SpeedX < -MaxGroundSpeed){
			SpeedX = -MaxGroundSpeed;
		}
		if(jumpBuffer > 0){
			if(!ceiled) SpeedY = JumpSpeed;
			jumpBuffer = 0;
			coyoteFrames = 0;
		}
	}
	else{
		Renderer.color = Color.blue;
		SpeedY += AccelY;

		if(SpeedY < MinYSpeed){
			SpeedY = MinYSpeed;
		}

		SpeedX += h_axis_input * AccelXAir;
		
		SpeedX *= AirFriction;

		if(SpeedX > MaxAirSpeed){
			SpeedX = MaxAirSpeed;
		}
		else if(SpeedX < -MaxAirSpeed){
			SpeedX = -MaxAirSpeed;
		}
	}
	if(ceiled && SpeedY > 0){
                        SpeedY=0;
        }
	Vector2 nextPos = new Vector2(transform.position.x + SpeedX, transform.position.y + SpeedY);

	//if(!IsWalled(nextPos)) gameObject.transform.position = nextPos;
	//else{ 
	//	SpeedX = 0;
		gameObject.transform.position = new Vector2(transform.position.x , transform.position.y + SpeedY);
	//}
   }
}
