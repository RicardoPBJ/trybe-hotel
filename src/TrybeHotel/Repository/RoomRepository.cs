using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 7. Refatore o endpoint GET /room
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            return _context.Rooms.Select(room => new RoomDto
            {
                roomId = room.RoomId,
                name = room.Name,
                capacity = room.Capacity,
                image = room.Image,
                hotel = new HotelDto
                {
                    hotelId = room.Hotel!.HotelId,
                    name = room.Hotel.Name,
                    address = room.Hotel.Address,
                    CityId = room.Hotel.City!.CityId,
                    cityName = room.Hotel.City!.Name,
                    state = room.Hotel.City!.State,
                }
            }).ToList();
        }

        // 8. Refatore o endpoint POST /room
        public RoomDto AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return _context.Rooms.Select(room => new RoomDto
            {
                roomId = room.RoomId,
                name = room.Name,
                capacity = room.Capacity,
                image = room.Image,
                hotel = new HotelDto
                {
                    hotelId = room.Hotel!.HotelId,
                    name = room.Hotel.Name,
                    address = room.Hotel.Address,
                    CityId = room.Hotel.City!.CityId,
                    cityName = room.Hotel.City!.Name,
                    state = room.Hotel.City!.State,
                }
            }).Last();
        }

        public void DeleteRoom(int RoomId)
        {
            var RoomDelete = _context.Rooms.FirstOrDefault(room => room.RoomId == RoomId);

            _context.Rooms.Remove(RoomDelete!);
            _context.SaveChanges();
        }
    }
}
