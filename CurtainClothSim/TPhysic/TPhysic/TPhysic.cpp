// TPhysic.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "TPhysic.h"

#ifdef _MANAGED
#pragma managed(push, off)
#endif

namespace TPhysic {

	BOOL APIENTRY DllMain( HMODULE hModule,
						   DWORD  ul_reason_for_call,
						   LPVOID lpReserved
						 )
	{
		switch (ul_reason_for_call)
		{
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
			break;
		}
		return TRUE;
	}

	#ifdef _MANAGED
	#pragma managed(pop)
	#endif

	// This is the constructor of a class that has been exported.
	// see TPhysic.h for the class definition
	CTPhysic::CTPhysic()
	{
		properties = new SysProperties(STANDARD);
		//cmanager = new CollisionManager();
		check_collision = true;
		return;
	}

	//
	void CTPhysic::SetAwningPoints(int nx, int ny, float *p) {
		int i;
		numnodesx = nx;
		numnodesy = ny;
		numnodes = nx*ny;
		numcoords = 3*nx*ny;
		points_pos = new float[numcoords];
		points_nrm = new float[numcoords];
		points_vel = new float[numcoords];
		points_for = new float[numcoords];

		anchor = new int[numnodes];

		for(i=0; i<numcoords; i++) {
			points_pos[i] = p[i];
			points_nrm[i] = 0.0f;
			points_vel[i] = 0.0f;
			points_for[i] = 0.0f;
		}
		for(i=0; i<numnodes; i++) {
			anchor[i] = 0;
		}

		spring_rest_x = (float)abs( points_pos[0] - points_pos[3]);
		spring_rest_y = (float)abs( points_pos[1] - points_pos[3*numnodesx+1]);
		spring_rest_diag = (float)sqrt(spring_rest_x*spring_rest_x+spring_rest_y*spring_rest_y);
		//cmanager->setMeasures(numnodesx, spring_rest_x, numnodesy, spring_rest_y, numnodes);
		spring_max_length = 3.5f * spring_rest_diag;
	}

	// movimento forzato dei punti, in massa e singolarmente
	void CTPhysic::MoveAllPoints(float dx, float dy, float dz) {
		int ii, n;
		for(ii=0; ii<numnodes; ii++) {
			n = 3*ii;
			//aggiorna posizione
			points_pos[n] += dx;
			n++;
			points_pos[n] += dy;
			n++;
			points_pos[n] += dz;
		}
	}

	void CTPhysic::AddForceToPoint(int ind, float dx, float dy, float dz) {
		int baseindex = 3*ind;
		points_for[baseindex] += dx;
		baseindex++;
		points_for[baseindex] += dy;
		baseindex++;
		points_for[baseindex] += dz;
	}

	void CTPhysic::AddVelToPoint(int ind, float dx, float dy, float dz) {
		int baseindex = 3*ind;
		points_vel[baseindex] += dx;
		baseindex++;
		points_vel[baseindex] += dy;
		baseindex++;
		points_vel[baseindex] += dz;
	}

	void CTPhysic::MovePoint(int ind, float dx, float dy, float dz) {
		int baseindex = 3*ind;
		//points_vel[baseindex] += dx - points_pos[baseindex];
		points_vel[baseindex] = 0.0f;
		points_pos[baseindex] += dx;
		baseindex++;
		//points_vel[baseindex] += dy - points_pos[baseindex];
		points_vel[baseindex] = 0.0f;
		points_pos[baseindex] += dy;
		baseindex++;
		//points_vel[baseindex] += dz - points_pos[baseindex];
		points_vel[baseindex] = 0.0f;
		points_pos[baseindex] += dz;
	}


	void CTPhysic::MoveAnchor(int ind, float ax, float ay) {
		int baseindex = 3*ind;
		float tmp;
		
		// points_vel[baseindex] = 0.0f;
		tmp = points_vel[baseindex];
		points_vel[baseindex] = ax - points_pos[baseindex];
		points_for[baseindex] = properties->mass * (tmp - points_vel[baseindex]);
		points_pos[baseindex] = ax;
		
		// points_vel[baseindex+1] = 0.0f;
		tmp = points_vel[baseindex+1];
		points_vel[baseindex+1] = ay - points_pos[baseindex+1];
		points_for[baseindex+1] = properties->mass * (tmp - points_vel[baseindex]);
		points_pos[baseindex+1] = ay;
	}

	//
	void CTPhysic::SetAnchors(int num, int *a) {
		int i;
		for(i=0; i<num; i++) {
			anchor[a[i]] = 1;
		}
	}

