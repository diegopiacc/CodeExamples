//
//  Colliding.m
//  DiceRoller0.3
//
//  Created by Diego on 7/21/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import "Colliding.h"

#import "numberFunctions.h"
#import "walls.h"


@implementation Colliding

-(id) init
{
	colliding = false;
	
	for(int i=0; i<3; i++){
		position[i] = 0;
		positionFuture[i] = 0;
		rotationEulers[i] = 0;
		
		speed[i] = randomFloat(2);
		speedFuture[i] = speed[i];
		acceleration[i] = 0;
	}
	
	return self;
}

-(void) setPosition: (float) x: (float) y: (float) z
{
	position[0] = x;
	position[1] = y;
	position[2] = z;
}

-(float) getPositionX { return position[0]; }
-(float) getPositionY { return position[1]; }
-(float) getPositionZ { return position[2]; }
-(float) getRotationX { return rotationEulers[0]; }
-(float) getRotationY { return rotationEulers[1]; }
-(float) getRotationZ { return rotationEulers[2]; }


-(void) setAcceleration: (float) ax: (float) ay: (float) az
{
	acceleration[0] = ax;
	acceleration[1] = ay;
	acceleration[2] = az;
}


-(void) computeTimeCapsule: (double) elapsed
{
	colliding = false;
	
	for(int i=0; i<3; i++) {
		float ds = acceleration[i] * elapsed;
		// NSLog(@"%f * %f = %f", acceleration[i], elapsed, ds);
		
		
		speedFuture[i] = speed[i] + ds;
		positionFuture[i] = position[i] + (speedFuture[i]*elapsed);
	}
	
	// NSLog(@"elapsedMs: %f", elapsedMs);
	// NSLog(@"speedFuture: %f %f %f", speedFuture[0], speedFuture[1], speedFuture[2]);
	// NSLog(@"positionFuture: %f %f %f", positionFuture[0], positionFuture[1], positionFuture[2]);
	
}


