using System;
using System.Collections;
using System.Text;

namespace TRender {
    class VolumeSubdivision {

        public float[] bounds;
        public float[] center;
        public float radius;

        public VolumeSubdivision() { }

        // Vertici contenuti nel volume
        public int reference_object;
        public ArrayList vertIndex;

        //
        public int[] GetVertIndexes() {
            return (int[])vertIndex.ToArray(typeof(int));
        }

        //
        public virtual void AddVertex(float[] v, int i) { }
        public virtual void RestartBounds() { }
        public virtual void RefreshBounds(float[] v) { }
        public virtual void RefreshCenter(float[] v) { }
        public virtual void RefreshVertex(float[] v) { }
        public virtual bool OverlapTo(VolumeSubdivision that) { return false; }
        public virtual float[] GetOverlaps(VolumeSubdivision that) { return null; }

    }

    //////////////////////////////////////////////////////////////////////////////

    class BoundingBox : VolumeSubdivision {
        
        //
        public BoundingBox(int obj_id) {
            reference_object = obj_id; // id dell'oggetto a cui si riferiscono i punti qui tracciati
            bounds = new float[6]; // -x -y -z +x +y +z
            // 14-DOP: 
            // bounds = new float[14]; // -x -y -z +x +y +z -x-y-z -x-y+z -x+y-z -x+y+z +x-y-z +x-y+z +x+y-z +x+y+z
            vertIndex = new ArrayList();
        }

        public BoundingBox(int obj_id, float[] v) {
            reference_object = obj_id;
            bounds = new float[6]; 
            bounds[0] = v[0];
            bounds[3] = v[0];
            bounds[1] = v[1];
            bounds[4] = v[1];
            bounds[2] = v[2];
            bounds[5] = v[2];
            vertIndex = new ArrayList();
        }

        //
        public override void AddVertex(float[] v, int i) {
            if(v[0] > bounds[3]) bounds[3] = v[0];
            if(v[0] < bounds[0]) bounds[0] = v[0];
            if(v[1] > bounds[4]) bounds[4] = v[1];
            if(v[1] < bounds[1]) bounds[1] = v[1];
            if(v[2] > bounds[5]) bounds[5] = v[2];
            if(v[2] < bounds[2]) bounds[2] = v[2];
            vertIndex.Add(i);
        }


        public override void RestartBounds() {
            bounds[0] = (bounds[0]+bounds[3])*0.5f;
            bounds[3] = bounds[0];
            bounds[1] = (bounds[1] + bounds[4]) * 0.5f;
            bounds[4] = bounds[1];
            bounds[2] = (bounds[2] + bounds[5]) * 0.5f;
            bounds[5] = bounds[2];
        }

        public override void RefreshBounds(float[] v) {
            bounds = new float[6];
            for(int i = 0; i < 6; i++) {
                bounds[i] = v[i];
            }
        }

        //
        public override void RefreshVertex(float[] v) {
            if(v[0] > bounds[3]) bounds[3] = v[0];
            if(v[0] < bounds[0]) bounds[0] = v[0];
            if(v[1] > bounds[4]) bounds[4] = v[1];
            if(v[1] < bounds[1]) bounds[1] = v[1];
            if(v[2] > bounds[5]) bounds[5] = v[2];
            if(v[2] < bounds[2]) bounds[2] = v[2];
        }

        public override bool OverlapTo(VolumeSubdivision that) {
            // sempre per l'AABB
            //Console.WriteLine("Debug\nX min di questo ogg: " + this.bounds[0] + "\nX max dell'altro ogg: " + that.bounds[3]);
            if(this.bounds[2] > that.bounds[5]) return false;
            if(that.bounds[2] > this.bounds[5]) return false;
        
            if(this.bounds[1] > that.bounds[4]) return false;
            if(that.bounds[1] > this.bounds[4]) return false;

            if(this.bounds[0] > that.bounds[3]) return false;
            if(that.bounds[0] > this.bounds[3]) return false;
            
            return true;
        }