	// interazioni di base: scambio dati con modulo superiore
	void CTPhysic::GetVertexCoords(int ix, int iy, float* coords) {
		int baseindex = (3*numnodesx*iy) + 3*ix;
		coords[0] = points_pos[baseindex];     
		coords[1] = points_pos[baseindex+1];
		coords[2] = points_pos[baseindex+2];
	}

/////////////////////////////////////////////////////////////////////////////

	float CTPhysic::GetVertexCoord(int ix, int iy, int xyz) {
		int baseindex = (3*numnodesx*iy) + 3*ix + xyz;
		return points_pos[baseindex];
	}

	float CTPhysic::GetVertexCoord(int baseindex) {
		return points_pos[baseindex];
	}

	float CTPhysic::GetVertexNormal(int ix, int iy, int xyz) {
		int baseindex = (3*numnodesx*iy) + 3*ix + xyz;
		return points_nrm[baseindex];
	}

	float CTPhysic::GetVertexNormal(int baseindex) {
		return points_nrm[baseindex];
	}

	float CTPhysic::GetVertexVelocity(int ix, int iy, int xyz) {
		int baseindex = (3*numnodesx*iy) + 3*ix + xyz;
		return points_vel[baseindex];
	}

	float CTPhysic::GetVertexVelocity(int baseindex) {
		return points_vel[baseindex];
	}

//////////////////////////////////////////////////////////////////////////////

	char* CTPhysic::ReturnVersion() {
		return "Engine Fisico v0.1";
	}

/////////////////////////////////////////////////////////////////////////////

	// reset delle forze
	void CTPhysic::StepSim_Reset() {
		int ii;
		for(ii=0; ii<3*numnodes; ii++) {
			points_for[ii] = 0.0f;
		}
	}


	//calcolo delle forze
	void CTPhysic::StepSim_Compute(void) {
		int ii, ij;// ik, nn;
		int hing_ix, mass_ix;
		float tmp;
		//int* tmpindexes = NULL;
		
		// substep 1: ricalcolo delle normali
		for(ij=0; ij<numnodesy-1; ij++) {
			for(ii=0; ii<numnodesx-1; ii++) {
				hing_ix = (3*numnodesx*ij) + 3*ii;
				points_nrm[hing_ix] = (points_pos[hing_ix+6] - points_pos[hing_ix]) * 
									(points_pos[hing_ix+5] - points_pos[hing_ix+2]) * (points_pos[hing_ix+4] - points_pos[hing_ix+7]) - 
									(points_pos[hing_ix+4] - points_pos[hing_ix+1]) * (points_pos[hing_ix+5] - points_pos[hing_ix+8]);
				points_nrm[hing_ix+1] =  (points_pos[hing_ix+7] - points_pos[hing_ix+1]) * 
									(points_pos[hing_ix+3] - points_pos[hing_ix]) * (points_pos[hing_ix+5] - points_pos[hing_ix+8]) - 
									(points_pos[hing_ix+5] - points_pos[hing_ix+2]) * (points_pos[hing_ix+3] - points_pos[hing_ix+6]);
				points_nrm[hing_ix+2] = (points_pos[hing_ix+8] - points_pos[hing_ix+2]) * 
									(points_pos[hing_ix+4] - points_pos[hing_ix+1]) * (points_pos[hing_ix+3] - points_pos[hing_ix+6]) - 
									(points_pos[hing_ix+3] - points_pos[hing_ix]) * (points_pos[hing_ix+4] - points_pos[hing_ix+7]);
				tmp = (float)sqrt(points_nrm[hing_ix]*points_nrm[hing_ix] + points_nrm[hing_ix+1]*points_nrm[hing_ix+1] + points_nrm[hing_ix+2]*points_nrm[hing_ix+2]);
				// opzionale: rivolto le normali verso l'osservatore
				if(points_nrm[hing_ix+2]<0)
					tmp *= -1.0f;
				points_nrm[hing_ix] /= tmp;
				points_nrm[hing_ix+1] /= tmp;
				points_nrm[hing_ix+2] /= tmp;
				//
			}
			// ultima colonna
			points_nrm[hing_ix+3] = points_nrm[hing_ix];
			points_nrm[hing_ix+4] = points_nrm[hing_ix+1];
			points_nrm[hing_ix+5] = points_nrm[hing_ix+2];
		}
		
		// substep 2: calcolo le forze						//    1-2-3
		for(ij=0; ij<numnodesy; ij++) {						//    |\|/|
			for(ii=0; ii<numnodesx; ii++) {					//    4-X-5
				hing_ix = (3*numnodesx*ij) + 3*ii;			//    |/|\|
				// X<->5									//    6-7-8
				if(ii!=numnodesx-1) {
					mass_ix = hing_ix+3; 								
					ComputeSpring(hing_ix, mass_ix, spring_rest_x);
					// X<->5++
					if(ii!=numnodesx-2) {
						mass_ix = hing_ix+6; 								
						ComputeLongSpring(hing_ix, mass_ix, 2.0f*spring_rest_x);
						// Spring(points_for, hing_ix, mass_ix, spring_rest_x));
					}
				}
					
				if(ij!=numnodesy-1) {
					// X<->2--
					if(ij!=numnodesy-2) {
						mass_ix = hing_ix+6*numnodesx; 								
						ComputeLongSpring(hing_ix, mass_ix, 2.0f*spring_rest_y);
					}
					// X<->2
					mass_ix = hing_ix+3*numnodesx; 	
					ComputeSpring(hing_ix, mass_ix, spring_rest_y);
					if(ii!=0) {
					// X<->1	
					mass_ix = hing_ix+3*(numnodesx-1); 	
					ComputeSpring(hing_ix, mass_ix, spring_rest_diag);
					}
					if(ii!=numnodesx-1) {
					// X<->3	
					mass_ix = hing_ix+3*(1+numnodesx); 	
					ComputeSpring(hing_ix, mass_ix, spring_rest_diag);
					}
				}
			}
		}
	}

