// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the TPHYSIC_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// TPHYSIC_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef TPHYSIC_EXPORTS
#define TPHYSIC_API __declspec(dllexport)
#else
#define TPHYSIC_API __declspec(dllimport)
#endif

#include <stdio.h>
#include <malloc.h>
#include <math.h>
#include "SysProperties.h"
#include "CollisionManager.h"

namespace TPhysic {

	// This class is exported from the TPhysic.dll
	public ref class CTPhysic {

		public:
			
			// caratteristiche fisiche del sistema
			SysProperties* properties;

			//
			bool check_collision;
			//CollisionManager* cmanager;

			// caratteristiche fisiche dell'oggetto
			int numnodesx, numnodesy, numnodes, numcoords;
			float spring_rest_diag, spring_rest_x, spring_rest_y;
			float critical_force;
			float spring_max_length;
			
			// informazioni sui nodi dell'oggetto
			float* points_pos;
			float* points_nrm;
			float* points_vel;
			float* points_for;
			int* anchor;	// nodi che hanno quetso valore !0 sono ancore

			// funzioni 
			CTPhysic(void);
			char* ReturnVersion(void);
			void SetAwningPoints(int, int, float*);
			void SetAnchors(int, int*);
			void GetVertexCoords(int, int, float*);
			// metodi di accesso alle informazioni sui vertici
			float GetVertexCoord(int, int, int);
			float GetVertexCoord(int);
			float GetVertexNormal(int, int, int);
			float GetVertexNormal(int);
			float GetVertexVelocity(int, int, int);
			float GetVertexVelocity(int);

			void AddForceToPoint(int, float, float, float);
			void AddVelToPoint(int, float, float, float);
			void MovePoint(int, float, float, float);
			void MoveAnchor(int, float, float);
			void MoveAllPoints(float, float, float);
			int FindNodeNear(float, float, float, float);
			void SetCollisionDetection(int);
			void AddAnchorAtIndex(int);
			void CleanAnchorAtIndex(int);
			
			//
			void StepSim_Reset(void);
			void StepSim_Compute(void);
			void StepSim_Apply(void);

			// implementazione nel file TSpring
			void ComputeSpring(int, int, float);
			void ComputeLongSpring(int, int, float);

			// implementazione nel file T3DIndexer
			//int ComputeTriangleIndexes(int);
	};
}