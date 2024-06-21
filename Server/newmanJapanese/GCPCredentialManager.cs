using System;
using System.IO;
namespace JLearning
{
    public class GCPCredentialManager
    {
        private static readonly string credentialPath = Path.Combine(Environment.CurrentDirectory, @"C:\Users\NITRO\Downloads\hale-silicon-427002-f5-4c06c4ac356a.json");

        /// <summary>GCPへアクセスするための認証情報設定をする。</summary>
        public static bool SetCredentials()
        {
            // GCPのNugetパッケージは環境変数"GOOGLE_APPLICATION_CREDENTIALS"から認証情報を参照しようとするので、ここでパスを指定する
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
            return true;
        }
    }
}
