using System;
using System.Collections.Generic;
using System.Text;

using Tao.OpenGl;

namespace TRender {
    class RenderableSphere : Renderable {

        public RenderableSphere() : base() {
        }

        public override void Init() {
            float[] Ambient = { 0.8f, 0.8f, 0.8f, 1.0f };
            float[] Diffuse = { 0.6f, 0.4f, 0.5f, 1.0f };Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, Ambient);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, Diffuse);
            Console.WriteLine("Test object initialized...");
            
        }

        public override void Render() {
            Glu.GLUquadric q;
            q = Glu.gluNewQuadric(); // note this
            Glu.gluQuadricDrawStyle(q, Glu.GLU_FILL);
            // Glu.gluQuadricDrawStyle(q, Glu.GLU_SILHOUETTE);

            Glu.gluSphere(q, 1.0, 48, 48);
        }





    }
}
