using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using PathCreation;

//This controller provides basic 'click-to-move' functionality;
//It can be used as a starting point for a variety of top-down (or isometric) games, which are primarily controlled via mouse input;
public class ClickToMoveController : Controller
{
	//Controller movement speed;
	public float movementSpeed = 10f;
	//Downward gravity;
	public float gravity = 30f;

	float currentVerticalSpeed = 0f;
	bool isGrounded = false;

	//Current position to move towards;
	Vector3 currentTargetPosition;
	//If the distance between controller and target position is smaller than this, the target is reached;
	float reachTargetThreshold = 0.001f;
	
	Vector3 lastVelocity = Vector3.zero;
	Vector3 lastMovementVelocity = Vector3.zero;

	//Abstarct ground plane used when 'AbstractPlane' is selected;
	Plane groundPlane;

    //Reference to attached 'Mover' and transform component;
    Mover mover;
	Transform tr;

	private void Awake()
	{
		pathCreator = FindObjectOfType<PathCreator>();
		joy = FindObjectOfType<Joystick>();
	}

	void Start()
    {
        //Get references to necessary components;
        mover = GetComponent<Mover>();
		tr = transform;
		

		//Initialize variables;
		currentTargetPosition = transform.position;
		groundPlane = new Plane(tr.up, tr.position);
		pathCreator.pathUpdated += OnPathChanged;
    }
	
	void OnGroundContactRegained(Vector3 _collisionVelocity)
	{
		//Call 'OnLand' delegate function;
		if(OnLand != null)
			OnLand(_collisionVelocity);
	}
	
    void OnPathChanged() {
	    distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
    void FixedUpdate()
    {
        //Run initial mover ground check;
        mover.CheckForGround();
        
        //Check whether the character is grounded;
        isGrounded = mover.IsGrounded();

        Vector3 _velocity = Vector3.zero;

        //Calculate the final velocity for this frame;
		_velocity = CalculateMovementVelocity();
		lastMovementVelocity = _velocity;

		//Calculate and apply gravity;
		HandleGravity();
		_velocity += tr.up * currentVerticalSpeed;

        //If the character is grounded, extend ground detection sensor range;
        mover.SetExtendSensorRange(isGrounded);
        //Set mover velocity;
        mover.SetVelocity(_velocity);
        
		//SaveLoad velocity for later;
		lastVelocity = _velocity;
		
		if(!isGrounded && mover.IsGrounded())
			OnGroundContactRegained(lastVelocity);
    }

    private Joystick joy;
    
    
    private void Update()
    {
	    distanceTravelled += speed * Time.deltaTime;

	    var pos = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

	    pos += new Vector3(joy.Output.x * 5,0,0);

	    currentTargetPosition = pos;
    }
    
    PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    float distanceTravelled;

	//Calculate movement velocity based on the current target position;
	Vector3 CalculateMovementVelocity()
	{
		//Return no velocity if controller currently has no target;	

		//Calculate vector to target position;
		Vector3 _toTarget = currentTargetPosition - tr.position;

		//Remove all vertical parts of vector;
		_toTarget = VectorMath.RemoveDotVector(_toTarget, tr.up);
		
		//Calculate distance to target;
		float _distanceToTarget = _toTarget.magnitude;

		//If controller has already reached target position, return no velocity;
		if(_distanceToTarget <= reachTargetThreshold)
		{
			return Vector3.zero;
		}
			
		Vector3 _velocity = _toTarget.normalized * movementSpeed;

		//Check for overshooting;
		if(movementSpeed * Time.fixedDeltaTime > _distanceToTarget)
		{
			_velocity = _toTarget.normalized * _distanceToTarget;
		}
		return _velocity;
	}

	//Calculate current gravity;
	void HandleGravity()
	{
		//Handle gravity;
		if(!isGrounded)
			currentVerticalSpeed -= gravity * Time.deltaTime;
		else
		{
			if(currentVerticalSpeed < 0f)
			{
				if(OnLand != null)
					OnLand(tr.up * currentVerticalSpeed);
			}
			
			currentVerticalSpeed = 0f;
		}
	}

	


	public override bool IsGrounded()
	{
		return isGrounded;
	}

	public override Vector3 GetMovementVelocity()
	{
		return lastMovementVelocity;
	}

	public override Vector3 GetVelocity()
	{
		return lastVelocity ;
	}
}