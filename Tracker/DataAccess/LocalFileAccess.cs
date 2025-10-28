namespace Tracker.DataAccess
{
    internal static class LocalFileAccess
    {
        //private static string mainDir = FileSystem.Current.AppDataDirectory;
        //private static string fileName = "nappistate.txt";
        //private static string filePath = System.IO.Path.Combine(mainDir, fileName);
        public static string GetLocalPath(string fileName)
        {
            return Path.Combine(FileSystem.Current.AppDataDirectory, fileName);
        }
        public static async Task<string> ReadFile(string filePath)
        {
            try
            {
                using Stream fileStream = File.OpenRead(filePath);
                using StreamReader reader = new StreamReader(fileStream);
                var c = await reader.ReadToEndAsync();
                return c;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Json reading error!",
                    $"{ex.Message}", "OK");                
                
            }
            return String.Empty;
        }
        public static async Task SaveState(string stringToWrite, string filePath)
        {
            try
            {
                using FileStream outputStream = System.IO.File.OpenWrite(filePath);
                using StreamWriter streamWriter = new StreamWriter(outputStream);
                //using var streamWriter = OpenStream(filePath);

                await streamWriter?.WriteAsync(stringToWrite);
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Error!", $"An exception occurred: {ex.Message}", "OK");
            }
            
        }
        public static async Task<string> ReadLocalFile(string filePath)
        {
            try
            {
                using Stream fileStream = System.IO.File.OpenRead(filePath);
                using StreamReader reader = new StreamReader(fileStream);

                var c = await reader.ReadToEndAsync();

                return c;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!",
                         $"An exception occurred: " +
                                   $"\nMessage: {ex.Message}", "OK");
            }

            return String.Empty;
        }
        private static StreamWriter? OpenStream(string path)
        {
            if (path is null)
            {
                Shell.Current.DisplayAlert("No file path",
                        $"You did not supply a file path.", "OK");

                return null;
            }

            try
            {
                var fileStream = new FileStream(path, FileMode.CreateNew);
                return new StreamWriter(fileStream);
            }
            catch (FileNotFoundException)
            {
                Shell.Current.DisplayAlert("Error!",
                        $"The file or directory cannot be found.", "OK");

            }
            catch (DirectoryNotFoundException)
            {
                Shell.Current.DisplayAlert("Error!",
                        $"The file or directory cannot be found.", "OK");

            }
            catch (DriveNotFoundException)
            {
                Shell.Current.DisplayAlert("Error!",
                        $"The drive specified in 'path' is invalid.", "OK");

            }
            catch (PathTooLongException)
            {
                Shell.Current.DisplayAlert("Error!",
                        $"'path' exceeds the maximum supported path length.", "OK");

            }
            catch (UnauthorizedAccessException)
            {
                Shell.Current.DisplayAlert("Error!",
                        $"You do not have permission to create this file.", "OK");

            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                Shell.Current.DisplayAlert("Error!",
                        $"There is a sharing violation.", "OK");

            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                Shell.Current.DisplayAlert("Error!",
                        $"The file already exists.", "OK");

            }
            catch (IOException e)
            {
                Shell.Current.DisplayAlert("Error!",
                        $"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}", "OK");

            }
            return null;
        }
    }
}
            //public static void DoSaveFile(string localPath, string contents)
            //{
            //    var dir = Path.GetDirectoryName(localPath);
            //    Directory.CreateDirectory(dir);

            //    File.WriteAllText(localPath, contents);
            //}

            //public static void DoDeleteFile(string localPath)
            //{
            //    if (File.Exists(localPath))
            //        File.Delete(localPath);
            //}
            //public static async Task<string> DoLoadFile(string localPath)
            //{
            //    if (File.Exists(localPath))
            //    {
            //        return File.ReadAllText(localPath);
            //    }
            //    return String.Empty;
            //}

      
    
