using System;
using System.Drawing;
using System.Collections;
using System.Text;

using Tao.OpenGl;

namespace TRender {
    class RenderableCloth : Renderable {
        //private Texture t;
        private RenderableVertex mouse_anchor;
        private PhysicWrapper pw;
        private TextureManager tm;

        private bool wireframe;
        private int ancfound, nodesx, nodesy, numnodes, numcoords, texture_id;
        private float nodesizex, nodesizey;
        private float[] points;
        
        private float mouseanchor_x, mouseanchor_y;

        public int Texture_id {
            get { return texture_id; }
            set { texture_id = value; }
        }

        public RenderableCloth(float sizex, float sizey, float posx, float posy, float posz) : base() {
            wireframe = false;
            mouse_anchor = new RenderableVertex();
            pw = new PhysicWrapper();
            tm = TextureManager.Instance;
            texture_id = 1;
            setAwningPoints(sizex, sizey, posx, posy, posz);
            //
            float[] tmp = new float[3] { 0, 0, 0 };

            //dop = new VolumeSubdivision(tmp);
            //InitDop();
        }

        public override void Render() {
            int index_thisrow, index_nextrow, index_thiscol, index_nextcol;
            float ti1, ti2, ti3;
            
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tm.texture[texture_id]);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            
            if(wireframe) {

                for(index_thisrow = 0; index_thisrow < (nodesx - 1); index_thisrow++) {
                    index_nextrow = index_thisrow + 1;
                    //rowj = 3*j*nodesx;
                    //nextrowj = 3*(j+1)*nodesx;

                    for(index_thiscol = 0; index_thiscol < (nodesy - 1); index_thiscol++) {
                        index_nextcol = index_thiscol + 1;
                        Gl.glBegin(Gl.GL_LINE_LOOP);
                        Gl.glVertex3fv(pw.GetVertexCoords(index_thisrow, index_thiscol));
                        Gl.glVertex3fv(pw.GetVertexCoords(index_nextrow, index_thiscol));
                        Gl.glVertex3fv(pw.GetVertexCoords(index_nextrow, index_nextcol));
                        Gl.glVertex3fv(pw.GetVertexCoords(index_thisrow, index_nextcol));
                        Gl.glEnd();
                    }
                }
            } else {
                for(index_thisrow = 0; index_thisrow < (nodesx-1); index_thisrow++) {
                    index_nextrow = index_thisrow + 1;
                    ti1 = (float)index_thisrow / (float)nodesx;
                    ti2 = (float)index_nextrow / (float)nodesx;
                    Gl.glBegin(Gl.GL_QUAD_STRIP);
                    for(index_thiscol = 0; index_thiscol < nodesy; index_thiscol++) {
                        ti3 = (float)index_thiscol / (float)nodesy;
                        Gl.glTexCoord2f(ti1, ti3);
                        Gl.glNormal3fv(pw.GetVertexNormal(index_thisrow, index_thiscol));
                        Gl.glVertex3fv(pw.GetVertexCoords(index_thisrow, index_thiscol));
                        Gl.glTexCoord2f(ti2, ti3);
                        Gl.glNormal3fv(pw.GetVertexNormal(index_nextrow, index_thiscol));
                        Gl.glVertex3fv(pw.GetVertexCoords(index_nextrow, index_thiscol));
                    }
                    Gl.glEnd();
                }
            }

            if(mouse_anchor.visible) {
                mouse_anchor.Position = pw.GetVertexCoords(ancfound % nodesx, ancfound / nodesx);
                mouse_anchor.Render();
            }
        }

        //
        public override float[] GetAvgVel(int[] points) {
            float[] ret = new float[3];
            float[] tmp;
            int i;
            int l = points.GetLength(0);
            for(i = 0; i < l; i++) {
                tmp = pw.GetVertexVelocity(points[i]);
                ret[0] += tmp[0];
                ret[1] += tmp[1];
                ret[2] += tmp[2];
            }
            ret[0] /= l;
            ret[1] /= l;
            ret[2] /= l;
            return ret;
        }

        //
        public override float[] GetCenter(int[] points) {
            float[] ret = new float[3];
            float[] tmp;
            int i;
            int l = points.GetLength(0);
            for(i = 0; i < l; i++) {
                tmp = pw.GetVertexCoords(points[i]);
                ret[0] += tmp[0];
                ret[1] += tmp[1];
                ret[2] += tmp[2];
            }
            ret[0] /= l;
            ret[1] /= l;
            ret[2] /= l;
            return ret;
        }

        public override void Move(float x, float y, float z) {
            pw.MoveAllPoints(x,y,z);
        }

        public override void MovePoints(int[] verts, float x, float y, float z) {
            int i;
            for(i = 0; i < verts.GetLength(0); i++) {
                pw.MovePoint(verts[i], x, y, z);
            }
        }

        public override void AddVelToPoints(int[] verts, float x, float y, float z) {
            int i;
            for(i = 0; i < verts.GetLength(0); i++) {
                pw.AddVelToPoint(verts[i], x, y, z);
            }
        }

        public override void AddForceToPoints(int[] verts, float x, float y, float z) {
            int i;
            for(i = 0; i < verts.GetLength(0); i++) {
                pw.AddForceToPoint(verts[i], x, y, z);
            }
        }
        // passa al gestore dei volumi i punti dell'oggetto
        public override VolumeSubdivision InitDop() { // un unico volume
            VolumeSubdivision dop = new BoundingBox(texture_id, pw.GetVertexCoords(0, 0));
            numdops = 1;
            int i,j;
            for(i = 0; i < nodesx; i++) {
                for(j = 0; j < nodesy-1; j++) {
                    // assegno per ora ad un unico volume
                    dop.AddVertex(pw.GetVertexCoords(i, j), i+j*nodesx);
                    // meglio più volumi separati...
                }
            }
            return dop;
        }

