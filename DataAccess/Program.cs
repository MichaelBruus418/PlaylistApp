using DataAccess.DatabaseContext;
using DataAccess.Model;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test");

            //DbContext context = new DbContext();
            //Track t = context.Tracks.Find(1);

            DTO.Model.Track t = TrackRepository.GetTrackById(1);

            Console.WriteLine(t.Title);
            Console.ReadLine();
        }
    }
}
