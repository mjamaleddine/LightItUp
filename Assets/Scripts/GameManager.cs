using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameManager
{

    public static GameManager Instance = new GameManager();

    public LevelManager CurrentLevel { get; set; }

}
