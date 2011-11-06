using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TMain {
    public sealed class Globals {

        // Variabili da accedere in ogni parte dell'applicazione 
        public string renderEngineName;
        public string physicEngineName;

        // variabili di simulazione: interfaccia
        public int ui_mode;
        // variabili di simulazione: rendering
        public bool render_wireframe;
        // variabili di simulazione: physic
        public int clock;       
        
        // variabili specifiche di questa classe
        private string optionsFile;
        private XmlTextReader optReader;
        //private XmlTextWriter optWriter;


        // costruttore
        Globals() {
            //
            optionsFile = "options.xml";
            // gestione del file xml con le opzioni
            optReader = new XmlTextReader(optionsFile);
            //optWriter = new XmlTextWriter(Globals.optionsFile, null);

            //
            ui_mode = 0;
            clock = 55;
        }

        // metodi di accesso al file xml
        public void getOptions() {
            try {
                Console.WriteLine("Lettura del file xml...");
                while(optReader.Read()) {
                    if(optReader.IsStartElement()) {
                        if(optReader.Name.Equals("renderEngine")) {
                            renderEngineName = optReader.ReadString() + ".dll";
                            Console.WriteLine("The render engine is: " + renderEngineName);  //Read the text content of the element.
                        }
                        if(optReader.Name.Equals("physicEngine")) {
                            physicEngineName = optReader.ReadString() + ".dll";
                            Console.WriteLine("The physic engine is: " + physicEngineName);  //Read the text content of the element.
                        }
                    }
                }
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        // meotdi ausiliari per utilizzare Globals come un pattern di tipo singleton
        
        static readonly Globals instance = new Globals();

        public static Globals Instance {
            get {
                return instance;
            }
        }
    }

}