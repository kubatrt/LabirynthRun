using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


public class MCSerialization
{
	public static BinaryFormatter binaryFormatter = new BinaryFormatter();

	//--------------------------------------------------------------------------------------------------------------------
	public static string SerializeToString(object obj)
	{
		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, obj);
		return System.Convert.ToBase64String(memoryStream.ToArray());
	}

	public static object DeserializeFromString(string byteArray)
	{
		MemoryStream memoryStream = new MemoryStream( System.Convert.FromBase64String(byteArray));
		return binaryFormatter.Deserialize(memoryStream);
	}
	
	//--------------------------------------------------------------------------------------------------------------------
	public static byte[] SerializeToByteArray(object request)
	{
		byte[] result;
		BinaryFormatter serializer = new BinaryFormatter();
		using (MemoryStream memStream = new MemoryStream())
		{
			serializer.Serialize(memStream, request);
			result = memStream.GetBuffer();
		}
		return result;
	}
	
	public static object DeserializeFromByteArray(byte[] buffer)
	{
		BinaryFormatter deserializer = new BinaryFormatter();
		using (MemoryStream memStream = new MemoryStream(buffer))
		{
			object newobj = deserializer.Deserialize(memStream);
			return newobj;
		}
	}
	

}

