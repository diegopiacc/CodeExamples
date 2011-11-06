using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

using Tao.OpenGl;

namespace TRender {
    class RenderableWall : Renderable {
        private TextureManager tm;
        private int texture_id;
        public int Texture_id {
            get { return texture_id; }
            set { texture_id = value; }
        }
        private float width;
        public float Width {
            get { return width; }
            set { width = value; }
        }
        private float heigth;
        public float Heigth {
            get { return heigth; }
            set { heigth = value; }
        }

        private float z = -1.0f;
        private float[,] points;

        public RenderableWall(float w, float h) : base() {
            //int i;
            width = w;
            heigth = h;
            tm = TextureManager.Instance;
            texture_id = 0;
            points = new float[,] { { -width * 0.5f, -heigth * 0.66f, z }, { width * 0.5f, -heigth * 0.66f, z }, { width * 0.5f, heigth * 0.33f, z }, { -width * 0.5f, heigth * 0.33f, z } };
            float[] tmp = new float[3] { 0, 0, z };
            //dop = new VolumeSubdivision(tmp);
            //for(i=0; i<4; i++) {
            //    tmp = new float[3] {points[i,0],points[i,1],points[i,2]};
            //    dop.AddVertex(tmp,i);
            //}
        }

        public override void Init() {
            //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, t.Ambient);
            //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, t.Diffuse);
            Gl.glEnable(Gl.GL_TEXTURE_2D);                                      // Enable Texture Mapping ( NEW )
            Gl.glShadeModel(Gl.GL_SMOOTH);
        }

        //
        // passa al gestore dei volumi i punti dell'oggetto
        public override VolumeSubdivision InitDop() { // un unico volume
            int i;
            float[] tmp;
            tmp = new float[3] {points[0,0],points[0,1],points[0,2]};
            VolumeSubdivision dop = new BoundingBox(0, tmp);
            for(i = 0; i < 4; i++) {
                tmp = new float[3] { points[i, 0], points[i, 1], points[i, 2] };
                dop.AddVertex(tmp, i);
            }
            return dop;
        }

        public override float[] GetAvgVel(int[] points) {
            float[] ret = new float[3] { 0.0f, 0.0f, 0.0f};
            return ret;
        }

        //
        public override void MovePoints(int[] verts, float x, float y, float z) {
            /*
            int i;
            for(i = 0; i < 4; i++) {
                points[i, 0] += x;
                points[i, 1] += y;
                points[i, 2] += z;
            }
            */ 
        }

        //
        public override void Render() {
            // Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        // Clear The Screen And The Depth Buffer

            // Gl.glMatrixMode(Gl.GL_TEXTURE);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tm.texture[0]);                     // Select Our Font Texture
            //Gl.glActiveTexture(tm.texture[1]);
            
            Gl.glBegin(Gl.GL_QUADS);

            //
            Gl.glNormal3f(0, 0, 1);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(points[0, 0], points[0, 1], points[0, 2]);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(points[1, 0], points[1, 1], points[1, 2]);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(points[2, 0], points[2, 1], points[2, 2]);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(points[3, 0], points[3, 1], points[3, 2]);
            Gl.glEnd();
            //
        }

        // realizzate per prova in renderablewall
        public override Bitmap GetTexture() {
            return tm.textureImage[texture_id];
        }

        public override void SetTexture(Bitmap bmp) {
            tm.textureImage[texture_id] = bmp;
            tm.ReinitTexture(texture_id);
        }
    }
}