#include "StdAfx.h"
#include "CollisionManager.h"

CollisionManager::CollisionManager(void){
}

void CollisionManager::setMeasures(int nx, float sizex, int ny, float sizey, int n) {
	numnodesx = nx;
	numnodesx3 = nx*3;
	nodesizex = sizex;
	numnodesy = ny;
	nodesizey = sizey;
	numnodes = n;
	tolerance = 0.1f;
}

/////////////////////////////////////////////////////////////////

void CollisionManager::Check(int nodeindex, float* pos, float* vel) {
	int i,k;
	int check_ix, check_iy, check_iz;
	float off_xmin, off_ymin, off_zmin, off_xmax, off_ymax, off_zmax, vnorm;
	//float check_xmin, check_ymin, check_zmin, check_xmax, check_ymax, check_zmax;
	float tmp[3];
	float nextpos[3];

	// indici delle coordinate del volume di collisione
	int base_coord[12] = {
		0,1,2,									3,4,5,
		numnodesx3,numnodesx3+1,numnodesx3+2,numnodesx3+3,numnodesx3+4,numnodesx3+5};
	
	// coordinate del punto di controllo
	check_ix = 3*nodeindex;
	check_iy = check_ix + 1;
	check_iz = check_ix + 2;
	// posizione del punto al prossimo step
	nextpos[0] = pos[check_ix] + vel[check_ix];
	nextpos[1] = pos[check_iy] + vel[check_iy];
	nextpos[2] = pos[check_iz] + vel[check_iz];

	// per tutti i poligoni, calcola un volume di collisone
	for(i=0; i<(numnodes-numnodesx); i++) {
		if( (i%numnodesx)!=(numnodesx-1)) {

			// volume di collisione: cubo che circonda un poligono 
			// next step
			off_xmin = Aux_ReturnMin(
				pos[base_coord[0]] + vel[base_coord[0]],
				pos[base_coord[3]] + vel[base_coord[3]],
				pos[base_coord[6]] + vel[base_coord[6]],
				pos[base_coord[9]] + vel[base_coord[9]]);
			off_xmax = Aux_ReturnMax(
				pos[base_coord[0]] + vel[base_coord[0]],
				pos[base_coord[3]] + vel[base_coord[3]],
				pos[base_coord[6]] + vel[base_coord[6]],
				pos[base_coord[9]] + vel[base_coord[9]]);
			off_ymin = Aux_ReturnMin(
				pos[base_coord[1]] + vel[base_coord[1]],
				pos[base_coord[4]] + vel[base_coord[4]],
				pos[base_coord[7]] + vel[base_coord[7]],
				pos[base_coord[10]] + vel[base_coord[10]]);
			off_ymax = Aux_ReturnMax(
				pos[base_coord[1]] + vel[base_coord[1]],
				pos[base_coord[4]] + vel[base_coord[4]],
				pos[base_coord[7]] + vel[base_coord[7]],
				pos[base_coord[10]] + vel[base_coord[10]]);
			off_zmin = Aux_ReturnMin(
				pos[base_coord[2]] + vel[base_coord[2]],
				pos[base_coord[5]] + vel[base_coord[5]],
				pos[base_coord[8]] + vel[base_coord[8]],
				pos[base_coord[11]] + vel[base_coord[11]]);
			off_zmax = Aux_ReturnMax(
				pos[base_coord[2]] + vel[base_coord[2]],
				pos[base_coord[5]] + vel[base_coord[5]],
				pos[base_coord[8]] + vel[base_coord[8]],
				pos[base_coord[11]] + vel[base_coord[11]]);
			// tolleranze
			off_xmin += 0.01f;
			off_ymin += 0.01f;
			off_zmin += 0.001f;
			off_xmax -= 0.01f;
			off_ymax -= 0.01f;
			off_zmax -= 0.001f;


			/* 
			//this step
			off_xmin = Aux_ReturnMin(pos[base_coord[0]],pos[base_coord[3]],pos[base_coord[6]],pos[base_coord[9]]);
			off_xmax = Aux_ReturnMax(pos[base_coord[0]],pos[base_coord[3]],pos[base_coord[6]],pos[base_coord[9]]);
			off_ymin = Aux_ReturnMin(pos[base_coord[1]],pos[base_coord[4]],pos[base_coord[7]],pos[base_coord[10]]);
			off_ymax = Aux_ReturnMax(pos[base_coord[1]],pos[base_coord[4]],pos[base_coord[7]],pos[base_coord[10]]);
			off_zmin = Aux_ReturnMin(pos[base_coord[2]],pos[base_coord[5]],pos[base_coord[8]],pos[base_coord[11]]);
			off_zmax = Aux_ReturnMax(pos[base_coord[2]],pos[base_coord[5]],pos[base_coord[8]],pos[base_coord[11]]);

			// punto di attraversamento:
			off_xmin = Aux_ReturnMean(
				pos[base_coord[0]] + vel[base_coord[0]],
				pos[base_coord[3]] + vel[base_coord[3]],
				pos[base_coord[6]] + vel[base_coord[6]],
				pos[base_coord[9]] + vel[base_coord[9]]);
			off_ymin = Aux_ReturnMean(
				pos[base_coord[1]] + vel[base_coord[1]],
				pos[base_coord[4]] + vel[base_coord[4]],
				pos[base_coord[7]] + vel[base_coord[7]],
				pos[base_coord[10]] + vel[base_coord[10]]);
			off_zmin = Aux_ReturnMean(
				pos[base_coord[2]] + vel[base_coord[2]],
				pos[base_coord[5]] + vel[base_coord[5]],
				pos[base_coord[8]] + vel[base_coord[8]],
				pos[base_coord[11]] + vel[base_coord[11]]); 
				
			*/

			// confronti con il punto di controllo (quello in input)
			if(!NodesConnected(i,nodeindex)) {
				
				// test attraversamento del piano del poligono
				// [molto oneroso da calcolare]

				// test attraversamento di uno dei lati del volume (Bounding Box)
				//if ( ((off_zmin-pos[check_iz])*(off_zmin-nextpos[2]) < 0) || ((off_zmax-pos[check_iz])*(off_zmax-nextpos[2]) < 0) ) {
				//	if ( ((off_ymin-pos[check_iy])*(off_ymin-nextpos[1]) < 0) || ((off_ymax-pos[check_iy])*(off_ymax-nextpos[1]) < 0) ) {
				//		if ( ((off_xmin-pos[check_ix])*(off_xmin-nextpos[0]) < 0) || ((off_xmax-pos[check_ix])*(off_xmax-nextpos[0]) < 0) ) {

				// test punto che va a cadere dentro il volume (Bounding Box)
				// finora, l'unico che abbia senso
				if ( off_zmin<nextpos[2] && off_zmax>nextpos[2] ) {
					if ( off_ymin<nextpos[1] && off_ymax>nextpos[1] ) {
						if ( off_xmin<nextpos[0] && off_xmax>nextpos[0] ) {
					
							// equilibrio via trascinamento: sposto tutto il poligono
							
							tmp[0] = vel[check_ix];// + 0.25f * (vel[base_coord[0]] + vel[base_coord[3]] + vel[base_coord[6]] + vel[base_coord[9]]);
							tmp[1] = vel[check_iy];// + 0.25f * (vel[base_coord[1]] + vel[base_coord[4]] + vel[base_coord[7]] + vel[base_coord[10]]);
							tmp[2] = vel[check_iz];// + 0.25f * (vel[base_coord[2]] + vel[base_coord[5]] + vel[base_coord[8]] + vel[base_coord[11]]);
							/*
							tmp[0] = 0.25f * (vel[base_coord[0]] + vel[base_coord[3]] + vel[base_coord[6]] + vel[base_coord[9]]);
							if(abs(vel[check_ix])>abs(tmp[0])) tmp[0] = vel[check_ix];
							tmp[1] = 0.25f * (vel[base_coord[1]] + vel[base_coord[4]] + vel[base_coord[7]] + vel[base_coord[10]]);
							if(abs(vel[check_iy])>abs(tmp[1])) tmp[1] = vel[check_iy];
							tmp[2] = 0.25f * (vel[base_coord[2]] + vel[base_coord[5]] + vel[base_coord[8]] + vel[base_coord[11]]);
							if(abs(vel[check_iz])>abs(tmp[2])) tmp[2] = vel[check_iz];
							*/
							vnorm = 5.0f + (float)sqrt(tmp[0]*tmp[0] + tmp[1]*tmp[1] + tmp[2]*tmp[2]);
							tmp[0] /= vnorm;
							tmp[1] /= vnorm;
							tmp[2] /= vnorm;
							// aggiornamenti di velocità e posizione
							//for(j=0; j<12; j++) {
							//	vel[base_coord[j]] = 0.0f;
							//}
							pos[base_coord[0]] += tmp[0];
							pos[base_coord[3]] += tmp[0];
							pos[base_coord[6]] += tmp[0];
							pos[base_coord[9]] += tmp[0];
							pos[base_coord[1]] += tmp[1];
							pos[base_coord[4]] += tmp[1];
							pos[base_coord[7]] += tmp[1];
							pos[base_coord[10]] += tmp[1];
							pos[base_coord[2]] += tmp[2];
							pos[base_coord[5]] += tmp[2];
							pos[base_coord[8]] += tmp[2];
							pos[base_coord[11]] += tmp[2];
				}}}
			}
		}
	// passa al prossimo poligono
		for(k=0; k<12; k++) {
			base_coord[k] += 3;
		}
	}
}

