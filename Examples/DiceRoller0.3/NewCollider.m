//
//  NewCollider.m
//  DiceRoller0.3
//
//  Created by Diego on 7/21/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import "NewCollider.h"

@implementation NewCollider


-(id) init
{
	cArray = [[ NSMutableArray alloc ] init ];
	
	NSDate *date0 = [NSDate date];
	time0 = [date0 timeIntervalSince1970];
	
	return self;
}

-(void) addObject: (Die *)obj
{
	NSLog(@"Adding a die... \n");
	
	[cArray addObject: obj];
	
}


-(void) setAcceleration: (float) x: (float) y: (float) z
{
	int objCount = [cArray count];
	for(int i=0; i<objCount; i++)
	{
		Die *d = [cArray objectAtIndex: i];
		Colliding *c = [d getCollider];
		[c setAcceleration:x :y :z];
	}
}


-(int) getDiceCount
{
	return [cArray count];
}

-(Die*) getDie:(int) diceIdx
{
	return [cArray objectAtIndex: diceIdx];
}

-(void) step
{
	
	NSDate *date1 = [NSDate date];
	NSTimeInterval time1 = [date1 timeIntervalSince1970];
	
	double dTime = ((double)time1) -  ((double)time0);
	
	int objCount = [cArray count];
	
	// NSLog(@"step ms %f\n", dTime );

	// evaluate new positions for dices
	for(int i=0; i<objCount; i++)
	{
		Die *d = [cArray objectAtIndex: i];
		Colliding *c = [d getCollider];
		[c computeTimeCapsule: dTime];
	}
	
	
	
	// find collisions
	for(int i=0; i<objCount; i++)
	{
		Die *d1 = [cArray objectAtIndex: i];
		Colliding *c1 = [d1 getCollider];
		
		[c1 compareTimeCapsuleWithWalls];
		
		for(int j=i+1; j<objCount; j++)
		{
			Die *d2 = [cArray objectAtIndex: j];
			Colliding *c2 = [d2 getCollider];
		
			// RIATTIVARE COLLISIONI TRA DADI!!!
			// [c1 compareTimeCapsule: c2];
		}
	}
	
	
	
	
	
	// intervent on colliding objs
	
	for(int i=0; i<objCount; i++)
	{
		Die *d1 = [cArray objectAtIndex: i];
		Colliding *c1 = [d1 getCollider];
		[c1 applyCollisions : dTime];
	}
	
	
	
	// apply new data to noncolliding objs
	for(int i=0; i<objCount; i++)
	{
		Die *d = [cArray objectAtIndex: i];
		Colliding *c = [d getCollider];
		[c moveWithNoCollision];
	}
	
	NSDate *date0 = [NSDate date];
	time0 = [date0 timeIntervalSince1970];
	
	
	// NSLog(@"step ms %f\n", timeSinceLastStep );
}

@end
