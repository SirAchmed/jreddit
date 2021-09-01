using System;
using RedditSharp; // reddit API
using System.Linq; // queries
using System.Collections.Generic; // lists
using System.IO; // read/write files

namespace JReddit
{
    class Program
    {
        public static void Main()
        {
            List<string> users; // users
            List<string> subs; // subs
            Reddit impReddit; // account you want to import to
            Reddit expReddit; ; // account you want to export from
            string subFilePath, userFilePath; // variables
            Console.Clear();
        login:
            Console.WriteLine("\n\n\nPlease log in to your Reddit account\n\n");
            impReddit = RLogin();
            Console.Clear();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nMake a choice:\n" +
                                  "1. Import subreddits and/or users from txt file.\n" +
                                  "2. Export subreddits and users to txt files.\n" +
                                  "3. Import subreddits and/or users from another account.\n" +
                                  "4. Unsubscribe/unfollow subreddits/users from txt file.\n" +
                                  "5. Unsubscribe and/or unfollow all current subreddits and users.\n" +
                                  "6. Display current subreddits and users.\n" +
                                  "7. Log in to a different account.\n" +
                                  "8. About.\n" +
                                  "\nTo exit, press ctrl + c" +
                                  $"\n\n\n\nCurrently logged in to: {impReddit.User}.");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                    rose1:
                        Console.WriteLine("Enter subreddits file path, or drag file here: ");
                        subFilePath = Console.ReadLine();
                        try
                        {
                            subs = new List<string>(File.ReadAllLines(subFilePath));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "\n");
                            goto rose1;
                        }
                    jack1:
                        Console.WriteLine("Enter users file path: ");
                        userFilePath = Console.ReadLine();
                        try
                        {
                            users = new List<string>(File.ReadAllLines(userFilePath));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "\n");
                            goto jack1;
                        }

                        Subscribe(impReddit, subs);
                        Follow(impReddit, users);
                        PAK();
                        break;

                    case "2":
                        Console.WriteLine("Enter a path to export files to, or drag folder here: ");
                        subFilePath = Console.ReadLine();
                        ShowSubsUsers(impReddit, out subs, out users);
                    rose2:
                        try
                        {
                            File.WriteAllLines(subFilePath + impReddit.User + "_users.txt", users); // export users list to file
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ERROR!\n" + e.Message + "\nPress enter to try again.");
                            Console.ReadKey();
                            goto rose2;
                        }
                    jack2:
                        try
                        {
                            File.WriteAllLines(subFilePath + impReddit.User + "_subs.txt", subs); // export subs list to file
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            goto jack2;
                        }
                        Console.WriteLine("\nSuccessfully exported users and subreddits to txt files.");
                        PAK();
                        break;

                    case "3":
                        Console.WriteLine("Please login to the account you want to export from.");
                        expReddit = RLogin();
                        Console.WriteLine($"\n{expReddit.User} is currently subsribed to the following subreddits:\n");
                        ShowSubsUsers(expReddit, out subs, out users);
                        Console.WriteLine($"\n\nDo you want {impReddit} to subscribe to all the listed subreddits?");
                        string answer1 = Console.ReadLine();
                        answer1 = answer1.ToLower();

                        while (true) // join subs
                        {
                            if (answer1 == "yes")
                            {
                                Subscribe(impReddit, subs);
                                break;
                            }
                            else if (answer1 == "no")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine("\nDo you want to subscribe to all listed subreddits?");
                                answer1 = Console.ReadLine();
                                answer1 = answer1.ToLower();
                            }
                        }

                        Console.WriteLine("\n\nDo you want to follow all the listed users?");
                        string answer2 = Console.ReadLine();
                        answer2 = answer2.ToLower();

                        while (true) // follow users
                        {
                            if (answer2 == "yes")
                            {
                                Follow(impReddit, users);
                                break;
                            }
                            else if (answer2 == "no")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine("\nDo you want to follow all listed users?");
                                answer2 = Console.ReadLine();
                                answer2 = answer2.ToLower();
                            }
                        }
                        Console.WriteLine("\nPress any key to go back to the main menu.");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.WriteLine("Enter subreddits file path, or drag file here: ");
                        subFilePath = Console.ReadLine();
                    rose4:
                        try
                        {
                            subs = new List<string>(File.ReadAllLines(subFilePath));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            goto rose4;
                        }

                        Console.WriteLine("Enter users file path: ");
                        userFilePath = Console.ReadLine();
                    jack4:
                        try
                        {
                            users = new List<string>(File.ReadAllLines(userFilePath));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            goto jack4;
                        }
                        Console.WriteLine($"\nDo you want to unsubscribe {impReddit.User} from all the listed subreddits?");
                        string answer3 = Console.ReadLine();
                        answer3 = answer3.ToLower();

                        while (true) // leave subs
                        {
                            if (answer3 == "yes")
                            {
                                Unsubscribe(impReddit, subs);
                                break;
                            }
                            else if (answer3 == "no")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine($"Do you want to unsubscribe {impReddit.User} from all the listed subreddits?");
                                answer3 = Console.ReadLine();
                                answer3 = answer3.ToLower();
                            }
                        }
                        Console.WriteLine($"\nDo you want to unfollow all the listed users from {impReddit.User}?");
                        string answer4 = Console.ReadLine();
                        answer4 = answer4.ToLower();
                        while (true) // unfollow users
                        {
                            if (answer4 == "yes")
                            {
                                Unfollow(impReddit, users);
                                break;
                            }
                            else if (answer4 == "no")
                            {
                                Console.WriteLine("Goodbye!");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine($"Do you want to unfollow all the listed users from {impReddit.User}?");
                                answer4 = Console.ReadLine();
                                answer4 = answer4.ToLower();
                            }
                        }
                        PAK();
                        break;
                    case "5":
                        Console.WriteLine($"\nDo you want {impReddit.User} to unfollow all users?");
                        string answer5 = Console.ReadLine();
                        answer5 = answer5.ToLower();
                        while (true)
                        {
                            if (answer5 == "yes")
                            {
                                UnfollowAll(impReddit);
                                break;
                            }
                            else if (answer5 == "no")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine($"Do you want to unfollow all the users from {impReddit.User}?");
                                answer5 = Console.ReadLine();
                                answer5 = answer5.ToLower();
                            }
                        }
                        Console.WriteLine($"\nDo you want to unsubscribe from all subreddits from {impReddit.User}?");
                        string answer6 = Console.ReadLine();
                        answer6 = answer6.ToLower();
                        while (true)
                        {
                            if (answer6 == "yes")
                            {
                                UnsubAll(impReddit);
                                break;
                            }
                            else if (answer6 == "no")
                            {
                                Console.WriteLine("Goodbye!");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine($"Do you want to unfollow all the users from {impReddit.User}?");
                                answer6 = Console.ReadLine();
                                answer6 = answer6.ToLower();
                            }
                        }
                        PAK();
                        break;
                    case "6":
                        ShowSubsUsers(impReddit, out _, out _);
                        PAK();
                        break;
                    case "7":
                        goto login;
                    case "8":
                        Console.Clear();
                        Console.WriteLine("\nWritten by Achmed");
                        Console.WriteLine("achmed_sb@outlook.com");
                        PAK();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("*************************");
                        Console.WriteLine("*  Enter a number 1–8   *");
                        Console.WriteLine("*************************");
                        PAK();
                        break;
                }
            }
        }
        public static void Subscribe(Reddit acc, List<string> subs)
        {
            Console.WriteLine("\nSubreddits:");
            int i = 0;
            foreach (string subreddit in subs)
            {
                try
                {
                    acc.GetSubreddit(subreddit).Subscribe();
                    i++;
                    Console.WriteLine($"Subscribed {acc.User} to {subreddit} ({i} of {subs.Count})");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not subscribe to {subreddit}. Please make sure this sub exists and is not private.");
                    Console.WriteLine(e.Message);
                }

            }
            Console.WriteLine($"\nSuccessfully subscribed to {subs.Count} subreddits.");
        }
        public static void Unsubscribe(Reddit acc, List<string> subs)
        {
            int i = 0;
            foreach (string subreddit in subs)
            {
                acc.GetSubreddit(subreddit).Unsubscribe();
                i++;
                Console.WriteLine($"Unsubscribed {acc.User} from {subreddit} ({i} of {subs.Count})");
            }
            Console.WriteLine($"\nSuccessfully unsubscribed from {subs.Count} subreddits.");
        }
        public static void UnsubAll(Reddit acc)
        {
            int i = 0;
            int j = acc.User.SubscribedSubreddits.Where(s => !s.Name.StartsWith("/user/")).Count();
            foreach (string subreddit in acc.User.SubscribedSubreddits.Where(s => !s.Name.StartsWith("/user")).Select(s => s.Name))
            {
                acc.GetSubreddit(subreddit).Unsubscribe();
                i++;
                Console.WriteLine($"Unsubscribed {acc.User} from {subreddit} ({i} of {j})");
            }
            Console.WriteLine($"\nSuccessfully unsubscribed from {i} subreddits.");
        }
        public static void UnfollowAll(Reddit acc)
        {
            int i = 0;
            int j = acc.User.SubscribedSubreddits.Where(s => s.Name.StartsWith("/user/")).Count();
            foreach (string subreddit in acc.User.SubscribedSubreddits.Where(s => s.Name.StartsWith("/user")).Select(s => s.Name))
            {
                acc.GetSubreddit("r/u_" + subreddit.Substring(6)).Unsubscribe();
                i++;
                Console.WriteLine($"{acc.User} unfollowed {subreddit.Substring(6)} ({i} of {j})");
            }
            Console.WriteLine($"\nSuccessfully unfollowed {i} users.");
        }
        public static void Follow(Reddit acc, List<string> users)
        {
            Console.WriteLine("\nUsers:\n");
            int i = 0;
            foreach (string user in users)
            {
                acc.GetSubreddit("r/u_" + user).Subscribe();
                i++;
                Console.WriteLine($"{acc.User} followed {user} ({i} of {users.Count})");
            }
            Console.WriteLine($"\nSuccessfully followed {users.Count} users.");
        }
        public static void Unfollow(Reddit acc, List<string> users)
        {
            int i = 0;
            foreach (string user in users)
            {
                acc.GetSubreddit("r/u_" + user).Unsubscribe();
                i++;
                Console.WriteLine($"{acc.User} unfollowed {user} ({i} of {users.Count})");
            }
            Console.WriteLine($"\nSuccessfully unfollowed {users.Count} users.");
        }
        public static void ShowSubsUsers(Reddit acc, out List<string> subs, out List<string> users)
        {
            users = new List<string>();
            subs = new List<string>();
            Console.WriteLine($"\n{acc.User} is currently subscribed to the following subreddits:\n");
            foreach (string subreddit in acc.User.SubscribedSubreddits.Where(s => !s.Name.StartsWith("/user/")).Select(s => s.Name))
            {
                try
                {
                    subs.Add(subreddit);
                    Console.WriteLine(subreddit);
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not retrieve subreddit.");
                }
            }
            Console.WriteLine($"\n{acc.User} is currently following to the following users:\n");
            foreach (string subreddit in acc.User.SubscribedSubreddits.Where(s => s.Name.StartsWith("/user/")).Select(s => s.Name))
            {
                users.Add(subreddit.Substring(6));
                Console.WriteLine(subreddit.Substring(6));
            }
            Console.WriteLine($"\nShowing {subs.Count} subreddits & {users.Count} users.\n");
        }
        public static Reddit RLogin()
        {
            Reddit acc = new Reddit();
            string usr, pwd;
        steve:
            Console.Write("\nEnter username: ");
            usr = Console.ReadLine();
            Console.Write("Enter password: ");
            pwd = Password();
            Console.WriteLine();
            try
            {
                acc.LogIn(usr, pwd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                goto steve;
            }
            return acc;
        }
        public static string Password()
        {
            string pwd = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pwd += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pwd.Length > 0)
                    {
                        pwd = pwd.Substring(0, (pwd.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            return pwd;
        }
        public static void PAK()
        {
            Console.WriteLine("\nPress any key to go back to the main menu.");
            Console.ReadKey();
        }
    }
}
