#ifndef _SYSPROPERTIES_H_
#define _SYSPROPERTIES_H_

#define STANDARD 0

public class SysProperties {
	public:
		SysProperties(void);
		SysProperties(int type);
		~SysProperties(void);

		// Variabili del sistema fisico

		// vector<Spring*>		springs;			// Lista di molle
		// vector<Vector3D*>	normals;			// Normali oggetti
		
		float airRes;					// Resistenza dell'aria
		float gRepulsion;				// Costante di repulsione del terreno
		float gResistance;				// Attrito del terreno
		float gAbsorb;					// Costante di assorbimento
		float gravity_y;				// Vettore gravita'

		// variabili relative all'oggetto

		float height,length;			//dimensioni
		int anchor_number;				//numero ancore
		int open_type;					//tipo di apertura
		float open_min_dis,open_step;	//distanza minima tra ancore e step apertura
		
		// variabili delle molle
		float mass;						//massa dei nodi
		float spring_k;
		float spring_stiff_k;
		float spring_max_length;
		float equil_dist_x, equil_dist_y;
		float energy_loss;				// perdita di energia
		int n,m;						//dimensione matrice
		float x;						//distanza nodi

};

#endif