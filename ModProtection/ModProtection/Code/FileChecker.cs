using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using StardewModdingAPI;

namespace ModProtection.Code
{
    public static class FileChecker
    {
        // Map of embedded resource → relative mod file path
        private static readonly (string resourceName, string relativePath)[] FilesToCheck =
        {
            ("ModProtection.Embedded.content.json", Path.Combine("Werewolf", "Werewolf", "content.json")),
            ("ModProtection.Embedded.assets.Custom_Buildings.Wolf_cave.json", Path.Combine("Werewolf", "Werewolf", "assets", "Custom_Buildings", "Wolf_cave.json")),
            ("ModProtection.Embedded.assets.Animails.Mounts.Wolf.json", Path.Combine("Werewolf", "Werewolf", "assets", "Animails", "Mounts", "Wolf.json"))
        };

        // Method to check and restore files
        public static void CheckAndRestoreFiles(IMonitor monitor, string modsPath)
        {
            foreach (var (resourceName, relativePath) in FilesToCheck)
            {
                // Correct the target path by combining the mods path with the relative file path
                string targetPath = Path.Combine(modsPath, relativePath);

                // Debugging log to see where the files are being checked
                monitor.Log($"Target Path: {targetPath}", LogLevel.Trace);

                // Remove any unnecessary nested "Werewolf" from the target path (fixing issue with double "Werewolf" in the path)
                targetPath = targetPath.Replace(Path.Combine("Werewolf", "Werewolf"), "Werewolf");

                // Log the corrected path
                monitor.Log($"Corrected Target Path: {targetPath}", LogLevel.Trace);

                // Calculate the hash of the embedded resource
                string? originalHash = GetEmbeddedFileHash(resourceName);
                // Calculate the hash of the current file
                string? currentHash = File.Exists(targetPath) ? GetFileHash(targetPath) : null;

                // If hashes don't match, restore the file
                if (originalHash != currentHash)
                {
                    monitor.Log($"File modified or missing: {targetPath}. Restoring original version...", LogLevel.Warn);
                    RestoreFile(resourceName, targetPath);
                }
                else
                {
                    monitor.Log($"Verified: {targetPath}", LogLevel.Trace);
                }
            }
        }

        // Method to get the MD5 hash of an embedded resource
        private static string? GetEmbeddedFileHash(string resourceName)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (stream == null) return null;

            using var md5 = MD5.Create();
            return Convert.ToBase64String(md5.ComputeHash(stream));
        }

        // Method to get the MD5 hash of a file
        private static string? GetFileHash(string path)
        {
            using var stream = File.OpenRead(path);
            using var md5 = MD5.Create();
            return Convert.ToBase64String(md5.ComputeHash(stream));
        }

        // Method to restore a file from the embedded resource
        private static void RestoreFile(string resourceName, string targetPath)
        {
            // Create the directory if it doesn't exist
            string? dir = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            // Get the embedded resource stream
            using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (resourceStream == null)
                return;

            // Create the target file and copy the resource stream to it
            using var fileStream = File.Create(targetPath);
            resourceStream.CopyTo(fileStream);
        }
    }
}
