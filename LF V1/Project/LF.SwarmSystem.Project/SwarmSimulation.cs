/*──────────────────────────────────────────────────────────────
 * FileName     : SwarmSimulation
 * Created      : 2020-10-29 10:42:02
 * Author       : Xu Zhe
 * Description  : 群仿真系统
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using LF.TaskAllocation;
using System.Collections.ObjectModel;
using LF.FlightControl;


namespace LF.SwarmSystem.Project
{
    /// <summary>
    /// 集群仿真
    /// </summary>
    public class SwarmSimulation : INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        private bool _isRun = false;                    // 是否在运行
        private Timer _timer = new Timer();             // 计时器
        private DateTime _startTime = new DateTime();   // 开始时间
        private TimeSpan _duration = new TimeSpan();    // 运行时长
        private TimeSpan _pauseTime = new TimeSpan();
        private string _log = "";    // 日志

        private bool isGotoTask = false;
        private bool isGotoFlight = false;

        private bool _isDone = false;   // 任务是否完成

        public static ObservableCollection<LFDrone> Drones = new ObservableCollection<LFDrone>();

        public static ObservableCollection<LFTask> Tasks = new ObservableCollection<LFTask>();

        #endregion

        #region Properties
        /// <summary>
        /// 是否运行
        /// </summary>
        public bool IsRun { get => _isRun; set => _isRun = value; }

        /// <summary>
        /// 计时器
        /// </summary>
        public Timer Timer { get => _timer; set => _timer = value; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("StartTime"));
                }
            }
        }

        /// <summary>
        /// 仿真时长
        /// </summary>
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Duration"));
                }
            }
        }

        /// <summary>
        /// 仿真步长
        /// </summary>
        public int Step { get => _timer.Interval; set => _timer.Interval = value; }

        /// <summary>
        /// 暂停前时长
        /// </summary>
        public TimeSpan PauseTime { get => _pauseTime; set => _pauseTime = value; }

        #endregion

        #region Constructors
        public SwarmSimulation()
        {
            _timer.Interval = 10;
            _timer.Tick += Timer_Tick;
        }


        #endregion

        #region Methods

        #region Time Methods
        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            StartTime = DateTime.Now;
            _timer.Start();
            SimInitialization();
            IsRun = true;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _timer.Stop();
            IsRun = false;
            PauseTime = Duration;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            IsRun = false;
            PauseTime = new TimeSpan();
            Duration = new TimeSpan();
        }
        #endregion

        /// <summary>
        /// 更新地图
        /// </summary>
        public void RefreshMap()
        {
            MainWindow.DroneOverlay.Items.Clear();
            foreach(LFDrone d in Drones)
            {
                MainWindow.MainMap.Figure.PlotDrone(d.X, d.Y, d.Yaw,MainWindow.DroneOverlay);
            }
            MainWindow.MainMap.Refresh();
        }

        #region Simulation Function


        /// <summary>
        /// 仿真初始化
        /// </summary>
        public void SimInitialization()
        {
            _log += "开始初始化. . .\n";
            foreach(LFDrone drone in Drones)
            {
                drone.Agent.Tasks.Clear();
            }
            isGotoTask = true;
        }

        /// <summary>
        /// 任务分配
        /// </summary>
        public void SimTaskAllocation()
        {
            LFTaa taa = new LFTaa();    // Task Allocation Algorithm
            /* 添加任务执行智体 */
            foreach(LFDrone drone in Drones)
            {
                taa.Agents.Add(drone.Agent);
            }
            /* 添加任务 */
            foreach(LFTask t in Tasks)
            {
                taa.Tasks.Add(t);
            }
            taa.Calculate();

            isGotoFlight = true;
        }

        /// <summary>
        /// 气动飞行控制仿真
        /// </summary>
        public void SimFlightControl()
        {
            foreach(LFDrone a in Drones)
            {
                a.Map.Start = new Mathematics.LFPoint2D(a.X, a.Y);
                a.Map.End = new Mathematics.LFPoint2D(a.Agent.Tasks[0].X, a.Agent.Tasks[0].Y);
                a.Route =  a.Map.GetRoute();

                MainWindow.MainMap.Figure.PlotRoute(a.Route);
                MainWindow.MainMap.Refresh();

                a.Run();
            }
        }

        /// <summary>
        /// 检测任务是否完成
        /// </summary>
        public void Check()
        {
            foreach (LFDrone d in Drones)
            {
                foreach (LFTask t in d.Agent.Tasks)
                {
                    if (!t.IsDone)
                    {
                        return;
                    }
                }
            }
            _isDone = true;
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// 时间函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            /* 计时功能 */
            Duration = DateTime.Now - _startTime;
            Duration += _pauseTime;

            /* 仿真运行 */
            if (!_isDone)
            {
                if (isGotoTask)
                {
                    SimTaskAllocation();
                    isGotoTask = false;
                }
                if (isGotoFlight)
                {
                    SimFlightControl();
                    isGotoFlight = false;
                }
                
                Check();
                RefreshMap();
            }
            else
            {
                Pause();
            }

        }

        /// <summary>
        /// 属性改变事件
        /// </summary>
        /// <param name="info"></param>
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
