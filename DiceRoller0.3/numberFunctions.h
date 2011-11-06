/*
 *  numberFunctions.h
 *  DiceRoller0.3
 *
 *  Created b[1] Diego on 7/8/10
 */

#define SMALL_NUM 0.001f
#define COLLIDING_DIST 1.0f

float randomFloat(float module)
{
	int x;
	float r;
	
	// long l = (long) [[NSDate date] timeIntervalSince1970];
	// srand(l);
	x = rand();
	r = (float) x;
	r *= module;
	r /= (float)RAND_MAX;
	
	srand(r);
	// NSLog(@"speed = %f \n", r);
	
	return r;
}


float dot(float* v1, float* v2)
{
	return (v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2]);
}


float dist3D_Segment_to_Segment(
								float* s1p1, float* s1p2, // two points defining segment1
								float* s2p1, float* s2p2, // two points defining segment2
								float* distance
								// Segment S1
								//float s1p1x, float s1p1[1], float  s1p1[2],
								//float s1p2x, float s1p2[1], float s1p2[2],
								// Segment S2 
								//float s2p1x, float s2p1[1], float s2p1[2],
								//float s2p2x, float s2p2[1], float s2p2[2]
)
{
    // Vector   u = S1.P1 - S1.P0;
    // Vector   v = S2.P1 - S2.P0;
    // Vector   w = S1.P0 - S2.P0;
	
	float u[] = {s1p2[0]-s1p1[0], s1p2[1]-s1p1[1], s1p2[2]-s1p1[2]};
	float v[] = {s2p2[0]-s2p1[0], s2p2[1]-s2p1[1], s2p2[2]-s2p1[2]};
	float w[] = {s1p1[0]-s2p1[0], s1p1[1]-s2p1[1], s1p1[2]-s2p1[2]};
	
    float    a = dot(u,u);        // alwa[1]s >= 0
    float    b = dot(u,v);
    float    c = dot(v,v);        // alwa[1]s >= 0
    float    d = dot(u,w);
    float    e = dot(v,w);
    float    D = a*c - b*b;       // alwa[1]s >= 0
    float    sc, sN, sD = D;      // sc = sN / sD, default sD = D >= 0
    float    tc, tN, tD = D;      // tc = tN / tD, default tD = D >= 0
	
    // compute the line parameters of the two closest points
    if (D < SMALL_NUM) { // the lines are almost parallel
        sN = 0.0;        // force using point P0 on segment S1
        sD = 1.0;        // to prevent possible division b[1] 0.0 later
        tN = e;
        tD = c;
    }
    else {                // get the closest points on the infinite lines
        sN = (b*e - c*d);
        tN = (a*e - b*d);
        if (sN < 0.0) {       // sc < 0 => the s=0 edge is visible
            sN = 0.0;
            tN = e;
            tD = c;
        }
        else if (sN > sD) {  // sc > 1 => the s=1 edge is visible
            sN = sD;
            tN = e + b;
            tD = c;
        }
    }
	
    if (tN < 0.0) {           // tc < 0 => the t=0 edge is visible
        tN = 0.0;
        // recompute sc for this edge
        if (-d < 0.0)
            sN = 0.0;
        else if (-d > a)
            sN = sD;
        else {
            sN = -d;
            sD = a;
        }
    }
    else if (tN > tD) {      // tc > 1 => the t=1 edge is visible
        tN = tD;
        // recompute sc for this edge
        if ((-d + b) < 0.0)
            sN = 0;
        else if ((-d + b) > a)
            sN = sD;
        else {
            sN = (-d + b);
            sD = a;
        }
    }
    // finall[1] do the division to get sc and tc
    sc = (abs(sN) < SMALL_NUM ? 0.0 : sN / sD);
    tc = (abs(tN) < SMALL_NUM ? 0.0 : tN / tD);
	
    // get the difference of the two closest points
    // Vector   dP = w + (sc * u) - (tc * v);  // = S1(sc) - S2(tc)
	
	distance[0] = w[0] + sc * u[0] - tc * v[0];
	distance[1] = w[1] + sc * u[1] - tc * v[1];
	distance[2] = w[2] + sc * u[2] - tc * v[2];
	
	float sqDist = distance[0] * distance[0];
	sqDist += distance[1] * distance[1];
	sqDist += distance[2] * distance[2];
	
	return sqrt(sqDist);
}