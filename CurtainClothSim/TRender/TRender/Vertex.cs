using System;
using System.Collections.Generic;
using System.Text;

namespace TRender {
    class Vertex {
        
        private float[] coords;

        public Vertex() {
        }

        public Vertex(float[] coordinates) {
            for(int i=0; i<coords.GetLength(0); i++) {
                coords[i] = coordinates[i];
            }
        }

    }
}
