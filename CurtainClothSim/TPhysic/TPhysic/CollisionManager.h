#ifndef _COLLISIONMANAGER_H_
#define _COLLISIONMANAGER_H_

#include "Math.h"

public class CollisionManager {
public:
	int numnodesx, numnodesx3, numnodesy, numnodes;
	float nodesizex, nodesizey;
	float tolerance;
	//
	CollisionManager(void);
	void setMeasures(int, float, int, float, int);
	//
	void Check(int, float*, float*);
	//
	bool NodesConnected(int, int);
	//void ComputeAuxSpring(float, float, float, float, float, float, float, float, float, float);
	// funzioni ausiliarie
	int CoordinatesHash(float, float, float);
	float Aux_ReturnMax(float, float, float, float);
	float Aux_ReturnMin(float, float, float, float);
	float Aux_ReturnMean(float, float, float, float);
	float normalize(float*);
};

#endif