        // volumi multipli separati
        public override ArrayList InitDops() {
            ArrayList dops = new ArrayList();
            VolumeSubdivision tmp_dop;
            numdops = 0;

            int i, j, dop_i, dop_j;
            int dopsize = 3;


            for(dop_i = 0; dop_i < (nodesx / dopsize); dop_i++) {
                for(dop_j = 0; dop_j < (nodesy / dopsize); dop_j++) {
                    
                    numdops++;
                    tmp_dop = new BoundingBox(texture_id, pw.GetVertexCoords(dopsize * dop_i, dopsize * dop_j));
                    for(i = 0; (i < dopsize-1) && ( i+dopsize*dop_i< nodesx); i++) {
                        for(j = 0; (j < dopsize-1) && ( j+dopsize*dop_j< nodesy); j++) {
                            tmp_dop.AddVertex(pw.GetVertexCoords(dopsize * dop_i + i, dopsize * dop_j + j), convertXYIndexesToInd(dopsize * dop_i + i , dopsize * dop_j + j));
                        }
                    }
                    dops.Add(tmp_dop);
                }
            }

            Console.WriteLine("Aggiunta di " + dops.Count + "DOPs");
            return dops;
        }

        public override void RefreshDop(VolumeSubdivision dop) {
            float[] center = new float[3];
            int[] indexesInThisDop = (int[])dop.vertIndex.ToArray(typeof(int));
            float[] coords;
            int i, ix, iy;
            int n = dop.vertIndex.Count;
/*
            // per la sfera
            // ridefinizione del centro del dop;
            for(i = 0; i < n; i++) {
                ix = (int)dop.vertIndex[i] % nodesx;
                iy = (int)dop.vertIndex[i] / nodesx;
                coords = pw.GetVertexCoords(ix, iy);
                center[0] += coords[0];
                center[1] += coords[1];
                center[2] += coords[2];
            }
            //
            center[0] /= n;
            center[1] /= n;
            center[2] /= n;
            dop.RefreshCenter(center);
*/             
            // per il box

            dop.RestartBounds();
            for(i = 0; i < n; i++) {
                ix = (int)dop.vertIndex[i] % nodesx;
                iy = (int)dop.vertIndex[i] / nodesx;
                coords = pw.GetVertexCoords(ix, iy);
                dop.RefreshVertex(coords);
            }
            
        }

        //
        public override void StepSim_Compute() {
            pw.StepSim_Compute();
        }

        public override void StepSim_Apply() {
            pw.StepSim_Apply();
        }

        public override void StepSim_Reset() {
            pw.StepSim_Reset();
        }

        public override void SwitchWireframe() {
            wireframe = !wireframe;
        }

        // realizzate per prova in renderablewall
        public override Bitmap GetTexture() {
            return tm.textureImage[texture_id];
        }

        public override void SetTexture(Bitmap bmp) {
            tm.textureImage[texture_id] = bmp;
            tm.ReinitTexture(texture_id);
        }

        //
        private void setAwningPoints(float sizex, float sizey, float borderx, float bordery, float posz) {
            int i, j, k;

            nodesx = (int)(sizex * 4.0f);
            //nodesy = 12;
            nodesy = (int)(sizey * 4.0f);
            nodesizex = sizex / (float)nodesx;
            nodesizey = sizey / (float)nodesy;
            numnodes = nodesx * nodesy;
            numcoords = numnodes * 3;
            points = new float[numcoords];

            //float borderx = -(sizex / 2.0f);
            //float bordery = -(sizey / 2.0f);

            // inizializzazione punti
            k = 0;
            
            for(j=0; j<nodesy; j++) {
                for(i=0; i<nodesx; i++) {
                    points[k] = borderx + i*nodesizex; // coordinata x del vertice
                    //Console.WriteLine(" coordinate punto - x: " + points[k]);
                    k++;
                    points[k] = bordery + j*nodesizey;  // coordinata y del vertice
                    //Console.WriteLine(" coordinate punto - y: " + points[k]);
                    k++;
                    points[k] = posz + (float)k*-0.0001f;  // coordinata z del vertice
                    //Console.WriteLine(" coordinate punto - z: " + points[k]);
                    k++;
                }
            }
            // settaggio di questi stessi dati nella dll fisica
            pw.SetAwningPoints(nodesx, nodesy, points); 
        }
     
        //
        public void setMouseAnchor(float mx, float my) {
            mouseanchor_x = mx;
            mouseanchor_y = my;
            // Console.WriteLine(mx + ", " + my);
            //
            ancfound = pw.FindPointsNear(mx, my, 0.15f);
            if(ancfound > -1) {
                //Console.WriteLine("Catturato punto #" + ancfound);
                mouse_anchor.visible = true;
                mouse_anchor.Position = pw.GetVertexCoords(ancfound%nodesx, ancfound/nodesx);
                pw.SetAnchor(ancfound);
            }
        }

        public void moveMouseAnchor(float mx, float my) {
            if(ancfound > -1) {
                pw.moveMouseAnchor(ancfound, mx, my);
            }
        }

        public void RemoveAnchor(float mx, float my) {
            mouseanchor_x = mx;
            mouseanchor_y = my;
            ancfound = pw.FindPointsNear(mx, my, 0.25f);
            if(ancfound > -1) {
                pw.ResetAnchor(ancfound);
            }
        }

        public void setMouseAnchorVisibility(bool v) {
            mouse_anchor.visible = v;
        }

        public int convertXYIndexesToInd(int x, int y) {
            return (x + y*nodesx);
        }
    }
}
