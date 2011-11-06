using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace TRender {
    class LightRecon {

        private Bitmap bmp;
        private float[] lposition;
        public float[] Lposition {
            get { return lposition; }
            set { lposition = value; }
        }

        private float lhue;
        public float Lhue {
            get { return lhue; }
            set { lhue = value; }
        }
        private float wall_w, wall_h;
        int ns = 12;
        
        // costruttori
        public LightRecon() {
        }

        public LightRecon(Bitmap b, float w, float h) {
            wall_w = w;
            wall_h = h;
            bmp = b;
        }

        public float[] GetAmbientHue() {
            float[] rgba_color = new float[4];
            Color tmp_color = new Color();
            int i,j;
            int n = 10 * bmp.Height * bmp.Width;

            // colore medio della bmp
            for(i=0; i<bmp.Height; i+=5) {
                for(j=0; j<bmp.Width; j+=5) {
                    tmp_color = bmp.GetPixel(i, j);
                    rgba_color[0] += tmp_color.R;
                    rgba_color[1] += tmp_color.G;
                    rgba_color[2] += tmp_color.B;
                }
            }
            rgba_color[0] /= n;
            rgba_color[1] /= n;
            rgba_color[2] /= n;
            rgba_color[3] = 1.0f;
            Console.WriteLine("Tinta Luce Calcolata:  " + rgba_color[0] + ", " + rgba_color[1] + ", " + rgba_color[2]);
            return rgba_color;
        }


        // 
        public void Recon() {
            int i, k, l;
			int sample_span = 40;
            
            // prepara l'oggetto che manterrà in memoria il campione di immagine
            Color tmp_color; // colore - sia rgb che hsb
			// float text_size = 512.0f;
            int[] ipx = new int[12] { 51, 203, 307, 461, 
								      51,           461, 
									  51,           461, 
									  51, 203, 307, 461};
            int[] ipy = new int[12] {51,  51,  51,  51,
			                         203,           203,
									 307,           307,
									 461, 461, 461, 461};
            
            /* coordinate x y se relative alle dimensioni muro
            float[] xcoord = new float[12] {-wall_w*0.4f, -wall_w*0.1f, wall_w*0.1f, wall_w*0.4f, 
											-wall_w*0.4f,							 wall_w*0.4f,
											-wall_w*0.4f,							 wall_w*0.4f,
											-wall_w*0.4f, -wall_w*0.1f, wall_w*0.1f, wall_w*0.4f};
            float[] ycoord = new float[12] {wall_h*0.25f, wall_h*0.25f, wall_h*0.25f, wall_h*0.25f, 
											0,							              0,
										   -wall_h*0.25f,							 -wall_h*0.25f,
										   -wall_h*0.5f, -wall_h*0.5f, -wall_h*0.5f, -wall_h*0.5f};
            
            */
            // coordinate x y "normalizzate"
            float[] xcoord = new float[12] {-0.4f, -0.1f, 0.1f, 0.4f, 
                                            -0.4f,			    0.4f,
                                            -0.4f,			    0.4f,
                                            -0.4f, -0.1f, 0.1f, 0.4f};
            float[] ycoord = new float[12] {0.25f, 0.25f, 0.25f, 0.25f, 
                                            0,				     0,
                                           -0.25f,				-0.25f,
                                           -0.5f, -0.5f, -0.5f, -0.5f};
   
            //float[] zcoord = new float[12] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 
            //                                 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
            float[] greylev = new float[12]; // livello di grigio - (1-b) del modello hsb(hsv)
            
            // equazione del "piano" di grigi
            float[] coeffs = new float[3];
            float plane_nrm;
            lposition = new float[3];
            
             // scandisci la bitmap e ottieni i campioni di grigio

			//lhue = 0.0f;
            for(i = 0; i < ns; i++) {
                greylev[i] = 0.0f;
                for(k = 0; k < sample_span; k++) {
                    for(l = 0; l < sample_span; l++) {
                        tmp_color = bmp.GetPixel(ipx[i], ipy[i]);
                        // se utilizzo la "brightness" del modello .net
                        // greylev[i] += tmp_color.GetBrightness();
                        // meglio utlizzare la "Luminance"!
                        greylev[i] -= toLum(tmp_color.R, tmp_color.G, tmp_color.B);
                        //lhue += tmp_color.GetHue();
                    }
                }
                greylev[i] /= (sample_span*sample_span);
            }
            //lhue /= (ns * sample_span * sample_span);

            // interpolazione lineare per identificare il piano che li approssima
            // approssimazione del gradiente del livello di grigio
            //coeffs = LeastSquares3DPlane(xcoord, ycoord, greylev);
            //plane_nrm = (float)Math.Sqrt(coeffs[0]*coeffs[0] + coeffs[1]*coeffs[1] + coeffs[2]*coeffs[2]);
            
            // interpolazione tramite sfera
            coeffs = LeastSquares3DSphere(xcoord, ycoord, greylev);
            plane_nrm = (float)Math.Sqrt(coeffs[0]*coeffs[0] + coeffs[1]*coeffs[1] + coeffs[2]*coeffs[2]);
            
            // la normale pone la luce a distanza 1; la divido
            // per la distanza a cui la vorrei
            plane_nrm *= 0.25f;

            if(coeffs[2] < 0) {
                coeffs[2] *= -1.0f;
            }

            coeffs[0] /= plane_nrm;
            coeffs[1] /= plane_nrm;
            coeffs[2] /= plane_nrm;

            // immagazzinamento della direzione relativa ai campioni
            lposition = coeffs;

        }

        // metodo per l'interpolazione lineare in 3d di un **piano**
        // date le coordinate x, y, z (qui z = livello di grigio!)
        private float[] LeastSquares3DPlane(float[] x, float[] y, float[] z) {
            int i;
            float[,] i_matr = new float[3, 3];
            float[] i_noti = new float[3];
            float[] i_result = new float[3];
            float i_determ;

            // coefficienti della matrice
            i_matr[0, 0] = 0.0f;
            for(i = 0; i < ns; i++) {
                i_matr[0, 0] += (float)(x[i]*x[i]);
            }
            i_matr[0, 1] = 0.0f;
            for(i = 0; i < ns; i++) {
                i_matr[0, 1] += (float)(x[i]*y[i]);
            }
            i_matr[0, 2] = 0.0f;
            for(i = 0; i < ns; i++) {
				i_matr[0, 2] += (float)(x[i]);
            }
            i_matr[1, 0] = i_matr[0, 1];
            i_matr[1, 1] = 0.0f;
            for(i = 0; i < ns; i++) {
                i_matr[1, 1] += (float)(y[i] * y[i]);
            }
            i_matr[1, 2] = 0.0f;
            for(i = 0; i < ns; i++) {
                i_matr[1, 2] += (float)(y[i]);
            }
            i_matr[2, 0] = i_matr[0, 2];
            i_matr[2, 1] = i_matr[1, 2];
            i_matr[2, 2] = (float)ns;
            // termini noti
            i_noti[0] = 0.0f;
            for(i = 0; i < ns; i++) {
                i_noti[0] += (float)(x[i] * z[i]); 
            }
            i_noti[1] = 0.0f;
            for(i = 0; i < ns; i++) {
                i_noti[1] += (float)(y[i] * z[i]);
            }
            i_noti[2] = 0.0f;
            for(i = 0; i < ns; i++) {
                i_noti[2] += (float)(z[i]);
            }
            
            // Test: visualizza la matrice // ok
            //Console.WriteLine("|  " + i_matr[0, 0] + " " + i_matr[0, 1] + " " + i_matr[0, 2] + " |");
            //Console.WriteLine("|  " + i_matr[1, 0] + " " + i_matr[1, 1] + " " + i_matr[1, 2] + " |");
            //Console.WriteLine("|  " + i_matr[2, 0] + " " + i_matr[2, 1] + " " + i_matr[2, 2] + " |");

            // soluzione matrice col metodo cramer
            i_determ = i_matr[0, 0] * i_matr[1, 1] * i_matr[2, 2] +
                        i_matr[0, 1] * i_matr[1, 2] * i_matr[2, 0] +
                        i_matr[0, 2] * i_matr[1, 0] * i_matr[2, 1] -
                        i_matr[0, 0] * i_matr[1, 2] * i_matr[2, 1] -
                        i_matr[0, 1] * i_matr[1, 0] * i_matr[2, 2] -
                        i_matr[0, 2] * i_matr[1, 1] * i_matr[2, 0];
            i_result[0] = (i_noti[0] * i_matr[1, 1] * i_matr[2, 2] +
                        i_noti[1] * i_matr[1, 2] * i_matr[2, 0] +
                        i_noti[2] * i_matr[1, 0] * i_matr[2, 1] -
                        i_noti[0] * i_matr[1, 2] * i_matr[2, 1] -
                        i_noti[1] * i_matr[1, 0] * i_matr[2, 2] -
                        i_noti[2] * i_matr[1, 1] * i_matr[2, 0]) / i_determ;
            i_result[1] = (i_matr[0, 0] * i_noti[1] * i_matr[2, 2] +
                        i_matr[0, 1] * i_noti[2] * i_matr[2, 0] +
                        i_matr[0, 2] * i_noti[0] * i_matr[2, 1] -
                        i_matr[0, 0] * i_noti[2] * i_matr[2, 1] -
                        i_matr[0, 1] * i_noti[0] * i_matr[2, 2] -
                        i_matr[0, 2] * i_noti[1] * i_matr[2, 0]) / i_determ;
            i_result[2] = (i_matr[0, 0] * i_matr[1, 1] * i_matr[2, 2] +
                        i_matr[0, 1] * i_matr[1, 2] * i_noti[0] +
                        i_matr[0, 2] * i_matr[1, 0] * i_noti[1] -
                        i_matr[0, 0] * i_matr[1, 2] * i_noti[1] -
                        i_matr[0, 1] * i_matr[1, 0] * i_noti[2] -
                        i_matr[0, 2] * i_matr[1, 1] * i_noti[0]) / i_determ;
            //
            return i_result;
        }

        // metodo per l'interpolazione lineare in 3d di un **piano**
        // date le coordinate x, y, z (qui z = livello di grigio!)
        private float[] LeastSquares3DSphere(float[] x, float[] y, float[] z) {
            int i, iterate;
            float[,] i_matr = new float[3, 3];
            float[] i_noti = new float[3];
            float[] i_result = new float[3];
            // float i_determ;
            float ll, li, la, lb, lc;

            // inizializzo il risultato con la media dei valori x y z
            i_result[0] = 0.0f;
            i_result[1] = 0.0f;
            i_result[2] = 0.0f;
            for(i = 0; i < ns; i++) {
                i_result[0] += x[i];
                i_result[1] += y[i];
                i_result[2] += z[i];
            }
            i_result[0] /= ns;
            i_result[1] /= ns;
            i_result[2] /= ns;

            // procedimento iterativo - numero di iter arbitrario
            for(iterate = 0; iterate < 9; iterate++) {
                ll = 0.0f; la = 0.0f; lb = 0.0f; lc = 0.0f;
                for(i = 0; i < ns; i++) {
                    li = (float)Math.Sqrt((i_result[0] - x[i]) * (i_result[0] - x[i]) + (i_result[1] - y[i]) * (i_result[1] - y[i]) + (i_result[2] - z[i]) * (i_result[2] - z[i]));
                    la += (i_result[0] - x[i]) / li;
                    lb += (i_result[1] - y[i]) / li;
                    lc += (i_result[2] - z[i]) / li;
                    ll += li;
                }
                ll /= ns;
                la /= ns;
                lb /= ns;
                lc /= ns;

                //
                i_result[0] = i_result[0] + ll * la;
                i_result[1] = i_result[1] + ll * lb;
                i_result[2] = i_result[2] + ll * lc;
            }

            // cambio di segno dei coefficienti
            /*
            i_result[0] *= -1.0f;
            i_result[1] *= -1.0f;
            i_result[2] *= -1.0f;
            */
            //
            return i_result;
        }


        // funzione per ottenere la mininanaza di un pixel dai valori rgb (diversa dalla luminosità(brightness))
        private float toLum(float r, float g, float b) {
            float max = Max3(r, g, b);
            float min = Min3(r, g, b);
            return 0.5f*(max + min);
        }

        // funzioni di utilità gestione numeri - testate e funzionanti
        private float Max3(float r, float g, float b) {
            return (r > g ? (r > b ? r : b) : (g > b ? g : b));
        }

        private float Min3(float r, float g, float b) {
            return (r < g ? (r < b ? r : b) : (g < b ? g : b));
        }

        /*
        private float toHue(float r, float g, float b) {
            float hue = 0.0f;
            float max = Max(r, g, b);
            float min = Min(r, g, b);
            //
            if(r > g && r > b) { // r è il valore massimo
                if(g > b) { // r > g > b
                    hue = 60.0f * (g - b) / (max - min);
                } else { // r > b > g
                    hue = 60.0f * (g - b) / (max - min) + 360.0f;
                }
            } else if(g > r && g > b) { // g è il valore massimo
                hue = 60.0f * (b - r) / (max - min) + 120.0f;
            } else if(b > r && b > g) { // b è il valore massimo
                hue = 60.0f * (r - g) / (max - min) + 240.0f;
            }
            return hue;
        }
        */
        
        // test
        public void Test() {
            // Print delle coordinate della luce calcolata
            Console.WriteLine("Coordinate Luce Calcolata:  " + lposition[0] + ", " + lposition[1] + ", " + lposition[2]);
        }
    }
}
