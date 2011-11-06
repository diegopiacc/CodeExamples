// Corso di Grafica Computerizzata A.A. 2003/2004
// Progetto 2 - Visualizzazione 3D Mesh
// Diego Piacentini

#include <stdio.h>
#include <math.h>
#include <glut.h>
#include <glui.h>

#define MAX_VERT 100000
#define MAX_FAC 100000
#define DIM_BUFF 256


// variabili globali: finestre //////////////////////////////////////////

int mainWindow;
int graphWindow;
GLUI *gluiSubWindow;

GLUI_RadioGroup *gluiRadioView;
GLUI_Panel *gluiRolloutView, *gluiRolloutLights;
GLUI_Spinner *gluiLight2Red, *gluiLight2Green, *gluiLight2Blue;
GLUI_Checkbox *gluiCheckLight1En, *gluiCheckLight2En, *gluiCheckBound;
GLUI_Button *gluiReset, *gluiExit;

int xsize=700;
int ysize=600;

int currentx, currenty;
int ix, iy;
float dx, dy;
bool leftclick, rightclick;

int SelectViewId=1000;
int ShowPointsId=1001;
int	ShowLinesId=1002;
int ShowMeshId=1003;
int EnableLight1Id=1004;
int EnableLight2Id=1005;
int SetLight2RedId=1006;
int SetLight2GreenId=1007;
int SetLight2BlueId=1008;
int EnableBoundBox=1009;
int ResetId=1098;
int QuitId=1099;



// variabili globali: variabili geometriche /////////////////////////////


char* file="bunny.ply";

float psize=0.1;
float lsize=0.1;

int selectview=2;
int visiblepoints=0;
int visiblelines=0;
int visiblemesh=1;
int light1on=1;
int light2on=1;
int showboundbox=0;

float eyeposx=0.0;
float eyeposy=0.15;
float eyeposz=0.15;

float xyratio;

double meshcenter[3];
int rotate[3]={0, 0, 0};

float light1pos[] = {-0.2f, 0.5f, 0.0f, 0.0f};
float light2pos[] = {0.0f, 0.1f, -0.5f, 0.0f};


// variabili globali: colori ////////////////////////////////////////////

float light1amb[] = {0.3f, 0.3f, 0.3f, 1.0f};
float light2amb[] = {0.3f, 0.3f, 0.4f, 1.0f};
float light1diff[] = {0.7f, 0.7f, 0.7f, 1.0f};

float mat1amb[] = {0.5f, 0.5f, 0.5f, 1.0f};
float mat1spec[] = {0.5f, 0.5f, 0.5f, 1.0f};
float mat1diff[] = {0.5f, 0.4f, 0.1f, 1.0f};
float mat1em[] = {0.1f, 0.1f, 0.1f, 1.0f};
float mat1shin = 50.0;


float pColor1[] = {0.9f, 0.9f, 0.9f};
float lColor1[] = {0.0f, 1.0f, 1.0f};
float xColor1[] = {0.5f, 0.5f, 0.5f};






////// metodo per disegnare gli assi x y z /////////////////////////////

void drawaxis( void ) {
		glDisable(GL_LIGHTING);
		glBegin(GL_LINES);
			glColor3f(1.0, 0.0, 0.0);
			glVertex3d(0.0, 0.0, 0.0);
			glVertex3d(0.05, 0.0, 0.0);
			glColor3f(0.0, 1.0, 0.0);
			glVertex3d(0.0, 0.0, 0.0);
			glVertex3d(0.0, 0.05, 0.0);
			glColor3f(0.0, 0.0, 1.0);
			glVertex3d(0.0, 0.0, 0.0);
			glVertex3d(0.0, 0.0, 0.05);
		glEnd();
		glBegin(GL_TRIANGLES);
			glColor3f(1.0, 0.0, 0.0);
			glVertex3d(0.05, 0.0, 0.0);
			glVertex3d(0.04, 0.0, 0.003);
			glVertex3d(0.04, 0.0, -0.003);
			glColor3f(0.0, 1.0, 0.0);
			glVertex3d(0.0, 0.05, 0.0);
			glVertex3d(0.003, 0.04, 0.0);
			glVertex3d(-0.003, 0.04, 0.0);		
			glColor3f(0.0, 0.0, 1.0);
			glVertex3d(0.0, 0.0, 0.05);
			glVertex3d(-0.003, 0.0, 0.04);
			glVertex3d(0.003, 0.0, 0.04);			
			
		glEnd();
		glEnable(GL_LIGHTING);

}


