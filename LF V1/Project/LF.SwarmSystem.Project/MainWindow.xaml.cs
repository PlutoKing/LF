/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindow
 * Created      : 2020-10-21 15:53:56
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System.Windows;
using LF.SwarmSystem.Project.Windows;
using LF.Figure;
using System.Drawing;
using LF.FlightControl;
using LF.TaskAllocation;

namespace LF.SwarmSystem.Project
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        SwarmSimulation sim = new SwarmSimulation();

        bool isAllowTask = false;
        bool isAllowDrone = false;

        public static FigureControl MainMap ;
        public static Overlay DroneOverlay;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            MainMap = map;
            map.Figure.Broder.IsVisible = true;
            map.Figure.Broder.Line.Color = Color.FromArgb(172, 172, 172);
            map.Figure.XAxis.Title.Text = "x/m";
            map.Figure.YAxis.Title.Text = "y/m";
            map.Figure.XAxis.Scale.Max = 1000;
            map.Figure.YAxis.Scale.Max = 1000;
            
            map.Figure.IsAxisEqual = true;
            map.Figure.XAxis.Scale.MajorStep = 100;
            map.Figure.XAxis.Scale.MajorStepAuto=false;
            map.Figure.YAxis.Scale.MajorStep = 100;
            map.Refresh();

            map.MouseMove += Map_MouseMove;
            map.MouseDoubleClick += Map_MouseDoubleClick;

            DroneOverlay = new Overlay(map.Figure);
            map.Figure.Overlays.Add(DroneOverlay);
            

            this.Loaded += MainWindow_Loaded;

            /* 数据 */
            this.DataContext = sim;
            DtgTasks.ItemsSource = SwarmSimulation.Tasks;
            DtgDrones.ItemsSource = SwarmSimulation.Drones;
        }

        private void Map_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (map.Figure.Chart.Area.Contains(e.X, e.Y))
            {
                double x = map.Figure.XAxis.Scale.ReverseTransform(e.X);
                double y = map.Figure.YAxis.Scale.ReverseTransform(e.Y);

                if (isAllowTask)
                {
                    map.Figure.PlotTask(x, y, 1);
                    map.Refresh();

                    LFTask task = new LFTask();
                    task.X = x;
                    task.Y = y;
                    task.ID = SwarmSimulation.Tasks.Count + 1;
                    SwarmSimulation.Tasks.Add(task);

                    isAllowTask = false;
                }

                if (isAllowDrone)
                {
                    map.Figure.PlotDrone(x, y, 0,DroneOverlay);
                    map.Refresh();

                    LFDrone drone = new LFDrone();
                    drone.X = x;
                    drone.Y = y;
                    drone.Yaw = 0;
                    SwarmSimulation.Drones.Add(drone);

                    isAllowDrone = false;
                }

            }
        }

        private void Map_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (map.Figure.Chart.Area.Contains(e.X, e.Y))
            {
                LblX.Content = map.Figure.XAxis.Scale.ReverseTransform(e.X).ToString();
                LblY.Content = map.Figure.YAxis.Scale.ReverseTransform(e.Y).ToString();

            }
        }




        #endregion

        #region Methods

        public void RefreshMap()
        {
            foreach (Overlay o in map.Figure.Overlays)
            {
                o.Items.Clear();
            }

            /* 添加任务 */
            foreach(LFTask t in SwarmSimulation.Tasks)
            {
                map.Figure.PlotTask(t.X, t.Y, t.ID);
            }

            map.Refresh();
        }

        public void ConfirmAgents()
        {
            
        }

        #endregion

        #region Events

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshMap();
        }
        #endregion

        #region Defaults
        #endregion

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!sim.IsRun)
            {
                sim.Start();
                
                BtnStart.Content = "暂停";
                BtnStop.IsEnabled = true;
            }
            else
            {
                sim.Pause();

                BtnStart.Content = "开始";
            }
        }

        private void BtnNewTask_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            sim.Pause();
            if(MessageBox.Show("是否停止当前仿真？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                sim.Stop();
                BtnStop.IsEnabled = false;
                BtnStart.Content = "开始";
            }
            else
            {
                sim.Start();
            }
            
        }

        private void BtnTa_Click(object sender, RoutedEventArgs e)
        {
            TaskAllocationWindow window = new TaskAllocationWindow();
            window.ShowDialog();
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            isAllowTask = true;
        }

        private void BtnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            isAllowDrone = true;
        }
    }
}
