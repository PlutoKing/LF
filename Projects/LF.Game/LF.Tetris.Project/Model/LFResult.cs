/*──────────────────────────────────────────────────────────────
 * FileName     : LFResult.cs
 * Created      : 2021-06-27 20:20:47
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.Tetris.Project.Model
{
    public class LFResult:LFNotify
    {
        #region Fields
        private static LFResult instance;
        private static readonly object syncRoot = new object();

        private int _score;

        public int Score
        {
            get { return _score; }
            set { _score = value; Notify(); }
        }

        private int _level;

        public int Level
        {
            get { return _level; }
            set { _level = value; Notify(); }
        }

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFResult()
        {
            Score = 0;
            Level = 1;
        }
        #endregion

        #region Methods
        public static LFResult GetInstance()
        {
            if (instance == null)
            {

                lock (syncRoot)
                {

                    if (instance == null)
                    {
                        instance = new LFResult();
                    }
                }
            }
            return instance;
        }

        public void CalculateScore(int Lines)
        {
            switch (Lines)
            {
                case 1:
                    Score += 5;
                    break;
                case 2:
                    Score += 15;
                    break;
                case 3:
                    Score += 30;
                    break;
                case 4:
                    Score += 50;
                    break;
                default:
                    Score += 0;
                    break;
            }

            if (Score < 20) Level = 1;
            else if (Score < 100) Level = 2;
            else if (Score < 300) Level = 3;
            else if (Score < 500) Level = 4;
            else if (Score < 1000) Level = 5;
            else if (Score < 3000) Level = 6;
            else if (Score < 5000) Level = 7;
            else Level = 8;

        }


        #endregion
    }
}