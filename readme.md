## Azenia Pre Screening Assessment.
## This is my solution to the Azenia assessment shared
## I believe the AddToEmail method can be optimized to accept list of events which can be sent to the customer's email directly', rather than accepting a single event object, which needs to go through iteration for just a single customer. This will amount to  performance issue if the AddToEmail methods should cater for multiple customers
## The GetDistance method was refactored to use cache in order to fetch the distance based on what is stored in the cache, rather than calling the API endpoint everytime 
