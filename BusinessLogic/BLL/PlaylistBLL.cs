using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BLL
{
    public class PlaylistBLL
    {
        public List<DTO.Model.Playlist> GetAll()
        {
            return PlaylistRepository.GetAll();
        }

        public DTO.Model.Playlist GetById(int id)
        {
            return PlaylistRepository.GetById(id);
        }

        public void AddPlaylist(DTO.Model.Playlist pl)
        {
            if (String.IsNullOrEmpty(pl.Name)) throw new InvalidOperationException("Name may not be null or empty.");
            PlaylistRepository.AddPlaylist(pl);
        }

        public void UpdatePlaylist(DTO.Model.Playlist pl)
        {
            if (!pl.Id.HasValue) throw new InvalidOperationException("Playlist Id may not be null.");
            PlaylistRepository.Update(pl);
        }

        public int DeletePlaylistById(int id)
        {
            return PlaylistRepository.DeletePlaylistById(id);
        }
        

    }
}
