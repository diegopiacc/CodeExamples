using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using Tao.OpenGl;
using Tao.Platform.Windows;

namespace TRender {
    public sealed class TextureManager {

        public int numtextures;
        public int[] texture;
        public Bitmap[] textureImage;
        Rectangle[] rectangle;
        BitmapData[] bitmapData;

        public TextureManager() {

            Gl.glEnable(Gl.GL_TEXTURE_2D);

            int i;
            //ambient = new float[] { 0.3f, 0.3f, 0.3f, 1.0f };
            //diffuse = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };
            //specular = new float[] { 0.6f, 0.6f, 0.6f, 1.0f };
            
            numtextures = 32;
            texture = new int[numtextures];
            textureImage = new Bitmap[numtextures];                       // Create Storage Space For The Texture
            rectangle = new Rectangle[numtextures];
            bitmapData = new BitmapData[numtextures];

            //
            Gl.glMatrixMode(Gl.GL_TEXTURE);
            Gl.glGenTextures(numtextures, texture);
    
            for(i = 0; i < numtextures; i++ ) {
                // Load The Bitmap
                if(i == 0) {
                    textureImage[i] = LoadBMP("muro.bmp");
                } else {
                    textureImage[i] = LoadBMP("tenda.bmp");    
                }
                if(textureImage[i] != null) {

                    //Gl.glPushMatrix();
                    //Gl.glLoadIdentity();
                    texture[i] = i;
                    
                    // textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     // Flip The Bitmap Along The Y-Axis
                    // Rectangle For Locking The Bitmap In Memory
                    rectangle[i] = new Rectangle(0, 0, textureImage[i].Width, textureImage[i].Height);
                    // Get The Bitmap's Pixel Data From The Locked Bitmap
                    bitmapData[i] = textureImage[i].LockBits(rectangle[i], ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    // Create Nearest Filtered Texture
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[i]);
                    Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
                    Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
                    Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
                    Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[i].Width, textureImage[i].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData[i].Scan0);

                    //Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
                    if(textureImage[i] != null) {                                   // If Texture Exists
                        textureImage[i].UnlockBits(bitmapData[i]);                     // Unlock The Pixel Data From Memory
                        textureImage[i].Dispose();                                  // Dispose The Bitmap
                    }
                    //Gl.glPopMatrix();
                }
            }

            //
            //Gl.glEnable(Gl.GL_TEXTURE_2D);
        }

        public void ReinitTexture(int i) {
            rectangle[i] = new Rectangle(0, 0, textureImage[i].Width, textureImage[i].Height);
            // Get The Bitmap's Pixel Data From The Locked Bitmap
            bitmapData[i] = textureImage[i].LockBits(rectangle[i], ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Create Nearest Filtered Texture
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[i]);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[i].Width, textureImage[i].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData[i].Scan0);

            //Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
            if(textureImage[i] != null) {                                   // If Texture Exists
                textureImage[i].UnlockBits(bitmapData[i]);                     // Unlock The Pixel Data From Memory
                textureImage[i].Dispose();                                  // Dispose The Bitmap
            }
        }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Bitmap LoadBMP(string fileName) {
            if(fileName == null || fileName == string.Empty) {                  // Make Sure A Filename Was Given
                return null;                                                    // If Not Return Null
            }

            string fileName1 = string.Format("Data{0}{1}",                      // Look For Data\Filename
                Path.DirectorySeparatorChar, fileName);
            string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",          // Look For ..\..\Data\Filename
                "..", Path.DirectorySeparatorChar, fileName);

            // Make Sure The File Exists In One Of The Usual Directories
            if(!File.Exists(fileName) && !File.Exists(fileName1) && !File.Exists(fileName2)) {
                return null;                                                    // If Not Return Null
            }

            if(File.Exists(fileName)) {                                         // Does The File Exist Here?
                return new Bitmap(fileName);                                    // Load The Bitmap
            } else if(File.Exists(fileName1)) {                                   // Does The File Exist Here?
                return new Bitmap(fileName1);                                   // Load The Bitmap
            } else if(File.Exists(fileName2)) {                                   // Does The File Exist Here?
                return new Bitmap(fileName2);                                   // Load The Bitmap
            }

            return null;                                                        // If Load Failed Return Null
        }


        // meotdi ausiliari per utilizzare TextureManager come un pattern di tipo singleton
        static readonly TextureManager instance = new TextureManager();

        public static TextureManager Instance {
            get {
                return instance;
            }
        }
    }

}
