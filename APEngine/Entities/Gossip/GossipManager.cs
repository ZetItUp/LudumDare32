using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using AtriLib2;

namespace APEngine.Entities.Gossip
{
    public class GossipManager
    {
        Dictionary<Gossip, string> NPCGossip;

        public GossipManager()
        {
            NPCGossip = new Dictionary<Gossip, string>();
        }

        public void LoadGossip(string NPCName)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Content\\Data\\Maps\\NPC\\" + NPCName + ".gossip";

            using(StreamReader sr = new StreamReader(path))
            {
                int lineNumber = 0;
                bool inGossip = false;

                while(sr.Peek() > 0)
                {
                    string currLine = sr.ReadLine();

                    if(!inGossip)
                    {
                        if(ATools.ContainsAtRange(currLine, "[gossip", 0, 7) && currLine.EndsWith("]"))
                        {
                            // Found beginning of gossip
                            inGossip = true;

                            int pos = ATools.GetStringPositionWithinString(currLine, "ID=\"");
                            pos += 3;
                            string tmpID = "";

                            for(int i = pos; i < currLine.Length; i++)
                            {
                                if(currLine[i] == '\"')
                                    break;
                                else
                                    tmpID += currLine[i];
                            }

                            tmpID += "1";
                        }
                        else if(currLine.StartsWith("[/gossip]"))
                        {
                            // Found end of gossip.. we are not inside a gossip, throw an error!
                            throw new Exception("Found end of gossip before gossip has begun!\nNPC: " + NPCName + "\nLine: " + currLine.ToString());
                        }
                    }
                    else if(inGossip)
                    {
                        if(currLine.StartsWith("[/gossip]"))
                        {
                            // Found end of gossip
                            inGossip = false;
                        }
                    }

                    lineNumber++;
                }
            }
        }
    }
}
