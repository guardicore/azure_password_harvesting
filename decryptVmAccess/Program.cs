using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

namespace decryptVmAccess
{
    class Program
    {
        static string basePath = "C:\\Packages\\Plugins\\Microsoft.Compute.VmAccessAgent\\2.4.2\\RuntimeSettings";
        static void Main(string[] args)
        {
            printWelcomeBanner();
            if (!IsAdministrator())
            {
                Console.WriteLine("You must run this with administrator privileges");
                return;
            }
            ProcessDirectory(basePath);

        }

        private static void printWelcomeBanner()
        {
            Console.WriteLine("****************************************************");
            Console.WriteLine("GuardiCore Azure Password recovery diagnostic tool");
            Console.WriteLine("Written By Guardicore Labs");
            Console.WriteLine("Contact us at: support@guardicore.com");
            Console.WriteLine("****************************************************");
        }

        private static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string [] fileEntries = Directory.GetFiles(targetDirectory);
            foreach(string fileName in fileEntries) {
                ProcessFile(fileName);
            }

        }

        private static void ProcessFile(string fileName)
        {
            string thumbprint = "";
            string data = "";
            string username = "";
            Console.WriteLine("Parsing file {0}", fileName);
            using (StreamReader r = new StreamReader(fileName))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(r));
                JArray settings = (JArray) o["runtimeSettings"];
                JToken handlerSettings = settings.First["handlerSettings"];
                //Console.WriteLine(handlerSettings);
                thumbprint = (string)handlerSettings["protectedSettingsCertThumbprint"];
                //Console.WriteLine(thumbprint);
                data = (string)handlerSettings["protectedSettings"];
                //Console.WriteLine(data);
                username = (string)handlerSettings["publicSettings"]["UserName"];
            }
            Console.WriteLine("User is {0}", username);
            ProtectedSettings protectedSettings = GetExtensionProtectedSettings(thumbprint, data);
            Console.WriteLine("Password is {0}",protectedSettings.Password);
        }

        private static ProtectedSettings GetExtensionProtectedSettings(string thumbprint, string b64Data)
        {
            byte[] encryptedBytes = Convert.FromBase64String(b64Data);
            //Console.WriteLine("got encrypted bytes");
            X509Certificate2 certificateByThumbprint = EncryptionHelpers.GetCertificateByThumbprint(StoreName.My, StoreLocation.LocalMachine, thumbprint);
            //Console.WriteLine("got thumbprint");
            if (certificateByThumbprint == null)
            {
                return (ProtectedSettings)null;
            }
            ProtectedSettings protectedSettings = JsonSerializationHelpers.DeserializeJsonString<ProtectedSettings>(EncryptionHelpers.DecryptWithCertificate(encryptedBytes, certificateByThumbprint), Encoding.Unicode);
            //Console.WriteLine("got decrypted");
            return protectedSettings;
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
