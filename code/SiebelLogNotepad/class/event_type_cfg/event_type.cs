using System;

namespace EventTypeCfg
{
    public abstract class EventType
    {
        /// <summary>
        /// Get output message
        /// </summary>
        public abstract string OutMessage();

        /// <summary>
        /// Check if event is the one
        /// </summary>
        public abstract bool IsEvent(string level0, string level1, string level2);

        /// <summary>
        /// Check if level needs to move up or down
        /// </summary>
        public abstract int MoveLevel(int level);

        public abstract string GetImg();
    }

    public class InOutEvent : EventType
    {
        // Temporary log levels
        private string _inTmpLevel0;
        private string _inTmpLevel1;
        private string _inTmpLevel2;

        private string _outTmpLevel0;
        private string _outTmpLevel1;
        private string _outTmpLevel2;

        // image
        private readonly string _img;

        private readonly string _level0;

        // IN
        private readonly string _inLevel1;
        private readonly string _inLevel2Search;
        private readonly string _inLevel2ReplaceCriteria;

        // OUT
        private readonly string _outLevel1;
        private readonly string _outLevel2Search;
        private readonly string _outLevel2ReplaceCriteria;

        // MOVE LEVEL
        private int _level;

        // BUILD OUTPUT
        private readonly string _buildCriteria;

        public InOutEvent(string level0, 
            string inLevel1, string inLevel2Search, string inLevel2ReplaceCriteria,
            string outLevel1, string outLevel2Search, string outLevel2ReplaceCriteria,
            string buildCriteria, string img)
        {
            _level0 = level0;
            _inLevel1 = inLevel1;
            _inLevel2Search = inLevel2Search;
            _inLevel2ReplaceCriteria = inLevel2ReplaceCriteria;
            _outLevel1 = outLevel1;
            _outLevel2Search = outLevel2Search;
            _outLevel2ReplaceCriteria = outLevel2ReplaceCriteria;

            _buildCriteria = buildCriteria;
            _img = img;

        }

        public override string OutMessage()
        {
            switch (_level)
            {
                case 1:
                    return _buildCriteria.Replace("{IN}", Level2Process.ReplaceCriteria(_inTmpLevel2, _inLevel2ReplaceCriteria));
                case -1:
                    return Level2Process.ReplaceCriteria(_outTmpLevel2, _outLevel2ReplaceCriteria);
            }

            return string.Empty;
        }

        public override bool IsEvent(string level0, string level1, string level2)
        {
            // Begin
            if (String.Equals(level0, _level0, StringComparison.CurrentCultureIgnoreCase) &&
                String.Equals(level1, _inLevel1, StringComparison.CurrentCultureIgnoreCase) &&
                Level2Process.Search(level2, _inLevel2Search)) // this criterias are to be ignored so this needs to be false
            {
                _inTmpLevel0 = level0;
                _inTmpLevel1 = level1;
                _inTmpLevel2 = level2;

                _level = 1;

                return true;
            }

            // End
            if (String.Equals(level0, _level0, StringComparison.CurrentCultureIgnoreCase) &&
                String.Equals(level1, _outLevel1, StringComparison.CurrentCultureIgnoreCase) &&
                Level2Process.Search(level2, _outLevel2Search))
            {
                _outTmpLevel0 = level0;
                _outTmpLevel1 = level1;
                _outTmpLevel2 = level2;

                _level = -1;

                return true;
            }

            _level = 0;

            return false;
        }

        public override int MoveLevel(int level)
        {
            return level + _level;
        }

        public override string GetImg()
        {
            return _img;
        }
    }

    public class NeutralEvent : EventType
    {
        // Temporary log levels
        private string _tmpLevel0;
        private string _tmpLevel1;
        private string _tmpLevel2;

        // image
        private readonly string _img;

        private readonly string _level0;
        private readonly string _level1;
        private readonly string _level2Search;
        private readonly string _level2ReplaceCriteria;
        private readonly string _level2IgnoreCriteria;

        // BUILD OUTPUT
        private readonly string _buildCriteria;

        public NeutralEvent(string level0, 
            string level1, string level2Search, string level2ReplaceCriteria, string level2IgnoreCriteria, 
            string buildCriteria, string img)
        {
            _level0 = level0;
            _level1 = level1;
            _level2Search = level2Search;
            _level2ReplaceCriteria = level2ReplaceCriteria;
            _level2IgnoreCriteria = level2IgnoreCriteria;
            _buildCriteria = buildCriteria;
            _img = img;
        }

        public override string OutMessage()
        {
            string msg = Level2Process.ReplaceCriteria(_tmpLevel2, _level2ReplaceCriteria);

            return _buildCriteria.Replace("{MSG}", msg);
        }

        public override bool IsEvent(string level0, string level1, string level2)
        {
            _tmpLevel0 = level0;
            _tmpLevel1 = level1;
            _tmpLevel2 = level2;

            return LevelCompare(_tmpLevel0, _level0) &&
                   LevelCompare(_tmpLevel1, _level1) &&
                   Level2Process.Search(_tmpLevel2, _level2Search) &&
                   (!Level2Process.Search(_tmpLevel2, _level2IgnoreCriteria) || _level2IgnoreCriteria == string.Empty); // this criterias are to be ignored so this needs to be false
        }

        public override int MoveLevel(int level)
        {
            return level;
        }

        public override string GetImg()
        {
            return _img;
        }

        /*************** Private ***************/
        /// <summary>
        /// Compare level, but if cfg level is empty if will return true, it means that user wants that event can be any type
        /// </summary>
        private static bool LevelCompare(string tmpLevel, string level)
        {
            return level == string.Empty || String.Equals(tmpLevel, level, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