////// definizione classe mesh /////////////////////////////////////////

class theMesh {
	
private:
	
	double p[MAX_VERT][3]; /*  p*punti*coordxyz  */
	double n[MAX_VERT][3]; /*  n*normali*coordxyz  */
	double normtemp[3]; /*  n*coordxyz  */
	double mod;
	int ti[MAX_FAC][3]; /*  t*triangoli*indicediogniverticedeltriangolo  */
	double t[MAX_FAC][6][3]; /*  t*triangoli*vertici123noramliaivertici123*coordxyzvertice  */

	FILE *f;
	double xmax, ymax, zmax;
	double xmin, ymin, zmin;

	int numvert, numfac;
	int indexv0, indexv1, indexv2;

public:

	void loadObject(char* fileName) {

		int i;
		char tmp[DIM_BUFF];

		f=fopen(fileName,"r");
		if(f==NULL) {
			printf("Errore in apertura file!");
			exit(0);
		}
		printf("File %s in fase di lettura...\n", fileName);

		for(i=0; i<38; i++) {
			fscanf(f,"%s", &tmp);
			if(i==13) {
				numvert=atoi(tmp);
				printf("Numero di vertici della mesh: %d\n", numvert); }
			if(i==31) {
				numfac=atoi(tmp);
				printf("Numero di triangoli della mesh: %d\n", numfac); }
		}

		for(i=0; i<numvert; i++) {

			fscanf(f, "%s", &tmp); p[i][0]=atof(tmp);
			fscanf(f, "%s", &tmp); p[i][1]=atof(tmp);
			fscanf(f, "%s", &tmp); p[i][2]=atof(tmp);	
			fscanf(f, "%s", &tmp);
			fscanf(f, "%s", &tmp);
			
			if(i==1) {
				xmax=p[1][0]; xmin=p[1][0];
				ymax=p[1][1]; ymin=p[1][1];
				zmax=p[1][2]; zmin=p[1][2];
			}

			if(p[i][0]>xmax) { xmax=p[i][0]; }
			if(p[i][0]<xmin) { xmin=p[i][0]; }
			if(p[i][1]>ymax) { ymax=p[i][1]; }
			if(p[i][1]<ymin) { ymin=p[i][1]; }
			if(p[i][2]>zmax) { zmax=p[i][2]; }
			if(p[i][2]<zmin) { zmin=p[i][2]; }

		// printf("\nCaricato in memoria verice numero %d\n",i);
		// printf("di coordinate %f %f %f\n", p[i][0], p[i][1], p[i][2]);

		}

		meshcenter[0]=0.5*(xmax+xmin);
		meshcenter[1]=0.5*(ymax+ymin);
		meshcenter[2]=0.5*(zmax+zmin);

		printf("La mesh e' centrata in %f %f %f\n", meshcenter[0], meshcenter[1], meshcenter[2]);


		for(i=0; i<numfac; i++) {
			fscanf(f, "%s", &tmp);
			fscanf(f, "%s", &tmp); indexv0=atoi(tmp);
			fscanf(f, "%s", &tmp); indexv1=atoi(tmp);
			fscanf(f, "%s", &tmp); indexv2=atoi(tmp);	
		// vertice #0 del triangolo i
			ti[i][0]=indexv0;
			t[i][0][0]=p[indexv0][0];
			t[i][0][1]=p[indexv0][1];
			t[i][0][2]=p[indexv0][2];
		// vertice #1 del triangolo i
			ti[i][1]=indexv1;
			t[i][1][0]=p[indexv1][0];
			t[i][1][1]=p[indexv1][1];
			t[i][1][2]=p[indexv1][2];
		// vertice #2 del triangolo i.
			ti[i][2]=indexv2;
			t[i][2][0]=p[indexv2][0];
			t[i][2][1]=p[indexv2][1];
			t[i][2][2]=p[indexv2][2];
		// normale del triangolo i
			normtemp[0]=(((t[i][1][1]-t[i][0][1])*(t[i][2][2]-t[i][0][2]))-((t[i][1][2]-t[i][0][2])*(t[i][2][1]-t[i][0][1])));
			normtemp[1]=(((t[i][1][2]-t[i][0][2])*(t[i][2][0]-t[i][0][0]))-((t[i][1][0]-t[i][0][0])*(t[i][2][2]-t[i][0][2])));
			normtemp[2]=(((t[i][1][0]-t[i][0][0])*(t[i][2][1]-t[i][0][1]))-((t[i][1][1]-t[i][0][1])*(t[i][2][0]-t[i][0][0])));
			mod=sqrt(normtemp[0]*normtemp[0]+normtemp[1]*normtemp[1]+normtemp[2]*normtemp[2]);
			normtemp[0]/=mod;
			normtemp[1]/=mod;
			normtemp[2]/=mod;
		//combinazione delle normali per i vertici
			n[indexv0][0]=(n[indexv0][0]+normtemp[0])/2.0;
			n[indexv0][1]=(n[indexv0][1]+normtemp[1])/2.0;
			n[indexv0][2]=(n[indexv0][2]+normtemp[2])/2.0;

			n[indexv1][0]=(n[indexv1][0]+normtemp[0])/2.0;
			n[indexv1][1]=(n[indexv1][1]+normtemp[1])/2.0;
			n[indexv1][2]=(n[indexv1][2]+normtemp[2])/2.0;

			n[indexv2][0]=(n[indexv2][0]+normtemp[0])/2.0;
			n[indexv2][1]=(n[indexv2][1]+normtemp[1])/2.0;
			n[indexv2][2]=(n[indexv2][2]+normtemp[2])/2.0;

		// printf("\nCaricato in memoria triangolo numero %d\n",i);
		// printf("di vertici numero %d %d %d\n", triang[i][0], triang[i][1],  triang[i][2]);

		}



	// settaggio dei valori delle normali nel vettore t: soluzione 'di compromesso' 
	// per sapere poi velocemente in fase di rendering a quali vertici corrispondono le normali

		for(i=0; i<numfac; i++) {
		// normale del vertice #0 
			indexv0=ti[i][0];
			t[i][3][0]=n[indexv0][0];
			t[i][3][1]=n[indexv0][1];
			t[i][3][2]=n[indexv0][2];
		// normale del vertice #1 
			indexv1=ti[i][1];
			t[i][4][0]=n[indexv1][0];
			t[i][4][1]=n[indexv1][1];
			t[i][4][2]=n[indexv1][2];
		// normale del vertice #2 
			indexv2=ti[i][2];
			t[i][5][0]=n[indexv2][0];
			t[i][5][1]=n[indexv2][1];
			t[i][5][2]=n[indexv2][2];

		}


	}




