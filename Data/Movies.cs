
// CAUTION: SQL Injection!!!
// - The following code uses plain SQL without any defense against SQL Injection attack for simplicity sake.
// - A real-world application should always use parametrized to defend against SQL Injection attack.

using BuyTixApi.Models.Purchase;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BuyTixApi.Data
{
    public static class Movies
    {
        public static DataTable GetAllActiveMovies()
        {
            string Sql = "Select Movies.*,ActiveMovieID, MovieDateTime, ActiveMovies.VenueID,Venues.VenueName from " +
                         "   ActiveMovies Inner Join Movies" +
                         "   on ActiveMovies.MovieID =  Movies.MovieID" +
                         "   Inner join Venues on ActiveMovies.VenueID = Venues.VenueID ";
            return BaseDB.GetDataTable(Sql);
        }

        public static DataTable GetVenueRows(int ActiveMovieId)
        {
            string Sql = "Select * from VenueRows " +
                         " WHERE VenueId = (Select VenueId from ActiveMovies Where ActiveMovieId = " + ActiveMovieId.ToString() + " )" +
                         " ORDER BY RowNumber";
            return BaseDB.GetDataTable(Sql);
        }

        public static DataTable GetReservedSeats(int ActiveMovieId)
        {
            string Sql = "Select ReservedSeats.*, Orders.ActiveMovieId from ReservedSeats " +
                        " INNER JOIN Orders" +
                        " ON ReservedSeats.OrderId =  Orders.OrderId " +
                        " WHERE Orders.ActiveMovieId = " + ActiveMovieId.ToString() +
                        " ORDER BY RowNumber, SeatNumber ";
            return BaseDB.GetDataTable(Sql);
        }

        public static string AddNewOrder(Order order)
        {
            string guid = Guid.NewGuid().ToString();
            string Sql;

            using (SqlConnection conn = new SqlConnection(BaseDB.CONN))
            {
                using(SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        //save the order
                        Sql = "Insert into Orders (ActiveMovieId, PurchaseGuid, FullName, Email, CCNumber) " +
                                " Values(" + order.ActiveMovieId.ToString() + ", " +
                                "'" + guid + "', " +
                                "'" + order.FullName + "', " +
                                "'" + order.Email + "', " +
                                "'" + order.CcNumber + "' " +
                                ")";

                        BaseDB.ExecuteNonQuery(conn, Sql, tr);

                        string SqlSelect = "Select OrderId from Orders Where PurchaseGuid = '" + guid + "' ;";
                        string strOrderId = BaseDB.ExecuteScalar(conn, SqlSelect, tr).ToString();
                        string SqlSeats;

                        for (int i = 0; i < order.ReservedSeats.Count; i++)
                        {
                            SqlSeats = "Insert into ReservedSeats (RowNumber, SeatNumber, OrderId ) VALUES (" +
                                order.ReservedSeats[i].RowNumber.ToString() + ',' +
                                order.ReservedSeats[i].ActualSeatNumber.ToString() + ',' +
                                strOrderId + ");";

                            BaseDB.ExecuteNonQuery(conn, SqlSeats, tr);
                        }

                        tr.Commit();

                        return strOrderId;
                    }

                    catch (Exception ex)
                    {
                        tr.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}