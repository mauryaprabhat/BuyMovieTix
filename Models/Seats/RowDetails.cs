using System;
using System.Collections.Generic;

namespace BuyTixApi.Models.Seats
{
    public class RowDetails
    {
        public int RowNumber;
        public List<SeatDetails> Seats;
    }
}