	theMesh () {
		
		loadObject(file);
		
	}




	void render( void ) {

		int i=0;
		if(visiblepoints) {
			glDisable(GL_LIGHTING);
			glBegin(GL_POINTS);
			glColor3fv( pColor1 );
			for(i=1; i<=numvert; i++) {
				glVertex3dv( p[i] );
			}
			glEnable(GL_LIGHTING);
			glEnd();
			
		}


		if(visiblelines) {
			glBegin(GL_LINES);
			for(i=1; i<=numfac; i++) {
				glNormal3dv( t[i][3] );
				glVertex3dv( t[i][0] );
				glVertex3dv( t[i][1] );
				glVertex3dv( t[i][1] );
				glVertex3dv( t[i][2] );
				glVertex3dv( t[i][2] );
				glVertex3dv( t[i][0] );
			}
			glEnd();

			
		}


		if(visiblemesh) {
			glBegin(GL_TRIANGLES);
			for(i=1; i<=numfac; i++) {
					glNormal3dv( t[i][3] );
					glVertex3dv( t[i][0] );
					glNormal3dv( t[i][4] );
					glVertex3dv( t[i][1] );
					glNormal3dv( t[i][5] );
					glVertex3dv( t[i][2] );
			}
			glEnd();
		}
		glFlush();


		if (showboundbox) {
			glDisable(GL_LIGHTING);
			glColor3fv( lColor1 );
			glBegin(GL_LINES);
				glVertex3d( xmax, ymax, zmax );
				glVertex3d( xmax, ymax, zmin );
				glVertex3d( xmax, ymax, zmin );
				glVertex3d( xmax, ymin, zmin );
				glVertex3d( xmax, ymin, zmin );
				glVertex3d( xmax, ymin, zmax );
				glVertex3d( xmax, ymin, zmax );
				glVertex3d( xmax, ymax, zmax );
				glVertex3d( xmin, ymax, zmax );
				glVertex3d( xmin, ymax, zmin );
				glVertex3d( xmin, ymax, zmin );
				glVertex3d( xmin, ymin, zmin );
				glVertex3d( xmin, ymin, zmin );
				glVertex3d( xmin, ymin, zmax );
				glVertex3d( xmin, ymin, zmax );
				glVertex3d( xmin, ymax, zmax );
				glVertex3d( xmin, ymax, zmin );
				glVertex3d( xmax, ymax, zmin );
				glVertex3d( xmin, ymin, zmin );
				glVertex3d( xmax, ymin, zmin );
				glVertex3d( xmin, ymin, zmax );
				glVertex3d( xmax, ymin, zmax );
				glVertex3d( xmin, ymax, zmax );
				glVertex3d( xmax, ymax, zmax );		
			glEnd();
			glEnable(GL_LIGHTING);	
		}

	}


} mesh;






