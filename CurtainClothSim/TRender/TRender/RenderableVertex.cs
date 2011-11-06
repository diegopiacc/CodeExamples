using System;
using System.Collections.Generic;
using System.Text;

using Tao.OpenGl;

namespace TRender {
    class RenderableVertex : Renderable {

        public bool visible;

        public RenderableVertex() : base() {
            visible = false;
            //dop = new VolumeSubdivision(position);
        }

        public override void Render() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_POINT_SMOOTH);
            Gl.glColor3f(1.0f, 0.0f, 0.0f);
            Gl.glPointSize(6.0f);
            Gl.glBegin(Gl.GL_POINTS);
            Gl.glVertex3fv(position);
            Gl.glEnd();
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
        }
    }
}
