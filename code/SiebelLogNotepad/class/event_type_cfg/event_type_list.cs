using System;
using System.Collections.Generic;
using System.Xml;

namespace EventTypeCfg
{
class EventTypeList
    {
        public List<EventType> ListEvents { get; private set; }

        public EventTypeList()
        {
            ListEvents = new List<EventType>();
        }

        public void GetEvents(string fileName)
        {
            try
            {
                GetInOutEvents(fileName);

                GetNeutralEvents(fileName);

                GetOneLineEvents(fileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetInOutEvents(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNodeList xmllList = doc.SelectNodes("/events/in_out_events/in_out_event");

                if (xmllList == null) return;

                foreach (XmlNode node in xmllList)
                {
                    try
                    {
                        if (node["level_0"] == null || node["in"] == null || node["out"] == null || node["build"] == null) continue;

                        // image
                        string img = node["image"].InnerText;

                        // Level0
                        string level0 = node["level_0"].InnerText;

                        // IN
                        XmlNodeList inEvent = node["in"].ChildNodes;
                        string inLevel1 = inEvent[0].InnerText;

                        XmlNodeList inLevel2 = inEvent[1].ChildNodes;
                        string inLevel2Search = inLevel2[0].InnerText;
                        string inLevel2ReplaceCriteria = inLevel2[1].InnerText;
                        
                        // OUT
                        XmlNodeList outEvent = node["out"].ChildNodes;
                        string outLevel1 = outEvent[0].InnerText;

                        XmlNodeList outLevel2 = outEvent[1].ChildNodes;
                        string outLevel2Search = outLevel2[0].InnerText;
                        string outLevel2ReplaceCriteria = outLevel2[1].InnerText;

                        // BUILD OUTPUT
                        string build = node["build"].InnerText;

                        // Get event
                        EventType evtType = new InOutEvent(
                                level0,
                                inLevel1, inLevel2Search, inLevel2ReplaceCriteria,
                                outLevel1, outLevel2Search, outLevel2ReplaceCriteria,
                                build, img
                            );

                        ListEvents.Add(evtType);
                    }
                    catch 
                    {                  
                        // implementar mecanismo de log
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Reading In Out Events Xml File:" + ex.Message);
            }
        }

        private void GetNeutralEvents(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNodeList xmllList = doc.SelectNodes("/events/neutral_events/neutral_event");

                if (xmllList == null) return;

                foreach (XmlNode node in xmllList)
                {
                    try
                    {
                        if (node["level_0"] == null || node["level_1"] == null || node["level_2"] == null || node["build"] == null) continue;

                        // image
                        string img = node["image"].InnerText;

                        // Level0
                        string level0 = node["level_0"].InnerText;

                        // Level1
                        string level1 = node["level_1"].InnerText;

                        // Level2
                        XmlNodeList level2 = node["level_2"].ChildNodes;
                        string level2Search = level2[0].InnerText;
                        string level2ReplaceCriteria = level2[1].InnerText;
                        string level2IgnoreCriteria = level2[2].InnerText;

                        // BUILD OUTPUT
                        string build = node["build"].InnerText;

                        // Get event
                        EventType evtType = new NeutralEvent(level0, level1, level2Search, level2ReplaceCriteria, level2IgnoreCriteria, build, img);

                        ListEvents.Add(evtType);
                    }
                    catch (Exception ex)
                    {
                        // implementar mecanismo de log
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Reading Neutral Events Xml File:" + ex.Message);
            }
        }

        private void GetOneLineEvents(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNodeList xmllList = doc.SelectNodes("/events/one_line_events/onle_line_event");

                if (xmllList == null) return;

                foreach (XmlNode node in xmllList)
                {
                    try
                    {
                        if (node["search"] == null || node["replace_criteria"] == null || node["ignore_criteria"] == null || node["build"] == null) continue;

                        // image
                        string img = node["image"].InnerText;

                        // search criteria
                        string search = node["search"].InnerText;

                        // replace criteria
                        string replace = node["replace_criteria"].InnerText;

                        // ignore criteria
                        string ignore = node["ignore_criteria"].InnerText;

                        // BUILD OUTPUT
                        string build = node["build"].InnerText;

                        // Get event
                        EventType evtType = new OneLineEvent(search, replace, ignore, build, img);

                        ListEvents.Add(evtType);
                    }
                    catch (Exception ex)
                    {
                        // implementar mecanismo de log
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Reading Neutral Events Xml File:" + ex.Message);
            }
        }
    }
}
