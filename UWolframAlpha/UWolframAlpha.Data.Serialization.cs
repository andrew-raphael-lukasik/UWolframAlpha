using System.Xml.Serialization;
using UnityEngine;

// https://products.wolframalpha.com/api/documentation/#informational-elements
namespace UWolframAlpha.Data
{
	
	[XmlRoot("queryresult")]
	[System.Serializable] public partial struct QueryResult
	{
		[XmlAttribute("success")] public bool success;
		[XmlAttribute("error")] public bool error;
		// [XmlAttribute("numpods")] public int numpods;
		[XmlAttribute("timedout")] public string timedout;
		[XmlAttribute("timing")] public float timing;
		[XmlElement("pod")] public Pod[] pod_array;
		[XmlElement("assumptions")] public Assumptions assumptions;
		
		public override string ToString () => JsonUtility.ToJson( this );
	}

	[System.Serializable] public partial struct Pod
	{
		[XmlAttribute("title")] public string title;
		// [XmlAttribute("scanner")] public string scanner;
		// [XmlAttribute("id")] public string id;
		[XmlAttribute("position")] public int position;
		// [XmlAttribute("error")] public bool error;
		// [XmlAttribute("numsubpods")] public int numsubpods;
		// [XmlAttribute("primary")] public bool primary;
		[XmlElement("subpod")] public SubPod[] subpod_array;
		[XmlElement("expressiontypes")] public ExpressionTypes expressiontypes;
		[XmlElement("states")] public States states;
		[XmlElement("infos")] public Infos infos;

		public override string ToString () => JsonUtility.ToJson( this );
	}
	
	
	[System.Serializable] public partial struct SubPod
	{
		[XmlAttribute("title")] public string title;
		[XmlElement("img")] public Img img;
		[XmlElement("plaintext")] public string plaintext;
		
		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct ExpressionTypes
	{
		[XmlAttribute("count")] public int count;
		[XmlElement("expressiontype")] public ExpressionType[] expressiontype_array;
		
		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct ExpressionType
	{
		[XmlAttribute("name")] public string name;

		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct States
	{
		[XmlAttribute("count")] public int count;
		[XmlElement("state")] public State[] state_array;

		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct State
	{
		[XmlAttribute("name")] public string name;
		[XmlAttribute("input")] public string input;

		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct Infos
	{
		[XmlAttribute("count")] public int count;
		[XmlElement("info")] public Info[] info_array;
		
		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct Info
	{
		[XmlAttribute("text")] public string text;
		[XmlElement("img")] public Img img;
		[XmlElement("link")] public Link[] link_array;

		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct Img
	{
		[XmlAttribute("src")] public string src;
		[XmlAttribute("alt")] public string alt;
		[XmlAttribute("title")] public string title;
		[XmlAttribute("width")] public int width;
		[XmlAttribute("height")] public int height;

		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct Link
	{
		[XmlAttribute("url")] public string url;
		[XmlAttribute("text")] public string text;
		[XmlAttribute("title")] public string title;

		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct Assumptions
	{
		[XmlAttribute("count")] public int count;
		[XmlElement("assumption")] public Assumption[] assumption_array;

		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct Assumption
	{
		[XmlAttribute("type")] public string type;
		[XmlAttribute("word")] public string word;
		[XmlAttribute("template")] public string template;
		[XmlAttribute("count")] public int count;
		[XmlElement("value")] public Value[] value_array;

		public override string ToString () => JsonUtility.ToJson( this );
	}

	
	[System.Serializable] public partial struct Value
	{
		[XmlAttribute("name")] public string name;
		[XmlAttribute("desc")] public string desc;
		[XmlAttribute("input")] public string input;

		public override string ToString () => JsonUtility.ToJson( this );
	}

}