        // misura di quanto si sovrappongono i volumi
        public override float[] GetOverlaps(VolumeSubdivision that) {
            int i;
            float[] delta_over = new float[3];

            for(i = 0; i < 3; i++) {
                //differenza tra il massimo dei minimi - il minimo dei massimi
                delta_over[i] = (this.bounds[i] > that.bounds[i] ? this.bounds[i] : that.bounds[i]) -
                                (this.bounds[i + 3] < that.bounds[i + 3] ? this.bounds[i + 3] : that.bounds[i + 3]);
            }
            return delta_over;
        }
    }



    //////////////////////////////////////////////////////////////////////////////

    class BoundingSphere : VolumeSubdivision {

        //
        public BoundingSphere(int obj_id) {
            reference_object = obj_id; // id dell'oggetto a cui si riferiscono i punti qui tracciati
            center = new float[3];
            radius = 0.2f;
            vertIndex = new ArrayList();
        }

        public BoundingSphere(int obj_id, float[] v) {
            reference_object = obj_id;

            center = new float[3];
            center[0] = v[0];
            center[1] = v[1];
            center[2] = v[2];
            radius = 0.09f;
            vertIndex = new ArrayList();
        }


        //
        public override void AddVertex(float[] v, int i) {
            int j;
            float tmp;
            for(j = 0; j < 3; j++) {
                center[j] = (center[j] + v[j]) / 2.0f;
            }
            
            tmp = distance(center, v);
            if(tmp > radius) {
                Console.WriteLine("    " + tmp );
                radius = 0.99f * tmp;
            }
            
            vertIndex.Add(i);
        }

        //
        public override void RefreshCenter(float[] v) {
            center[0] = v[0];
            center[1] = v[1];
            center[2] = v[2];
        }

        //
        public override void RefreshVertex(float[] v) {
            int j;
            //float tmp;
            for(j = 0; j < 3; j++) {
                center[j] = (center[j] + v[j]) / 2.0f;
            }
            //tmp = distance(center, v);
            //if(tmp > radius) radius = tmp;
        }

        public override bool OverlapTo(VolumeSubdivision that) {
            if(distance(this.center, that.center) > (2.0f * this.radius)) return false;
            return true;
        }

        // misura di quanto si sovrappongono i volumi
        public override float[] GetOverlaps(VolumeSubdivision that) {

            int i;
            float[] delta_over = new float[3];
            for(i = 0; i < 3; i++) {
                delta_over[i] = this.center[i] - that.center[i];
                if(Math.Abs(delta_over[i])>2*radius) {
                    delta_over[i] = 0.0f;
                } else {
                    delta_over[i] -= radius * 2.0f;
                    delta_over[i] *= Math.Abs(delta_over[i]);
                }
            }

            return delta_over;
        }

        /*
        public void Move(float dx, float dy, float dz) {
        }
        */

        public float distance(float[] p1, float[] p2) {
            return (float)(Math.Sqrt((p1[0]-p2[0])*(p1[0]-p2[0])+(p1[1]-p2[1])*(p1[1]-p2[1])+(p1[2]-p2[2])*(p1[2]-p2[2])));
        }
    }


//////////////////////////////////////////////////////////////////////////////

    class Bounding14DOP : VolumeSubdivision {
        //
        public Bounding14DOP(int obj_id) {
            reference_object = obj_id; // id dell'oggetto a cui si riferiscono i punti qui tracciati
            bounds = new float[14]; // -x -y -z +x +y +z -x-y-z -x-y+z -x+y-z -x+y+z +x-y-z +x-y+z +x+y-z +x+y+z
            vertIndex = new ArrayList();
        }

        public Bounding14DOP(int obj_id, float[] v) {
            reference_object = obj_id;
            bounds = new float[14];
            /* ? */
            vertIndex = new ArrayList();
        }

        /* .... e altri metodi ...*/
    }
}