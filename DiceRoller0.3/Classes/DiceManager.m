//
//  untitled.m
//  DiceRoller0.1
//
//  Created by Diego on 3/6/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import "DiceManager.h"

@implementation DiceManager


-(id) init
{

	collider = [[ NewCollider alloc ] init ];
	return self;
}

-(void) setAccelerationValue: (float)accx: (float)accy: (float)accz
{
	[collider setAcceleration: accx: accy: accz];
}



-(void) addDie
{
	[self addDx: 6];
}



-(void) addDx: (int) numFaces
{	
	Die* die = [[ Die alloc ] init: numFaces ];
	[ collider addObject: die ];
}

-(void) step
{
	[collider step];
}

-(int) getDiceCount
{
	return [collider getDiceCount];
}

-(Die*) getDie:(int) diceIdx
{
	return [collider getDie: diceIdx];
}


@end
