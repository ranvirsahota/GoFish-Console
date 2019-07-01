﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> cards = new List<string>() {
            "ace", "ace", "ace", "ace",
            "two", "two", "two", "two",
            "three", "three", "three", "three",
            "four", "four", "four", "four",
            "five", "five", "five", "five",
            "six", "six", "six", "six",

            };

            /*
             *             "nine", "nine", "nine", "nine",
             "ten", "ten", "ten", "ten",
             "jack", "jack", "jack", "jack",
             "queen", "queen", "queen", "queen",
             "king", "king", "king", "king"
                         "seven", "seven", "seven", "seven",
            "eight", "eight", "eight", "eight",*/

            var rnd = new Random();
            cards = cards.OrderBy(x => rnd.Next()).ToList<string>();
            Dictionary<string, CardPlayer> cardPlayers = new Dictionary<string, CardPlayer>() {
                {"Jack", new DumbCardPlayer("Jack")},
                {"Archer", new SmartCardPlayer("Archer")},
                {"Mr Pickles", new SmartCardPlayer("Mr Pickles")},
                {"Ranvir", new Player("Ranvir")}
            };
            for (int counter = 1; counter < 5; counter++)
            {
                foreach (KeyValuePair<string, CardPlayer> cardPlayer in cardPlayers)
                {
                    cardPlayer.Value.CardsSet(cards[0]);
                    cards.RemoveAt(0);
                }
            }

            Globals.Stack = cards;

            for (int counter = 0; counter < Globals.Stack.Count; counter++)
            {
                Console.WriteLine(Globals.Stack[counter]);
            }
            Console.WriteLine("This is index 0: " + Globals.Stack[0]);
            Console.ReadLine();
            //Dictionary<string, CardPlayer> cardPlayersOut = new Dictionary<string, CardPlayer>();
            CardPlayer currentWinner = null;
            do
            {
                foreach (KeyValuePair<string, CardPlayer> cardPlayer in cardPlayers)
                {
                    if (currentWinner == null) { currentWinner = cardPlayer.Value; }

                    if (currentWinner.Books.Count < cardPlayer.Value.Books.Count) { currentWinner = cardPlayer.Value; }
                    for (int i = 0; i < 1;)
                    {
                        string cardSeeking = null;
                        int cardSeekingAmount = 0;

                        if (cardPlayer.Value.CardCount == 0)
                        {
                            if (Globals.Stack.Count >= 1)
                            {
                                cardPlayer.Value.CardsSet(Globals.Stack[0]);
                                Console.WriteLine("I got a: " + Globals.Stack[0]);//
                                Globals.PUBLICLY_KNOWN_CARDS[cardPlayer.Key].Add(Globals.Stack[0]);
                                Globals.Stack.Remove(Globals.Stack[0]);
                            }
                            else
                            {
                                Globals.CardPlayerNames.Remove(cardPlayer.Key);
                                Globals.PUBLICLY_KNOWN_CARDS.Remove(cardPlayer.Key);
                                break;
                            }
                        }
                        try
                        {
                            cardPlayer.Value.Decision(true);
                        }
                        catch (CardPlayerDoesNotHaveCardType ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        cardSeeking = Globals.CARD_TRANSACTION.Keys.First();
                        if (!Globals.PUBLICLY_KNOWN_CARDS[cardPlayer.Key].Contains(cardSeeking)) {
                            Globals.PUBLICLY_KNOWN_CARDS[cardPlayer.Key].Add(cardSeeking);
                        }
                        cardPlayers[Globals.CARD_PLAYERS_FROM_TO_TRANSACTION[1]].Decision(false, cardSeeking);

                        cardSeekingAmount = Globals.CARD_TRANSACTION[Globals.CARD_TRANSACTION.Keys.First()];

                        if (cardSeekingAmount > 0)
                        {
                            cardPlayer.Value.CardsSet(cardSeeking, cardSeekingAmount);
                        }

                        else
                        {
                            if (Globals.Stack.Count >= 1)
                            {
                                cardPlayer.Value.CardsSet(Globals.Stack[0]);
                                Console.WriteLine("I got a: " + Globals.Stack[0]);
                                Globals.PUBLICLY_KNOWN_CARDS[cardPlayer.Key].Add(Globals.Stack[0]);
                                if (Globals.Stack[0] != cardSeeking)
                                {
                                    i++;
                                    Console.WriteLine("Not a match");
                                }
                                Globals.Stack.Remove(Globals.Stack[0]);
                            }
                            else
                            {
                                i++;
                            }
                        }

                        Console.ReadLine();

                        Globals.CARD_TRANSACTION.Remove(cardSeeking);
                        Globals.CARD_PLAYERS_FROM_TO_TRANSACTION[1] = null;
                        Globals.CARD_PLAYERS_FROM_TO_TRANSACTION[0] = null;
                    }
                }
            } while (Globals.CardPlayerNames.Count > 1);
            Console.WriteLine("Card Player: " + currentWinner.Name);
            Console.ReadLine();
        }
    }
}