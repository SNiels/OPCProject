using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControls
{
    public interface IDraggableOPCNode
    {
        string NodeName { get; set; }
        double X { get; set; }
        double Y { get; set; }
    }
    public static class IDraggableExtensions
    {
        public static String GetCSVLine(this IDraggableOPCNode node){
            return node.NodeName + ";" + node.X + ";" + node.Y;
        }
        
    }
    public class OPCCSV:IDraggableOPCNode
    {
        public const string CSVHEADER = "Name;X;Y";
        public double X { get; set; }
        public double Y { get; set; }
        public string NodeName { get; set; }

        public static OPCCSV ParseCSVLine(string csvLine){
            String[] parts = csvLine.Split(';');
            string name = parts[0];
            double x =double.Parse(parts[1]);
            double y =double.Parse(parts[2]);
            return new OPCCSV{NodeName = name,X=x,Y=y };
        }

        public static IEnumerable<OPCCSV> GetOPCCSVS(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                reader.ReadLine();
                string line;
                while((line = reader.ReadLine())!=null){
                    yield return ParseCSVLine(line);
                }
            }
        }

        public static void WriteCSV(IEnumerable<IDraggableOPCNode> nodes, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(CSVHEADER);
                foreach (var node in nodes)
                {
                    writer.WriteLine(node.GetCSVLine());
                }
            }
        }

        public override string ToString()
        {
            return this.GetCSVLine();
        }

        public bool Equals(IDraggableOPCNode node)
        {
            return node.NodeName == NodeName;
        }
    }
}
