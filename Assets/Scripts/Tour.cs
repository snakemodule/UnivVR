using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tour {
    public List<TourCommand> commands;

    public Tour(List<TourCommand> commands)
    {
        this.commands = commands;
    }

    
}
