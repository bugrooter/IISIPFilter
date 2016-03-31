using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace IISIPFilter
{
    public sealed class IpFilterListCollection : ConfigurationElementCollection
    {
        private List<IPModel> settings;

        protected override ConfigurationElement CreateNewElement()
        {
            return new IpFilterListElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            IpFilterListElement ep = (IpFilterListElement)element;

            return ep.Max;
        }

        protected override string ElementName
        {
            get
            {
                return base.ElementName;
            }
        }

        public List<IPModel> List
        {
            get
            {
                if (settings == null)
                {
                    settings = new List<IPModel>();
                    foreach (IpFilterListElement e in this)
                    {
                        IPModel t = new IPModel();
                        t.Min = Comm.IpToInt(e.Min);
                        t.Max = Comm.IpToInt(e.Max);
                        settings.Add(t);
                    }
                }
                return settings;
            }
        }
    }

    public class IpFilterListElement : ConfigurationElement
    {
        [ConfigurationProperty("min", IsRequired = true)]
        public string Min
        {
            get { return (string)base["min"]; }
            set { base["min"] = value; }
        }
        [ConfigurationProperty("max", IsRequired = true)]
        public string Max
        {
            get { return (string)base["max"]; }
            set { base["max"] = value; }
        }
    }

    public class IpFilterFileElement : ConfigurationElement
    {
        /// <summary>  
        ///   
        /// </summary>  
        [ConfigurationProperty("files", IsRequired = true)]
        public string files
        {
            get { return (string)base["files"]; }
            set { base["files"] = value; }
        }
    }

    public class IPModel
    {
        public long Max { set; get; }
        public long Min { set; get; }
    }

    public class IpFilterBlackListElement : ConfigurationElement
    {
        /// <summary>  
        ///   
        /// </summary>  
        [ConfigurationProperty("ips", IsRequired = true)]
        public string Ips
        {
            get { return (string)base["ips"]; }
            set { base["ips"] = value; }
        }
    }

    /// <summary>  
    /// 对应config文件中的  
    /// </summary>  
    public sealed class IpFilterSettings : ConfigurationSection
    {
        /// <summary>  
        /// 对应IpFilterSettings节点下的IpFilterList子节点  
        /// </summary>  
        [ConfigurationProperty("IpFilterList", IsRequired = false)]
        public IpFilterListCollection IpFilterList
        {
            get { return (IpFilterListCollection)base["IpFilterList"]; }
        }

        /// <summary>  
        /// 对应IpFilterSettings节点下的IpFilterFile子节点，非必须  
        /// </summary>  
        [ConfigurationProperty("IpFilterFile", IsRequired = false)]
        public IpFilterFileElement IpFilterFile
        {
            get { return (IpFilterFileElement)base["IpFilterFile"]; }
            set { base["IpFilterFile"] = value; }
        }

        /// <summary>  
        /// 对应IpFilterSettings节点下的IpFilterBlackList子节点，非必须  
        /// </summary>  
        [ConfigurationProperty("IpFilterBlackList", IsRequired = false)]
        public IpFilterBlackListElement IpFilterBlackList
        {
            get { return (IpFilterBlackListElement)base["IpFilterBlackList"]; }
            set { base["IpFilterBlackList"] = value; }
        }
    }  
}