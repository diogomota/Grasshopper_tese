   private void RunScript(double Largura, int Altura, double horiz_div, double subdiv, int N_cabos, List<int> dist_centro, List<int> h_cabos, ref object Debug, ref object Cloud)
  {
    List<Point3d> Pt_cloud = new List<Point3d>(); // no programa final usar 2D array !!! [x,y,z]

    int x = 0;
    int y = 1;
    int h = 1;
    bool reverse = false;

    //#######################//
    //Tilt calc + N of rings //
    //#######################//
    double ring_z_step = (Altura) / horiz_div; // ok

    double tilt;
    tilt = (Largura * 0.5) / (Altura) * ring_z_step; //ok

    //##################//
    //main pt cloud loop//
    //##################//

    //Pts apoio
    Pt_cloud.Add(new Point3d(0, 0, 0));
    Pt_cloud.Add(new Point3d(Largura, 0, 0));
    Pt_cloud.Add(new Point3d(Largura, Largura, 0));
    Pt_cloud.Add(new Point3d(0, Largura, 0));

    for(h = 1; h <= horiz_div - 1; h++){ // ring loop

      double scale_factor = (1 - (h / horiz_div));
      double step = h * tilt;

      if(!reverse){ // x++ y++

        for(x = 0;x <= subdiv; x++){

          Pt_cloud.Add(new Point3d(step + x * (Largura / subdiv) * scale_factor, step, h * ring_z_step));

          if (x == subdiv){

            for(y = 1;y <= subdiv; y++){

              Pt_cloud.Add(new Point3d(Largura - step, step + y * (Largura / subdiv) * scale_factor, h * ring_z_step));

              if(x == subdiv && y == subdiv){reverse = true;} // start backwards loop
            }
          }
        }

      }

      if(reverse){ // x-- y--
        for(x = (int) subdiv - 1;x >= 0; x--){
          Pt_cloud.Add(new Point3d(step + x * (Largura / subdiv) * scale_factor, Largura - step, h * ring_z_step));

          if (x == 0){
            for(y = (int) subdiv - 1;y >= 1; y--){
              Pt_cloud.Add(new Point3d(step, step + y * (Largura / subdiv) * scale_factor, h * ring_z_step));


            }
          }
        }
      }

      reverse = false;
    }

    //Arms
    //encontrar os horiz + perto
    for(int n = 0;n < (N_cabos / 2);n++){
      int init_h = find_nearest(h_cabos[n], horiz_div, ring_z_step);

      //Debug = init_h; // remover

      double arm_lenght = Math.Abs(dist_centro[n] - (Largura * 0.5 - init_h * tilt));
      //lower arm angle
      double XY_m = (Largura * 0.5 - init_h * tilt) / arm_lenght; //inclinação da reta Y=mx em XY
      double XZ_m = (h_cabos[n] - init_h * ring_z_step) / arm_lenght;
      //upper arm angle
      double XZ_m_upp = (h_cabos[n] - (init_h + 1) * ring_z_step) / arm_lenght;


      //############//
      //#lower arms#//
      //###########//

      //right
      //1st lower arm
      for(int _subdiv = 1;_subdiv <= 5;_subdiv++){ // criar variavel se necessario controlo sobre o refinamento do braço
        double _x = (Largura - init_h * tilt) + (arm_lenght / 5) * _subdiv;
        if(_subdiv == 5){
          Pt_cloud.Add(new Point3d(_x, Largura * 0.5, h_cabos[n]));
          break;
        }
        Pt_cloud.Add(new Point3d(_x, init_h * tilt + XY_m * (_x - (Largura - init_h * tilt)), init_h * ring_z_step + XZ_m * (_x - (Largura - init_h * tilt))));//1st lower arm
      }

      //2nd lower arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // nao "<=" para nao criar 2 pts de convergencia
        double _x = (Largura - init_h * tilt) + (arm_lenght / 5) * _subdiv;
        Pt_cloud.Add(new Point3d(_x, Largura - init_h * tilt - XY_m * (_x - (Largura - init_h * tilt)), init_h * ring_z_step + XZ_m * (_x - (Largura - init_h * tilt))));
      }
      //left
      for(int _subdiv = 1;_subdiv <= 5;_subdiv++){ // criar variavel se necessario controlo sobre o refinamento do braço
        double _x = ( init_h * tilt) - (arm_lenght / 5) * _subdiv;
        if(_subdiv == 5){
          Pt_cloud.Add(new Point3d(_x, Largura * 0.5, h_cabos[n]));
          break;
        }
        Pt_cloud.Add(new Point3d(_x, init_h * tilt - XY_m * (_x - init_h * tilt), init_h * ring_z_step - XZ_m * (_x - init_h * tilt)));//1st lower arm
      }

      //2nd lower arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // nao "<=" para nao criar 2 pts de convergencia
        double _x = ( init_h * tilt) - (arm_lenght / 5) * _subdiv;
        Pt_cloud.Add(new Point3d(_x, Largura - init_h * tilt + XY_m * (_x - init_h * tilt), init_h * ring_z_step - XZ_m * (_x - init_h * tilt)));
      }

      //############//
      //#Upper arms#//
      //############//

      //right
      //1st upper arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // criar variavel se necessario controlo sobre o refinamento do braço
        double _x = (Largura - init_h * tilt) + (arm_lenght / 5) * _subdiv;

        Pt_cloud.Add(new Point3d(_x, init_h * tilt + XY_m * (_x - (Largura - init_h * tilt)), (init_h + 1) * ring_z_step + XZ_m_upp * (_x - (Largura - init_h * tilt))));
      }
      //2nd upper arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // nao "<=" para nao criar 2 pts de convergencia
        double _x = (Largura - init_h * tilt) + (arm_lenght / 5) * _subdiv;

        Pt_cloud.Add(new Point3d(_x, Largura - init_h * tilt - XY_m * (_x - (Largura - init_h * tilt)), (init_h + 1) * ring_z_step + XZ_m_upp * (_x - (Largura - init_h * tilt))));
      }

      //left
      //1st upper arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // criar variavel se necessario controlo sobre o refinamento do braço
        double _x = (init_h * tilt) - (arm_lenght / 5) * _subdiv;

        Pt_cloud.Add(new Point3d(_x, init_h * tilt - XY_m * (_x - init_h * tilt), (init_h + 1) * ring_z_step - XZ_m_upp * (_x - init_h * tilt)));
      }
      //2nd upper arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // nao "<=" para nao criar 2 pts de convergencia
        double _x = (init_h * tilt) - (arm_lenght / 5) * _subdiv;

        Pt_cloud.Add(new Point3d(_x, Largura - init_h * tilt + XY_m * (_x - init_h * tilt), (init_h + 1) * ring_z_step - XZ_m_upp * (_x - init_h * tilt)));
      }
    }

    Cloud = Pt_cloud;
  }

  // <Custom additional code> 
  private int find_nearest(int altura, double horiz_div, double ring_z_step){
    int nearest = -1;
    double dist = 10000;

    for (int h = 1; h <= horiz_div - 2; h++){
      if(dist > Math.Abs(altura - h * ring_z_step)){
        dist = Math.Abs(altura - h * ring_z_step);
        nearest = h;
      }
    }

    return nearest;
  }