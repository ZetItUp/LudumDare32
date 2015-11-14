using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APEngine.Entities.Gossip
{
    public class GossipChoice
    {
        public string Data { get; set; }
        public string Next { get; set; }

        public GossipChoice()
        {
            Data = "";
            Next = "";
        }
    }

    public class Gossip
    {
        public string ID { get; set; }
        private List<string> Lines = new List<string>();
        public List<GossipChoice> Choices = new List<GossipChoice>();
        public int CurrentLine
        {
            private set;
            get;
        }

        public string GetNextLine
        {
            get 
            {
                if(CurrentLine >= Lines.Count)
                    return "";
                else
                {
                    CurrentLine++;
                    return Lines[CurrentLine - 1];
                }
            }
        }

        public Gossip()
        {
            ID = "";
            CurrentLine = 0;
        }
    }
}