////////////////////////////////////////////////////////////////////////////

bool CollisionManager::NodesConnected(int a, int b) {
	if ((a>b-2) && (a<b+2)) {
		return true;
	}
	if ((a>b+3*numnodesx-2) && (a<b+3*numnodesx+2)) {
		return true;
	}
	//if ((a>b+6*numnodesx-2) && (a<b+6*numnodesx+2)) {
	//	return true;
	//}
	if (a>3*numnodesx+1) {
		if ((a>b-3*numnodesx-2) && (a<b-3*numnodesx+2)) {
			return true;
		}
	}
	//if (a>6*numnodesx+1) {
	//	if ((a>b-6*numnodesx-2) && (a<b-6*numnodesx+2)) {
	//		return true;
	//	}
	//}
	return false;
}

// restituisce un indice di DOVE si trova approssimativamente un punto
int CollisionManager::CoordinatesHash(float x, float y, float z) {
	/*
	part_size = 1.0f;
	int hash = (int)floor(8*(4.0f + x)/(part_size*part_size) + (4.0f + y)/part_size);
	*/
	int hash = (int)floor(8*(4.0f + x) + (4.0f + y));
	return hash;
}

float CollisionManager::Aux_ReturnMax(float a, float b, float c, float d) {
	float t;
	t= (a>b) ? a : b;
	t= (c>t) ? c : t;
	t= (d>t) ? d : t;
	return t;
}

float CollisionManager::Aux_ReturnMin(float a, float b, float c, float d) {
	float t;
	t= (a<b) ? a : b;
	t= (c<t) ? c : t;
	t= (d<t) ? d : t;
	return t;
}

float CollisionManager::Aux_ReturnMean(float a, float b, float c, float d) {
	float t;
	t = (a+b+c+d) * 0.25f;
	return t;
}

float CollisionManager::normalize(float* v) {
	float f;
	f = (float)sqrt(v[0]*v[0] + v[1]*v[1] + v[2]*v[2]);
	v[0] /= f; v[1] /= f; v[2] /= f;
	return f;
}