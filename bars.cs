 private void RunScript(List<Point3d> Cloud, int subdiv, int horiz_div, ref object bars)
  {
    List<Line> Lines = new List<Line>();
    //
    //Connect support pts
    //
    //add Lcr
    for(int i = 0; i <= 4;i++){

      if (i == 0){
        for (int j = 4;j <= 4 + subdiv;j++){
          Lines.Add(new Line(Cloud[0], Cloud[j]));
        }
        for (int j = 8 + 4 * (subdiv - 1) - 1;j >= 8 + 4 * (subdiv - 1) - subdiv;j--){
          Lines.Add(new Line(Cloud[0], Cloud[j]));
        }

      }
      if (i == 1){
        for (int j = 4;j <= 4 + 2 * subdiv;j++){
          Lines.Add(new Line(Cloud[1], Cloud[j]));
        }
      }
      if (i == 2){
        for (int j = 4 + subdiv;j <= 4 + 3 * subdiv;j++){
          Lines.Add(new Line(Cloud[2], Cloud[j]));
        }
      }
      if (i == 3){
        for (int j = 4 + subdiv * 2;j <= 4 + 4 * subdiv - 1;j++){
          Lines.Add(new Line(Cloud[3], Cloud[j]));
        }
        Lines.Add(new Line(Cloud[3], Cloud[4]));
      }
    }

    //Connect all other points
    //add Lcr nesta fase !!!
    int ring_pt = 4 + (subdiv - 1) * 4;
    //init and end pts
    for(int h = 0; h <= horiz_div - 3;h++){

      /*for(int i = 4 + ring_pt * h;i < 4 + ring_pt * (h+1);i++){//init coord.
        //init pt
        if(i == 4 + ring_pt * h){
          for(int j = 4+ring_pt * (h + 1);j <= subdiv + 4 + ring_pt * (h + 1);j++){
            Lines.Add(new Line(Cloud[i], Cloud[j]));
          }
          for(int j = 4 + ring_pt * (h + 2)-1;j >= 4 + ring_pt * (h + 2) - subdiv;j--){
          Lines.Add(new Line(Cloud[i], Cloud[j]));
          }
        }
        //end pt
        if(){

        }

        //inside pts

      }*/

      // Lado +XX
      for(int i = 4 + ring_pt * h;i < 4 + ring_pt * (h + 1);i++){//init coord.
        //lados
        if(i == 4 + ring_pt * h || i == 4 + ring_pt * h + subdiv){//cantos
          for(int j = 4 + ring_pt * (h + 1);j <= subdiv + 4 + ring_pt * (h + 1);j++){
            Lines.Add(new Line(Cloud[i], Cloud[j]));
          }
        }else{ //pts centrais
          if(i > 4 + ring_pt * h && i < 4 + ring_pt * h + subdiv){
            for(int j = 4 + ring_pt * (h + 1);j <= subdiv + 4 + ring_pt * (h + 1);j++){
              Lines.Add(new Line(Cloud[i], Cloud[j]));
            }
          }
        }
      }
      // Lado +YY
      for(int i = 4 + subdiv + ring_pt * h;i < 4 + subdiv + ring_pt * (h + 1);i++){//init coord.
        //lados
        if(i == 4 + subdiv + ring_pt * h || i == 4 + ring_pt * h + 2 * subdiv){//cantos
          for(int j = 4 + subdiv + ring_pt * (h + 1);j <= 2 * subdiv + 4 + ring_pt * (h + 1);j++){
            Lines.Add(new Line(Cloud[i], Cloud[j]));
          }
        }else{ //pts centrais
          if(i > 4 + subdiv + ring_pt * h && i < 4 + ring_pt * h + 2 * subdiv){
            for(int j = 4 + subdiv + ring_pt * (h + 1);j <= 2 * subdiv + 4 + ring_pt * (h + 1);j++){
              Lines.Add(new Line(Cloud[i], Cloud[j]));
            }
          }
        }
      }

      // Lado -XX
      for(int i = 4 + 2 * subdiv + ring_pt * h;i < 4 + 2 * subdiv + ring_pt * (h + 1);i++){//init coord.
        //lados
        if(i == 4 + 2 * subdiv + ring_pt * h || i == 4 + ring_pt * h + 3 * subdiv){//cantos
          for(int j = 4 + 2 * subdiv + ring_pt * (h + 1);j <= 3 * subdiv + 4 + ring_pt * (h + 1);j++){
            Lines.Add(new Line(Cloud[i], Cloud[j]));
          }
        }else{ //pts centrais
          if(i > 4 + 2 * subdiv + ring_pt * h && i < 4 + ring_pt * h + 3 * subdiv){
            for(int j = 4 + 2 * subdiv + ring_pt * (h + 1);j <= 3 * subdiv + 4 + ring_pt * (h + 1);j++){
              Lines.Add(new Line(Cloud[i], Cloud[j]));
            }
          }
        }
      }

      // Lado -YY
      for(int i = 4 + 3 * subdiv + ring_pt * h;i < 4 + 3 * subdiv + ring_pt * (h + 1);i++){//init coord.
        //lados
        if(i == 4 + 3 * subdiv + ring_pt * h){//cantos
          for(int j = 4 + 3 * subdiv + ring_pt * (h + 1);j <= 4 * subdiv + 4 + ring_pt * (h + 1) - 1;j++){
            Lines.Add(new Line(Cloud[i], Cloud[j]));
            Lines.Add(new Line(Cloud[4 + ring_pt * h], Cloud[j])); //conect first corner w/ side -YY
          }
        }else{ //pts centrais
          if(i > 4 + 3 * subdiv + ring_pt * h && i < 4 + ring_pt * h + 4 * subdiv){
            for(int j = 4 + 3 * subdiv + ring_pt * (h + 1);j <= 4 * subdiv + 4 + ring_pt * (h + 1) - 1;j++){
              Lines.Add(new Line(Cloud[i], Cloud[j]));
            }
            Lines.Add(new Line(Cloud[i], Cloud[4 + ring_pt * (h + 1)])); //conect to init pt
          }
        }
      }

    }

    //TODO: Horizontal connections
    //add Lcr nesta fase!!!
    Lines.Add(new Line(Cloud[0], Cloud[1]));
    //
    bars = Lines;


  }