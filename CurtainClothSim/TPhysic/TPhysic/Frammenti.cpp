/*							
							// equilibrioper il collision management

							// equilibrio via trascinamento: sposto il VERTICE più VICINO alla collisione
							// provo il primo vertice
							tmp[0] = nextpos[0] - pos[base_coord[0]] - vel[base_coord[0]];
							tmp[1] = nextpos[1] - pos[base_coord[1]] - vel[base_coord[1]];
							tmp[2] = nextpos[2] - pos[base_coord[2]] - vel[base_coord[2]];
							dist_min = (float)sqrt(tmp[0]*tmp[0] + tmp[1]*tmp[1] + tmp[2]*tmp[2]);
							nearest_i = base_coord[0];
							// 2°
							tmp[0] = nextpos[0] - pos[base_coord[3]] - vel[base_coord[3]];
							tmp[1] = nextpos[1] - pos[base_coord[4]] - vel[base_coord[4]];
							tmp[2] = nextpos[2] - pos[base_coord[5]] - vel[base_coord[5]];
							dist_tmp = (float)sqrt(tmp[0]*tmp[0] + tmp[1]*tmp[1] + tmp[2]*tmp[2]);
							if(dist_tmp<dist_min) {
								dist_tmp = dist_tmp;
								nearest_i = base_coord[3];
							}
							// 3°
							tmp[0] = nextpos[0] - pos[base_coord[6]] - vel[base_coord[6]];
							tmp[1] = nextpos[1] - pos[base_coord[7]] - vel[base_coord[7]];
							tmp[2] = nextpos[2] - pos[base_coord[8]] - vel[base_coord[8]];
							dist_tmp = (float)sqrt(tmp[0]*tmp[0] + tmp[1]*tmp[1] + tmp[2]*tmp[2]);
							if(dist_tmp<dist_min) {
								dist_tmp = dist_tmp;
								nearest_i = base_coord[6];
							}
							// 4°
							tmp[0] = nextpos[0] - pos[base_coord[9]] - vel[base_coord[9]];
							tmp[1] = nextpos[1] - pos[base_coord[10]] - vel[base_coord[10]];
							tmp[2] = nextpos[2] - pos[base_coord[11]] - vel[base_coord[11]];
							dist_tmp = (float)sqrt(tmp[0]*tmp[0] + tmp[1]*tmp[1] + tmp[2]*tmp[2]);
							if(dist_tmp<dist_min) {
								dist_tmp = dist_tmp;
								nearest_i = base_coord[9];
							}
							
							//nearest_i = findNearestIndex(); // devo sapere qual'è il vertice più vicino
							vel[nearest_i] = 0.5f * (vel[nearest_i] + vel[check_ix]);
							nearest_i++;
							vel[nearest_i+1] = 0.5f * (vel[nearest_i] + vel[check_iy]);
							nearest_i++;
							vel[nearest_i+2] = 0.5f * (vel[nearest_i] + vel[check_iz]);
							*/