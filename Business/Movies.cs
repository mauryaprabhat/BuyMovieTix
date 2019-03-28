using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
using BuyTixApi.Data;
using BuyTixApi.Models.Seats;
using BuyTixApi.Models.Movies;
using BuyTixApi.Models.Purchase;

namespace BuyTixApi.Business
{
    public static class Movies
    {
        public static List<ActiveMovie> GetAllActiveMovies()
        {
            //retrieve all movies 
            List<ActiveMovie> ActiveMovies = new List<ActiveMovie>();

            DataTable dtActiveMovies = Data.Movies.GetAllActiveMovies();

            for (int i = 0; i < dtActiveMovies.Rows.Count; i++)
            {
                DataRow dr = dtActiveMovies.Rows[i];
                
                ActiveMovie _activeMovie = new ActiveMovie();
                _activeMovie.ActiveMovieId = (int)dr["ActiveMovieId"];
                _activeMovie.Date =  ((DateTime)dr["MovieDateTime"]).ToString("dd/MM/yyyy");
                _activeMovie.Time = ((DateTime)dr["MovieDateTime"]).ToString("HH:mm");
               
                Venue venue = new Venue();
                venue.VenueId = (int)dr["VenueId"];
                venue.VenueName = (string)dr["VenueName"];
                _activeMovie.Venue = venue;
               
                Movie movie = new Movie();
                movie.MovieId = (int)dr["MovieId"];
                movie.MovieName = (string)dr["MovieName"];
                movie.MovieDescription = (string)dr["MovieDescription"];
                _activeMovie.Movie = movie;

                ActiveMovies.Add(_activeMovie);
            }
            return ActiveMovies;
        }

        // public static SeatingPlan GetActiveMovieSeatingPlan(int ActiveMovieId)
        // {
        //     SeatingPlan _SeatingPlan = new SeatingPlan();
        //     _SeatingPlan.Rows = new List<RowDetails>();

        //     //retrieve the list of rows
        //     DataTable dtVenueRows = Data.Movies.GetVenueRows(ActiveMovieId);

        //     DataTable dtReservedSeats = Data.Movies.GetReservedSeats(ActiveMovieId);

        //     for (int i = 0; i < dtVenueRows.Rows.Count; i++)
        //     {
        //         DataRow drVenue = dtVenueRows.Rows[i];
        //         RowDetails _newRow = new RowDetails();
        //         _newRow.RowNumber = (int)drVenue["RowNumber"];
        //         _newRow.Seats = new List<SeatDetails>();

        //         //for each row in the venue, we want to create all the SEATS in that row

        //         //first, we create a "SEAT" for every OFFSET seat (which are with status 0)
        //         for (int j = 1; j <= (int)drVenue["SeatsOffset"]; j++)
        //         {
        //             SeatDetails seatt = new SeatDetails();
        //             seatt.RowNumber = (int)drVenue["RowNumber"];
        //             seatt.PositionId = i;
        //             seatt.ActualSeatNumber = 0;
        //             seatt.Status = 0;
        //             _newRow.Seats.Add(seatt);

        //         //create a "SeatStatus" object for each seat.
        //         // if seat is already taken - update status to 2

        //         for (int jj = 1; jj <= (int)drVenue["SeatsInRow"]; jj++)
        //         {
        //             SeatDetails seat = new SeatDetails();
        //             seat.RowNumber = (int)drVenue["RowNumber"];
        //             seat.PositionId = jj + (int)drVenue["SeatsOffset"];
        //             seat.ActualSeatNumber = jj;
        //             seat.Status = 0;
                    
        //             //check if seat is taken

        //             if (IsReservedSeat(dtReservedSeats, seat.RowNumber, seat.ActualSeatNumber))
        //                 seat.Status = 2;
        //             else
        //                 seat.Status = 1;

        //             //add the new seat the SEATS list , in the ROW
        //             _newRow.Seats.Add(seat);
        //         }
        //         //add the row to the Seating Plan
        //         _SeatingPlan.Rows.Add(_newRow);
        //     }
        //     return _SeatingPlan;
        // }


        private static bool IsReservedSeat(DataTable dtReservedSeats, int rowNumber, int seatNumber)
        {
            DataRow[] drs = dtReservedSeats.Select("RowNumber = " + rowNumber.ToString() +
                        " AND SeatNumber = " + seatNumber.ToString());
            return (drs.Length > 0);
        }

        public static string PurchaseTickets(Order order)
        {
            //return the orderID
            return Data.Movies.AddNewOrder(order);
        }
    }
}