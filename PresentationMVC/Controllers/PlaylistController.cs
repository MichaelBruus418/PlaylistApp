using BusinessLogic.BLL;
using DTO.Model;
using PresentationMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationMVC.Controllers
{
    public class PlaylistController : Controller
    {
        // GET: Playlist
        public ActionResult Index()
        {
            PlaylistBLL plBLL = new PlaylistBLL();
            List<Playlist> playlists = plBLL.GetAll();

           

            return View(playlists);
        }

        [HttpGet]
        public ActionResult Display(int? id)
        {
            Playlist pl = new Playlist();
            
            if (id.HasValue)
            {
                try
                {
                    PlaylistBLL plBLL = new PlaylistBLL();
                    pl = plBLL.GetById(id.Value);
                }
                catch
                {
                    pl.Name = "ERROR: Playlist Id doesn't exist";
                }

            }
            else
            {
                pl.Name = "ERROR: Id is invalid";
            }

            return View("Display", pl);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Add", new Playlist());
        }

        [HttpPost]
        public ActionResult Add(Playlist pl)
        {
            if (String.IsNullOrEmpty(pl.Name) || pl.Name.Trim().Length == 0)
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if (ModelState.IsValid)
            {
                PlaylistBLL plBLL = new PlaylistBLL();
                plBLL.AddPlaylist(pl);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Add", pl);
            }
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            if (!String.IsNullOrEmpty(form["id"]))
            {
                try
                {
                    int id = Int32.Parse(form["id"]);
                    PlaylistBLL plBLL = new PlaylistBLL();
                    int numOfRowsDeleted = plBLL.DeletePlaylistById(id);
                    Trace.WriteLine("Num of rows deleted: " + numOfRowsDeleted);

                }
                catch (FormatException)
                {
                    Trace.WriteLine("Unable to parse form value to int: " + form["id"]);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            } 

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Playlist pl = new Playlist();
            List<Track> trackPool = new List<Track>(); 

            // --- Load playlist ---
            if (id.HasValue)
            {
                try
                {
                    PlaylistBLL plBLL = new PlaylistBLL();
                    pl = plBLL.GetById(id.Value);
                }
                catch
                {
                    pl.Name = "ERROR: Playlist Id doesn't exist";
                }
            }
            else
            {
                pl.Name = "ERROR: Id is invalid";
            }

            // --- Load track pool ---
            try
            {
                TrackBLL trackBLL = new TrackBLL();
                trackPool = trackBLL.GetAll(pl);
            }
            catch
            {
                pl.Name = "ERROR: Unable to load track pool";
            }
           
            PlaylistEditViewModel vm = new PlaylistEditViewModel(pl, trackPool);
            return View("Edit", vm);
        }

        // Model binding to ViewModel.
        // Note:
        // ViewModel will only be populated with the attributes passed via Views form submit.
        [HttpPost]
        public ActionResult Edit(PlaylistEditViewModel vm)
        {
            // Get list of selected ids from returned vm.TrackPool
            List<int> selectedIds = vm.TrackPool.FindAll(x => x.IsSelected == true).Select(x => (int) x.Id).ToList();

            // Query DB for tracks with selectedIds
            TrackBLL trackBLL = new TrackBLL();
            List<Track> selectedTracks = trackBLL.GetTracksById(selectedIds);

            // Query DB for the Playlist we shall update
            PlaylistBLL plBLL = new PlaylistBLL();
            Playlist pl = plBLL.GetById((int) vm.Playlist.Id);

            // Set selectedTracks in playlist
            pl.Tracks = selectedTracks;

            Trace.WriteLine("Count :" + pl.Tracks.Count());

            // Update DB
            plBLL.UpdatePlaylist(pl);

            return RedirectToAction("Index");
        }
    }
}
