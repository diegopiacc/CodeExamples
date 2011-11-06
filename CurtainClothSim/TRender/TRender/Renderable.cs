using System;
using System.Drawing;
using System.Collections;
using System.Text;

using Tao.OpenGl;

namespace TRender {
    class Renderable {
        protected float[] position;
       
        public float[] Position {
            get { return position; }
            set { position = value; }
        }

        public int numdops;

        public Renderable() { }
        public virtual void Init() { }
        public virtual void Render() { }
        public virtual float[] GetAvgVel(int[] points) { return null; }
        public virtual float[] GetCenter(int[] points) { return null; }
        public virtual void Move(float x, float y, float z) { }
        public virtual void MovePoints(int[] ind, float x, float y, float z) { }
        public virtual void AddVelToPoints(int[] ind, float x, float y, float z) { }
        public virtual void AddForceToPoints(int[] ind, float x, float y, float z) { }

        //
        public virtual void SwitchWireframe() { }
        public virtual void StepSim_Compute() { }
        public virtual void StepSim_Apply() { }
        public virtual void StepSim_Reset() { }

        public virtual VolumeSubdivision InitDop() { return null; }
        public virtual ArrayList InitDops() { return null; }
        public virtual void RefreshDop(VolumeSubdivision vs) { }
        //public virtual void CheckCollision(Renderable r) { }
        public virtual void SetTexture(Bitmap image) { }
        public virtual Bitmap GetTexture() { return null; }
    }
}
