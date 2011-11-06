//
//  untitled.h
//  DiceRoller0.1
//
//  Created by Diego on 3/6/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "NewCollider.h"

@interface DiceManager : NSObject {
	
	NewCollider *collider;
	float accelerationx, accelerationy, accelerationz;

}

-(id) init;
-(void) setAccelerationValue: (float)accx: (float)accy: (float)accz;
-(void) addDie;
-(void) addDx: (int) numFaces;

-(void) step;
-(int) getDiceCount;
-(Die*) getDie:(int) diceIdx;

@end
