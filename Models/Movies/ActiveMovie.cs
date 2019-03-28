using System;
 namespace BuyTixApi.Models.Movies { 
         public class ActiveMovie {
         public int ActiveMovieId { get; set; } 
         public string Date { get; set; } 
         public string Time { get; set; } 
         public Venue Venue { get; set; } 
         public Movie Movie { get; set; }
          }
     }