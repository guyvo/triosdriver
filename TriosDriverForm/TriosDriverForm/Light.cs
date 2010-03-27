using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TriosDriverForm
{
    
    [XmlRoot("DATA",Namespace="")]
    public class TriosModel
    {
       
        [System.Xml.Serialization.XmlArray("Cortexes")]
        [System.Xml.Serialization.XmlArrayItem("Cortex", typeof(Cortex))]
        public ArrayList arrayCortex = new ArrayList();

        public TriosModel() {
        }

        public void addCortex(Cortex c) {
            arrayCortex.Add(c); 
        }

    }

    public class Cortex
    {
       
        private string _name="CORTEX";
        private ushort _tempsensor;
        private ushort _watchdog;
        private ushort _toggle;
        private ushort _dimmer;
        private ushort _hours;
        private ushort _masks;
       
        // This attribute enables the ArrayList to be serialized:
        [System.Xml.Serialization.XmlArray("Lights")]
        // Explicitly tell the serializer to expect the Item class
        // so it can be properly written to XML from the collection:
        [System.Xml.Serialization.XmlArrayItem("Light", typeof(Light))]
        
        public ArrayList arrayLights = new ArrayList();
        

        public Cortex()
        {
        }

        public Cortex(string name)
        {
            _name = name;
        }

        public void addLight(Light l) {
            arrayLights.Add(l);
        }

        [XmlAttribute( "CORTEX" )]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }

        }
        
        [XmlAttribute ( "SENSOR" )]
        public ushort tempsensor
        {
           get
            {
                return _tempsensor;
            }
            set
            {
                _tempsensor = value;
            }
        }

        [XmlAttribute( "WATCHDOG" )]
        public ushort watchdog
        {
            get
            {
                return _watchdog;
            }
            set
            {
                _watchdog = value;
            }
        }

        [XmlAttribute( "TOGGLE" )]
        public ushort toggle
        {
            get
            {
                return _toggle;
            }
            set
            {
                _toggle = value;
            }
        }

        [XmlAttribute( "DIMMER" )]
        public ushort dimmer
        {
            get
            {
                return _dimmer;
            }
            set
            {
                _dimmer = value;
            }
        }

        [XmlAttribute( "HOURS" )]
        public ushort hours
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = value;
            }
        }

        [XmlAttribute( "MASKS" )]
        public ushort masks
        {
            get
            {
                return _masks;
            }
            set
            {
                _masks = value;
            }
        }

    }

    public class Light
    {
        private string _name;
        private ushort _val=0;
        private ushort _min=0;
        private ushort _max=0;
        private ushort _step=0;
        private ushort _pinin=0;
        private ushort _pinout=0;
        public Light()
        {
        }

        public Light(string name , ushort[] vals)
        {
            _name = name;
            _val = vals[0];
            _min = vals[1];
            _max = vals[2];
            _step = vals[3];
            _pinin = vals[4];
            _pinout = vals[5];
        }

        [XmlAttribute("NAME")]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }

        }

        [XmlAttribute("VALUE")]
        public ushort val
        {
            get
            {
                return _val;
            }
            set
            {
                _val = value;
            }

        }
        [XmlAttribute("MIN")]
        public ushort min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
            }

        }
        [XmlAttribute("MAX")]
        public ushort max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }

        }
        [XmlAttribute("DELTA")]
        public ushort step
        {
            get
            {
                return _step;
            }
            set
            {
                _step = value;
            }

        }
        [XmlAttribute("PININ")]
        public ushort pinin
        {
            get
            {
                return _pinin;
            }
            set
            {
                _pinin = value;
            }

        }
        [XmlAttribute("PINOUT")]
        public ushort pinout
        {
            get
            {
                return _pinout;
            }
            set
            {
                _pinout = value;
            }

        }

  
    }
}
