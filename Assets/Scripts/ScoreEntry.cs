using UnityEngine;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.Serialization;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


public sealed class VersionDeserializationBinder : SerializationBinder 
{ 
    public override Type BindToType( string assemblyName, string typeName )
    { 
        if ( !string.IsNullOrEmpty( assemblyName ) && !string.IsNullOrEmpty( typeName ) ) 
        { 
            Type typeToDeserialize = null; 
            assemblyName = Assembly.GetExecutingAssembly().FullName; 
            typeToDeserialize = Type.GetType( String.Format( "{0}, {1}", typeName, assemblyName ) ); 
 
            return typeToDeserialize; 
        } 
 
        return null; 
    } 
}


[Serializable()]
public class ScoreEntry : ISerializable
{
	public string name;
	public int score;
	
	#region Serialization functions
	
	public void SaveData(SerializationInfo info, StreamingContext ctxt) 
	{
		name = (string)info.GetValue("name", typeof(string));
		score = (int)info.GetValue("score", typeof(int));
	}
	
	public void GetObjectData (SerializationInfo info, StreamingContext ctxt) 
	{
		info.AddValue("name", name);
	    info.AddValue("score", score);
	}
	
	#endregion
}


