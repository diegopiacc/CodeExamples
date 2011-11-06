using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
// using System.Timers;
using System.Text;

using Tao.OpenGl;

namespace TRender {
    public class TRenderer {

        private RenderableLight light;
        private ArrayList objlist;

        private VolumeManager volumes;
        //private ArrayList doplist;

        private Trackball viewsphere, lightsphere;
        // private Timer timer;

        private int active_object_index;
        private float fov, fovy;
        private float[] pointed;
        private float wall_w, wall_h;
        private bool light_visible, collisions_det;
        private float[] bgcolor;
        
        // private string DllName;

        public TRenderer() {
            // DllName = "TPhysic";
            objlist = new ArrayList();
            volumes = new VolumeManager();
            //doplist = new ArrayList();
            //
            light_visible = true;
            collisions_det = true;
            // posizione di default della camera
            viewsphere = new Trackball(10.0f);
            viewsphere.CartesianCoords = new float[3] { 0.0f, 0.0f, 10.0f };
            //
            lightsphere = new Trackball(4.0f, 3.0f, 0.0f);
            //
            fovy = 30.0f;
            //fov_sin = 0.258819f;
            wall_w = 9.0f;
            wall_h = 7.0f;
            bgcolor = new float[4] { 0.85f, 0.85f, 0.9f, 1.0f };
        }

        public void SetCollisionDetection(bool cd) {
            collisions_det = cd;
        }

        public void InitAmbient() {
            // background
            
            // depth sorting
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);
           
            //
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            
            //
            Gl.glEnable(Gl.GL_LIGHTING);
            light = new RenderableLight();
            light.Init();
            
        }

        //
        public void addObject() {
            Renderable obj = new Renderable();
            objlist.Add(obj);
        }

        public void addWall() {
            RenderableWall muro = new RenderableWall(wall_w, wall_h);
            muro.Init();
            //volumes.Add(muro.InitDop());
            objlist.Add(muro);
            active_object_index = 0;
        }

        // inizializzazione nuovo oggetto in tessuto: default e custom
        public void addAwning() {
            addAwning(4.0f, 3.0f, -2.0f, -1.5f, 0.0f);
        }

        public void addAwning(float sizex, float sizey, float posx, float posy, float posz) {
            RenderableCloth tenda = new RenderableCloth(sizex, sizey, posx, posy, posz);
            active_object_index = objlist.Count;
            ((RenderableCloth)tenda).Texture_id = active_object_index;
            volumes.Add(tenda.InitDops());
            objlist.Add(tenda);
        }

        // rimozione oggetti dalla scena
        public void DeleteSelectedAwning() {
            objlist.RemoveAt(active_object_index);
        }

        public void DeleteLastAwning() {
            volumes.Remove(((Renderable)objlist[objlist.Count - 1]).numdops);
            objlist.RemoveAt(objlist.Count - 1);
        }

        // callbacks per eventi di diplay e reshape
        public void Display() {
            bgcolor = light.Ambient1;
            Gl.glClearColor(bgcolor[0], bgcolor[1], bgcolor[2], bgcolor[3]);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_ACCUM_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_LIGHTING);

            Glu.gluLookAt(viewsphere.GetCartesianX(), viewsphere.GetCartesianY(), viewsphere.GetCartesianZ(),
                          0, 0, 0, // look at
                          0, 1, 0);// up-vector

            foreach(Renderable r in objlist) {
                r.Render();
            }
            if(light_visible) {
                light.Render();
            }
            
            //Gl.glFlush();
        }

        public void Reshape(int width, int height) {
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glViewport(0, 0, width, height);
            fov = (float)width / (float)height;
            Glu.gluPerspective(fovy, fov, 0.1f, 100.0f);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
        }

        public void SwitchRenderingMode() {
            foreach(Renderable r in objlist) {
                r.SwitchWireframe();
            }
        }

        public void SetLighVisible(bool lv) {
            light_visible = lv;
        }


