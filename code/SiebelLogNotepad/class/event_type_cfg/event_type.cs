using System;

namespace EventTypeCfg
{
    public abstract class EventType
    {
        public string EventName { get; set; }

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

            // set event name
            EventName = "InOutEvent";
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

        // type of operation selector
        /*
         * 0 is empty
         * 1 = equal
         * 2 = contains
         * 3 = starts with
         * 4 = ends with
         * 
         */
        private readonly int _level0OpType;
        private readonly int _level1OpType;

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
            _level0 = level0.Contains("%") ? level0.Replace("%", " ").Trim() : level0;
            _level1 = level1.Contains("%") ? level1.Replace("%", " ").Trim() : level1;
            
            _level2Search = level2Search;
            _level2ReplaceCriteria = level2ReplaceCriteria;
            _level2IgnoreCriteria = level2IgnoreCriteria;
            _buildCriteria = buildCriteria;
            _img = img;

            // type of operation selector
            _level0OpType = OperationSelector(level0);
            _level1OpType = OperationSelector(level1);

            // set event name
            EventName = "NeutralEvent";
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

            return LevelCompare(_tmpLevel0, _level0, _level0OpType) &&
                   LevelCompare(_tmpLevel1, _level1, _level1OpType) &&
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
        private int OperationSelector(string level)
        {
            if (level == string.Empty)
                return 0;

            if (level[0].Equals('%') && level[level.Length - 1].Equals('%'))
                return 2;

            if (level[0].Equals('%'))
                return 3;

            if (level[level.Length - 1].Equals('%'))
                return 4;

            return 1;
        }

        private static bool LevelCompare(string tmpLevel, string level, int op)
        {
            switch (op)
            {
                case 0:
                    return true;
                case 1:
                    return LevelEqualCompare(tmpLevel, level);
                case 2:
                    return LevelContainsCompare(tmpLevel, level);
                case 3:
                    return LevelStartCompare(tmpLevel, level);
                case 4:
                    return LevelEndCompare(tmpLevel, level);
            }

            return false;
        }

        /// <summary>
        /// Compare level, but if cfg level is empty if will return true, it means that user wants that event can be any type
        /// </summary>
        /// <param name="tmpLevel">value in log</param>
        /// <param name="level">value in configuration</param>
        private static bool LevelEqualCompare(string tmpLevel, string level)
        {
            return level == string.Empty || String.Equals(tmpLevel, level, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Stategy equal to top but not equal its startswith
        /// </summary>
        private static bool LevelStartCompare(string tmpLevel, string level)
        {
            return level == string.Empty || tmpLevel.StartsWith(level, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Stategy equal to top but not equal its endswith
        /// </summary>
        private static bool LevelEndCompare(string tmpLevel, string level)
        {
            return level == string.Empty || tmpLevel.EndsWith(level, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Stategy equal to top but not equal its contains
        /// </summary>
        private static bool LevelContainsCompare(string tmpLevel, string level)
        {
            return level == string.Empty || tmpLevel.Contains(level);
        }
    }

    public class OneLineEvent : EventType
    {
        // Temporary log levels
        private string _tmpLine;

        // image
        private readonly string _img;

        private readonly string _replaceCriteria;

        private readonly string _ignoreCriteria;

        private readonly string _searchCriteria;

        // BUILD OUTPUT
        private readonly string _buildCriteria;

        public OneLineEvent(string searchCriteria, string replaceCriteria, string ignoreCriteria, string buildCriteria, string img)
        {
            _img = img;
            _ignoreCriteria = ignoreCriteria;
            _replaceCriteria = replaceCriteria;
            _buildCriteria = buildCriteria;
            _searchCriteria = searchCriteria;

            // set event name
            EventName = "OneLineEvent";
        }

        /*************** Public ***************/
        public override string OutMessage()
        {
            string msg = Level2Process.ReplaceCriteria(_tmpLine, _replaceCriteria);

            return _buildCriteria.Replace("{MSG}", msg);
        }

        /// <summary>
        /// This is a line event
        /// </summary>
        /// <param name="line">Log line</param>
        /// <param name="none">WTF</param>
        /// <param name="none2">WTF</param>
        /// <returns></returns>
        public override bool IsEvent(string line, string none, string none2)
        {
            _tmpLine = line;



            bool data = Level2Process.Search(line, _searchCriteria);
            bool data2 = (Level2Process.Search(line, _ignoreCriteria) || _ignoreCriteria == string.Empty);

            return Level2Process.Search(line, _searchCriteria) && 
                (!Level2Process.Search(line, _ignoreCriteria) || _ignoreCriteria == string.Empty);
        }

        /// <summary>
        /// Its just cosmetic there is no level to move in here
        /// </summary>
        public override int MoveLevel(int level)
        {
            return level;
        }

        public override string GetImg()
        {
            return _img;
        }
    }
}
