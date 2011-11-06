using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection.Emit;
using System.Reflection;

using TPhysic;

namespace TRender {
    class PhysicWrapper {

        private CTPhysic physic;

        // costruttori
        public PhysicWrapper() {
            physic = new CTPhysic();
        }
        
//////////////////////////////////////////////////////////////////////////////

        public float[] GetVertexCoords(int ix, int iy) {
            float[] ret = { 0.0f, 0.0f, 0.0f };
            try {
                ret[0] = physic.GetVertexCoord(ix, iy, 0);
                ret[1] = physic.GetVertexCoord(ix, iy, 1);
                ret[2] = physic.GetVertexCoord(ix, iy, 2);
            } catch(Exception e) {
                 Console.WriteLine(e.Message);
            }
            return ret;
        }

        public float[] GetVertexCoords(int index) {
            float[] ret = { 0.0f, 0.0f, 0.0f };
            try {
                ret[0] = physic.GetVertexCoord(index); index++;
                ret[1] = physic.GetVertexCoord(index); index++;
                ret[2] = physic.GetVertexCoord(index);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            return ret;
        }

        public float[] GetVertexNormal(int ix, int iy) {
            float[] ret = { 0.0f, 0.0f, 0.0f };
            try {
                ret[0] = physic.GetVertexNormal(ix, iy, 0);
                ret[1] = physic.GetVertexNormal(ix, iy, 1);
                ret[2] = physic.GetVertexNormal(ix, iy, 2);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            return ret;
        }

        public float[] GetVertexVelocity(int ix, int iy) {
            float[] ret = { 0.0f, 0.0f, 0.0f };
            try {
                ret[0] = physic.GetVertexVelocity(ix, iy, 0);
                ret[1] = physic.GetVertexVelocity(ix, iy, 1);
                ret[2] = physic.GetVertexVelocity(ix, iy, 2);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            return ret;
        }

        public float[] GetVertexVelocity(int index) {
            float[] ret = { 0.0f, 0.0f, 0.0f };
            try {
                ret[0] = physic.GetVertexVelocity(index); index++;
                ret[1] = physic.GetVertexVelocity(index); index++;
                ret[2] = physic.GetVertexVelocity(index);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            return ret;
        }

//////////////////////////////////////////////////////////////////////////////

        public unsafe void MoveAllPoints(float x, float y, float z) {
            physic.MoveAllPoints(x,y,z);
        }

        public unsafe void MovePoint(int index, float x, float y, float z) {
            physic.MovePoint(index, x, y, z); // agisce sulla posizione
        }

        public unsafe void AddVelToPoint(int index, float x, float y, float z) {
            physic.AddVelToPoint(index, x, y, z); // agisce sulla velocità
        }

        public unsafe void AddForceToPoint(int index, float x, float y, float z) {
            physic.AddForceToPoint(index, x, y, z); // agisce sulla forza
        }

        public unsafe void SetAwningPoints(int nodesx, int nodesy, float[] points) {
            int i;
            //int psize = points.GetLength(0);
            int psize = nodesx * nodesy * 3;
            int numanchors = 4;

            try {
                // coordinate di tutti i nodi della tenda
                float* parray = stackalloc float[psize];
                for(i = 0; i < psize; i++) {
                    parray[i] = points[i];
                }
                physic.SetAwningPoints(nodesx, nodesy, parray);
                //Console.WriteLine("Numero di nodi x e y: " + nodesx + ", " + nodesy);
                //Console.WriteLine("dim array: " + psize);

                // indici dei nodi che fungono da àncore
                int* anchor = stackalloc int[numanchors];
                anchor[0] = nodesx * (nodesy - 1);
                anchor[1] = nodesx * (nodesy - 1) + (nodesx / 3);
                anchor[2] = nodesx * (nodesy - 1) + (nodesx * 2 / 3);
                anchor[3] = (nodesx * nodesy) - 1;

                physic.SetAnchors(4, anchor);

            } catch (Exception e) {
                 Console.WriteLine(e.Message);
            }
        }

        public int FindPointsNear(float approxX, float approxY, float dist) {
            int found = physic.FindNodeNear(approxX, approxY, 0.0f, dist);
            // Console.WriteLine(" indice " + found); // ok
            return found;
        }

        //
        public void moveMouseAnchor(int ancfound, float mx, float my) {
            physic.MoveAnchor(ancfound, mx, my);
            //physic.MovePoint(ancfound, mx, my, 0);
        }

        public void SetAnchor(int a) {
            physic.AddAnchorAtIndex(a);
        }

        public void ResetAnchor(int a) {
            physic.CleanAnchorAtIndex(a);
        }

        //
        public void StepSim_Compute() {
            physic.StepSim_Compute();
        }

        public void StepSim_Apply() {
            physic.StepSim_Apply();
        }

        public void StepSim_Reset() {
            physic.StepSim_Reset();
        }
    }
}