///////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void StepSimulation() {
            int i;
            
            for(i = 0; i < objlist.Count; i++) {
                ((Renderable)objlist[i]).StepSim_Compute();
            }

            volumes.RefreshAll(objlist);

            if(collisions_det) {
                volumes.CheckCD(objlist);
            }

            for(i = 0; i < objlist.Count; i++) {
                ((Renderable)objlist[i]).StepSim_Apply();
            }

            for(i = 0; i < objlist.Count; i++) {
                ((Renderable)objlist[i]).StepSim_Reset();
            }

        }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // metodi per il trace del movimento di un punto 2D/3D
        public void FindNearestAnchor(float screenX, float screenY) {
            // trasformazione coordinate 2D schermo (0-1) in 3D
            // o meglio, in 2d sul piano Z=0

            // proiezione su piano Y|_X
            pointed = viewsphere.GetProjectionOnZ(screenX, screenY, fov);

            RenderableCloth objHit = getHitInteractor();
            objHit.setMouseAnchor(pointed[0], pointed[1]);
        }

        public void MoveMouseAnchor(float screenX, float screenY) {
            pointed = viewsphere.GetProjectionOnZ(screenX, screenY, fov);

            RenderableCloth objHit = getHitInteractor();
            objHit.moveMouseAnchor(pointed[0], pointed[1]);
            
        }

        public void RemoveAnchor(float screenX, float screenY) {
            pointed = viewsphere.GetProjectionOnZ(screenX, screenY, fov);

            RenderableCloth objHit = getHitInteractor();
            objHit.RemoveAnchor(pointed[0], pointed[1]);
        }

        public void SetMouseAnchorVisibility(bool v) {
            int i;
            if(v) {
                RenderableCloth objHit = getHitInteractor();
                objHit.setMouseAnchorVisibility(true);
            } else {
                for(i = 1; i < objlist.Count; i++) {
                    ((RenderableCloth)objlist[i]).setMouseAnchorVisibility(false);
                }
            }
        }

        public void SetCollisionDetection(int cd) {
            if(cd==0) {
                collisions_det = false;
            } else {
                collisions_det = true;
            }
        }


        // metodo per la selezione dell'oggetto interattivo (da implementare)
        public void SetActiveObjectIndex(int oid) {
            active_object_index = oid;
        }

        private RenderableCloth getHitInteractor() {
            if(active_object_index < 1 || active_object_index >= objlist.Count)
                active_object_index = 1;
            return (RenderableCloth)objlist[active_object_index];
        }

        // Gestione Textures
        public Bitmap GetTexture(int object_id) {
            if(object_id < 0 || object_id > objlist.Count)
                object_id = 0;
            return ((Renderable)objlist[object_id]).GetTexture();
        }

        public void SetTexture(int object_id, Bitmap bmp) {
            if(object_id < 0 || object_id >= objlist.Count) {
                object_id = 0;
            }
            if(object_id == 0) {
                // Image Based Lighting
                LightRecon lr = new LightRecon(bmp, wall_w, wall_h);
                lr.Recon();
                lr.Test();
                lightsphere = new Trackball(lr.Lposition[0], lr.Lposition[1], lr.Lposition[2]);

                light.Move(lightsphere.CartesianCoords);
                light.RefreshAmbient(lr.GetAmbientHue());

                Display();
            }
            ((Renderable)objlist[object_id]).SetTexture(bmp);
        }

        // gestione luce ambientale
        public void SetAmbientType(int type) {
            light.SetAmbientType(type);
        }

        // metodi per il movimento della camera
        public void ResetTrackMouse(float dx, float dy) {
            viewsphere.FreezeCoords(dx, dy);
            lightsphere.FreezeCoords(-dx, -dy);
        }

        public void TrackLight(float dx, float dy) {
            // lightsphere
            lightsphere.CartesianCoords = light.Position0;
            lightsphere.CartesianToSphere();
            lightsphere.Track(-dx, -dy);
            light.Move(lightsphere.CartesianCoords);
            Display();
        }

        public void TrackView( float dx, float dy) {
            viewsphere.Track(dx, dy);
            Display();
        }

        public void StrafeView(float dx, float dy) {
            viewsphere.Strafe(dx, dy);
            Display();
        }
    }
}