////// Callback controlli /////////////////////////////////////////

void gluiControl(int control) {

	switch(control) {

	case 1000: {
		switch (selectview) {
			case 0: { visiblepoints=1; visiblelines=0; visiblemesh=0; break; }
			case 1: { visiblepoints=0; visiblelines=1; visiblemesh=0; break; }
			case 2: { visiblepoints=0; visiblelines=0; visiblemesh=1; break; }
		}
		break; }

	case 1001: {
		printf("Mostra punti\n");
		break; }
	case 1002: {
		printf("Mostra wireframe\n");
		break; }
	case 1003: {
		printf("Mostra mesh\n");
		break; }
	case 1004: {
		if (light1on) {
			printf("Luce 1 accesa\n");
			glEnable(GL_LIGHT1); }
		else {
			printf("Luce 1 spenta\n");			
			glDisable(GL_LIGHT1); }
		break; }
	case 1005: {
		if (light2on) {
			printf("Luce 2 accesa\n");
			glEnable(GL_LIGHT2); }
		else {
			printf("Luce 2 spenta\n");			
			glDisable(GL_LIGHT2); }
		break; }
	case 1098: { 
		printf("Ripristino stato iniziale\n");
		eyeposz=0.15;
		rotate[0]=0.0;
		rotate[1]=0.0;
		rotate[2]=0.0;
		light2amb[0]=0.3;
		light2amb[1]=0.3;
		light2amb[2]=0.4;
		selectview=2;
		visiblepoints=0;
		visiblelines=0;
		visiblemesh=1;
		light1on=1;
		light2on=1;
		showboundbox=false;
		break; }
	case 1099: {
		exit(0);
		break; }

	default: break;

	}
	gluiSubWindow->sync_live();

}







// display callback /////////////////////////////////////////////////

