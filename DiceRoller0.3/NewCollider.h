//
//  NewCollider.h
//  DiceRoller0.3
//
//  Created by Diego on 7/21/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "Colliding.h"
#import "Die.h"


@interface NewCollider : NSObject {

	NSMutableArray *cArray;
	
	//NSDate *date = [NSDate date];
	NSTimeInterval time0; // it's a double...
	
	// do work...
	
	// Find elapsed time and convert to milliseconds
	// Use (-) modifier to conversion since receiver is earlier than now
	// NSTimeInterval timePassed_ms = [date timeIntervalSinceNow] * -1000.0;
}


-(void) addObject: (Die *)obj;

-(void) setAcceleration: (float) x: (float) y: (float) z;

-(void) step;


-(int) getDiceCount;

-(Die*) getDie:(int) diceIdx;

@end
