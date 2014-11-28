using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage;

namespace DomoticaApp
{
    public interface IDraggableWCFNode
    {
        string NodeName { get; set; }
        double X { get; set; }
        double Y { get; set; }
    }
    public static class IDraggableExtensions
    {
        public static String GetCSVLine(this IDraggableWCFNode node)
        {
            return node.NodeName + ";" + node.X + ";" + node.Y;
        }
        
    }
    public class OPCCSV : IDraggableWCFNode
    {
        public const string CSVHEADER = "Name;X;Y";
        public const string CSVFILE = "nodes.xml";
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

        public async static Task<IEnumerable<OPCCSV>> GetOPCCSVS()
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile file = await folder.GetFileAsync(CSVFILE);
                return (await FileIO.ReadLinesAsync(file)).Select(s => OPCCSV.ParseCSVLine(s));
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public async static void WriteCSV(IEnumerable<IDraggableWCFNode> nodes)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(CSVFILE, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteLinesAsync(file,
                nodes.Select(n => n.GetCSVLine())
                );
        }

        public override string ToString()
        {
            return this.GetCSVLine();
        }

        public bool Equals(IDraggableWCFNode node)
        {
            return node.NodeName == NodeName;
        }
    }
}
