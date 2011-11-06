//
//  Die.h
//  DiceRoller0.1
//
//  Created by Diego on 3/5/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "Colliding.h"

@interface Die : NSObject {

	Colliding* cObj;
	
	float* collisionNormals;
	
	int faceCount;
	int indicesCount;
	int indicesOffset;

}

-(id) init: (int) fCount;
-(void) setPosition: (float)x: (float)y: (float)z;

-(Colliding *) getCollider;

-(void) render;
 
@end
