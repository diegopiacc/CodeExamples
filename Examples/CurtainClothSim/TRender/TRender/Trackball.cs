using System;
using System.Collections.Generic;
using System.Text;

namespace TRender {
    public class Trackball {
        
        // coordinate della trackball
       
        //private float[] freezedSphereCoords;
        //private float[] freezedCartesianCoords;

        private float fdx, fdy;
        
        private float[] sphereCoords; // radius, phi, theta
        public float[] SphereCoords {
            get { return sphereCoords; }
            set { sphereCoords = value;
                  SphereToCartesian();
            }
        }

        private float[] cartesianCoords; // x,y,z
        public float[] CartesianCoords {
            get { return cartesianCoords; }
            set { cartesianCoords = value;
                  CartesianToSphere();
            }
        }

        // costruttori
        public Trackball() {
            //freezedSphereCoords = new float[3];
            sphereCoords = new float[3];
            //freezedCartesianCoords = new float[3];
            cartesianCoords = new float[3];
        }

        public Trackball(float r) {
            //freezedSphereCoords = new float[3];
            sphereCoords = new float[3];
            //freezedCartesianCoords = new float[3];
            cartesianCoords = new float[3];
            cartesianCoords[2] = r;
            CartesianToSphere();
        }

        public Trackball(float x, float y, float z) {
            sphereCoords = new float[3];
            cartesianCoords = new float[3];
            cartesianCoords[0] = x;
            cartesianCoords[1] = y;
            cartesianCoords[2] = z;
            CartesianToSphere();
        }

        // accessors per le singole coordinate cartesiane
        public float GetCameraDistance() {
            return sphereCoords[0];
        }
        public float GetCartesianX() {
            return cartesianCoords[0];
        }
        public float GetCartesianY() {
            return cartesianCoords[1];
        }
        public float GetCartesianZ() {
            return cartesianCoords[2];
        }


        // metodo per la risoluzione delle corrdinate da 2d-schermo a 3d-sulla tenda
        public float[] GetProjectionOnZ(float screenX, float screenY, float xyratio) {
            float[] coords = new float[3];
            float fov_sin = 0.258819f;
            float frustum_xmax, frustum_ymax;

            
            // traslazione delle coordinate nel dominio -1,1
            screenX = 2.0f * screenX - 1.0f;
            screenY = 2.0f * screenY - 1.0f;
            //
            frustum_ymax = sphereCoords[0] * fov_sin;
            //frustum_ymin = -frustum_ymax;
            frustum_xmax = frustum_ymax * xyratio;
            //frustum_xmin = -frustum_xmax;

            //
            coords[0] = frustum_xmax * screenX;
            coords[1] = frustum_ymax * screenY;
            coords[2] = 0.0f;

            // rotazioni
            // rotazione attorno all'asse y
            coords[2] = coords[2] * (float)Math.Cos(-sphereCoords[1]) - coords[0] * (float)Math.Sin(-sphereCoords[1]);
            coords[0] = coords[2] * (float)Math.Sin(-sphereCoords[1]) + coords[0] * (float)Math.Cos(-sphereCoords[1]);

            //
            return coords;
        }

        // metodi per la conversione dei due sistemi di coordinate
        public float[] SphereToCartesian() {
            cartesianCoords[0] = sphereCoords[0] * (float)Math.Sin(sphereCoords[1]) * (float)Math.Cos(sphereCoords[2]);
            cartesianCoords[1] = sphereCoords[0] * (float)Math.Sin(sphereCoords[1]) * (float)Math.Sin(sphereCoords[2]);
            cartesianCoords[2] = sphereCoords[0] * (float)Math.Cos(sphereCoords[1]);
            return cartesianCoords;
        }

        public float[] CartesianToSphere() {
            sphereCoords[0] = (float)Math.Sqrt(cartesianCoords[0] * cartesianCoords[0] + cartesianCoords[1] * cartesianCoords[1] + cartesianCoords[2] * cartesianCoords[2]);
            sphereCoords[1] = (float)Math.Acos(cartesianCoords[2] / sphereCoords[0]);
            sphereCoords[2] = (float)Math.Atan(cartesianCoords[1] / cartesianCoords[0]);
            return sphereCoords;
        }

        // utility per il tracking
        public void FreezeCoords(float dx, float dy) {
            //fdx = (dx * 2.0f) - 1.0f;
            //fdy = (dy * 2.0f) - 1.0f;
            fdx = dx;
            fdy = dy;
        }

        public void Track(float dx, float dy) {

            float alpha;

            // dx e dy = COORDINATE ASSOLUTE in 0..1 in 2D della posizione del mouse
            // ovvero non sono riferite all'ultima posizione in cui si è cliccato
           
            // shift nel dominio -1..1
            // dx = (dx * 2.0f) - 1.0f;
            // dy = (dy * 2.0f) - 1.0f;

            // rotazione attorno all'asse x
            alpha = (float)Math.Atan(dy - fdy);
            cartesianCoords[1] = cartesianCoords[1] * (float)Math.Cos(alpha) - cartesianCoords[2] * (float)Math.Sin(alpha);
            cartesianCoords[2] = cartesianCoords[1] * (float)Math.Sin(alpha) + cartesianCoords[2] * (float)Math.Cos(alpha);

            // rotazione attorno all'asse y
            alpha = (float)Math.Atan(fdx - dx);
            cartesianCoords[2] = cartesianCoords[2] * (float)Math.Cos(alpha) - cartesianCoords[0] * (float)Math.Sin(alpha);
            cartesianCoords[0] = cartesianCoords[2] * (float)Math.Sin(alpha) + cartesianCoords[0] * (float)Math.Cos(alpha);
            //
            CartesianToSphere();
        }

        public void Strafe(float dx, float dy) {
            sphereCoords[0] = sphereCoords[0] + 3.0f * ( fdy - dy);
            SphereToCartesian();
        }

        //
        private float CheckAngle180(float angle){
            return (float)(angle % Math.PI);
		}

        private float CheckAngle360(float angle) {
            return (float)(angle % (2.0f * Math.PI));
        }
        //

    }
}