	// 
	void CTPhysic::StepSim_Apply(void) {
		int ii, ij;// ik, nn;
		int hing_ix, hing_iy, hing_iz;
		float critical_vel;

		// substep 1: calcolo delle velocità
		for(ij=0; ij<numnodesy; ij++) {
			for(ii=0; ii<numnodesx; ii++) {
				hing_ix = (3*numnodesx*ij) + 3*ii;
				hing_iy = hing_ix+1;
				hing_iz = hing_iy+1;
				if(anchor[(numnodesx*ij)+ii]==0) {
					// gravità
					points_for[hing_iy] += properties->gravity_y;
					// a = dv = F/m;
					points_vel[hing_ix] += (points_for[hing_ix] / properties->mass);
					points_vel[hing_iy] += (points_for[hing_iy] / properties->mass);
					points_vel[hing_iz] += (points_for[hing_iz] / properties->mass);
				}
			}
		}

		// substep 2: check della superelasticità

		critical_vel = 0.08f;
		for(ii=0; ii<numnodes; ii++) {
			hing_ix = 3*ii;
			hing_iy = hing_ix+1;
			hing_iz = hing_ix+2;
			if(points_vel[hing_ix] > critical_vel)
				points_vel[hing_ix] = critical_vel;
			if(points_vel[hing_ix] < -critical_vel)
				points_vel[hing_ix] = -critical_vel;
			if(points_vel[hing_iy] > critical_vel)
				points_vel[hing_iy] = critical_vel;
			if(points_vel[hing_iy] < -critical_vel)
				points_vel[hing_iy] = -critical_vel;
			if(points_vel[hing_iz] > critical_vel)
				points_vel[hing_iz] = critical_vel;
			if(points_vel[hing_iz] < -critical_vel)
				points_vel[hing_iz] = -critical_vel;
		}

		// substep 3: applicazione effettiva degli spostamenti
		for(ii=0; ii<numnodes; ii++) {
			if(anchor[ii]==0) {
				hing_ix = 3*ii;
				hing_iy = hing_ix+1;
				hing_iz = hing_ix+2;
				//aggiorna posizione
				points_pos[hing_ix] += points_vel[hing_ix];
				points_pos[hing_iy] += points_vel[hing_iy];
				points_pos[hing_iz] += points_vel[hing_iz];
			}
			// smorza energia
			points_vel[hing_ix] *= properties->energy_loss;
			points_vel[hing_iy] *= properties->energy_loss;
			points_vel[hing_iz] *= properties->energy_loss;
		}	
	}

	//
	int CTPhysic::FindNodeNear(float ax, float ay, float az, float radius) {
		int ii, baseindex;
		for(ii=0; ii<numnodes; ii++) {
			baseindex = 3*ii;
			if(abs(points_pos[baseindex] - ax) < radius){
				if(abs(points_pos[baseindex+1] - ay) < radius){
					//if(abs(points_pos[baseindex+2] - az) < 0.1f){
					return ii;
					//}
				}
			}
		}
		return -1;
	}
	
	//
	void CTPhysic::SetCollisionDetection(int cd) {
		if(cd==0) {
			check_collision = false;
		} else if (cd==1) {
			check_collision = true;
		}
	}
	//
	void CTPhysic::AddAnchorAtIndex(int aind) {
		anchor[aind] = 1;
	}

	void CTPhysic::CleanAnchorAtIndex(int aind) {
		anchor[aind] = 0;
	}
}