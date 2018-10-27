﻿//#define serveStaticPages

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
        static List<PartyGoer>_players = new List< PartyGoer>() { };
        static List<PartyGoer> _onlinePlayers = new List<PartyGoer>() { };
        static bool fileLock = false;

#if serveStaticPages
        static Dictionary<string, byte[]> serverPages = new Dictionary<string, byte[]>();
#endif
        static int Main(string[] args)
        {
            /*
            var playermock = new PartyGoer();
            playermock.id = 0;
            playermock.level = 4;
            playermock.characterName = "will";
            playermock.passPhrase = "qq";
            playermock.inventory = new int[] { 0, 0, 0 };
            _players.Add(playermock);

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
            fileLock = true;
            var playerDb = File.Open("players.json", FileMode.OpenOrCreate);
            _players = json.read<List<PartyGoer>>(playerDb) ?? new List<PartyGoer>() { };
            playerDb.Close();

            var duelDb = File.Open("duelChallenges.json", FileMode.OpenOrCreate);
            activeDuelChallenges = (json.read<duel[]>(duelDb) ?? new duel[] { }).ToList();
            duelDb.Close();

            var onlinePlayerFile = File.Open("onlinePlayers.json",FileMode.OpenOrCreate);
            _onlinePlayers = json.read< List<PartyGoer>> (onlinePlayerFile);
            onlinePlayerFile.Close();
            fileLock = false;

            Console.WriteLine("Loaded " + _players.Count + " players, " + _notifications.Count + " notification lists, and " + activeDuelChallenges.Count + " duels from files.");
            #endregion

            
            var backupThread = new Thread(backup);
            var passiveIncomeThread = new Thread(passiveIncome);
            backupThread.Start();
            passiveIncomeThread.Start();

            http.Prefixes.Add("http://localhost:8220/");
            http.Prefixes.Add("http://+:8220/");
            http.Prefixes.Add("http://magiccrystalball:8220/");
            http.Prefixes.Add("http://magiccrystalball:8220/players/");
            http.Start();

            while (true)
            {
                var context = http.GetContext();
                var request = context.Request;
                context.Response.Headers.Add("Access-Control-Allow-Origin: *");
                context.Response.Headers.Add("Cache-Control: max-age=0");
                context.Response.Headers.Add("Access-Control-Allow-Methods: PUT, GET, POST, DELETE, OPTIONS");
                context.Response.Headers.Add("Access-Control-Allow-Headers: Content-Type, origin");
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
                    case "levelUp":
                        levelUp(context);
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

        private static void levelUp(HttpListenerContext context)
        {
            var playerId = int.Parse( context.Request.QueryString["userId"]);
            var player = _players[playerId];
            var goldCost = player.level * player.level + 3;
            if (player.inventory[(int)currencyEnum.crystals] > 0)
            {
                player.inventory[(int)currencyEnum.crystals] -= 1;
                player.level++;
                var note = new notification();
                note.text = player.characterName + " is now level " + player.level + "!";
                _players.ForEach(x => x.notifications.Add(note));
            }
            else if(player.inventory[(int)currencyEnum.gold] > goldCost && player.inventory[(int)currencyEnum.ironOre] > 0)
            {
                player.inventory[(int)currencyEnum.gold] -= goldCost;
                player.inventory[(int)currencyEnum.ironOre] -= 1;
                player.level++;
                var note = new notification();
                note.text = player.characterName + " is now level " + player.level + "!";
                _players.ForEach(x => x.notifications.Add(note));
            }
            json.write(context.Response.OutputStream, player);
        }

        static void backup()
        {
            while (true)
            {
                while (fileLock) Thread.Sleep(100);
                fileLock = true;
                Console.WriteLine("backing up.");
                var onlinePlayerFile = File.Open("onlinePlayers.json", FileMode.OpenOrCreate);
                json.write(onlinePlayerFile, _onlinePlayers);
                onlinePlayerFile.Close();

                var playerFile = File.Open("players.json", FileMode.OpenOrCreate);
                json.write(playerFile, _players.ToArray());
                Thread.Sleep(1000);
                playerFile.Close();
                
                var db = File.Open("duelChallenges.json", FileMode.OpenOrCreate);
                json.write(db, activeDuelChallenges);
                db.Close();

                fileLock = false;
                Thread.Sleep(15000);
            }
        }

        static void passiveIncome()
        {
            while (true)
            {
                Thread.Sleep(15 * 60 * 1000);
                foreach (var player in _onlinePlayers)
                {
                    var p = _players.First(x => x.id == player.id);
                    p.inventory[(int)currencyEnum.gold] += (int)(p.level * p.level) / 2 + 1;
                }
                Console.WriteLine("delivered passive income to " + _onlinePlayers.Count + " players.");
            }
        }

        #region web methods
        private static void login(HttpListenerContext context)
        {
            var id =int.Parse( context.Request.QueryString["userId"]);
            if (!_onlinePlayers.Any(x => x.id == id))
            {
                _onlinePlayers.Add(_players[id]);
                if (_onlinePlayers.Count == 5)
                {
                    _players.ForEach(x => x.achievements.Add((int)achievementsEnum.partyoffive));
                }

                while (fileLock) Thread.Sleep(100);
                fileLock = true;
                var f=File.Open("onlinePlayers.json", FileMode.OpenOrCreate);
                json.write(f, _onlinePlayers);
                f.Close();
                fileLock = false;
            }
        }

        private static void unlock(HttpListenerContext context)
        {
            var player = _players[int.Parse( context.Request.QueryString["userId"])];
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
                    json.write(context.Response.OutputStream, _players);
                    break;
            }
        }
        private static void onlinePlayers(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    json.write(context.Response.OutputStream, _players.Where(x=>_onlinePlayers.Any(y=>y.id==x.id)));
                    break;
            }
        }

        private static void notifications(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    var user = int.Parse(context.Request.QueryString[0]);
                    json.write(context.Response.OutputStream, _players[user].notifications);
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
                        challenge.id = Guid.NewGuid().ToString();
                        activeDuelChallenges.Add(challenge);
                        foreach (var notificationList in _notifications)
                        {
                            var target = _players[challenge.target];
                            var challenger = _players[challenge.src];
                            foreach (var p in _players)
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
                    }
                    break;
                case "DELETE":
                    {
                        var duelId = context.Request.QueryString["duelId"];
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
                            foreach (var player in _players)
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
                            foreach (var player in _players)
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
                            x.target==playerId));
                    break;
            }
        }

    }
}
#endregion