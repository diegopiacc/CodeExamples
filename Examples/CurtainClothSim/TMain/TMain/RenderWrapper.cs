using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using TRender;

namespace TMain {
    public sealed class RenderWrapper {
        
        private TRenderer tr;
        public int active_object_index;

        public RenderWrapper() {
            active_object_index = 1;
            tr = new TRenderer();
        }

        //
        public void Init(int w, int h) {
            tr.InitAmbient();
            tr.addWall();
            tr.addAwning();       
            tr.Reshape(w, h);        
        }

        public void ResetSimulation() {
            // ...
        }

        // aggiunta nuovo oggetto in tessuto: default e custom
        public void AddNewAwning() {
            tr.addAwning();
        }

        public void AddNewAwning(float sizex, float sizey, float posx, float posy, float posz) {
            tr.addAwning(sizex, sizey, posx, posy, posz);
        }

        public void DeleteSelectedAwning() {
            tr.DeleteSelectedAwning();
        }

        public void DeleteLastAwning() {
            tr.DeleteLastAwning();
        }

        //
        public void SetSelected(int oid) {
            active_object_index = oid;
            tr.SetActiveObjectIndex(oid);
        }

        // propagazione delle callback di sistema
        public void Render() {
            tr.Display();
        }

        public void Reshape(int w, int h) {
            tr.Reshape(w, h);
        }

        public void OnTimer() {
            tr.StepSimulation();
            tr.Display();
        }

        // propagazione delle interazioni con l'engine fisico
        public void ResetTrackMouse(float dx, float dy) {
            tr.ResetTrackMouse(dx, dy);
        }

        public void TrackView(float dx, float dy) {
            tr.TrackView(dx, dy);
        }

        public void StrafeView(float dx, float dy) {
            tr.StrafeView(dx, dy);
        }

        public void FindNearestAnchor(float screenX, float screenY) {
            tr.FindNearestAnchor(screenX, screenY);
        }

        public void SetAmbientType(int type) {
            tr.SetAmbientType(type);
        }

        public void TrackLight(float screenX, float screenY) {
            tr.TrackLight(screenX, screenY);
        }
        
        public void MoveMouseAnchor(float screenX, float screenY) {
            tr.MoveMouseAnchor(screenX, screenY);
        }

        public void RemoveAnchor(float screenX, float screenY) {
            tr.RemoveAnchor(screenX, screenY);
        }

        public void SetMouseAnchorVisibility(bool v) {
            tr.SetMouseAnchorVisibility(v);
        }

        public void SwitchRenderingMode() {
            tr.SwitchRenderingMode();
        }

        //
        public void SetWallTexture(Bitmap bmp) {
            tr.SetTexture(0, bmp);
        }

        public void SetAwningTexture(Bitmap bmp) {
            tr.SetTexture(active_object_index, bmp);
        }

        // meotdi ausiliari per utilizzare RenderWrapper come un pattern di tipo singleton
        
        static readonly RenderWrapper instance = new RenderWrapper();

        public static RenderWrapper Instance {
            get {
                return instance;
            }
        }
    }
}
