using System;
using System.Collections.Generic;
using System.Text;

using Tao.OpenGl;

namespace TRender {
    class RenderableLight : Renderable {

        private float[] position0, position1, position2, ambient0, ambient1, ambient2, diffuse0, diffuse1, diffuse2;
        public float[] Position0 {
            get { return position0; }
            set { position0 = value; }
        }
        public float[] Ambient1 {
            get { return ambient1; }
            set { ambient1 = value; }
        }
        public float[] Ambient2 {
            get { return ambient2; }
            set { ambient2 = value; }
        }
        public RenderableLight() : base() {
        }

        public override void Init() {
            // luce mobile
            ambient0 = new float[] { 0.8f, 0.75f, 0.70f, 1.0f };
            diffuse0 = new float[] { 1.0f, 0.95f, 0.9f, 1.0f };
            position0 = new float[] { 0.0f, 0.0f, 5.0f, 1.0f };
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, ambient0);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, diffuse0);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, position0);
            Gl.glEnable(Gl.GL_LIGHT0);

            // luce fissa esterna
            ambient1 = new float[] { 0.6f, 0.6f, 0.6f, 0.6f };
            diffuse1 = new float[] { 0.5f, 0.5f, 0.5f, 0.6f };
            position1 = new float[] { 0.0f, 10.0f, 30.0f, 1.0f };
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, ambient1);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, diffuse1);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, position1);
            Gl.glEnable(Gl.GL_LIGHT1);

            // luce fissa interna (esterna rifratta)
            ambient2 = new float[] { 0.3f, 0.3f, 0.3f, 0.4f };
            diffuse2 = new float[] { 0.3f, 0.3f, 0.3f, 0.4f };
            position2 = new float[] { 0.0f, 1.0f, 4.0f, 1.0f };
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_AMBIENT, ambient2);
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_DIFFUSE, diffuse2);
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, position2);
            Gl.glEnable(Gl.GL_LIGHT2);      
        }

        //
        public void Move(float[] pos) {
            position0 = pos;
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, position0);
        }

        public override void Render() {
            //Gl.glMatrixMode(Gl.GL_MODELVIEW);
            
            Gl.glDisable(Gl.GL_TEXTURE);
            Gl.glEnable(Gl.GL_POINT_SMOOTH);
            //Gl.glLoadIdentity();
            Gl.glPointSize(6.0f);
            Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3fv(ambient0);
                Gl.glVertex3fv(position0);
                //Console.WriteLine("luce x: " + position0[0]);
            Gl.glEnd();
            
            Gl.glEnable(Gl.GL_TEXTURE);
        }

        //
        public void RefreshAmbient(float[] a2) {
            ambient2 = a2;
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_AMBIENT, ambient2);
        }

        //
        public void SetAmbientType(int type) {
            //
            if(type == 0) { // notte (luce blu diffusa scarsa)
                ambient1 = new float[] { 0.01f, 0.01f, 0.01f, 0.01f };
                diffuse1 = new float[] { 0.01f, 0.01f, 0.01f, 0.01f };
                Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, ambient1);
                Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, diffuse1);
                ambient2 = new float[] { 0.0f, 0.0f, 0.05f, 0.2f };
                diffuse2 = new float[] { 0.0f, 0.0f, 0.05f, 0.2f };
                Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_AMBIENT, ambient2);
                Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_DIFFUSE, diffuse2);
            } else { // giorno default: (luce ambientale incolore )
                ambient1 = new float[] { 0.6f, 0.6f, 0.6f, 0.6f };
                diffuse1 = new float[] { 0.5f, 0.5f, 0.5f, 0.6f };
                Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, ambient1);
                Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, diffuse1);
                ambient2 = new float[] { 0.3f, 0.3f, 0.3f, 0.4f };
                diffuse2 = new float[] { 0.3f, 0.3f, 0.3f, 0.4f };
                Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_AMBIENT, ambient2);
                Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_DIFFUSE, diffuse2);
            }

        }

    }
}
