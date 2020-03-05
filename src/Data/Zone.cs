using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RogueSurvivor.Data
{
    [Serializable]
    class Zone
    {
        string m_Name = "unnamed zone";
        Rectangle m_Bounds;
        Dictionary<string, object> m_Attributes = null;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public Rectangle Bounds
        {
            get { return m_Bounds; }
            set { m_Bounds = value; }
        }

        public Zone(string name, Rectangle bounds)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            m_Name = name;
            m_Bounds = bounds;
        }

        public bool HasGameAttribute(string key)
        {
            if (m_Attributes == null)
                return false;
            return m_Attributes.Keys.Contains(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">must be serializable</param>
        public void SetGameAttribute<T>(string key, T value)
        {
            if (m_Attributes == null)
                m_Attributes = new Dictionary<string, object>(1);

            if (m_Attributes.Keys.Contains(key))
                m_Attributes[key] = value;
            else
                m_Attributes.Add(key, value);
        }

        public T GetGameAttribute<T>(string key)
        {
            if (m_Attributes == null)
                return default(T);

            object value;
            if (m_Attributes.TryGetValue(key, out value))
            {
                if (!(value is T))
                    throw new InvalidOperationException("game attribute is not of requested type");
                return (T)value;
            }
            else
                return default(T);
        }
    }
}
