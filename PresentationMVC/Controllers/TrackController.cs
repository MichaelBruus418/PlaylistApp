using BusinessLogic.BLL;
using DTO.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace PresentationMVC.Controllers
{
    public class TrackController : Controller
    {
        public ActionResult Index()
        {
            TrackBLL trackBLL = new TrackBLL();
            List<Track> trackList = trackBLL.GetAll();

            return View(trackList);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Add", new Track());
        }

        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            // Note:
            // Jeg bruger ikke model binding til Track, da auto-konvertering til TimeSpan er problematisk.

            Track track = new Track();

            if (String.IsNullOrEmpty(form["Title"]) || form["Title"].Trim().Length == 0)
            {
                ModelState.AddModelError("Title", "Title is required");
            }
            else
            {
                track.Title = form["Title"];
            }

            if (!String.IsNullOrEmpty(form["Length"]))
            {
                // --- Parse Length ---
                // Expected format is (string): hh:mm:ss|hh:mm|ss
                String errorMessage = "Length was invalid";
                List<String> strValues = form["Length"].Trim().Split(':').ToList();

                if (strValues.Count > 3)
                {
                    ModelState.AddModelError("Length", errorMessage);
                }

                // Pad with zeroes for hours and minutes
                while (strValues.Count < 3)
                {
                    strValues.Insert(0, "0");
                }

                try
                {
                    // Convert to list of ints
                    List<int> intValues = strValues.ConvertAll(v => int.Parse(v));
                    // Create TimeSpan object
                    track.Length = new TimeSpan(intValues[0], intValues[1], intValues[2]);
                }
                catch
                {
                    ModelState.AddModelError("Length", errorMessage);
                }
            }


            if (ModelState.IsValid)
            {
                TrackBLL trackBLL = new TrackBLL();
                trackBLL.AddTrack(track);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Add", track);
            }
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            TrackBLL trackBLL = new TrackBLL();

            // Only checked elements are included in form
            // Create list of Ids to delete
            List<int> ids = new List<int>();
            foreach (var f in form)
            {
                try
                {
                    int id = Int32.Parse(f.ToString());
                    ids.Add(id);
                }
                catch (FormatException)
                {
                    Trace.WriteLine("Unable to parse form value to int: " + f.ToString());
                }

            }

            if (ids.Count > 0)
            {
                int numOfRowsDeleted = trackBLL.DeleteByIds(ids);
                Trace.WriteLine("Num of rows deleted: " + numOfRowsDeleted);
            }

            List<Track> trackList = trackBLL.GetAll();
            return View("Index", trackList);
        }

        [ChildActionOnly]
        public ActionResult TrackLengthChildAction(Track t)
        {
            // Here we use a childAction to format output of Length attribute in track.
            String txt = "";
          
            if (t.Length.HasValue)
            {
                TimeSpan ts = (TimeSpan)t.Length;
                txt = (ts.Hours > 0) ? ts.ToString(@"h\:mm\:ss") : ts.ToString(@"mm\:ss");
            }
            
            return PartialView("_OutputSingleValue", txt);
        }


    }
}
