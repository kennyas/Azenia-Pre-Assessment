using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Azenia_Assessment
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
    }
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
    }
    public class EventNotification
    {
        public Event Event { get; set; }
        public int? distance { get; set; }
        public int? Price { get; set; }
        public int? BirthDayDifference { get; set; }
    }
    public class Solution
    {
        static readonly Dictionary<string, int> cacheCityDistanceDict = new Dictionary<string, int>();
        static void Main(string[] args)
        {
            var events = new List<Event>{
                                        new Event{ Name = "Phantom of the Opera", City = "New York", Date= new DateTime(2022, 8, 1)},
                                        new Event{ Name = "Metallica", City = "Los Angeles", Date= new DateTime(2022, 8, 2)},
                                        new Event{ Name = "Metallica", City = "New York" , Date= new DateTime(2022, 8, 3)},
                                        new Event{ Name = "Metallica", City = "Boston",  Date= new DateTime(2022, 7, 30)},
                                        new Event{ Name = "LadyGaGa", City = "New York",  Date= new DateTime(2022, 8, 5)},
                                        new Event{ Name = "LadyGaGa", City = "Boston", Date= new DateTime(2022, 7, 31)},
                                        new Event{ Name = "LadyGaGa", City = "Chicago",  Date= new DateTime(2022, 8, 4)},
                                        new Event{ Name = "LadyGaGa", City = "San Francisco", Date= new DateTime(2022, 8, 7)},
                                        new Event{ Name = "LadyGaGa", City = "Washington", Date= new DateTime(2022, 7, 31)}
                                        };
            //1. find out all events that are in cities of customer
            // then add to email.
            var customer = new Customer { Name = "Mr. Fake", City = "New York", BirthDate = new DateTime(1989, 8, 3) };
            

            // 1. TASK SendEventsInCustomerCity
            var eventsInCustomerCity = events.Where(x => x.City != null && x.City == customer.City).ToList();
            Console.WriteLine($"Fetch events in customer city");
            foreach (var item in eventsInCustomerCity)
            {
                AddToEmail(customer, item);
            }
            Console.WriteLine("=================================================");


            // Find 5 events closest to customer's location
            var eventsCloseToCustomerCity = events.Select(e => new EventNotification { Event = e, distance = FetchDistanceInDictionaryCache(customer.City, e.City) }).Where(x => x.distance >= 0).OrderBy(e => e.distance).Take(5);

            Console.WriteLine($"Fetch events closest to customer's city");
            foreach (var item in eventsCloseToCustomerCity)
            {
                AddToEmail(customer, item.Event);
            }
            Console.WriteLine("=================================================");


            // Find cheapest events
            var cheapestEvents = events.Select(e => new EventNotification { Event = e, Price = GetPrice(e) }).OrderBy(e => e.distance).ThenBy(e=>e.Price).Take(5);
            Console.WriteLine($"Fetch cheapest events for the customer");
            foreach (var item in cheapestEvents)
            {
                AddToEmail(customer, item.Event);
            }
            Console.WriteLine("=================================================");

            //The Distance is cached assuming the GetDistance method is expensive and the error is catched and a value is returned in order to ensure the code is still running, irrespective of whether the GetDistance API/method fails
            static int FetchDistanceInDictionaryCache(string origin, string destination)
            {

                var cacheKey = $"{origin}:{destination}";
                bool isKeyFound = cacheCityDistanceDict.TryGetValue(cacheKey, out int distance);
                if (isKeyFound)
                {
                    return distance;
                }
                else if (origin.Equals(destination))
                {
                    cacheCityDistanceDict.Add(cacheKey, 0);
                }
                else
                {
                    try
                    {
                        distance = GetDistance(origin, destination);
                        cacheCityDistanceDict.Add(cacheKey, distance);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An exception has occurred while fetching the GetDistance method/API payloads(fromCity: {origin}, toCity: {destination}) with exception message: {ex.Message} ");
                        return -1;
                    }
                }
                return distance;
            }

            /**
We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
*/
        } // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i
            <
            Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
} /*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/