-(void) compareTimeCapsuleWithWalls
{
	if((position[0] - WALL_LT) * (positionFuture[0] - WALL_LT) < 0){
		
		// collision with left wall!
		colliding = true;
		float progress = (WALL_LT - position[0]) / (positionFuture[0] - position[0]); 
		
		collisionPoint[0] = WALL_LT;
		collisionPoint[1] = position[1] + progress * (positionFuture[1] - position[1]);
		collisionPoint[2] = position[2] + progress * (positionFuture[2] - position[2]);
		remainingSpeed[0] = -1 * WALL_COLLISION_FACTOR * speedFuture[0];
		remainingSpeed[1] = WALL_COLLISION_FACTOR * speedFuture[1];
		remainingSpeed[2] = WALL_COLLISION_FACTOR * speedFuture[2];
		return;
	}

	if((position[0] - WALL_RT) * (positionFuture[0] - WALL_RT) < 0){
		
		// collision with right wall!
		colliding = true;
		float progress = (WALL_RT - position[0]) / (positionFuture[0] - position[0]); 
		
		collisionPoint[0] = WALL_RT;
		collisionPoint[1] = position[1] + progress * (positionFuture[1] - position[1]);
		collisionPoint[2] = position[2] + progress * (positionFuture[2] - position[2]);		
		remainingSpeed[0] = -1 * WALL_COLLISION_FACTOR * speedFuture[0];
		remainingSpeed[1] = WALL_COLLISION_FACTOR * speedFuture[1];
		remainingSpeed[2] = WALL_COLLISION_FACTOR * speedFuture[2];
		return;
	}
	
	if((position[1] - WALL_BM) * (positionFuture[1] - WALL_BM) < 0){
		
		// collision with bottom wall!
		colliding = true;
		float progress = (WALL_BM - position[1]) / (positionFuture[1] - position[1]); 
		
		collisionPoint[0] = position[0] + progress * (positionFuture[0] - position[0]);
		collisionPoint[1] = WALL_BM;
		collisionPoint[2] = position[2] + progress * (positionFuture[2] - position[2]);		
		remainingSpeed[0] = WALL_COLLISION_FACTOR * speedFuture[0];
		remainingSpeed[1] = -1 * WALL_COLLISION_FACTOR * speedFuture[1];
		remainingSpeed[2] = WALL_COLLISION_FACTOR * speedFuture[2];
		return;
	}
	
	if((position[1] - WALL_TP) * (positionFuture[1] - WALL_TP) < 0){
		
		// collision with top wall!
		colliding = true;
		float progress = (WALL_TP - position[1]) / (positionFuture[1] - position[1]); 
		
		collisionPoint[0] = position[0] + progress * (positionFuture[0] - position[0]);
		collisionPoint[1] = WALL_TP;
		collisionPoint[2] = position[2] + progress * (positionFuture[2] - position[2]);
		remainingSpeed[0] = WALL_COLLISION_FACTOR * speedFuture[0];
		remainingSpeed[1] = -1 * WALL_COLLISION_FACTOR * speedFuture[1];
		remainingSpeed[2] = WALL_COLLISION_FACTOR * speedFuture[2];
		return;
	}
	
	if((position[2] - WALL_BK) * (positionFuture[2] - WALL_BK) < 0){
		
		// collision with back wall!
		colliding = true;
		float progress = (WALL_BK - position[2]) / (positionFuture[2] - position[2]); 
		
		collisionPoint[0] = position[0] + progress * (positionFuture[0] - position[0]);
		collisionPoint[1] = position[1] + progress * (positionFuture[1] - position[1]);
		collisionPoint[2] = WALL_BK;
		remainingSpeed[0] = WALL_COLLISION_FACTOR * speedFuture[0];
		remainingSpeed[1] = WALL_COLLISION_FACTOR * speedFuture[1];
		remainingSpeed[2] = -1 * WALL_COLLISION_FACTOR * speedFuture[2];
		return;
	}
	
	if((position[2] - WALL_FR) * (positionFuture[2] - WALL_FR) < 0){
		
		// collision with back wall!
		colliding = true;
		float progress = (WALL_FR - position[2]) / (positionFuture[2] - position[2]); 
		
		collisionPoint[0] = position[0] + progress * (positionFuture[0] - position[0]);
		collisionPoint[1] = position[1] + progress * (positionFuture[1] - position[1]);
		collisionPoint[2] = WALL_FR;
		remainingSpeed[0] = WALL_COLLISION_FACTOR * speedFuture[0];
		remainingSpeed[1] = WALL_COLLISION_FACTOR * speedFuture[1];
		remainingSpeed[2] = -1 * WALL_COLLISION_FACTOR * speedFuture[2];
		return;
	}
	



}

-(void) compareTimeCapsule: (Colliding *) other
{
	// find the distance vector between the two time capsules
	
	float distVec[] = {0,0,0};
	
	float dist = dist3D_Segment_to_Segment(
		position, positionFuture,
		other->position, other->positionFuture,
		distVec);
	
	float compenetrationForce = COLLIDING_DIST - dist;
	if(compenetrationForce > 0) {
		colliding = true;
		other->colliding = true;
		
		for(int i=0; i<3; i++) {
			collisionPoint[i] = position[i] + distVec[i];
			other->collisionPoint[i] = collisionPoint[i];
			remainingSpeed[i] = -1*distVec[i];
			other->remainingSpeed[i] = distVec[i];
		}
	
	}
	//
	// 
	
	
	
}

-(void) applyCollisions: (double) elapsed
{
	if(colliding) {
		
		// NSLog(@"Collision located in (%f, %f, %f)",collisionPoint[0],collisionPoint[1],collisionPoint[2]);
		for(int i=0; i<3; i++) {
			speedFuture[i] = remainingSpeed[i];
			positionFuture[i] = collisionPoint[i] + remainingSpeed[i] * elapsed;
		}
	}
}

-(void) moveWithNoCollision
{
	for(int i=0; i<3; i++) {
		speed[i] = speedFuture[i];
		position[i] = positionFuture[i];
	}
	
	// NSLog(@"Position: %f %f %f", position[0], position[1], position[2]);
}




@end
