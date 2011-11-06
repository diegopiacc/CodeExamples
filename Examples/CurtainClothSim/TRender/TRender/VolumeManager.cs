using System;
using System.Collections;
using System.Text;

namespace TRender {
    class VolumeManager {
        
        private ArrayList v;

        public VolumeManager() {
            v = new ArrayList();            
        }

        public void Add(VolumeSubdivision vs) {
            v.Add(vs);
        }

        public void Add(ArrayList l) {
            v.AddRange(l);
        }

        public void Remove(int n) {
            v.RemoveRange(v.Count-n, n);
        }

        public int getVolN() {
            return v.Count;
        }

        public void RefreshAll(ArrayList ol) {
            int i;
            for(i = 0; i < v.Count; i++) {
                // refresh dei DOP
                ((Renderable)ol[((BoundingBox)v[i]).reference_object]).RefreshDop((BoundingBox)v[i]);
            }
        }

        public void CheckCD(ArrayList ol) {
            int i, j;
            int obj_ref_i, obj_ref_j;
            int[] verts_ref_i, verts_ref_j;
            float[] ov, vi, vj, vn, vni, vnj, ci, cj;
            float vimod, vjmod;
            //float vnmod, vnimod, vnjmod, ovmod, ovimod, ovjmod;
            vn = new float[3];
            vni = new float[3];
            vnj = new float[3];
            
            for(i = 0; i < v.Count; i++) {
                for(j = i+1; j < v.Count; j++) {
                    // Console.WriteLine("Test tra " + i + " e " + j);
                    if(((BoundingBox)v[i]).OverlapTo((BoundingBox)v[j])) {
                        // calcolo le dimensioni dell'overlap
                        ov = ((BoundingBox)v[i]).GetOverlaps((BoundingBox)v[j]);
                        //Console.WriteLine("Dimensioni dell'overlap: " + tmp[0] + ", " + tmp[1] + ", " + tmp[2]);

                        obj_ref_j = ((BoundingBox)v[j]).reference_object;
                        verts_ref_j = ((BoundingBox)v[j]).GetVertIndexes();
                        obj_ref_i = ((BoundingBox)v[i]).reference_object;
                        verts_ref_i = ((BoundingBox)v[i]).GetVertIndexes();
                        ci = ((Renderable)ol[obj_ref_i]).GetCenter(verts_ref_i);
                        ((BoundingBox)v[i]).RefreshCenter(ci);
                        cj = ((Renderable)ol[obj_ref_j]).GetCenter(verts_ref_j);
                        ((BoundingBox)v[j]).RefreshCenter(cj);
                        vi = ((Renderable)ol[obj_ref_i]).GetAvgVel(verts_ref_i);
                        vj = ((Renderable)ol[obj_ref_j]).GetAvgVel(verts_ref_j);
                        vimod = Module(vi);
                        vjmod = Module(vj);
                        
                        // spostamenti basati su overlap
                        if(vimod > vjmod) {
                            if(ov[0] < ov[1] && ov[0] < ov[2]) {
                                ((Renderable)ol[obj_ref_j]).AddVelToPoints(verts_ref_j, -ov[0], 0, 0);
                            } else if(ov[1] < ov[0] && ov[1] < ov[2]) {
                                ((Renderable)ol[obj_ref_j]).AddVelToPoints(verts_ref_j, 0, -ov[1], 0);
                            } else if(ov[2] < ov[1] && ov[2] < ov[0]) {
                                ((Renderable)ol[obj_ref_j]).AddVelToPoints(verts_ref_j, 0, 0, -ov[2]);
                            }
                        } else {
                            if(ov[0] < ov[1] && ov[0] < ov[2]) {
                                ((Renderable)ol[obj_ref_i]).AddVelToPoints(verts_ref_i, ov[0], 0, 0);
                            } else if(ov[1] < ov[0] && ov[1] < ov[2]) {
                                ((Renderable)ol[obj_ref_i]).AddVelToPoints(verts_ref_i, 0, ov[1], 0);
                            } else if(ov[2] < ov[1] && ov[2] < ov[0]) {
                                ((Renderable)ol[obj_ref_i]).AddVelToPoints(verts_ref_i, 0, 0, ov[2]);
                            }
                        }
                        
                        /*
                        // collision avoidance basata su sola componente NORMALE all'urto
                        // calcolo della velocità normale e tangenziale
                        for(k = 0; k < 3; k++) {
                            vn[k] = ci[k] - cj[k];
                        }
                        vnmod = Module(vn);
                        vnimod = vi[0]*vn[0] + vi[1]*vn[1] + vi[2]*vn[2];
                        vnjmod = vj[0]*vn[0] + vj[1]*vn[1] + vj[2]*vn[2];

                        for(k = 0; k < 3; k++) {
                            vni[k] = -vn[k] * vnimod / vnmod;
                            //vni[k] = vnmod * ov[k];
                            //vti[k] = vi[k] - vni[k];

                            vnj[k] = vn[k] * vnjmod / vnmod;
                            //vnj[k] = -vnmod * ov[k];
                            //vtj[k] = vj[k] - vni[k];
                        }
                        ((Renderable)ol[obj_ref_i]).AddVelToPoints(verts_ref_i, vni[0], vni[1], vni[2]);
                        ((Renderable)ol[obj_ref_j]).AddVelToPoints(verts_ref_j, vnj[0], vnj[1], vnj[2]);
                        
                                       
                        // collision avoidance basata su velocità media
                        ((Renderable)ol[obj_ref_j]).AddVelToPoints(verts_ref_j, -vj[0], -vj[1], -vj[2]);
                        ((Renderable)ol[obj_ref_i]).AddVelToPoints(verts_ref_i, -vi[0], -vi[1], -vi[2]);
                        */
                    }
                }
            }
        }


        public float Module(float[] v) {
            int i;
            float tmp = 0.0f;
            for(i = 0; i < v.Length; i++)
                tmp += v[i] * v[i];
            return (float)Math.Sqrt(tmp);
        }

    }
}
