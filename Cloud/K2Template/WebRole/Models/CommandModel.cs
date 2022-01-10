using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class CommandModel
    {
        public string Command { get; set; }

        public CommandModel()
        {
        }

        public CommandModel(string command)
        {
            Command = command;
        }
    }
}