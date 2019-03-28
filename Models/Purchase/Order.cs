using System;
using System.Collections.Generic;
using BuyTixApi.Models.Seats;

namespace BuyTixApi.Models.Purchase
{
    public class Order
    {
        public int ActiveMovieId;
        public List<SeatDetails> ReservedSeats;
        public string FullName;
        public string Email;
        public string CcNumber;
    }
}