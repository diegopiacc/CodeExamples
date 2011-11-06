//
//  Colliding.h
//  DiceRoller0.3
//
//  Created by Diego on 7/21/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface Colliding : NSObject {
	
	float position[3];
	float positionFuture[3];
	
	float speed[3];
	float speedFuture[3];
	
	float rotationEulers[3];
	
	bool colliding;
	float collisionPoint[3];
	float remainingSpeed[3];
	
	float acceleration[3];
}


// initialization
-(id) init;

// setter for the position of the object;
// ignores past calculations
-(void) setPosition: (float) x: (float) y: (float) z;

-(float) getPositionX;
-(float) getPositionY;
-(float) getPositionZ;
-(float) getRotationX;
-(float) getRotationY;
-(float) getRotationZ;

// setter for the acceleration acting on object
-(void) setAcceleration: (float) ax: (float) ay: (float) az;

// Caluclate the trajctory of the object in space, 
// given the time passed since last computation in seconds
-(void) computeTimeCapsule: (double) elapsed;

// Check if the trajectory of this object crosses the one
// of another object
-(void) compareTimeCapsuleWithWalls;
-(void) compareTimeCapsule: (Colliding *) other;

//
-(void) applyCollisions: (double) elapsed;

// class functions
-(void) moveWithNoCollision;

@end
