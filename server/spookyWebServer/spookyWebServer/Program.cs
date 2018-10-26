//#define serveStaticPages

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
        static PartyGoer[] _players = new PartyGoer[] { };
        static PartyGoer[] _onlinePlayers = new PartyGoer[] { };

#if serveStaticPages
        static Dictionary<string, byte[]> serverPages = new Dictionary<string, byte[]>();
#endif
        static int Main(string[] args)
        {

            var backupThread = new Thread(backup);
            /*
            var playermock = new PartyGoer();
            playermock.id = 2;
            playermock.level = 4;
            playermock.characterName = "will";
            playermock.inventory = new int[] { 10, 42, 3, 4, 5, 2, 0, 0, 0, 0 };
            _players[0]= playermock;
            
            var writeFile = File.Open("players.json", FileMode.OpenOrCreate);
            json.write(writeFile, _players);
            writeFile.Close();
            */

#if serveStaticPages
            foreach (var file in Directory.EnumerateFiles("./www/"))
            {
                var fileFromRoot = file.Remove(0, "./www/".Length);
                var stream = File.OpenRead(file);
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                serverPages[fileFromRoot] = bytes;
            }
#endif

            #region setup
            var playerDb = File.Open("players.json", FileMode.OpenOrCreate);
            _players = json.read<PartyGoer[]>(playerDb) ?? new PartyGoer[] { };
            playerDb.Close();

            var duelDb = File.Open("duelChallenges.json", FileMode.OpenOrCreate);
            activeDuelChallenges = (json.read<duel[]>(duelDb) ?? new duel[] { }).ToList();
            duelDb.Close();

            var onlinePlayerFile = File.Open("onlinePlayers.json",FileMode.OpenOrCreate);
            _onlinePlayers = json.read<PartyGoer[]>(onlinePlayerFile);
            onlinePlayerFile.Close();

            Console.WriteLine("Loaded " + _players.Length + " players, " + _notifications.Count + " notification lists, and " + activeDuelChallenges.Count + " duels from files.");
#endregion

            http.Prefixes.Add("http://localhost:8220/");
            http.Start();

            while (true)
            {
                var context = http.GetContext();
                var request = context.Request;
                context.Response.Headers.Add("Access-Control-Allow-Origin: *");
                context.Response.Headers.Add("Cache-Control: max-age=0");
                var segments = request.Url.Segments.ToList();

                Console.WriteLine(request.RawUrl);

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
                    case "onlinePlayers":
                        onlinePlayers(context);
                        break;
                    case "unlock":
                        unlock(context);
                        break;
                    case "login":
                        login(context);
                        break;
                    default:
#if serveStaticPages
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
#endif
                        break;
                }
                context.Response.Close();
            }
        }

        static void backup()
        {
            var onlinePlayerFile = File.Open("onlinePlayers.json", FileMode.OpenOrCreate);
            json.write(onlinePlayerFile, _onlinePlayers);
            onlinePlayerFile.Close();

            var playerDb = File.Open("players.json", FileMode.OpenOrCreate);
            _players = json.read<PartyGoer[]>(playerDb) ?? new PartyGoer[] { };
            playerDb.Close();

            var db = File.Open("duelChallenges.json", FileMode.OpenOrCreate);
            json.write(db, activeDuelChallenges);
            db.Close();

            Thread.Sleep(5000);
        }

        private static void login(HttpListenerContext context)
        {
            var id =int.Parse( context.Request.QueryString["userId"]);
            _onlinePlayers[id] = _players[id];
            var onlinePlayerFile = File.Open("onlinePlayers.json", FileMode.OpenOrCreate);
            json.write(onlinePlayerFile, _onlinePlayers);
            onlinePlayerFile.Close();
        }

        private static void unlock(HttpListenerContext context)
        {
            var player = _onlinePlayers[int.Parse( context.Request.QueryString["userId"])];
            var code = context.Request.QueryString["code"];
            switch (code)
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
                    json.write(context.Response.OutputStream, "You looted the golden slime for 10 gold!");
                    player.inventory[(int)currencyEnum.gold] += 10;
                    break;
                case "sonata":
                    json.write(context.Response.OutputStream, "You looted sonata the slime for 4 gold!");
                    player.inventory[(int)currencyEnum.gold] += 4;
                    break;
                case "thief":
                    player.achievements.Add((int)achievementsEnum.sneakthief); break;

                default:
                    json.write(context.Response.OutputStream, "You didn't unlock anything...");
                    break;
            }
        }

