using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace bot
{
    [Serializable]
    public static class data//data for warns is stored here.
    {
        public static serverdata serverdata;
        [Serializable]
        public class save
        {
            public save()
            {
                server.set(data.serverdata);
            }
            public serverdata server;
            [Serializable]
            public static class system
            {
                public static string path = "./data.txt";
                public static void save()
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(path, FileMode.Create);
                    save data = new save();
                    formatter.Serialize(stream, data);
                    stream.Close();
                }
                public static void load()
                {
                    if (File.Exists(path))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        FileStream stream = new FileStream(path, FileMode.Open);
                        save ata = formatter.Deserialize(stream) as save;
                        stream.Close();
                        serverdata.set(ata.server);
                    }
                    else
                    {
                        Console.WriteLine("You fucked up");
                    }
                }
            }
        }
    }
    [Serializable]
    public struct serverdata
    {
        public warneduser[] warnedusers;
        public void set(serverdata input)
        {
            this = new serverdata
            {
                warnedusers = input.warnedusers
            };
        }
        public awarneduser[] awarnedusers;
        public void sett(serverdata input)
        {
            this = new serverdata
            {
                awarnedusers = input.awarnedusers
            };
        }

        [Serializable]
        public class warneduser
        {
            public ulong userId;
            public string[] reasons;
            public string[] caseId;
        }
        [Serializable]
        public class awarneduser
        {
            public ulong userId;
            public int count;
        }
    }
}