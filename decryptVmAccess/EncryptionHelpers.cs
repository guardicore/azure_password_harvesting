// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsAzure.GuestAgent.Plugins.EncryptionHelpers
// Assembly: JsonBasedExtension, Version=2.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C6B13A0F-3941-4396-8155-0FB70B0F08E5
// Assembly location: C:\w\azure\guest_installed\VMAccessAgent\Microsoft.Compute.VMAccessAgent_2.0.0.0\bin\JsonBasedExtension.dll

using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace decryptVmAccess
{
  public class EncryptionHelpers
  {
    public static X509Certificate2 GetCertificateByThumbprint(StoreName storeName, StoreLocation storeLocation, string thumprint)
    {
      X509Store x509Store = new X509Store(storeName, storeLocation);
      x509Store.Open(OpenFlags.ReadOnly);
      X509Certificate2Collection certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, (object) thumprint, false);
      x509Store.Close();
      X509Certificate2 x509Certificate2 = (X509Certificate2) null;
      if (certificate2Collection != null && certificate2Collection.Count > 0)
        x509Certificate2 = certificate2Collection[0];
      using (x509Certificate2.GetRSAPrivateKey()) { } // pure black magic
      return x509Certificate2;
    }

    public static string DecryptWithCertificate(byte[] encryptedBytes, X509Certificate2 cert)
    {
      EnvelopedCms envelopedCms = new EnvelopedCms();
      envelopedCms.Decode(encryptedBytes);
      X509Certificate2Collection extraStore = new X509Certificate2Collection(cert);
      envelopedCms.Decrypt(extraStore);
      return Encoding.UTF8.GetString(envelopedCms.ContentInfo.Content);
    }
  }
}
