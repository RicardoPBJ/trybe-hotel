using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 9. Refatore o endpoint POST /booking
        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            Room Room = _context.Rooms!.FirstOrDefault(r => r.RoomId == booking.RoomId)!;
            Hotel Hotel = _context.Hotels!.FirstOrDefault(h => h.HotelId == Room.HotelId)!;
            City City = _context.Cities!.FirstOrDefault(c => c.CityId == Hotel.CityId)!;
            User User = _context.Users!.FirstOrDefault(u => u.Email == email)!;

            if (Room.Capacity < booking.GuestQuant) return null!;

            EntityEntry<Booking> newBooking = _context.Bookings!.Add(new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                UserId = User.UserId,
                RoomId = booking.RoomId,

                User = User,
                Room = Room,
            });

            _context.SaveChanges();

            return new BookingResponse
            {
                BookingId = newBooking.Entity.BookingId,
                CheckIn = newBooking.Entity.CheckIn,
                CheckOut = newBooking.Entity.CheckOut,
                GuestQuant = newBooking.Entity.GuestQuant,
                Room = new RoomDto
                {
                    roomId = Room.RoomId,
                    name = Room.Name,
                    capacity = Room.Capacity,
                    image = Room.Image,
                    hotel = new HotelDto
                    {
                        hotelId = Room.Hotel!.HotelId,
                        name = Room.Hotel!.Name,
                        address = Room.Hotel!.Address,
                        CityId = Room.Hotel.City!.CityId,
                        cityName = Room.Hotel.City.Name,
                        state = Room.Hotel.City!.State,
                    }
                },
            };
        }

        // 10. Refatore o endpoint GET /booking
        public BookingResponse GetBooking(int bookingId, string email)
        {
            User User = _context.Users!.FirstOrDefault(u => u.Email == email)!;
            Booking RetrievedBooking = _context.Bookings!.FirstOrDefault(b => b.BookingId == bookingId)!;

            if (RetrievedBooking == null || RetrievedBooking.UserId != User.UserId) return null!;
            var retorno = from Booking in _context.Bookings
                          where bookingId == Booking.BookingId
                          select new BookingResponse
                          {
                              BookingId = Booking.BookingId,
                              CheckIn = Booking.CheckIn,
                              CheckOut = Booking.CheckOut,
                              GuestQuant = Booking.GuestQuant,
                              Room = new RoomDto
                              {
                                  roomId = Booking.Room!.RoomId,
                                  name = Booking.Room!.Name,
                                  capacity = Booking.Room!.Capacity,
                                  image = Booking.Room!.Image,
                                  hotel = new HotelDto
                                  {
                                      hotelId = Booking.Room.Hotel!.HotelId,
                                      name = Booking.Room.Hotel!.Name,
                                      address = Booking.Room.Hotel!.Address,
                                      CityId = Booking.Room.Hotel!.CityId,
                                      cityName = Booking.Room.Hotel.City!.Name,
                                      state = Booking.Room.Hotel.City!.State,
                                  }
                              }
                          };

            return retorno.First();
        }

        public Room GetRoomById(int RoomId)
        {
            throw new NotImplementedException();
        }

    }

}
