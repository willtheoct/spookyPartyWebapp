using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Threading;

namespace spookyWebServer
{
    public static class Program
    {
        static HttpListener http = new HttpListener();
        static List<duel> activeDuelChallenges = new List<duel>();
        static Dictionary<int, List<notification>> _notifications = new Dictionary<int, List<notification>>();
        static PartyGoer[] _players = new PartyGoer[]{};

        static Dictionary<string, byte[]> serverPages = new Dictionary<string, byte[]>();
        static int Main(string[] args)
        {
            var testNote = new notification();
            testNote.text = "Crystal ball is working";
            _notifications.Add(0, new List<notification> { testNote });
            /*
            var playermock = new PartyGoer();
            playermock.id = 2;
            playermock.level = 4;
            playermock.characterName = "will";
            playermock.inventory = new int[] { 10, 42, 3, 4, 5, 2, 0, 0, 0, 0 };
            _players[0]= playermock;
            */

            foreach (var file in Directory.EnumerateFiles("./www/"))
            {
                var fileFromRoot = file.Remove(0, "./www/".Length);
                var stream = File.OpenRead(file);
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                serverPages[fileFromRoot] = bytes;
            }

            #region setup
            var playerDb = File.Open("players.json", FileMode.OpenOrCreate);
            //json.write(playerDb, _players);
            _players = (json.read<PartyGoer[]>(playerDb) ?? new PartyGoer[] { });
            playerDb.Close();

            var duelDb = File.Open("ActiveDuels.json", FileMode.OpenOrCreate);
            activeDuelChallenges = (json.read<duel[]>(duelDb) ?? new duel[] { }).ToList();
            duelDb.Close();

            Console.WriteLine("Loaded " + _players.Length + " players, " + _notifications.Count + " notification lists, and " + activeDuelChallenges.Count + " duels from files.");
            #endregion

            http.Prefixes.Add("http://localhost:8220/");
            http.Start();

            while (true)
            {
                try
                {
                    var context = http.GetContext();
                    var request = context.Request;
                    context.Response.Headers.Add("Access-Control-Allow-Origin: *");
                    context.Response.Headers.Add("Cache-Control: max-age=0");
                    var segments = request.Url.Segments.ToList();

                    Console.Write(request.RawUrl);

                    if (request.HttpMethod == "OPTIONS")
                    {
                        options(context);
                    }
                    else if (segments.Count == 1)
                    {
                        segments.Add("index.html");
                    }
                    switch (segments[1])
                    {
                        case "duelChallenge":
                            duelChallenge(context); break;
                        case "duels":
                            duels(context); break;
                        case "notifications":
                            notifications(context);
                            break;
                        case "players":
                            players(context);
                            break;
                        case "unlocks":
                            unlock(context);
                            break;
                        default:
                            if (request.HttpMethod == "GET")
                            {
                                byte[] page;
                                if (serverPages.ContainsKey(segments[1]))
                                {
                                    page = serverPages[segments[1]];
                                }
                                else
                                {
                                    page = serverPages["index.html"];
                                }
                                context.Response.OutputStream.Write(page, 0, page.Length);
                            }
                            break;
                    }
                    context.Response.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void unlock(HttpListenerContext context)
        {
            var player = _players[int.Parse( context.Request.QueryString["userId"])];
            switch (context.Request.QueryString["code"])
            {
                case "sylvester":
                    json.write(context.Response.OutputStream, "You looted sylvester the slime for 4 gold!");
                    player.inventory[(int)currencyEnum.gold] += 4;
                    break;
                case "sammy":
                    json.write(context.Response.OutputStream, "You looted sammy the slime for 3 gold!");
                    player.inventory[(int)currencyEnum.gold] += 3;
                    break;
                case "sparrow":
                    json.write(context.Response.OutputStream, "You looted sparrow the spider for 4 gold!");
                    player.inventory[(int)currencyEnum.gold] += 4;
                    break;
                case "spike":
                    json.write(context.Response.OutputStream, "You looted spike the spider for 4 gold!");
                    player.inventory[(int)currencyEnum.gold] += 4;
                    break;
                case "slayer":
                    json.write(context.Response.OutputStream, "You looted slayer the slime for 4 gold!");
                    player.inventory[(int)currencyEnum.gold] += 4;
                    break;
                case "goldie":
                    json.write(context.Response.OutputStream, "You looted golden slime for 10 gold!");
                    player.inventory[(int)currencyEnum.gold] += 10;
                    break;
                case "sonata":
                    json.write(context.Response.OutputStream, "You looted sonata the slime for 4 gold!");
                    player.inventory[(int)currencyEnum.gold] += 4;
                    break;
                case "protein":
                    json.write(context.Response.OutputStream, "Wow, you did 100 pushups!"); //TODO reward
                    break;
                default:
                    json.write(context.Response.OutputStream, "You didn't unlock anything...");
                    break;
            }
        }

        private static void serveStaticPage(HttpListenerContext context)
        {
            var request = context.Request;
        }

        private static void players(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    json.write(context.Response.OutputStream, _players);
                    break;
            }
        }

        private static void notifications(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    var user = int.Parse(context.Request.QueryString[0]);
                    json.write(context.Response.OutputStream, _notifications[user]);
                    break;
            }
        }

        private static void options(HttpListenerContext context)
        {
            var response = context.Response;
            byte[] body = { };
            response.Headers.Add("Access-Control-Allow-Methods: PUT, GET, POST, DELETE, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers: Content-Type, origin");
            response.OutputStream.Write(body, 0, 0);
        }

        private static void duelChallenge(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "PUT":
                    var challenge = json.read<duel>(context.Request.InputStream);
                    activeDuelChallenges.Add(challenge);
                    foreach (var notificationList in _notifications)
                    {
                        if (notificationList.Key != challenge.target)
                        {
                            var notification = new notification();
                            var srcName = _players[challenge.src].characterName;
                            var dstName = _players[challenge.target].characterName;
                            notification.text = srcName + " has challenged " + dstName + " to a duel!";
                            _notifications[notificationList.Key].Add(notification);
                        }
                        else
                        {
                            var note = new notification();
                            note.text = _players[challenge.src].characterName + " has challenged you to a duel!";
                            _notifications[notificationList.Key].Add(note);
                        }
                    }
                    context.Response.Close();
                    var db = File.Open("duelChallenges.json", FileMode.OpenOrCreate);
                    json.write(db, activeDuelChallenges);
                    db.Close();
                    break;
                case "DELETE":
                    break;
                case "GET":
                    var playerId = int.Parse(context.Request.QueryString["playerId"]);
                    json.write(context.Response.OutputStream, activeDuelChallenges.Where(x=>x.target==playerId));
                    break;
            }
        }

        private static void duels(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    var playerId = int.Parse(context.Request.QueryString["playerId"]);
                    json.write(
                        context.Response.OutputStream,
                        activeDuelChallenges.Where(x=>
                            x.src==int.Parse( 
                                context.Request.QueryString["duelId"])
                                ));
                    break;
            }
        }

    }
}
