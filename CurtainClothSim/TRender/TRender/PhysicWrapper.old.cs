using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection.Emit;
using System.Reflection;

namespace TRender {
    class PhysicWrapper {

        // costruttori
        public PhysicWrapper() {
            DllName = "TPhysic.dll";
            InitPhysic("TPhysic.dll");
            wrapp = new object();
           }

        public PhysicWrapper(string dllname) {
            DllName = dllname;
            InitPhysic(dllname);
            wrapp = new object();
        }

        public int InitPhysic(string dllname) {
        }


        // metodo per ottenere il nome della versione della dll
        public string GetDllVersion() {
            string error = "Error";
            try {

            } catch(Exception e) {

            }
            return error;
        }

        // settaggio dei parametri del modello della superficie 
        // (DA CHIAMARE SOLO 1 VOLTA)
        public int SetPhysicParam(float height, float length, int anchor_number, int open_type, 
		float mass, float stiffness, float sheer, float bend, float energy_loss) {
        }

        // settaggio dati per motore fisico
        // (DA CHIAMARE SOLO 1 VOLTA)
        public int SetAwningPoints(int nodesx, int nodesy, float[,,] points) {
        }



        // restituisce le coordinate di un vertice
        public float[] GetVertexCoords(int ix, int iy) {
        }

    }
}
