using AADS.Views.ReportManagement;
using Demo.WindowsForms.CustomMarkers;
using Demo.WindowsForms.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Net_GmapMarkerWithLabel;
using NewRadarUX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AADS
{
    public partial class mainForm : Form
    {
        private List<PointLatLng> _points = new List<PointLatLng>();
        internal readonly GMapOverlay top = new GMapOverlay();
        internal readonly GMapOverlay markersP = new GMapOverlay("markersP");
        internal readonly GMapOverlay Radar = new GMapOverlay("Radar");
        internal readonly GMapOverlay lineDistance = new GMapOverlay("lineDistance");
        internal readonly GMapOverlay polygons = new GMapOverlay("polygons");
        internal readonly GMapOverlay objects = new GMapOverlay("objects");
        internal readonly GMapOverlay Track = new GMapOverlay("Track");
        internal readonly GMapOverlay Test = new GMapOverlay("Test");
        internal readonly GMapOverlay BordersTrack = new GMapOverlay("BordersTrack");
        internal readonly GMapOverlay midlineDistance = new GMapOverlay("midlineDistance");

        internal readonly GMapOverlay minMapOverlay = new GMapOverlay("minMapOverlay");

        List<MapMode> mapModes = new List<MapMode>();
        GMapMarker currentMarker;
        GMapMarkerRect CurentRectMarker = null;
        GMapMarker markertag = null;
        GMapPolygon polygon;
        string action = null;
        static int simID = 300;
        bool minMapAutoZoom = false;
        MarkerAndPolygon markerAndPolygon;
        public RadarOverlay radarP;
        private List<PointLatLng> pointsToMark = new List<PointLatLng>();
        private bool lineClickCheck;
        public GMapOverlay GetOverlay(string Name)
        {
            return mainMap.Overlays.FirstOrDefault(x => x.Id == Name);
        }
        public static mainForm GetInstance()
        {
            return Instance;
        }
        public GMapControl GetmainMap()
        {
            return mainMap;
        }
        public static mainForm Instance;
        public mainForm()
        {
            InitializeComponent();
            Instance = this;
            mainMap.Manager.BoostCacheEngine = true;
            mainMap.MapScaleInfoEnabled = true;
            mainMap.MapScaleInfoPosition = MapScaleInfoPosition.Bottom;

            mapModes.Add(new MapMode
            {
                Name = "Map",
                MapProvider = GMapProviders.GoogleMap,
                MinZoom = 5,
                MaxZoom = 15
            });
            mapModes.Add(new MapMode
            {
                Name = "TerrainMap",
                MapProvider = GMapProviders.YandexMap,
                MinZoom = 5,
                MaxZoom = 15
            });
            mapModes.Add(new MapMode
            {
                Name = "SatelliteMap",
                MapProvider = GMapProviders.GoogleTerrainMap,
                MinZoom = 5,
                MaxZoom = 15
            });
            mapModes.Add(new MapMode
            {
                Name = "ReserveMap",
                MapProvider = GMapProviders.YandexHybridMap,
                MinZoom = 5,
                MaxZoom = 15
            });
            mapModes.Add(new MapMode
            {
                Name = "ReserveSatelliteMap",
                MapProvider = GMapProviders.GoogleHybridMap,
                MinZoom = 5,
                MaxZoom = 15
            });

            if (!GMapControl.IsDesignerHosted)
            {
                mainMap.Overlays.Add(Radar);
                mainMap.Overlays.Add(polygons);
                mainMap.Overlays.Add(lineDistance);
                mainMap.Overlays.Add(midlineDistance);
                mainMap.Overlays.Add(markersP);
                mainMap.Overlays.Add(objects);
                mainMap.Overlays.Add(Test);
                mainMap.Overlays.Add(Track);
                mainMap.Overlays.Add(BordersTrack);
                mainMap.Overlays.Add(top);
                minMap1.Overlays.Add(minMapOverlay);

                mainMap.Manager.Mode = AccessMode.ServerAndCache;
                mainMap.MapProvider = GMapProviders.GoogleMap; // set default map
                mainMap.Position = new PointLatLng(13.7563, 100.5018);
                mainMap.MinZoom = 5;
                mainMap.MaxZoom = 15;
                mainMap.Zoom = 10;

                minMap1.Manager.Mode = AccessMode.ServerAndCache;
                minMap1.MapProvider = GMapProviders.GoogleMap; // set default map
                minMap1.Position = new PointLatLng(13.7563, 100.5018);
                minMap1.MinZoom = 1;
                minMap1.MaxZoom = 15;
                minMap1.Zoom = 6;

                {
                    mainMap.OnPositionChanged += new PositionChanged(mainMap_OnPositionChanged);

                    //mainMap.OnMapZoomChanged += new MapZoomChanged(mainMap_OnMapZoomChanged);
                    mainMap.OnMapTypeChanged += new MapTypeChanged(mainMap_OnMapTypeChanged);

                    mainMap.MouseUp += new MouseEventHandler(mainMap_MouseUp);
                    mainMap.MouseDown += new MouseEventHandler(mainMap_MouseDown);
                    mainMap.MouseMove += new MouseEventHandler(mainMap_MouseMove);
                    mainMap.MouseClick += new MouseEventHandler(mainMap_MouseClick);

                    mainMap.OnMarkerClick += new MarkerClick(mainMap_OnMarkerClick);
                }

                {
                    flightWorker.DoWork += new DoWorkEventHandler(flight_DoWork);
                    flightWorker.ProgressChanged += new ProgressChangedEventHandler(flight_ProgressChanged);
                    flightWorker.WorkerSupportsCancellation = true;
                    flightWorker.WorkerReportsProgress = true;
                }

                flightWorker.RunWorkerAsync();

                currentMarker = new GMarkerGoogle(mainMap.Position, GMarkerGoogleType.arrow);
                currentMarker.IsHitTestVisible = false;
                top.Markers.Add(currentMarker);
                //Console.WriteLine(CPC.Intersection(new PointLatLng(51.8853, 0.2545), new PointLatLng(49.0034, 2.5735), 108.547, 32.435).ToString());
                //Console.WriteLine(CPC.destinationPoint(new PointLatLng(51.127, 1.338), 40300, 116.7));
                //Console.WriteLine(CPC.rhumbBearingTo(new PointLatLng(51.127, 1.338),new PointLatLng(50.964, 1.853)).ToString());
            }
        }
        void updateMinMap()
        {
            minMap1.Position = mainMap.Position;
            List<PointLatLng> plist = new List<PointLatLng>();
            int width = mainMap.Size.Width - 1;
            int height = mainMap.Size.Height - 1;
            plist.Add(mainMap.FromLocalToLatLng(0, 0));
            plist.Add(mainMap.FromLocalToLatLng(width, 0));
            plist.Add(mainMap.FromLocalToLatLng(width, height));
            plist.Add(mainMap.FromLocalToLatLng(0, height));
            GMapPolygon poly = new GMapPolygon(plist, "");
            poly.Fill = Brushes.Transparent;
            poly.Stroke = new Pen(Brushes.Red, 1.6f);
            minMapOverlay.Polygons.Clear();
            minMapOverlay.Polygons.Add(poly);
            if (minMapAutoZoom)
            {
                int minMapArea = minMap1.Size.Height * minMap1.Size.Width;
                long dX = minMap1.FromLatLngToLocal(poly.Points[1]).X - minMap1.FromLatLngToLocal(poly.Points[0]).X;
                long dY = minMap1.FromLatLngToLocal(poly.Points[2]).Y - minMap1.FromLatLngToLocal(poly.Points[1]).Y;
                long area = dX * dY;
                double ratio = (double)area / minMapArea;
                if (ratio < 0.2)
                {
                    minMap1.Zoom++;
                }
                else if (ratio > 0.8)
                {
                    minMap1.Zoom--;
                }
            }
        }
        #region -- flight Progress --

        BackgroundWorker flightWorker = new BackgroundWorker();

        void flight_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!flightWorker.CancellationPending)
            {
                try
                {
                    /*string sql = "SELECT * FROM `info_track`";
                    MySqlConnection com = new MySqlConnection("host=10.109.68.154;user=adminRadar;password=AdminPK23710;database=radar_info;Max Pool Size=100;");
                    MySqlCommand cmd = new MySqlCommand(sql, com);

                    try
                    {
                        com.Open();
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            var ex = new GMapMarkerPlane(PositionConverter.ParsePointFromString(dataReader.GetString("TK_PosX") + "," + dataReader.GetString("TK_PosY")), (float)dataReader.GetInt32("TK_Dir"));

                            int status = dataReader.GetInt32("TK_IFF");
                            if (status == 0)
                            {
                                ex.icon = (Bitmap)Image.FromFile("Images/airplane_Friendly.png");
                            }
                            else if (status == 1)
                            {
                                ex.icon = (Bitmap)Image.FromFile("Images/airplane_Hostile.png");
                            }
                            else
                            {
                                ex.icon = (Bitmap)Image.FromFile("Images/airplane_Unknown.png");
                            }

                            //ex.ToolTipText = PositionConverter.ParsePointToString(MainMap.Position, comboBoxScale.Text);
                            //ex.ToolTipPosition.Offset(ex.Offset);
                            //ex.ToolTipMode = MarkerTooltipMode.Always;
                            Track.Markers.Add(ex);
                        }
                    }
                    finally
                    {
                        com.Close();
                    }*/

                    flightWorker.ReportProgress(100);
                }
                finally
                {

                }
                Thread.Sleep(1 * 1000);
            }
        }

        private int index;
        private Track track = new Track();
        public int SearchPlane(int id)
        {
            foreach (int faker in track.Fakers)
            {
                FlightRadarData fd = track.GetFaker(faker);
                if (fd.Id == id)
                {
                    return faker;
                }
            }
            return -1;
        }
        bool IsPointInPolygon(List<PointLatLng> points, PointLatLng point)
        {
            bool isInside = false;
            for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
            {
                if (((points[i].Lat > point.Lat) != (points[j].Lat > point.Lat)) &&
                (point.Lng < (points[j].Lng - points[i].Lng) * (point.Lat - points[i].Lat) / (points[j].Lat - points[i].Lat) + points[i].Lng))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }
        private int indexfaker;
        public PointLatLng pointRader = new PointLatLng();
        public FlightRadarData TagPlane = null;
        void flight_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            mainMap.HoldInvalidation = true;

            lock (track)
            {
                foreach (int id in ProcessFlight.AllflightMarkers.Keys)
                {
                    List<PointLatLng> points = new List<PointLatLng>();

                    points.Add(ProcessFlight.AllflightMarkers[id].Position);
                    points.Add(pointRader);
                    GMapPolygon Distance = new GMapPolygon(points, "Distance");

                    if (Distance.Distance > (double)RadarRadius)
                    {
                        track.RemoveFaker(id);
                        if (TagPlane != null && TagPlane.Id == id)
                        {
                            TagPlane = null;
                        }
                    }

                    /*if (flightIdInfo.Text != "" && ProcessFlight.AllflightMarkers[id].id == int.Parse(flightIdInfo.Text))
                    {
                        indexfaker = id;
                    }*/
                    Track.Markers.Remove(ProcessFlight.AllflightMarkers[id]);
                    BordersTrack.Markers.Remove(ProcessFlight.AllBordersTrack[id]);
                }
                ProcessFlight.AllflightMarkers.Clear();
                ProcessFlight.AllBordersTrack.Clear();

                ProcessFlight.testDoWork();
                MarkerAndPolygon map = MarkerAndPolygon.GetInstance();
                foreach (int faker in track.Fakers)
                {
                    FlightRadarData fd = track.GetFaker(faker);

                    var ex = new GMapMarkerPlane(fd.point, (float)fd.bearing, fd.speed, fd.Id, mainMap.Zoom);
                    ex.id = fd.Id;
                    ex.setIcon(fd.identification);
                    ex.flight = fd;
                    ex.Tag = new SimpleTrackInfo(TrackType.Faker, fd.Id);
                    //ex.ToolTipText = PositionConverter.ParsePointToString(MainMap.Position, comboBoxScale.Text);
                    //ex.ToolTipPosition.Offset(ex.Offset);
                    //ex.ToolTipMode = MarkerTooltipMode.Always;

                    GMapMarkerRectPlane mBorders = new GMapMarkerRectPlane(ex.Position);
                    mBorders.InnerMarker = ex;

                    ProcessFlight.AllflightMarkers.Add(fd.Id, ex);
                    ProcessFlight.AllBordersTrack.Add(fd.Id, mBorders);

                    bool isInside = false;
                    foreach (GMapPolygon poly in map.RestrictedArea)
                    {
                        isInside = IsPointInPolygon(poly.Points, fd.point);
                        if (isInside) break;
                    }
                    if (!isInside || track.isFollow(faker))
                    {
                        BordersTrack.Markers.Add(mBorders);
                        Track.Markers.Add(ex);
                    }
                    /*var strDecoder = ConvertDataDecoder.Convertcode(fd.point, fd.bearing, fd.altitude, fd.speed, fd.Id, fd.identification, SetSerialPort.decoder);
                    if (SetSerialPort.sportPort_send.IsOpen)
                    {
                        SetSerialPort.sportPort_send.Write(strDecoder);
                        Console.WriteLine(strDecoder);
                    }

                    // to Wait for correction
                    Encoder encoder = new Encoder();
                    Radar radar = new Radar(41, radarP.Position);
                    string strdecode = encoder.Encode(fd, radar);
                    Console.WriteLine(strdecode);
                    SetSerialPort.sportPort_send.Write(strdecode);*/
                }

                Dictionary<int, FlightRadarData> realTrack = track.CloneRealTrack();

                foreach (int real in realTrack.Keys)
                {
                    FlightRadarData fd = realTrack[real];
                    var ex = new GMapMarkerPlane(fd.point, (float)fd.bearing, fd.speed, fd.Id, mainMap.Zoom);
                    ex.setIcon(track.getStatus(fd.Id));
                    ex.flight = fd;
                    ex.Tag = new SimpleTrackInfo(TrackType.Real, fd.Id);

                    GMapMarkerRectPlane mBorders = new GMapMarkerRectPlane(ex.Position);
                    mBorders.InnerMarker = ex;

                    ProcessFlight.AllflightMarkers.Add(fd.Id, ex);
                    ProcessFlight.AllBordersTrack.Add(fd.Id, mBorders);

                    bool isInside = false;
                    foreach (GMapPolygon poly in map.RestrictedArea)
                    {
                        isInside = IsPointInPolygon(poly.Points, fd.point);
                        if (isInside) break;
                    }
                    if (!isInside || track.isFollow(real))
                    {
                        BordersTrack.Markers.Add(mBorders);
                        Track.Markers.Add(ex);
                    }
                }
                track.Save();
            }

            //updateline();
            //CalculateCPC();
            //updatePlaneInfo();
            mainMap.Refresh();
        }
        #endregion

        #region -- event mainMap --
        void mainMap_OnPositionChanged(PointLatLng point)
        {
            updateMinMap();
        }

        void mainMap_OnMapTypeChanged(GMapProvider type)
        {

        }

        void mainMap_OnMapZoomChanged()
        {
            updateMinMap();
        }
        void mainMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {

        }

        bool isMouseDown = false;
        bool isRightClick = false;
        Point lastLocation;
        void mainMap_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        void mainMap_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            isRightClick = e.Button == MouseButtons.Right;
        }

        void mainMap_MouseMove(object sender, MouseEventArgs e)
        {
            lastLocation = e.Location;
        }

        void mainMap_MouseClick(object sender, MouseEventArgs e)
        {
            PointLatLng pnew = mainMap.FromLocalToLatLng(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                currentMarker.Position = pnew;
            }
            if (action != null)
            {
                string ca = action;
                action = null;
                if (ca == "fixedPointAdd")
                {
                    labelCurrentAction.Text = "Action: Free";
                    callFixedPoint();
                }
            }
        }
        #endregion

        #region -- data Code --
        private int RadarRadius = 140;
        private Dictionary<string, RadarOverlay> radars = new Dictionary<string, RadarOverlay>();
        private void loadPointR()
        {
            //first section
            {
                string path = @"พิกัดเรดาร์.txt";
                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                            //PointLatLng pointR = PositionConverter.ParsePointFromString(line);
                            //pointRader = pointR;
                            //textBoxPositionR.Text = PositionConverter.ParsePointToString(pointR, comboBoxScale.Text);
                        }
                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine("13.75, 100.517");
                        //textBoxPositionR.Text = "13.75, 100.517";
                        writer.Close();
                    }
                }
            }
            string name = "Test";
            int interval = 140;//Convert.ToInt32(textBoxRadarInterval.Text);
            radarP = new RadarOverlay(name, Radar);
            int x = 0;
            //Int32.TryParse(textBoxRadarRadius.Text, out x);
            radarP.InitialRadar(PositionConverter.ParsePointFromString("13.75, 100.517"), x, interval);
            radars.Add(name, radarP);
        }
        #endregion
        private void updateCmbMapMode()
        {
            cmbMapMode.Items.Clear();
            foreach (MapMode mapMode in mapModes)
            {
                cmbMapMode.Items.Add(mapMode.Name);
            }
        }
        private void mainForm_Load(object sender, EventArgs e)
        {
            timeNow.Start();
            updateMinMap();
            updateCmbMapMode();
            cmbMapMode.SelectedIndex = 0;
            //createRoute();
        }
        private void closeBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void maximizeBox_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void minimizeBox_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void timeNow_Tick(object sender, EventArgs e)
        {
            DateTime Date = DateTime.Now;
            dateLabel.Text = Date.ToString("dd-MMM-yyyy");
            time_label.Text = Date.ToString("HH:mm:ss");
        }
        private void callFixedPoint()
        {
            using (Views.FixedPoint.main form = new Views.FixedPoint.main())
            {
                form.OnClickAdd += new EventHandler(fixedPoint_ClickAdd);
                form.ShowDialog();
            }
        }
        private void cmbMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMenu.SelectedIndex > -1)
            {
                if (cmbMenu.SelectedItem.ToString().Equals("Fixed Point"))
                {
                    callFixedPoint();
                }
            }
        }
        private void fixedPoint_ClickAdd(object sender, EventArgs e)
        {
            action = "fixedPointAdd";
            labelCurrentAction.Text = "Action: Fixed Point";
        }

        private void cmbMapMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbMapMode.SelectedIndex;
            MapMode mode = mapModes[index];
            if (mainMap.MapProvider != mode.MapProvider)
            {
                mainMap.MapProvider = mode.MapProvider;
                minMap1.MapProvider = mode.MapProvider;
                mainMap.MinZoom = mode.MinZoom;
                mainMap.MaxZoom = mode.MaxZoom;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tableName = "track";
            reportClicked();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tableName = "trackHostile";
            reportClicked();
        }
        private string tableName;
        private void reportClicked()
        {
            using (Views.ReportManagement.main form = new main(this.tableName))
            {
                form.ShowDialog();
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            btnVit.Visible = true;
        }

        private void marker1_Load(object sender, EventArgs e)
        {

        }

        private void marker1_Load_1(object sender, EventArgs e)
        {

        }
        private int xPoint;
        private int yPoint;
        private void mainMap_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (lineClickCheck)
            {
                xPoint = e.X;
                yPoint = e.Y;
                
                if (e.Button == MouseButtons.Left)
                {
                    createMarker(e.X, e.Y);
                }
            }



            //clickCheck = true;
        }
        private List<GMarkerGoogle> markerArr = new List<GMarkerGoogle>();
        GMapOverlay markers = new GMapOverlay("markers");
        private bool vitSelected = false;
        private void createMarker(int x, int y)
        {
            //string var = "lblPoint" + i.ToString();
            var pointMarker = mainMap.FromLocalToLatLng(x, y);
            
            GMarkerGoogle marker = new GMarkerGoogle(pointMarker, GMarkerGoogleType.arrow);
            markerArr.Add(marker);
            markers.Markers.Add(marker);
            mainMap.Overlays.Add(markers);
            pointsToMark.Add(new PointLatLng(marker.Position.Lat, marker.Position.Lng));
            createRoute();
            updateMap();
            rightPanel.points.Add(pointMarker.ToString());
            rightPanel.setListBox();

        }
        private void btnVit_Click(object sender, EventArgs e)
        {
            panelVit.Visible = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnMarker_Click(object sender, EventArgs e)
        {
            btnVit.Visible = true;
            if (btnLine.Visible)
            {
                btnLine.Visible = false;
                btnLineRoute.Visible = false;
            }
        }
        private void btnVit_Click_1(object sender, EventArgs e)
        {
            GMapOverlay markers = new GMapOverlay("markers");
            GMarkerGoogle marker;
            panelVit.Visible = true;
            if (rdbVitClick2M.Checked)
            {
                updateMap();
                var point = mainMap.FromLocalToLatLng(xPoint, yPoint);
                txtVitLat.Text = currentMarker.Position.Lat.ToString();
                txtVitLng.Text = currentMarker.Position.Lng.ToString();
                marker = new GMarkerGoogle(point, GMarkerGoogleType.red);
                markers.Markers.Add(marker);
                mainMap.Overlays.Add(markers);
                currentMarker.IsVisible = false;
            }
        }

        private void mainMap_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            btnLine.Location = btnMarker.Location;
            btnMarker.Visible = false;
            btnVit.Visible = false;
            btnLineRoute.Visible = true;
            btnLineRoute.Location = btnLine.Location;
        }

        private void btnLineRoute_Click(object sender, EventArgs e)
        {
            btnLineRoute.Visible = false;
            panelVit.Visible = false;
            rightPanel.Visible = true;
            lineClickCheck = rightPanel.addClick;
            panelControlOptions.Visible = true;
        }

        private void btnCreateRoute_Click(object sender, EventArgs e)
        {
            createRoute();
            updateMap();
        }
        void updateMap()
        {
            mainMap.Zoom += 0.0000001;
            mainMap.Zoom -= 0.0000001;
        }
        private List<PointLatLng> testArr = new List<PointLatLng>();
        private bool checkRouteSelectedType = false;
        GMapOverlay overlay = new GMapOverlay("route");
        private List<GMapRoute> routeArr = new List<GMapRoute>();
        void createRoute()
        {
            GMapRoute route = new GMapRoute(pointsToMark, "route");
            routeArr.Add(route);
            if (rightPanel.colorCheck != "white")
            {
                checkRouteSelectedType = true;
                if (checkRouteSelectedType)
                {
                    if (rightPanel.colorCheck == "Red")
                    {
                        route.Stroke = new Pen(Brushes.Red, 1);
                    }
                    else if (rightPanel.colorCheck == "Brown")
                    {
                        route.Stroke = new Pen(Brushes.Brown, 1);
                    }
                    else if (rightPanel.colorCheck == "Deepblue")
                    {
                        route.Stroke = new Pen(Brushes.Blue, 1);
                    }
                }
            }
            route.IsHitTestVisible = true;
            overlay.Routes.Add(route);
            mainMap.Overlays.Add(overlay);
            updateMap();
        }
        private void rightPanel1_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            
        }
        void reset()
        {
            foreach (var m in markerArr)
            {
                m.IsVisible = false;
                mainMap.Overlays.Remove(overlay);
            }
            foreach (var r in routeArr)
            {
                r.IsVisible = false;
                mainMap.Overlays.Remove(markers);
            }
            rightPanel.reset(true);
            pointsToMark.Clear();
            lineClickCheck = false;
        }
        private void btnStopMarking_Click(object sender, EventArgs e)
        {
            lineClickCheck = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lineClickCheck = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lineClickCheck = true;
        }

        private void mainMap_OnRouteClick(GMapRoute item, MouseEventArgs e)
        {
            rightPanel.Visible = true;
        }

        //testing polygon and route
        void testRoute()
        {
            List<PointLatLng> testPoints = new List<PointLatLng>();
            testPoints.Add(new PointLatLng(13.999, 100.30));
            testPoints.Add(new PointLatLng(12.999, 100.30));
            GMapRoute route = new GMapRoute(testPoints,"test");
            GMapOverlay routeOverlay = new GMapOverlay("route");
            routeOverlay.Routes.Add(route);
            mainMap.Overlays.Add(routeOverlay);
        }

        void testPolygon()
        {

            List<PointLatLng> points = new List<PointLatLng>();
            points.Add(new PointLatLng(14.999, 85.1));
            points.Add(new PointLatLng(13.999, 90.1));
            points.Add(new PointLatLng(11.999, 102.1));
            points.Add(new PointLatLng(18.999, 101.1));
            GMapPolygon polygon = new GMapPolygon(points, "testPoly");
            GMapOverlay polyOverlay = new GMapOverlay("polygon");
            polygon.IsHitTestVisible = true;
            polyOverlay.Polygons.Add(polygon);
            mainMap.Overlays.Add(polyOverlay);
            updateMap();
        }

        private void mainMap_OnPolygonClick(GMapPolygon item, MouseEventArgs e)
        {
            MessageBox.Show("This polygon has been clicked");
        }
    }
}
