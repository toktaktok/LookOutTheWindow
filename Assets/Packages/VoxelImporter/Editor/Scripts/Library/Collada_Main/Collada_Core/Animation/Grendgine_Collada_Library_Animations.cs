using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
namespace VoxelImporter.grendgine_collada
{
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
	public partial class Grendgine_Collada_Library_Animations
	{
		[XmlAttribute("id")]
		public string ID;
		
		[XmlAttribute("name")]
		public string Name;

        [XmlElement(ElementName = "asset")]
        public Grendgine_Collada_Asset Asset;

        [XmlElement(ElementName = "animation")]
		public Grendgine_Collada_Animation[] Animation;	
		
	    [XmlElement(ElementName = "extra")]
		public Grendgine_Collada_Extra[] Extra;	

	}
}

//check done
