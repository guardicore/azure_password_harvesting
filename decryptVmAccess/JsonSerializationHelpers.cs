// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsAzure.GuestAgent.Plugins.JsonExtensions.JsonSerialization.JsonSerializationHelpers
// Assembly: JsonBasedExtension, Version=2.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C6B13A0F-3941-4396-8155-0FB70B0F08E5
// Assembly location: C:\w\azure\guest_installed\VMAccessAgent\Microsoft.Compute.VMAccessAgent_2.0.0.0\bin\JsonBasedExtension.dll

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace decryptVmAccess
{
  public class JsonSerializationHelpers
  {
    public static T DeserializeJsonStringFromFile<T>(string fileName)
    {
      try
      {
        return JsonSerializationHelpers.DeserializeJsonBytes<T>(File.ReadAllBytes(fileName));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static T DeserializeJsonString<T>(string jsonString, Encoding encoding)
    {
      try
      {
        return JsonSerializationHelpers.DeserializeJsonBytes<T>(encoding.GetBytes(jsonString));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private static T DeserializeJsonBytes<T>(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException("Cannot deserialize a null array of bytes");
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return (T) ((XmlObjectSerializer) new DataContractJsonSerializer(typeof (T))).ReadObject((Stream) memoryStream);
    }

    public static string SerializeObjectToJsonString<T>(T obj)
    {
      try
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          ((XmlObjectSerializer) new DataContractJsonSerializer(typeof (T))).WriteObject((Stream) memoryStream, (object) obj);
          memoryStream.Position = 0L;
          return new StreamReader((Stream) memoryStream).ReadToEnd();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static void SerializeObjectToJsonFile<T>(T obj, string fileName, Encoding encoding)
    {
      try
      {
        string jsonString = JsonSerializationHelpers.SerializeObjectToJsonString<T>(obj);
        File.WriteAllText(fileName, jsonString, encoding);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

  }
}
