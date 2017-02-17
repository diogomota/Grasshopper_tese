 private List<Line> arm_bars(ref List<Line> Lines, List<Point3d> arm_cld, List<int> connection_rings, List<Point3d> Cloud, int sub, int n_cabos){
    List<Line> _tempLines = new List<Line>();
    for (int a = 0;a < n_cabos / 2;a++){
      for (int i = 0;i <= 2;i++){
        if(i == 0){
          // connect w/ tower

          //Lower right
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] - 1) + sub], arm_cld[i + a * 34]));
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] - 1) + 2 * sub], arm_cld[5 + a * 34]));
          //Upper right
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] ) + sub], arm_cld[i + 9 + a * 34]));
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] ) + 2 * sub], arm_cld[i + 13 + a * 34]));

          //Lower left
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] - 1)], arm_cld[i + 17 + a * 34]));
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] - 1) + 3 * sub], arm_cld[i + 17 + 5 + a * 34]));
          //Upper left
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] )], arm_cld[i + 26 + a * 34]));
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] ) + 3 * sub], arm_cld[i + 17 + 5 + 8 + a * 34]));

          //Diagonals
          //1st plane right
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] - 1) + sub], arm_cld[9 + a * 34]));
          //1st plane left
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] - 1)], arm_cld[26 + a * 34]));

          //2nd plane right
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] - 1) + 2 * sub], arm_cld[13 + a * 34]));
          //2nd plane left
          Lines.Add(new Line(Cloud[4 + (4 + (sub - 1) * 4) * (connection_rings[0 + a] - 1) + 3 * sub], arm_cld[30 + a * 34]));


        }

        //diagonals 1st plane right
        Lines.Add(new Line(arm_cld[i + a * 34], arm_cld[10 + i + a * 34]));
        Lines.Add(new Line(arm_cld[i + a * 34], arm_cld[9 + i + a * 34]));
        //diagonals 1st plane left
        Lines.Add(new Line(arm_cld[17 + i + a * 34], arm_cld[27 + i + a * 34]));
        Lines.Add(new Line(arm_cld[17 + i + a * 34], arm_cld[26 + i + a * 34]));

        //diagonals 2nd plane right
        Lines.Add(new Line(arm_cld[5 + i + a * 34], arm_cld[14 + i + a * 34]));
        Lines.Add(new Line(arm_cld[5 + i + a * 34], arm_cld[13 + i + a * 34]));
        //diagonals 2nd plane left
        Lines.Add(new Line(arm_cld[22 + i + a * 34], arm_cld[31 + i + a * 34]));
        Lines.Add(new Line(arm_cld[22 + i + a * 34], arm_cld[30 + i + a * 34]));

        //1st lower chord
        //right
        Lines.Add(new Line(arm_cld[i + a * 34], arm_cld[i + 1 + a * 34]));
        //left
        Lines.Add(new Line(arm_cld[17 + i + a * 34], arm_cld[18 + i + a * 34]));

        //2nd lower chord
        //right
        Lines.Add(new Line(arm_cld[5 + i + a * 34], arm_cld[6 + i + a * 34]));
        //left
        Lines.Add(new Line(arm_cld[22 + i + a * 34], arm_cld[23 + i + a * 34]));

        //1st upper chord
        //right
        Lines.Add(new Line(arm_cld[i + 9 + a * 34], arm_cld[i + 10 + a * 34]));
        //left
        Lines.Add(new Line(arm_cld[i + 26 + a * 34], arm_cld[i + 27 + a * 34]));

        //2nd upper chord

        Lines.Add(new Line(arm_cld[13 + i + a * 34], arm_cld[14 + i + a * 34]));
        Lines.Add(new Line(arm_cld[30 + i + a * 34], arm_cld[31 + i + a * 34]));

        //end lines
        if(i == 2){
          //1nd lower chord
          //right
          Lines.Add(new Line(arm_cld[i + 1 + a * 34], arm_cld[i + 2 + a * 34]));
          //left
          Lines.Add(new Line(arm_cld[18 + i + a * 34], arm_cld[19 + i + a * 34]));

          //2nd lower chord
          //right
          Lines.Add(new Line(arm_cld[6 + i + a * 34], arm_cld[4 + a * 34]));
          //left
          Lines.Add(new Line(arm_cld[23 + i + a * 34], arm_cld[21 + a * 34]));

          //1st upper chord
          //right
          Lines.Add(new Line(arm_cld[10 + i + a * 34], arm_cld[4 + a * 34]));
          //left
          Lines.Add(new Line(arm_cld[24 + i + a * 34], arm_cld[21 + a * 34]));

          //2nd upper chord
          //right
          Lines.Add(new Line(arm_cld[14 + i + a * 34], arm_cld[4 + a * 34]));
          //left
          Lines.Add(new Line(arm_cld[28 + i + a * 34], arm_cld[21 + a * 34]));

          //Diagonals
          //1st plane right
          Lines.Add(new Line(arm_cld[i + 1 + a * 34], arm_cld[10 + i + a * 34]));
          //1st plane left
          Lines.Add(new Line(arm_cld[18 + i + a * 34], arm_cld[27 + i + a * 34]));
          //2nd plane right
          Lines.Add(new Line(arm_cld[i + 6 + a * 34], arm_cld[14 + i + a * 34]));
          //2nd plane left
          Lines.Add(new Line(arm_cld[23 + i + a * 34], arm_cld[31 + i + a * 34]));
        }
      }


    }
    return Lines;
  }