#if serveStaticPages
        private static void serveStaticPage(HttpListenerContext context)
        {
            var request = context.Request;
        }
#endif
        private static void players(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    json.write(context.Response.OutputStream, _onlinePlayers);
                    break;
            }
        }
        private static void onlinePlayers(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    json.write(context.Response.OutputStream, _onlinePlayers);
                    break;
            }
        }

        private static void notifications(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    var user = int.Parse(context.Request.QueryString[0]);
                    json.write(context.Response.OutputStream, _onlinePlayers[user].notifications);
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
                    {

                        var challenge = json.read<duel>(context.Request.InputStream);
                        activeDuelChallenges.Add(challenge);
                        foreach (var notificationList in _notifications)
                        {
                            var target = _onlinePlayers[challenge.target];
                            var challenger = _onlinePlayers[challenge.src];
                            foreach (var p in _onlinePlayers)
                            {
                                var n = new notification();
                                if (p == target)
                                {
                                    n.text = challenger.characterName + " has challenged you to a duel!";
                                    n.link = "duel";
                                    p.notifications.Add(n);
                                }
                                else
                                {
                                    n.text = challenger.characterName + " has challenged " + target.characterName + " to a duel!";
                                    n.link = "duel";
                                }
                            }
                        }
                        context.Response.Close();
                        var db = File.Open("duelChallenges.json", FileMode.OpenOrCreate);
                        json.write(db, activeDuelChallenges);
                        db.Close();
                    }
                    break;
                case "DELETE":
                    //duel complete
                    {
                        var challenge = json.read<duel>(context.Request.InputStream);
                        var target = _players[challenge.target];
                        var challenger = _players[challenge.src];
                        target.duelCount++;
                        challenger.duelCount++;
                        foreach (var player in new PartyGoer[] { target, challenger })
                        {
                            if (player.duelCount == 5) player.achievements.Add((int)achievementsEnum.galois);
                            if (player.duelCount == 10) player.achievements.Add((int)achievementsEnum.duelist);
                            if (player.duelCount == 15) player.achievements.Add((int)achievementsEnum.champion);
                        }

                        if (challenge.src == challenge.winner)
                        {
                            challenger.inventory = challenger.inventory.Zip(challenge.targetWager.Select(x => x.count), (x, y) => x + y).ToArray();
                            target.inventory = target.inventory.Zip(challenge.targetWager.Select(x => x.count), (x, y) => x - y).ToArray();
                            foreach (var player in _onlinePlayers)
                            {
                                var n = new notification();
                                n.text = challenger.characterName + " has defeated " + target.characterName + " in a duel!";
                                player.notifications.Add(n);
                            }
                        }
                        else
                        {
                            challenger.inventory = challenger.inventory.Zip(challenge.srcWager.Select(x => x.count), (x, y) => x - y).ToArray();
                            target.inventory = target.inventory.Zip(challenge.srcWager.Select(x => x.count), (x, y) => x + y).ToArray();
                            foreach(var player in _onlinePlayers)
                            {
                                var n = new notification();
                                n.text = target.characterName + " has defeated " + challenger.characterName + " in a duel!";
                                player.notifications.Add(n);
                            }
                        }
                    }
                    break;
                case "GET":
                    var playerId = int.Parse(context.Request.QueryString["playerId"]);
                    json.write(context.Response.OutputStream, activeDuelChallenges.Where(x => x.target == playerId));
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
