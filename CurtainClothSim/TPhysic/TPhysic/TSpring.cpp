// Parte della classe CTPhysic

#include "stdafx.h"
#include "TPhysic.h"

namespace TPhysic {
	void CTPhysic::ComputeSpring(int hing_ix, int mass_ix, float rest_size) {
		
		int hing_iy, hing_iz, mass_iy, mass_iz;
		float distance, delta, tmp_x, tmp_y, tmp_z;

		hing_iy = hing_ix+1;				
		hing_iz = hing_ix+2;				
		mass_iy = mass_ix+1;
		mass_iz = mass_ix+2;

		// lunghezza attuale della molla
		tmp_x = points_pos[hing_ix] - points_pos[mass_ix];
		tmp_y = points_pos[hing_iy] - points_pos[mass_iy];
		tmp_z = points_pos[hing_iz] - points_pos[mass_iz];
		distance = (float)sqrt(tmp_x*tmp_x + tmp_y*tmp_y + tmp_z*tmp_z);
		if( distance<REST_TOLERANCE ) { // molla a riposo
			tmp_x = 0.0f;
			tmp_y = 0.0f;
			tmp_z = 0.0f;
		} /*else if( distance> spring_max_length) { // punto di rottura
			tmp_x = 0.0f;
			tmp_y = 0.0f;
			tmp_z = 0.0f;
		} */

		// calcolo della forza tra i due nodi
		
		// semplificato!
		delta = properties->spring_k * (distance - rest_size) / distance;
		/*
		if(distance > (2.0f * rest_size)) {
			delta *= exp(2.0f * rest_size - distance);
		}
		*/

		tmp_x *= delta;
		tmp_y *= delta;
		tmp_z *= delta;
		// if(!anchor[hing_ix/3]) {
			points_for[hing_ix] -= tmp_x;
			points_for[hing_iy]	-= tmp_y;
			points_for[hing_iz]	-= tmp_z;
		// }
		// if(!anchor[mass_ix/3]) {
			points_for[mass_ix] += tmp_x;
			points_for[mass_iy]	+= tmp_y;
			points_for[mass_iz]	+= tmp_z;
		// }
	}

	void CTPhysic::ComputeLongSpring(int hing_ix, int mass_ix, float rest_size) {
		
		int hing_iy, hing_iz, mass_iy, mass_iz;
		float distance, delta, tmp_x, tmp_y, tmp_z;

		hing_iy = hing_ix+1;				
		hing_iz = hing_ix+2;				
		mass_iy = mass_ix+1;
		mass_iz = mass_ix+2;

		// lunghezza attuale della molla
		tmp_x = points_pos[hing_ix] - points_pos[mass_ix];
		tmp_y = points_pos[hing_iy] - points_pos[mass_iy];
		tmp_z = points_pos[hing_iz] - points_pos[mass_iz];
		distance = (float)sqrt(tmp_x*tmp_x + tmp_y*tmp_y + tmp_z*tmp_z);
		if( distance<REST_TOLERANCE ) { // molla a riposo
			tmp_x = 0.0f;
			tmp_y = 0.0f;
			tmp_z = 0.0f;
		} /*else if( distance> spring_max_length) { // punto di rottura
			tmp_x = 0.0f;
			tmp_y = 0.0f;
			tmp_z = 0.0f;
		} */

		// calcolo della forza tra i due nodi
		
		// semplificato!
		delta = properties->spring_stiff_k * (distance - rest_size) / distance;
		/*
		if(distance > (2.0f * rest_size)) {
			delta *= exp(2.0f * rest_size - distance);
		}
		*/

		tmp_x *= delta;
		tmp_y *= delta;
		tmp_z *= delta;
		// if(!anchor[hing_ix/3]) {
			points_for[hing_ix] -= tmp_x;
			points_for[hing_iy]	-= tmp_y;
			points_for[hing_iz]	-= tmp_z;
		// }
		// if(!anchor[mass_ix/3]) {
			points_for[mass_ix] += tmp_x;
			points_for[mass_iy]	+= tmp_y;
			points_for[mass_iz]	+= tmp_z;
		// }
	}







}