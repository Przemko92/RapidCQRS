﻿using RapidCqrs.Models.Interfaces;

namespace RapidCqrsApp.Models.Events
{
    public class MySimpleEvent 
    {
        public string First { get; set; }
        public int Second { get; set; }
    }
}
