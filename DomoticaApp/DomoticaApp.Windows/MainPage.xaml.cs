using JDADragBehavior;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DomoticaApp
{
    public sealed partial class MainPage : Page
    {
        private string _baseSocket = new Windows.ApplicationModel.Resources.ResourceLoader().GetString("base_url");
        private const string GETWCFNODESPATH = "GetWCFNodes";
        private const string GETWCFNODEVALUEPATH = "GetWCFNodeValue";
        private IEnumerable<WCFNode> _lamps;
        private List<OPCCSV> _opcCSVS=new List<OPCCSV>();

        public MainPage()
        {
            this.InitializeComponent();
            try
            {
                ReadCSV();
            }
            catch (Exception) { }
            
            App.Current.Suspending += Current_Suspending;
            this.Loaded += MainPage_Loaded;
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await InitData();
            ShowLamps();
        }

        private async void ReadCSV()
        {
            var csvs= await OPCCSV.GetOPCCSVS();
            if(csvs!=null)
                _opcCSVS.AddRange(csvs);
        }

        private async Task<IEnumerable<WCFNode>> InitData()
        {
            IEnumerable<WCFNode> nodes = await GetWCFNodes(new Uri(_baseSocket + GETWCFNODESPATH));
            _lamps = GetLamps(nodes);
            foreach (WCFNode node in _lamps)
            {
                node.Value = Boolean.Parse(await GetWCFValue(new Uri(_baseSocket + GETWCFNODEVALUEPATH+"/"+node.ItemId)));
            }
            return nodes;
        }

        private void ShowLamps()
        {
            foreach (WCFNode node in _lamps)
            {
                CCLamp lamp = new CCLamp(node);
                rootGrid.Children.Add(lamp);
                lamp.Height = 30;
                lamp.Width = 30;
                lamp.HorizontalAlignment = HorizontalAlignment.Left;
                lamp.VerticalAlignment = VerticalAlignment.Top;
                lamp.RenderTransform = new TranslateTransform();

                lamp.SetValue(JDADragDepProp.JDADragProperty, true);
                lamp.PointerReleased += lamp_PointerReleased;
            }
            PositionControls(rootGrid.Children.OfType<CCLamp>());
        }

        void lamp_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PositionControls(IEnumerable<IDraggableWCFNode> nodes)
        {
            foreach (IDraggableWCFNode node in nodes)
            {
                OPCCSV opccsv = _opcCSVS.Find(n => n.NodeName == node.NodeName);
                if (opccsv != null)
                {
                    node.X = opccsv.X;
                    node.Y = opccsv.Y;
                }
            }
        }



        void Current_Suspending(object sender, SuspendingEventArgs e)
        {
            OPCCSV.WriteCSV(rootGrid.Children.OfType<IDraggableWCFNode>());
        }



        public static async Task<IEnumerable<WCFNode>> GetWCFNodes(Uri uri)
        {
            HttpClient client = new HttpClient();
            String response = await client.GetStringAsync(uri);
            return ParseJSONToNodes(response);
        }

        public static IEnumerable<WCFNode> ParseJSONToNodes(String response)
        {
            IEnumerable<WCFNode> nodes;
            nodes = JsonConvert.DeserializeObject<IEnumerable<WCFNode>>(response);
            return nodes;
        }

        public static IEnumerable<WCFNode> GetLamps(IEnumerable<WCFNode> nodes)
        {
            return nodes.Where(l => l.Name.StartsWith("lamp") || l.Name.StartsWith("lmp"));
        }

        public static async Task<string> GetWCFValue(Uri uri)
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync(uri);
        }
    }
}
