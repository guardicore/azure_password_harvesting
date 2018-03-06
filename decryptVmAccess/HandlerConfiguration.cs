// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsAzure.GuestAgent.Plugins.JsonExtensions.Settings.VMAccess.TopLevelHandlerConfiguration
// Assembly: JsonVMAccessExtension, Version=2.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 6EE11626-5510-4589-B2AF-658D86A328A7
// Assembly location: C:\w\azure\guest_installed\VMAccessAgent\Microsoft.Compute.VMAccessAgent_2.0.0.0\bin\JsonVMAccessExtension.exe

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace decryptVmAccess
{
    public class ProtectedSettings
    {
        public string Password { /*Method get_Password with token 06000014*/get; /*Method set_Password with token 06000015*/set; }

        public override string ToString()
        {
            return string.Format("Password is {0}set", string.IsNullOrEmpty(this.Password) ? (object)"not " : (object)string.Empty);
        }
    }
}
