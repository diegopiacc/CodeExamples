//
//  Die.m
//  DiceRoller0.1
//
//  Created by Diego on 3/5/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import <stdlib.h>
#import <OpenGLES/ES1/gl.h>

#import "Die.h"
//#import "teapot.h"

float wallSizes[] = {1.7, 2.6, 2};

float collisionDamp = 0.8f;

unsigned short diceIndicies [] = {


// d4
20, 21, 22,   23, 24, 25,   26, 27, 28,   29, 30, 31,

// d6

//
//

// d8

// d10


// d20
32, 33, 34,   35, 36, 37,   38, 39, 40,   41, 42, 43, 
44, 45, 46,   47, 48, 49,   50, 51,
52,   
53, 54, 55,   
56, 57, 58,   59, 60, 61,   62, 63, 64,
65, 66, 67,   
68, 69, 70,   71, 72, 73,   74, 75,
76, 77, 78, 79,   
80, 81, 82,   83, 84, 85,   86, 87, 88,   89, 90, 91

};

@implementation Die

-(id) init: (int) fCount
{
	cObj = [[Colliding alloc] init];
	
	faceCount = fCount;
	indicesCount = 3 * fCount;
	collisionNormals = malloc(3 * fCount * sizeof(float));
	
	
	if(faceCount==4) {
		indicesOffset = 0;
		// memcpy(collisionNormals, all_normals, 3 * fCount);
		// collisionNormals[0]=0.0f; 
		
		// collisionNormals = {0,0,0,0,0,0,0,0,0,0,0,0};
		
		
		
	} else if(faceCount==6) {
		indicesOffset = 0;
		
		
		
	} else if(faceCount==8) {
		indicesOffset = 0;
	} else if(faceCount==10) {
		indicesOffset = 0;
	} else if(faceCount==20) {
		indicesOffset = 12;
	} else {
		indicesOffset = 0;
	}
	
	return self;
}


-(void) setPosition: (float)x: (float)y: (float)z
{
	[cObj setPosition:x :y :z];
}

-(Colliding *) getCollider 
{
	return cObj;
}

-(void) render 
{
	glPushMatrix();
	//glLoadIdentity();
		
	glTranslatef([cObj getPositionX], [cObj getPositionY], [cObj getPositionZ]);
	glRotatef([cObj getRotationX], 1.0f, 0.0f, 0.0f);
	glRotatef([cObj getRotationY], 0.0f, 1.0f, 0.0f);
	glRotatef([cObj getRotationZ], 0.0f, 0.0f, 1.0f);
	
	
	// glDrawElements(GL_TRIANGLE_STRIP, indicesCount, GL_UNSIGNED_SHORT, &diceIndicies[indicesOffset])	
	glDrawElements(GL_TRIANGLES, indicesCount, GL_UNSIGNED_SHORT, &diceIndicies[indicesOffset]);

	glPopMatrix();
	
}

@end
