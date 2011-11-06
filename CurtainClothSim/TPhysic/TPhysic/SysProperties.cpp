#include "StdAfx.h"
#include "SysProperties.h"

SysProperties::SysProperties(void)
{
}

SysProperties::SysProperties(int type) {

	mass = 3.0f;
	energy_loss = 0.88f;
	spring_k = 0.7f;//1.2f;
	spring_stiff_k = 0.2f;
	spring_max_length = 0.1f;

	if (type==STANDARD) {
		gravity_y = -0.0098f;

		airRes = 0.02f;
		gRepulsion = 100.0f;
		gResistance = 0.2f;
		gAbsorb = 3.0f;
	}
}



SysProperties::~SysProperties(void){
	
}