void theDisplay(void) {

	glClearColor(0.0f, 0.1f, 0.2f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
		
	glFrustum(-0.01*xyratio, 0.01*xyratio, -0.01, 0.01, 0.01, 1.0);
	gluLookAt(eyeposx, eyeposy, eyeposz, meshcenter[0], meshcenter[1], meshcenter[2], 0.0, 1.0, 0.0);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	glLightfv(GL_LIGHT1, GL_POSITION, light1pos);
	glLightfv(GL_LIGHT1, GL_AMBIENT, light1amb);
	glLightfv(GL_LIGHT1, GL_DIFFUSE, light1diff);

	glLightfv(GL_LIGHT2, GL_POSITION, light2pos);
	glLightfv(GL_LIGHT2, GL_AMBIENT, light2amb);
	glLightfv(GL_LIGHT2, GL_DIFFUSE, light1diff); 
	glLightModelfv(GL_LIGHT_MODEL_AMBIENT, light2amb); 


	if (light1on) glEnable(GL_LIGHT1);
	if (light2on) glEnable(GL_LIGHT2);

	drawaxis();

	glPushMatrix();
	glTranslatef(meshcenter[0], meshcenter[1], meshcenter[2]);
	glRotatef(rotate[0], 1.0, 0.0, 0.0);
	glRotatef(rotate[1], 0.0, 1.0, 0.0);
	glRotatef(rotate[2], 0.0, 0.0, 1.0);	
	glTranslatef(-1*meshcenter[0], -1*meshcenter[1], -1*meshcenter[2]);
	mesh.render();
	glPopMatrix(); 

	glutPostRedisplay();
		
	glFlush();
	glutSwapBuffers();
}   


// reshape callback //////////////////////////////////////

void theReshape( int x, int y) {

	int tx, ty, tw, th;
	GLUI_Master.get_viewport_area(&tx, &ty, &tw, &th);
	glViewport(tx, ty, tw, th);

	xyratio=(float)tw/(float)th;

	glutPostRedisplay();


}



// mouse callback ///////////////////////////////////////

void theMouse(int button, int state, int x, int y) {

	if(button==GLUT_LEFT_BUTTON && state==GLUT_DOWN) {
		leftclick=true;
		currentx=x;
		currenty=y;
	} else leftclick=false;
	
	if(button==GLUT_RIGHT_BUTTON && state==GLUT_DOWN) {
		rightclick=true;
		currentx=x;
		currenty=y;
	} else rightclick=false;

	glutPostRedisplay(); 
		
}




// Motion callback /////////////////////////////////////

void  theMotion(int x, int y) {
	
	if(leftclick==true) {
		dy=float(currenty-y)/60000.0f;
		eyeposz-=dy;
	}

	if(rightclick==true) { // trackball x ruotare la mesh
		
		ix=(currentx-x)/20;	
		iy=(currenty-y)/20;		
		rotate[1]=(rotate[1]-ix)%360;
		rotate[0]=(rotate[0]-iy)%360;
		
	}

	glutPostRedisplay();

 }



// Callback tastiera //////////////////////////////////

void theKey (unsigned char key, int x, int y) {
	switch(key) {
		case 'q':
			exit(0);
			break;
		case 'p':
			visiblepoints=!visiblepoints;
			break;
		case 'l':
			visiblelines=!visiblelines;
			break;
		case 'm':
			visiblemesh=!visiblemesh;
			break;
		case 's':
			showboundbox=!showboundbox;
			break;
		case 'z':
			rotate[2]=(rotate[2]-1)%360;
			break;
	
	};

	gluiSubWindow->sync_live();
	glutPostRedisplay();

}



// Idle callback ///////////////////////////////////////////

void theIdle( void ) {
	if ( glutGetWindow() != mainWindow ) 
		glutSetWindow(mainWindow);  
	glutPostRedisplay();
}



// Inizializzazione ///////////////////////////////////////////




void masterInit( void ) {

	glutInitWindowSize( xsize, ysize) ;
	glutInitWindowPosition( 200, 100 );
	glutInitDisplayMode( GLUT_RGB | GLUT_DOUBLE | GLUT_DEPTH);

	mainWindow=glutCreateWindow("Mesh Viewer");
	
	glutDisplayFunc( theDisplay );
	glutMotionFunc( theMotion );
	GLUI_Master.set_glutIdleFunc( theIdle );
	GLUI_Master.set_glutReshapeFunc( theReshape );
	GLUI_Master.set_glutMouseFunc( theMouse );
	GLUI_Master.set_glutKeyboardFunc( theKey ); 

	glEnable(GL_LIGHTING);
	glEnable(GL_NORMALIZE);
	glEnable(GL_DEPTH_TEST);
	glEnable( GL_CULL_FACE );
	glFrontFace( GL_CW );
	glCullFace( GL_BACK );
	glHint( GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST );
	glShadeModel( GL_SMOOTH );
	glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, mat1amb);
	glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, mat1diff);
	glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, mat1spec);
	// glMaterialfv(GL_FRONT_AND_BACK, GL_EMISSION, mat1em);
	glMaterialf(GL_FRONT_AND_BACK, GL_SHININESS, mat1shin);


	/* glui e controlli */

	gluiSubWindow=GLUI_Master.create_glui_subwindow(mainWindow, GLUI_SUBWINDOW_RIGHT);

	gluiSubWindow->set_main_gfx_window(mainWindow);
	gluiSubWindow->add_statictext("       Mesh Viewer");


	gluiRolloutView=gluiSubWindow->add_rollout("                 Visualizza",true);

	gluiRadioView=gluiSubWindow->add_radiogroup_to_panel(gluiRolloutView, &selectview, SelectViewId, gluiControl);
	gluiSubWindow->add_radiobutton_to_group(gluiRadioView,"Punti");
	gluiSubWindow->add_radiobutton_to_group(gluiRadioView,"Wireframe");
	gluiSubWindow->add_radiobutton_to_group(gluiRadioView,"Mesh");
	gluiCheckBound=gluiSubWindow->add_checkbox_to_panel(gluiRolloutView, "Bound Box", &showboundbox, EnableBoundBox, gluiControl );

	
	gluiRolloutLights=gluiSubWindow->add_rollout("                          Luci",true);
	
	gluiCheckLight1En=gluiSubWindow->add_checkbox_to_panel(gluiRolloutLights, "Luce 1", &light1on, EnableLight1Id, gluiControl );
	gluiCheckLight2En=gluiSubWindow->add_checkbox_to_panel(gluiRolloutLights, "Luce 2", &light2on, EnableLight2Id, gluiControl );
	
	gluiSubWindow->add_statictext_to_panel( gluiRolloutLights, "Componenti Luce #2:");
	gluiLight2Red=gluiSubWindow->add_spinner_to_panel(gluiRolloutLights, "R", GLUI_SPINNER_FLOAT, &light2amb[0], SetLight2RedId, gluiControl );
	gluiLight2Red->set_float_limits( 0.0, 1.0, GLUI_LIMIT_CLAMP );
	gluiLight2Red->set_speed( 0.25 );
	gluiLight2Green=gluiSubWindow->add_spinner_to_panel(gluiRolloutLights, "G", GLUI_SPINNER_FLOAT, &light2amb[1], SetLight2GreenId, gluiControl );
	gluiLight2Green->set_float_limits( 0.0, 1.0, GLUI_LIMIT_CLAMP );
	gluiLight2Green->set_speed( 0.25 );
	gluiLight2Blue=gluiSubWindow->add_spinner_to_panel(gluiRolloutLights, "B", GLUI_SPINNER_FLOAT, &light2amb[2], SetLight2BlueId, gluiControl );
	gluiLight2Blue->set_float_limits( 0.0, 1.0, GLUI_LIMIT_CLAMP );
	gluiLight2Blue->set_speed( 0.25 );
	
	gluiReset=gluiSubWindow->add_button("Reset", ResetId, gluiControl);

	gluiExit=gluiSubWindow->add_button("Esci", QuitId, gluiControl);


}


void main( int argc, char **argv) {

	glutInit(&argc, argv);
	
	masterInit();

	glutMainLoop();

}
