 public class Script_Instance : GH_ScriptInstance
{
#region Utility functions
  /// <summary>Print a String to the [Out] Parameter of the Script component.</summary>
  /// <param name="text">String to print.</param>
  private void Print(string text) { /* Implementation hidden. */ }
  /// <summary>Print a formatted String to the [Out] Parameter of the Script component.</summary>
  /// <param name="format">String format.</param>
  /// <param name="args">Formatting parameters.</param>
  private void Print(string format, params object[] args) { /* Implementation hidden. */ }
  /// <summary>Print useful information about an object instance to the [Out] Parameter of the Script component. </summary>
  /// <param name="obj">Object instance to parse.</param>
  private void Reflect(object obj) { /* Implementation hidden. */ }
  /// <summary>Print the signatures of all the overloads of a specific method to the [Out] Parameter of the Script component. </summary>
  /// <param name="obj">Object instance to parse.</param>
  private void Reflect(object obj, string method_name) { /* Implementation hidden. */ }
#endregion

#region Members
  /// <summary>Gets the current Rhino document.</summary>
  private readonly RhinoDoc RhinoDocument;
  /// <summary>Gets the Grasshopper document that owns this script.</summary>
  private readonly GH_Document GrasshopperDocument;
  /// <summary>Gets the Grasshopper script component that owns this script.</summary>
  private readonly IGH_Component Component;
  /// <summary>
  /// Gets the current iteration count. The first call to RunScript() is associated with Iteration==0.
  /// Any subsequent call within the same solution will increment the Iteration count.
  /// </summary>
  private readonly int Iteration;
#endregion

  /// <summary>
  /// This procedure contains the user code. Input parameters are provided as regular arguments,
  /// Output parameters as ref arguments. You don't have to assign output parameters,
  /// they will have a default value.
  /// </summary>
  private void RunScript(double Largura, int Altura, double horiz_div, double subdiv, int N_cabos, List<int> dist_centro, List<int> h_cabos, ref object Debug, ref object Cloud, ref object Arm_pts, ref object connection_rings)
  {
    List<Point3d> Pt_cloud = new List<Point3d>(); // no programa final usar 2D array !!! [x,y,z]
    List<Point3d> arm_pt_cloud = new List<Point3d>();
    List<Int32> con_ring_set = new List<Int32>();

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

    ///////////////////////////
    //#######################//
    //######## Arms #########//
    //#######################//
    ///////////////////////////


    for(int n = 0;n < (N_cabos / 2);n++){   //for each set of cables

      int init_h = find_nearest(h_cabos[n], horiz_div, ring_z_step); //encontrar os horiz ring + perto
      con_ring_set.Add(init_h);
      double arm_lenght = Math.Abs(dist_centro[n] - (Largura * 0.5 - init_h * tilt));

      //lower arm angle
      double XY_m = (Largura * 0.5 - init_h * tilt) / arm_lenght; //inclinação da reta Y=mx em XY
      double XZ_m = (h_cabos[n] - init_h * ring_z_step) / arm_lenght; //inclinação da reta Z=mx em XZ

      //upper arm angle
      double XZ_m_upp = (h_cabos[n] - (init_h + 1) * ring_z_step) / arm_lenght;


      //############//
      //#Right arm#//
      //###########//

      //1st lower arm
      for(int _subdiv = 1;_subdiv <= 5;_subdiv++){ // criar variavel se necessario controlo sobre o refinamento do braço

        double _x = (Largura - init_h * tilt) + (arm_lenght / 5) * _subdiv;

        if(_subdiv == 5){
          arm_pt_cloud.Add(new Point3d(_x, Largura * 0.5, h_cabos[n]));
          break;
        }
        arm_pt_cloud.Add(new Point3d(_x, init_h * tilt + XY_m * (_x - (Largura - init_h * tilt)), init_h * ring_z_step + XZ_m * (_x - (Largura - init_h * tilt))));//1st lower arm
      }

      //2nd lower arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // nao "<=" para nao criar 2 pts de convergencia
        double _x = (Largura - init_h * tilt) + (arm_lenght / 5) * _subdiv;
        arm_pt_cloud.Add(new Point3d(_x, Largura - init_h * tilt - XY_m * (_x - (Largura - init_h * tilt)), init_h * ring_z_step + XZ_m * (_x - (Largura - init_h * tilt))));
      }

      //1st upper arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // criar variavel se necessario controlo sobre o refinamento do braço
        double _x = (Largura - init_h * tilt) + (arm_lenght / 5) * _subdiv;

        arm_pt_cloud.Add(new Point3d(_x, init_h * tilt + XY_m * (_x - (Largura - init_h * tilt)), (init_h + 1) * ring_z_step + XZ_m_upp * (_x - (Largura - init_h * tilt))));
      }
      //2nd upper arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // nao "<=" para nao criar 2 pts de convergencia
        double _x = (Largura - init_h * tilt) + (arm_lenght / 5) * _subdiv;

        arm_pt_cloud.Add(new Point3d(_x, Largura - init_h * tilt - XY_m * (_x - (Largura - init_h * tilt)), (init_h + 1) * ring_z_step + XZ_m_upp * (_x - (Largura - init_h * tilt))));
      }

      //############//
      //##Left arm##//
      //############//

      //left
      //1st lower arm
      for(int _subdiv = 1;_subdiv <= 5;_subdiv++){ // criar variavel se necessario controlo sobre o refinamento do braço

        double _x = ( init_h * tilt) - (arm_lenght / 5) * _subdiv;

        if(_subdiv == 5){
          arm_pt_cloud.Add(new Point3d(_x, Largura * 0.5, h_cabos[n]));
          break;
        }

        arm_pt_cloud.Add(new Point3d(_x, init_h * tilt - XY_m * (_x - init_h * tilt), init_h * ring_z_step - XZ_m * (_x - init_h * tilt)));//1st lower arm
      }

      //2nd lower arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // nao "<=" para nao criar 2 pts de convergencia
        double _x = ( init_h * tilt) - (arm_lenght / 5) * _subdiv;

        arm_pt_cloud.Add(new Point3d(_x, Largura - init_h * tilt + XY_m * (_x - init_h * tilt), init_h * ring_z_step - XZ_m * (_x - init_h * tilt)));
      }

      //1st upper arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // criar variavel se necessario controlo sobre o refinamento do braço
        double _x = (init_h * tilt) - (arm_lenght / 5) * _subdiv;

        arm_pt_cloud.Add(new Point3d(_x, init_h * tilt - XY_m * (_x - init_h * tilt), (init_h + 1) * ring_z_step - XZ_m_upp * (_x - init_h * tilt)));
      }
      //2nd upper arm
      for(int _subdiv = 1;_subdiv < 5;_subdiv++){ // nao "<=" para nao criar 2 pts de convergencia
        double _x = (init_h * tilt) - (arm_lenght / 5) * _subdiv;

        arm_pt_cloud.Add(new Point3d(_x, Largura - init_h * tilt + XY_m * (_x - init_h * tilt), (init_h + 1) * ring_z_step - XZ_m_upp * (_x - init_h * tilt)));
      }
    }

    Arm_pts = arm_pt_cloud;
    Cloud = Pt_cloud;
    connection_rings = con_ring_set;
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
  // </Custom additional code> 
}