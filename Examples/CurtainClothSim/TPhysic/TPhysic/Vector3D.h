#ifndef _VECTOR3D_H_
#define _VECTOR3D_H_
#include "math.h"

class Vector3D
{
public:
	float x, y, z;
	bool anchor;

	Vector3D(){
		x = 0.0f;
		y = 0.0f;
		z = 0.0f;
		anchor = false;
	};

	Vector3D(float x, float y, float z): x(x), y(y), z(z){	};

	void setAnchor(bool a) {
		anchor = a;
	};

	bool isAnchor() {
		return anchor;
	};

	Vector3D& operator= (Vector3D v){
		x = v.x;
		y = v.y;
		z = v.z;
		return *this;
	};

	Vector3D operator+ (Vector3D v)	{
		return Vector3D(x + v.x, y + v.y, z + v.z);
	};

	Vector3D operator- (Vector3D v)	{
		return Vector3D(x - v.x, y - v.y, z - v.z);
	};

	Vector3D operator* (float value){
		return Vector3D(x * value, y * value, z * value);
	};

	Vector3D operator/ (float value){
		return Vector3D(x / value, y / value, z / value);
	};

	Vector3D operator- (float value){
		return Vector3D(x - value, y - value, z - value);
	};

	Vector3D& operator+= (Vector3D v){
		x += v.x;
		y += v.y;
		z += v.z;
		return *this;
	};

	Vector3D& operator-= (Vector3D v){
		x -= v.x;
		y -= v.y;
		z -= v.z;
		return *this;
	};

	BOOL operator!= (Vector3D v){
		if (x != v.x || y != v.y || z != v.z)
			return true;
		else 
			return false;
	};

	BOOL operator== (Vector3D v){
		if (x == v.x || y == v.y || z == v.z)
			return true;
		else 
			return false;
	};

	Vector3D& operator*= (float value){
		x *= value;
		y *= value;
		z *= value;
		return *this;
	};

	Vector3D& operator/= (float value){
		x /= value;
		y /= value;
		z /= value;
		return *this;
	};

	Vector3D operator- (){
		return Vector3D(-x, -y, -z);
	};

	float length(){
		return sqrtf(x*x + y*y + z*z);
	};

	float length(Vector3D v){
		return sqrtf(pow(x-v.x,2) + pow(y-v.y,2) + pow(z-v.z,2));
	};	

	float innerProduct(Vector3D v){
		return x*v.x + y*v.y + z*v.z;
	};

	void init(){
		this->x = 0.0;
		this->y = 0.0;
		this->z = 0.0;
	};

	void normalize(){
		float length = this->length();
		if (length == 0)
			return;
		x /= length;
		y /= length;
		z /= length;
	};

protected:

};

#endif